using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using ZedGraph;

namespace CHaMPWorkbench.Experimental.James
{
     public partial class frmUSGS_StreamDataViewer : Form
    {
        private OleDbConnection m_dbCon;
        private int m_iGageID;
        private USGS_StreamData m_USGS_StreamData;

        public frmUSGS_StreamDataViewer(OleDbConnection dbCon, string sSiteName, string sWatershedName)
        {
            InitializeComponent();
            m_dbCon = dbCon;

            LoadVisits();
            cmbCHaMPSite.Text = sSiteName;
            cmbWatershed.Text = sWatershedName;
            //m_iGageID = iGageID;

            m_USGS_StreamData = new USGS_StreamData(m_dbCon, sSiteName);
            SetStreamGageBasedOnSite(m_USGS_StreamData);

        }

        private void cmdGetData_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            //Get the selected USGS Gage number
            int iGageID = Convert.ToInt32(txtUSGS_SiteNumber.Text);

            //Get the data
            List<StreamFlowSample> lStreamData = m_USGS_StreamData.GetUSGS_DischargeData(iGageID);
            if (lStreamData.Count > 1)
            {
                string sSiteName  = cmbCHaMPSite.SelectedItem.ToString();
                //plot data
                PlotStreamData(m_dbCon, m_USGS_StreamData.StreamData, sSiteName, iGageID);
            }
            else
            {
                //need to put something on the form that says this site has no preloaded gage, user must chose from drop-down
                MessageBox.Show(String.Format("There is not data for USGS gage number {0}.", m_USGS_StreamData.GageNumber),
                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        //private void cmdOutputDirectory_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog dlg = new FolderBrowserDialog();
        //    dlg.SelectedPath = @"C:\Users\A01674762\Box Sync\CHAMP\GCD_Analysis_Meta\raw_Data\USGS\StreamGages";
        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        txtOutputDirectory.Text = dlg.SelectedPath;
        //    }
        //}

        private void LoadVisits()
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sGroupFields = " W.WatershedID, W.WatershedName, V.VisitID, V.VisitYear, V.SampleDate,V.HitchName,V.CrewName,V.PanelName, S.SiteID, S.SiteName, V.Organization, V.QCVisit, V.CategoryName, V.VisitPhase,V.VisitStatus,V.AEM,V.HasStreamTempLogger,V.HasFishData";

            string sSQL = "SELECT " + sGroupFields + ", Count(C.SegmentID) AS ChannelUnits" +
                " FROM ((CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID) LEFT JOIN CHaMP_Segments AS Seg ON V.VisitID = Seg.VisitID) LEFT JOIN CHAMP_ChannelUnits AS C ON Seg.SegmentID = C.SegmentID" +
                " GROUP BY " + sGroupFields +
                " ORDER BY W.WatershedName, S.SiteName, V.VisitID";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);

            // Load the field seasons
            OleDbCommand comFS = new OleDbCommand("SELECT VisitYear FROM CHAMP_Visits WHERE (VisitYear Is Not Null) GROUP BY VisitYear ORDER BY VisitYear", m_dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();
            // Load the watersheds
            comFS = new OleDbCommand("SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) GROUP BY WatershedID, WatershedName ORDER BY WatershedName", m_dbCon);
            dbRead = comFS.ExecuteReader();
            cmbWatershed.Items.Add("");
            while (dbRead.Read())
            {
                int nSel = cmbWatershed.Items.Add(new ListItem((string)dbRead[1], (int)dbRead[0]));
            }
            dbRead.Close();

            // Load the Sites
            comFS = new OleDbCommand("SELECT SiteID, SiteName FROM CHAMP_Sites WHERE (SiteName Is Not Null) ORDER BY SiteName", m_dbCon);
            dbRead = comFS.ExecuteReader();
            cmbCHaMPSite.Items.Add("");
            while (dbRead.Read())
            {
                int nSel = cmbCHaMPSite.Items.Add(new ListItem((string)dbRead[1], (int)dbRead[0]));
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        public class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }

        public Coordinate GetCoordinate(OleDbConnection dbCon, string sValue)
        {
            string sSQL = "SELECT Latitude, Longitude " +
                          " FROM CHAMP_Sites" +
                          " WHERE SiteName = @value";

            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            dbCom.Parameters.Add(new OleDbParameter("@value", sValue));
            dbCom.ExecuteNonQuery();
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            var coordinate = new Coordinate(0, 0);
            while (dbRead.Read())
            {
                coordinate = new Coordinate(Convert.ToDouble(dbRead[0]), Convert.ToDouble(dbRead[1]));
            }
            dbRead.Close();
            return coordinate;
        }

        private void cmbCHaMPSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbUSGS_Gage.Items.Clear();
            string sCHaMP_Site = cmbCHaMPSite.SelectedItem.ToString();
            Coordinate champCoordinate = GetCoordinate(m_dbCon, sCHaMP_Site);

            string sSQL = "SELECT GageID, Latitude, Longitude " +
                          " FROM USGS_gages";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            dbCom.ExecuteNonQuery();
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            List<KeyValuePair<string, double>> lUSGS_GageSites = new List<KeyValuePair<string, double>>();
            while (dbRead.Read())
            {
                string usgsGageNumber = dbRead[0].ToString();
                Coordinate usgsCoordinate = new Coordinate(Convert.ToDouble(dbRead[1]), Convert.ToDouble(dbRead[2]));
                double distance = Math.Round(Coordinate.Distance(champCoordinate, usgsCoordinate, Coordinate.UnitsOfLength.Kilometer), 2);
                Coordinate.CardinalPoints compass = Coordinate.Bearing(champCoordinate, usgsCoordinate).ToCardinalMark();
                lUSGS_GageSites.Add(new KeyValuePair<string, double>(String.Format("{0}; Distance: {1} Direction: {2}", usgsGageNumber, distance, compass), distance));
            }
            dbRead.Close();

            //sort in ascending order by distance from champ site 
            var lUSGS_Sorted = lUSGS_GageSites.OrderBy(site => site.Value);

            foreach (KeyValuePair<string, double> usgsGage in lUSGS_Sorted)
            {
                cmbUSGS_Gage.Items.Add(usgsGage.Key);
            }

            //Check if there is a pre-selected USGS gage, if so select that
            if (m_USGS_StreamData != null)
            {
                m_USGS_StreamData.CheckCHaMP_SiteForAssociatedGage(sCHaMP_Site);
                SetStreamGageBasedOnSite(m_USGS_StreamData);
            }

            
        }

        private void SetStreamGageBasedOnSite(USGS_StreamData pUSGS_StreamData)
        {
            if (m_USGS_StreamData.SiteHasGageID == true)
            {
                //Set selected gage to the site gage id
                bool bFoundGage = false;
                char splitCharacter = (char)59;
                for (int i = 0; i < cmbUSGS_Gage.Items.Count; i++ )
                {
                    if (string.Compare(cmbUSGS_Gage.Items[i].ToString().Split(splitCharacter)[0], m_USGS_StreamData.GageNumber.ToString()) == 0)
                    {
                        cmbUSGS_Gage.SelectedIndex = i;
                        lblWarningNoUSGS_Gage.Text = String.Format("USGS gage {0} selected above is the pre-selected gage associated with this CHaMP site.", m_USGS_StreamData.GageNumber.ToString());
                        bFoundGage = true;
                        break;
                    }
                }
                if (bFoundGage == false)
                {
                    lblWarningNoUSGS_Gage.Text = String.Format("The pre-selected USGS gage {0} associated with this CHaMP site, is currently not in the database.", m_USGS_StreamData.GageNumber.ToString());
                }
            }
            else if (m_USGS_StreamData.SiteHasGageID == false)
            {
                lblWarningNoUSGS_Gage.Text = "Currently there is no USGS gage associated with this CHaMP site. Please select a site from the drop-down above.";
            }


        }

        private void cmbUSGS_Gage_SelectedIndexChanged(object sender, EventArgs e)
        {
            char splitCharacter = (char)59;
            string sUSGS_GageNumber = cmbUSGS_Gage.SelectedItem.ToString().Split(splitCharacter)[0];
            txtUSGS_SiteNumber.Text = sUSGS_GageNumber;
        }

        private void PlotStreamData(OleDbConnection dbCon, List<StreamFlowSample> lStreamData, string sCHaMPSiteName, int iGageID)
        {
            //plot data


            //Get Gage Description
            string sGroupFields = " Description";
            string sSQL = "SELECT " + sGroupFields +
                          " FROM USGS_Gages  " +
                          " WHERE GageID = " + iGageID;

            OleDbCommand comFS = new OleDbCommand(sSQL, dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();
            
            string sUSGS_Description = string.Empty;
            while (dbRead.Read())
            {
                sUSGS_Description  = Convert.ToString(dbRead[0]);
            }
            dbRead.Close();

            //set auxillary info axis, visit labels, etc.
            ZedGraph.GraphPane zPane = zedGraphControl.GraphPane;

            zPane.GraphObjList.Clear();
            zPane.CurveList.Clear();

            //set and configure the axis
            zPane.Title.Text = String.Format("USGS Gage:{0} {1} {2}", iGageID.ToString(), Environment.NewLine, sUSGS_Description);
            zPane.XAxis.Type = ZedGraph.AxisType.Date;
            zPane.XAxis.Scale.MajorUnit = ZedGraph.DateUnit.Year;
            zPane.XAxis.Scale.MajorStep = .5;
            zPane.XAxis.Scale.Format = "MM-yyyy";
            zPane.XAxis.Scale.MinorUnit = ZedGraph.DateUnit.Month;
            zPane.XAxis.Scale.MinorStep = 4;
            zPane.XAxis.Scale.FontSpec.Angle = 45;
            zPane.XAxis.Title.Text = "Date";
            DateTime minDate = lStreamData.Min(d => d.Date.Date);
            ZedGraph.XDate xMin = new ZedGraph.XDate(minDate.Year, minDate.Month, minDate.Day);
            DateTime maxDate = lStreamData.Max(d => d.Date.Date);
            ZedGraph.XDate xMax = new ZedGraph.XDate(maxDate.Year, maxDate.Month, maxDate.Day);
            zPane.XAxis.Scale.Min = xMin;
            zPane.XAxis.Scale.Max = xMax;
            zPane.YAxis.Title.Text = "Flow (cubic feet/second)";

            //Add the survey dates for the site in question
           sGroupFields = " V.VisitID, V.SiteID, V.SampleDate, S.SiteID, S.SiteName";
           sCHaMPSiteName = String.Format("\"{0}\"", sCHaMPSiteName);
           sSQL = "SELECT " + sGroupFields +
                " FROM (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) " +
                " WHERE S.SiteName = " + sCHaMPSiteName +
                " ORDER BY V.SampleDate";

            comFS = new OleDbCommand(sSQL, dbCon);
            dbRead = comFS.ExecuteReader();

            TupleList<ZedGraph.XDate, int> zVisitSampleDates = new TupleList<ZedGraph.XDate, int>();
            while (dbRead.Read())
            {
                DateTime sampleDate = Convert.ToDateTime(dbRead[2]);
                int iVisitID = Convert.ToInt32(dbRead[0]);
                ZedGraph.XDate xdSample = new ZedGraph.XDate(sampleDate.Year, sampleDate.Month, sampleDate.Day);
                zVisitSampleDates.Add(xdSample, iVisitID);
            }
            dbRead.Close();

            if (lStreamData.Count > 0)
            {

                //Add stream data to PointPairList
                ZedGraph.PointPairList zList = new ZedGraph.PointPairList();
                foreach (StreamFlowSample sample in lStreamData)
                {
                    ZedGraph.XDate x = new ZedGraph.XDate(sample.Date.Year, sample.Date.Month, sample.Date.Day);

                    double y = sample.Flow;
                    if (y < 0)
                    {
                        y = 0;
                    }

                    for (int i = 0; i < zVisitSampleDates.Count; i++)
                    {
                        if (x == zVisitSampleDates[i].Item1)
                        {
                            ZedGraph.LineItem hightlightDate = new ZedGraph.LineItem("Visit " + zVisitSampleDates[i].Item2.ToString(), new double[] { x }, new double[] { y }, Color.Red, SymbolType.Circle);
                            hightlightDate.Symbol.Size = 20;
                            hightlightDate.Symbol.Fill = new ZedGraph.Fill(Color.Transparent);
                            hightlightDate.Label.IsVisible = false;
                            //http://stackoverflow.com/questions/13763420/zedgraph-how-to-label-lineitem-directly-in-chart-using-textobj
                            ZedGraph.TextObj visitText = new ZedGraph.TextObj("Visit " + zVisitSampleDates[i].Item2.ToString(), x, y + 5, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                            visitText.ZOrder = ZedGraph.ZOrder.A_InFront;
                            visitText.FontSpec.Border.IsVisible = false;
                            visitText.FontSpec.Fill.IsVisible = false;
                            visitText.FontSpec.Angle = 45;
                            zPane.CurveList.Add(hightlightDate);
                            zPane.GraphObjList.Add(visitText);
                        }
                    }
                    zList.Add(x, y);
                }

                //Generate time series
                ZedGraph.CurveItem zCurve = zPane.AddCurve("Stream Flow", zList, Color.Blue, ZedGraph.SymbolType.None);
                zedGraphControl.IsShowPointValues = true;
                zedGraphControl.PointValueFormat = "0.00";
                zedGraphControl.PointDateFormat = "dd-MM-yyyy";


                //Configure figure            
                zedGraphControl.AxisChange();
                zedGraphControl.IsShowHScrollBar = true;
                zedGraphControl.IsShowVScrollBar = true;
            }
            else
            {
                //MessageBox.Show(String.Format("There is not data for USGS gage number {0}.", sSiteNumber),
                //                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Information);
            }

        }

    }
}
