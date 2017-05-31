using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class frmVisitDetails : Form
    {
        private string DBCon { get; set; }
        private long VisitID { get; set; }

        private BindingSource bsLogMessages;

        public frmVisitDetails(string sDBCon, long nVisitID)
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
            ConfigureDataGrid(ref grdLogMessages);

            grdVisitDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                LoadVisitHeaderAndNotes();
                LoadChannelUnits();
                LoadVisitDetails();
                LoadLogMessageCombos();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void LoadVisitHeaderAndNotes()
        {
            txtVisitID.Text = VisitID.ToString();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT WatershedName, VisitYear, Organization, PanelName, SiteName, V.Remarks AS Remarks" +
                    " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                    " WHERE (V.VisitID = @VisitID)", dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", VisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                if (dbRead.Read())
                {
                    txtFieldSeason.Text = GetSafeString(ref dbRead, "VisitYear");
                    txtWatershed.Text = GetSafeString(ref dbRead, "WatershedName");
                    txtSite.Text = GetSafeString(ref dbRead, "SiteName");
                    txtPanel.Text = GetSafeString(ref dbRead, "PanelName");
                    txtOrganization.Text = GetSafeString(ref dbRead, "Organization");
                    txtNotes.Text = GetSafeString(ref dbRead, "Remarks");
                }
            }
        }

        private string GetSafeString(ref SQLiteDataReader dbRead, string sFieldName)
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
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                string sSQL = "SELECT SegmentNumber AS [Segment Number], C.ChannelUnitNumber AS [Unit Number], C.Tier1, C.Tier2," +
                     "C.BouldersGT256, C.Cobbles65255, C.CoarseGravel1764, C.FineGravel316, C.Sand0062, C.FinesLT006, C.SumSubstrateCover, C.Bedrock, C.LargeWoodCount" +
                     " FROM CHAMP_ChannelUnits AS C" +
                     " WHERE (C.VisitID = @VisitID)" +
                     " ORDER BY C.ChannelUnitNumber, SegmentNumber";

                SQLiteDataAdapter da = new SQLiteDataAdapter(sSQL, dbCon);
                da.SelectCommand.Parameters.AddWithValue("@VisitID", VisitID);
                DataTable ta = new DataTable();
                da.Fill(ta);

                grdChannelUnits.DataSource = ta;

                foreach (DataGridViewColumn aCol in grdChannelUnits.Columns)
                    aCol.ReadOnly = true;
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

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                string sSQL = "SELECT W.WatershedName, S.SiteName, V.*, S.UTMZone, S.UC_Chin, S.SN_Chin, S.LC_Steel, S.MC_Steel, S.UC_Steel, S.SN_Steel, S.Latitude, S.Longitude, S.GageID, P.Title AS Protocol" +
                    " FROM CHaMP_Watersheds W" +
                    " INNER JOIN CHaMP_Sites S ON W.WatershedID = S.WatershedID" +
                    " INNER JOIN CHaMP_Visits V ON S.SiteID = V.SiteID" +
                    " INNER JOIN LookupListItems P ON V.ProtocolID = P.ItemID" +
                    " WHERE V.VisitID = @VisitID";

                SQLiteDataAdapter da = new SQLiteDataAdapter(sSQL, dbCon);
                da.SelectCommand.Parameters.AddWithValue("@VisitID", VisitID);
                DataTable ta = new DataTable();
                da.Fill(ta);

                // Add fields that should be ignored to this array
                string[] lColsToIgnore = { "SiteID" };

                foreach (DataColumn col in ta.Columns)
                {
                    col.ReadOnly = true;

                    if (lColsToIgnore.Contains<string>(col.ColumnName))
                        continue;

                    int nRow = grdVisitDetails.Rows.Add();
                    grdVisitDetails.Rows[nRow].Cells[0].Value = col.ColumnName;
                    grdVisitDetails.Rows[nRow].Cells[1].Value = ta.Rows[0].ItemArray.GetValue(col.Ordinal).ToString();
                }
            }
        }

        private void LoadLogMessageCombos()
        {
            grdLogMessages.AutoGenerateColumns = false;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT LogFiles.ResultID AS ResultID, Metric_Results.ModelVersion AS ModelVersion, LogFiles.Status AS Status, Metric_Results.RunDateTime AS RunDateTime, LookupListItems.Title AS ScavengeType" +
                    " FROM LookupListItems INNER JOIN (Metric_Results INNER JOIN LogFiles ON Metric_Results.ResultID = LogFiles.ResultID) ON LookupListItems.ItemID = Metric_Results.ScavengeTypeID" +
                    " WHERE (Metric_Results.VisitID = @VisitID) ORDER BY RunDateTime DESC", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", VisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    cboLogResults.Items.Add(new naru.db.NamedObject(dbRead.GetInt64(dbRead.GetOrdinal("ResultID")), string.Format("Version {0} on {1:dd MMM yyy} status of {2}", dbRead["ModelVersion"], dbRead["RunDateTime"], dbRead["Status"])));
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
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT LogMessages.LogMessageID, LogMessages.MessageType, LogMessages.LogSeverity, LogMessages.LogMessage" +
                   " FROM LogFiles INNER JOIN LogMessages ON LogFiles.LogID = LogMessages.LogID" +
                   " WHERE (LogFiles.ResultID = @ResultID) ORDER BY LogMessages.LogDateTime", dbCon);
                da.SelectCommand.Parameters.AddWithValue("@ResultID", ((naru.db.NamedObject)cboLogResults.SelectedItem).ID);

                DataTable taLogMessages = new DataTable();
                da.Fill(taLogMessages);
                bsLogMessages = new BindingSource(taLogMessages, "");
                grdLogMessages.DataSource = bsLogMessages;

                foreach (DataGridViewColumn grdCol in grdLogMessages.Columns)
                {
                    grdCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    grdCol.ReadOnly = true;
                }
            }
        }

        private void FilterLogMessages(object sender, EventArgs e)
        {
            if (bsLogMessages != null)
            {
                bsLogMessages.Filter = string.Empty;
                if (cboLogMessageTypes.SelectedIndex > 0)
                    bsLogMessages.Filter = string.Format("LogSeverity = '{0}'", cboLogMessageTypes.Text.ToLower());
            }
        }
    }
}
