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
                        DataRowView drv = (DataRowView)grdData.SelectedRows[0].DataBoundItem;
                        DataRow aRow = drv.Row;
                        int visitColindex = aRow.Table.Columns["Visit"].Ordinal;
                        nVisitID = long.Parse(aRow.ItemArray[visitColindex].ToString(), System.Globalization.NumberStyles.Any);
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

                string sCols = string.Format("SELECT D.MetricID, DisplayNameShort FROM Metric_Definitions D INNER JOIN Metric_Schema_Definitions S ON D.MetricID = S.MetricID WHERE (SchemaID = {0}) AND (DisplayNameShort IS NOT NULL) AND (IsActive != 0) ORDER BY DisplayNameShort", schema.ID);
                string sqlRows = string.Format("SELECT VisitID, CAST(VisitID AS str) AS VisitTitle FROM Metric_Instances I INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                    " WHERE (SchemaID = {0}) AND (ScavengeTypeID = 1) AND VisitID IN ({1}) GROUP BY VisitID", schema.ID, string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                string sqlContent = string.Empty;
                switch (schema.DatabaseTable.ToLower())
                {
                    case "metric_visitmetrics":
                        sqlContent = string.Format("SELECT VisitID, MetricID, MetricValue FROM Metric_VisitMetrics V INNER JOIN Metric_Instances I ON V.InstanceID = I.InstanceID INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                            " WHERE (B.ScavengeTypeID = 1) AND VisitID IN ({0})", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                        dt = naru.db.sqlite.CrossTab.CreateCrossTab(DBCon, "Visit", sCols, sqlRows, sqlContent);

                        break;

                    case "metric_channelunitmetrics":

                        List<Tuple<string, string>> keyColumns = new List<Tuple<string, string>>();
                        keyColumns.Add(new Tuple<string, string>("VisitID", "Visit"));
                        keyColumns.Add(new Tuple<string, string>("ChannelUnitNumber", "Channel Unit Number"));

                        sqlContent = sqlContent = string.Format("SELECT VisitID, ChannelUnitNumber, MetricID, MetricValue FROM Metric_ChannelUnitMetrics V INNER JOIN Metric_Instances I ON V.InstanceID = I.InstanceID INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                            " WHERE (B.ScavengeTypeID = 1) AND VisitID IN ({0})", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));
                        
                        dt = naru.db.sqlite.CrossTabMultiColumn.CreateCrossTab(DBCon, keyColumns, sCols, sqlRows, sqlContent);

                        break;


                    case "metric_tiermetrics":

                        break;


                    default:
                        throw new Exception("Unhandled metric schema database table");
                }

                grdData.DataSource = dt;

                //
                foreach (DataGridViewColumn aCol in grdData.Columns)
                {
                    if (!aCol.HeaderText.ToLower().EndsWith("id"))
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
    }
}
