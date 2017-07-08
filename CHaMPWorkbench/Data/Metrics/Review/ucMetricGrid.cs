using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class ucMetricGrid : UserControl
    {
        public string DBCon { get; set; }
        public List<CHaMPData.VisitBasic> VisitIDs { get; set; }
        public long ProgramID { get; set; }

        public event EventHandler SelectedVisitChanged;

        public long SelectedVisit
        {
            get
            {
                long nVisitID = 0;
                if (!string.IsNullOrEmpty(DBCon))
                {
                    if (grdData.SelectedRows.Count == 1)
                    {
                        //DataRowView drv = (DataRowView)grdData.SelectedRows[0].DataBoundItem;
                        //DataRow aRow = drv.Row;
                        //int visitColindex = aRow.Table.Columns["Visit"].Ordinal;
                        //nVisitID = long.Parse(aRow.ItemArray[visitColindex].ToString(), System.Globalization.NumberStyles.Any);
                    }
                }
                return nVisitID;
            }
        }

        public ucMetricGrid()
        {
            InitializeComponent();

            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.ReadOnly = true;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdData.MultiSelect = false;
            grdData.Dock = DockStyle.Fill;
        }

        private void LoadData(CHaMPData.MetricSchema schema)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DataTable dt = null;

                string sCols = string.Format("SELECT D.MetricID, DisplayNameShort FROM Metric_Definitions D INNER JOIN Metric_Schema_Definitions S ON D.MetricID = S.MetricID WHERE (SchemaID = {0}) AND (DisplayNameShort IS NOT NULL) AND (IsActive != 0) AND (DataTypeID = 10023) ORDER BY DisplayNameShort", schema.ID);

                string sqlContent = string.Empty;
                switch (schema.DatabaseTable.ToLower())
                {
                    case "metric_visitmetrics":
                        sqlContent = string.Format("SELECT VisitID, MetricID, MetricValue FROM Metric_VisitMetrics V INNER JOIN Metric_Instances I ON V.InstanceID = I.InstanceID INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                            " WHERE (B.ScavengeTypeID = 1) AND VisitID IN ({0})", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                        string sqlRows = string.Format("SELECT VisitID, CAST(VisitID AS str) AS VisitTitle FROM Metric_Instances I INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                           " WHERE (SchemaID = {0}) AND (ScavengeTypeID = 1) AND VisitID IN ({1}) GROUP BY VisitID", schema.ID, string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                        dt = naru.db.sqlite.CrossTab.CreateCrossTab(DBCon, "VisitID", sCols, sqlRows, sqlContent);

                        break;

                    case "metric_channelunitmetrics":

                        List<Tuple<string, string>> channelUnitKeys = new List<Tuple<string, string>>();
                        channelUnitKeys.Add(new Tuple<string, string>("VisitID", "Visit"));
                        channelUnitKeys.Add(new Tuple<string, string>("ChannelUnitNumber", "Channel Unit Number"));

                        sqlContent = sqlContent = string.Format("SELECT VisitID, ChannelUnitNumber, MetricID, MetricValue FROM Metric_ChannelUnitMetrics V INNER JOIN Metric_Instances I ON V.InstanceID = I.InstanceID INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                            " WHERE (B.ScavengeTypeID = 1) AND VisitID IN ({0})", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                        dt = naru.db.sqlite.CrossTabMultiColumn.CreateCrossTab(DBCon, channelUnitKeys, sCols, sqlContent);
                        AddchannelUnitTiers(ref dt);

                        break;


                    case "metric_tiermetrics":

                        List<Tuple<string, string>> tierKeys = new List<Tuple<string, string>>();
                        tierKeys.Add(new Tuple<string, string>("VisitID", "Visit"));
                        tierKeys.Add(new Tuple<string, string>("TierID", "Tier Name"));

                        long nListID = 5;
                        if (schema.Name.Contains("2"))
                            nListID = 11;

                        sqlContent = sqlContent = string.Format("SELECT VisitID, TierID, MetricID, MetricValue" +
                            " FROM Metric_TierMetrics V INNER JOIN Metric_Instances I ON V.InstanceID = I.InstanceID INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                            " INNER JOIN LookupListItems T ON V.TierID = T.ItemID" +
                            " WHERE (B.ScavengeTypeID = 1) AND VisitID IN ({0}) AND (T.ListID = {1})", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()), nListID);

                        dt = naru.db.sqlite.CrossTabMultiColumn.CreateCrossTab(DBCon, tierKeys, sCols, sqlContent);
                        AddcTierNames(ref dt);

                        break;

                    default:
                        throw new Exception("Unhandled metric schema database table");
                }

                AddMetaFields(ref dt, 1);

                grdData.DataSource = dt;

                //
                foreach (DataGridViewColumn aCol in grdData.Columns)
                {
                    if (!aCol.HeaderText.ToLower().EndsWith("id") && string.Compare(aCol.HeaderText, "year", true) != 0)
                        aCol.DefaultCellStyle.Format = "#,##0.000";

                    // All columns are read only.
                    aCol.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.WaitCursor;
            }
        }

        private void AddchannelUnitTiers(ref DataTable dt)
        {
            dt.Columns.Add("Tier1", Type.GetType("System.String")).SetOrdinal(2);
            dt.Columns.Add("Tier2", Type.GetType("System.String")).SetOrdinal(3);

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT Tier1, Tier2 FROM CHaMP_ChannelUnits WHERE (VisitID = @VisitID) AND (ChannelUnitNumber = @ChannelUnitNumber)", dbCon);
                SQLiteParameter pVisitID = dbCom.Parameters.Add("VisitID", DbType.Int64);
                SQLiteParameter pCUNum = dbCom.Parameters.Add("ChannelUnitNumber", DbType.Int64);

                foreach (DataRow dr in dt.Rows)
                {
                    pVisitID.Value = long.Parse(dr["VisitID"].ToString());
                    pCUNum.Value = long.Parse(dr["ChannelUnitNumber"].ToString());
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    if (dbRead.Read())
                    {
                        dr["Tier1"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Tier1");
                        dr["Tier2"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Tier2");
                    }
                    dbRead.Close();
                }
            }
        }

        private void AddMetaFields(ref DataTable dt, int colInsertIndex)
        {
            dt.Columns.Add("Site", Type.GetType("System.String")).SetOrdinal(colInsertIndex);
            dt.Columns.Add("Stream", Type.GetType("System.String")).SetOrdinal(colInsertIndex + 1);
            dt.Columns.Add("Watershed", Type.GetType("System.String")).SetOrdinal(colInsertIndex + 3);
            dt.Columns.Add("Year", Type.GetType("System.Int64")).SetOrdinal(colInsertIndex + 2);

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT SiteName, StreamName, VisitYear, WatershedName FROM CHaMP_Sites S INNER JOIN CHaMP_Visits V ON (S.SiteID = V.SiteID)" +
                    " INNER JOIN CHaMP_Watersheds W ON S.WatershedID = W.WatershedID WHERE (V.VisitID = @VisitID)", dbCon);
                SQLiteParameter pVisitID = dbCom.Parameters.Add("VisitID", DbType.Int64);

                foreach (DataRow dr in dt.Rows)
                {
                    pVisitID.Value = long.Parse(dr["VisitID"].ToString());
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    if (dbRead.Read())
                    {
                        dr["Site"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "SiteName");
                        dr["Stream"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "StreamName");
                        dr["Watershed"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "WatershedName");
                        dr["Year"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueInt(ref dbRead, "VisitYear");
                    }
                    dbRead.Close();
                }
            }
        }

        private void AddcTierNames(ref DataTable dt)
        {
            dt.Columns.Add("Tier", Type.GetType("System.String")).SetOrdinal(1);

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT Title FROM LookupListItems WHERE (ItemID = @ItemID)", dbCon);
                SQLiteParameter pTierID = dbCom.Parameters.Add("ItemID", DbType.Int64);

                foreach (DataRow dr in dt.Rows)
                {
                    pTierID.Value = long.Parse(dr["TierID"].ToString());
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    if (dbRead.Read())
                    {
                        dr["Tier"] = naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "title");
                    }
                    dbRead.Close();
                }
            }

            // Now remove the column that has the Tier ID
            dt.Columns.Remove("TierID");
        }

        public void ExportDataToCSV(System.IO.FileInfo fiExport)
        {
            StringBuilder sb = new StringBuilder();

            DataTable dt = (DataTable)grdData.DataSource;
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            System.IO.File.WriteAllText(fiExport.FullName, sb.ToString());
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count > 0)
            {
                if (grdData.SelectedRows[0].Index >= 0)
                {
                    EventHandler handler = this.SelectedVisitChanged;
                    if (handler != null)
                        handler(this, e);
                }
            }
        }

        public void OnSelectedSchemaChanged(CHaMPData.MetricSchema schema)
        {
            try
            {
                LoadData(schema);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void exportMetricDataToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Metric Data CSV File";
            frm.Filter = "Comma Separated Value Files (*.csv)|*.csv";
            frm.InitialDirectory = System.IO.Path.GetDirectoryName(naru.db.sqlite.DBCon.DatabasePath);
            frm.FileName = string.Format("{0:yyyyMMdd}_metric_export", DateTime.Now);
            frm.AddExtension = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportDataToCSV(new System.IO.FileInfo(frm.FileName));
                    if (MessageBox.Show("CSV file written successfully. Do you want to open the file?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        System.Diagnostics.Process.Start(frm.FileName);
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }
    }
}
