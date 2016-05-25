using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class frmVisitDetails : Form
    {
        private string DBCon { get; set; }
        private int VisitID { get; set; }

        private BindingSource bsLogMessages;

        public frmVisitDetails(string sDBCon, int nVisitID)
        {
            InitializeComponent();
            DBCon = sDBCon;
            VisitID = nVisitID;
        }

        private void ConfigureDataGrid(ref DataGridView grd)
        {
            grd.AllowUserToAddRows = false;
            grd.AllowUserToDeleteRows = false;
            grd.AllowUserToResizeRows = false;
            grd.RowHeadersVisible = false;
            grd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void frmVisitDetails_Load(object sender, EventArgs e)
        {
            ConfigureDataGrid(ref grdChannelUnits);
            ConfigureDataGrid(ref grdVisitDetails);
            ConfigureDataGrid(ref grdMetrics);
            ConfigureDataGrid(ref grdLogMessages);

            grdVisitDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                LoadVisitHeader();
                LoadChannelUnits();
                LoadVisitDetails();
                LoadMetricResults();

                LoadLogMessageCombos();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void LoadVisitHeader()
        {
            txtVisitID.Text = VisitID.ToString();

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT W.WatershedName, V.VisitYear, V.Organization, V.PanelName, S.SiteName" +
                    " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                    " WHERE (V.VisitID = @VisitID)", dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", VisitID);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                if (dbRead.Read())
                {
                    txtFieldSeason.Text = GetSafeString(ref dbRead, "VisitYear");
                    txtWatershed.Text = GetSafeString(ref dbRead, "WatershedName");
                    txtSite.Text = GetSafeString(ref dbRead, "SiteName");
                    txtPanel.Text = GetSafeString(ref dbRead, "PanelName");
                    txtOrganization.Text = GetSafeString(ref dbRead, "Organization");
                }
            }
        }

        private string GetSafeString(ref OleDbDataReader dbRead, string sFieldName)
        {
            string sResult = string.Empty;
            int nField = dbRead.GetOrdinal(sFieldName);
            if (nField >= 0)
            {
                if (!dbRead.IsDBNull(nField))
                {
                    switch (dbRead.GetFieldType(nField).Name)
                    {
                        case "String":
                            sResult = dbRead.GetString(nField);
                            break;

                        case "Int16":
                            sResult = dbRead.GetInt16(nField).ToString();
                            break;

                        case "Int32":
                            sResult = dbRead.GetInt32(nField).ToString();
                            break;

                        case "Int64":
                            sResult = dbRead.GetInt64(nField).ToString();
                            break;
                    }
                }
            }
            return sResult;
        }

        private void LoadChannelUnits()
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                string sSQL = "SELECT S.SegmentNumber AS [Segment Number], C.ChannelUnitNumber AS [Unit Number], C.Tier1, C.Tier2," +
                     "C.BouldersGT256, C.Cobbles65255, C.CoarseGravel1764, C.FineGravel316, C.Sand0062, C.FinesLT006, C.SumSubstrateCover, C.Bedrock, C.LargeWoodCount" +
                     " FROM CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS C ON S.SegmentID = C.SegmentID" +
                     " WHERE (S.VisitID = @VisitID)" +
                     " ORDER BY S.SegmentNumber, C.ChannelUnitNumber";

                OleDbDataAdapter da = new OleDbDataAdapter(sSQL, dbCon);
                da.SelectCommand.Parameters.AddWithValue("@VisitID", VisitID);
                DataTable ta = new DataTable();
                da.Fill(ta);

                grdChannelUnits.DataSource = ta;
            }
        }

        /// <summary>
        /// Load the gridview of visit details
        /// </summary>
        /// <remarks>This method works unlike all the others on this form.
        /// The code uses a data adaptor to fill a data table of all the visit
        /// details that it intends to display as **rows**. It then loops over
        /// the **columns** in the data table and un-pivots them as rows 
        /// (checking that they are not in the list of rows to ignore).</remarks>
        private void LoadVisitDetails()
        {
            grdVisitDetails.Rows.Clear();
            grdVisitDetails.Columns.Clear();

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.ReadOnly = true;
            col1.Width = 200;
            col1.HeaderText = "Attribute";
            grdVisitDetails.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.ReadOnly = true;
            col2.Width = 200;
            col2.HeaderText = "Value";
            grdVisitDetails.Columns.Add(col2);

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                string sSQL = "SELECT CHAMP_Watersheds.WatershedName, S.SiteName, V.*, S.UTMZone, S.UC_Chin, S.SN_Chin, S.LC_Steel, S.MC_Steel, S.UC_Steel, S.SN_Steel, S.Latitude, S.Longitude, S.GageID, P.Title AS Protocol" +
                    " FROM (CHAMP_Watersheds INNER JOIN ((CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) INNER JOIN (CHaMP_Segments INNER JOIN CHAMP_ChannelUnits ON CHaMP_Segments.SegmentID = CHAMP_ChannelUnits.SegmentID) ON V.VisitID = CHaMP_Segments.VisitID) ON CHAMP_Watersheds.WatershedID = S.WatershedID) INNER JOIN LookupListItems AS P ON V.ProtocolID = P.ItemID" +
                    " WHERE V.VisitID = @VisitID";

                OleDbDataAdapter da = new OleDbDataAdapter(sSQL, dbCon);
                da.SelectCommand.Parameters.AddWithValue("@VisitID", VisitID);
                DataTable ta = new DataTable();
                da.Fill(ta);

                // Add fields that should be ignored to this array
                string[] lColsToIgnore = { "SiteID" };

                foreach (DataColumn col in ta.Columns)
                {
                    if (lColsToIgnore.Contains<string>(col.ColumnName))
                        continue;

                    int nRow = grdVisitDetails.Rows.Add();
                    grdVisitDetails.Rows[nRow].Cells[0].Value = col.ColumnName;
                    grdVisitDetails.Rows[nRow].Cells[1].Value = ta.Rows[0].ItemArray.GetValue(col.Ordinal).ToString();
                }
            }
        }

        /// <summary>
        /// Fills the combo box with the unique model result runs for this visit
        /// </summary>
        private void LoadMetricResults()
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                string sSQL = "TRANSFORM Max(Metric_VisitMetrics.MetricValue) AS MaxOfMetricValue" +
                    " SELECT Metric_Definitions.Title AS Metric" +
                    " FROM Metric_Results INNER JOIN (Metric_Definitions INNER JOIN Metric_VisitMetrics ON Metric_Definitions.MetricID = Metric_VisitMetrics.MetricID) ON Metric_Results.ResultID = Metric_VisitMetrics.ResultID" +
                    " WHERE (Metric_Results.VisitID = " + VisitID.ToString() + ")" +
                    " GROUP BY Metric_Definitions.Title" +
                    " PIVOT Metric_VisitMetrics.ResultID";

                OleDbDataAdapter da = new OleDbDataAdapter(sSQL, dbCon);
                //da.SelectCommand.Parameters.AddWithValue("@VisitID", VisitID);
                DataTable ta = new DataTable();
                da.Fill(ta);

                // So far the column headings are just the result IDs.
                // Now replace them with nice formatting
                OleDbCommand comResults = new OleDbCommand("SELECT Metric_Results.ModelVersion, Metric_Results.RunDateTime, LookupListItems.Title AS ScavengeType" +
                    " FROM LookupListItems INNER JOIN Metric_Results ON LookupListItems.ItemID = Metric_Results.ScavengeTypeID" +
                    " WHERE (Metric_Results.ResultID = @VisitID)", dbCon);
                OleDbParameter pResultID = comResults.Parameters.Add("@ResultID", OleDbType.Integer);

                foreach (DataColumn aCol in ta.Columns)
                {
                    int nResultID = 0;
                    if (int.TryParse(aCol.ColumnName, out nResultID))
                    {
                        pResultID.Value = nResultID;
                        OleDbDataReader dbRead = comResults.ExecuteReader();
                        if (dbRead.Read())
                        {
                            aCol.ColumnName = string.Format("{0} on {1:dd MMM yy}", dbRead["ModelVersion"], dbRead["RunDateTime"]);
                        }
                        dbRead.Close();
                    }
                }

                grdMetrics.DataSource = ta;
                grdMetrics.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                foreach (DataGridViewColumn grdCol in grdMetrics.Columns)
                {
                    if (string.Compare(grdCol.HeaderText, "Metric", true) != 0)
                    {
                        grdCol.DefaultCellStyle.Format = "#0.00";
                        grdCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
        }

        private void LoadLogMessageCombos()
        {

            grdLogMessages.AutoGenerateColumns = false;

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT LogFiles.ResultID, Metric_Results.ModelVersion, LogFiles.Status, Metric_Results.RunDateTime, LookupListItems.Title AS ScavengeType" +
                    " FROM LookupListItems INNER JOIN (Metric_Results INNER JOIN LogFiles ON Metric_Results.ResultID = LogFiles.ResultID) ON LookupListItems.ItemID = Metric_Results.ScavengeTypeID" +
                    " WHERE (Metric_Results.VisitID = @VisitID) ORDER BY Metric_Results.RunDateTime DESC", dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", VisitID);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    cboLogResults.Items.Add(new ListItem(string.Format("Version {0} on {1:dd MMM yyy} status of {2}", dbRead["ModelVersion"], dbRead["RunDateTime"], dbRead["Status"]), dbRead.GetInt32(dbRead.GetOrdinal("ResultID"))));
                }

                cboLogResults.SelectedIndexChanged += LoadLogMessages;
                if (cboLogResults.Items.Count > 0)
                    cboLogResults.SelectedIndex = 0;
                
                cboLogMessageTypes.Items.Add("All");
                cboLogMessageTypes.Items.Add("Error");
                cboLogMessageTypes.Items.Add("Info");
                cboLogMessageTypes.Items.Add("Warning");
                cboLogMessageTypes.SelectedIndex = 0;
                cboLogMessageTypes.SelectedIndexChanged += FilterLogMessages;
            }
        }

        private void LoadLogMessages(object sender, EventArgs e)
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbDataAdapter da = new OleDbDataAdapter("SELECT LogMessages.LogMessageID, LogMessages.MessageType, LogMessages.LogSeverity, LogMessages.LogMessage" +
                   " FROM LogFiles INNER JOIN LogMessages ON LogFiles.LogID = LogMessages.LogID" +
                   " WHERE (LogFiles.ResultID = @ResultID) ORDER BY LogMessages.LogDateTime", dbCon);
                da.SelectCommand.Parameters.AddWithValue("@ResultID", ((ListItem)cboLogResults.SelectedItem).Value);

                DataTable taLogMessages = new DataTable();
                da.Fill(taLogMessages);
                bsLogMessages = new BindingSource(taLogMessages, "");
                grdLogMessages.DataSource = bsLogMessages;

                foreach (DataGridViewColumn grdCol in grdLogMessages.Columns)
                    grdCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void FilterLogMessages(object sender, EventArgs e)
        {
            bsLogMessages.Filter = string.Empty;
            if (cboLogMessageTypes.SelectedIndex > 0)
                bsLogMessages.Filter = string.Format("LogSeverity = '{0}'", cboLogMessageTypes.Text.ToLower());
        }
    }
}
