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

        public frmVisitDetails(string sDBCon, int nVisitID)
        {
            InitializeComponent();
            DBCon = sDBCon;
            VisitID = nVisitID;
        }

        private void frmVisitDetails_Load(object sender, EventArgs e)
        {
            grdChannelUnits.AllowUserToAddRows = false;
            grdChannelUnits.AllowUserToDeleteRows = false;
            grdChannelUnits.AllowUserToResizeRows = false;
            grdChannelUnits.RowHeadersVisible = false;

            try
            {
                LoadVisitHeader();
                LoadChannelUnits();
                LoadVisitDetails();
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

        private void LoadVisitDetails()
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                string sSQL = "";

                OleDbDataAdapter da = new OleDbDataAdapter(sSQL, dbCon);
                da.SelectCommand.Parameters.AddWithValue("@VisitID", VisitID);
                DataTable ta = new DataTable();
                da.Fill(ta);



            }
        }
    }
}
