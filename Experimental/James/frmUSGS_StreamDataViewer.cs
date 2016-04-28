using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization;

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
            this.msnChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.msnChart_GetToolTipText);
            //msnChart.GetToolTipText += new System.Windows.Forms.DataVisualization.Charting.ToolTipEventHandler(msnChart_GetToolTipText);

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

            if (cmbUSGS_Gage.SelectedItem == null && String.IsNullOrEmpty(txtUSGS_SiteNumber.Text) == true)
            {
                MessageBox.Show("Please select a USGS gage from the drop-down menu or enter it manually into the text box.",
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            if (cmbCHaMPSite.SelectedItem == null)
            {
                MessageBox.Show("Please select a CHaMP site from the drop-down menu.",
                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }

            //Check that provided gage id is valid
            bool bGageIDValid = m_USGS_StreamData.VerifyGageID(txtUSGS_SiteNumber.Text);

            if (bGageIDValid == true)
            {
                //Get the selected USGS Gage number
                int iGageID = Convert.ToInt32(txtUSGS_SiteNumber.Text);

                //Get the data
                List<StreamFlowSample> lStreamData = m_USGS_StreamData.GetUSGS_DischargeData(iGageID);
                if (lStreamData.Count > 1)
                {
                    string sSiteName = cmbCHaMPSite.SelectedItem.ToString();
                    //plot data
                    PlotStreamDataMicrosoftChart(m_dbCon, m_USGS_StreamData.StreamData, sSiteName, iGageID);
                }
                else
                {
                    //need to put something on the form that says this site has no preloaded gage, user must chose from drop-down
                    MessageBox.Show(String.Format("There is no flow data for USGS gage number {0}.", m_USGS_StreamData.GageNumber),
                    CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(String.Format("The gage number provided: {0} is either invalid or does not exist in the database.", txtUSGS_SiteNumber.Text),
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

        private void PlotStreamDataMicrosoftChart(OleDbConnection dbCon, List<StreamFlowSample> lStreamData, string sCHaMPSiteName, int iGageID)
        {
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
                sUSGS_Description = Convert.ToString(dbRead[0]);
            }
            dbRead.Close();

            //set auxillary info axis, visit labels, etc.
            System.Windows.Forms.DataVisualization.Charting.ChartArea pChartArea = msnChart.ChartAreas["ChartArea"];


            //Clear annotations and lines
            msnChart.Annotations.Clear();
            msnChart.Titles.Clear();
            msnChart.Series.Clear();
            foreach (var pSeries in msnChart.Series)
            {
                pSeries.Points.Clear();
            }

            //set and configure the title
            System.Windows.Forms.DataVisualization.Charting.Title pTitle = new System.Windows.Forms.DataVisualization.Charting.Title();
            pTitle.Name ="Title";
            pTitle.Text = String.Format("CHaMP Site: {0}{1} USGS Gage:{2}{1} {3}", sCHaMPSiteName, Environment.NewLine, iGageID.ToString(), sUSGS_Description);
            System.Drawing.Font pTitleFont = new System.Drawing.Font("Verdana", 12f, FontStyle.Bold);
            pTitle.Font = pTitleFont;
            pTitle.DockedToChartArea = "ChartArea";
            pTitle.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            pTitle.IsDockedInsideChartArea = false;
            msnChart.Titles.Add(pTitle);

            //set and configure the x-axis
            pChartArea.AxisX.LabelStyle.Format = "MM-yyyy";
            pChartArea.AxisX.Interval = 4;
            pChartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
            System.Drawing.Font pAxisFont = new System.Drawing.Font("Verdana", 10f, FontStyle.Bold);
            pChartArea.AxisX.TitleFont = pAxisFont;
            pChartArea.AxisX.LabelStyle.Angle = 45;
            pChartArea.AxisX.Title = "Date";
            DateTime minDate = lStreamData.Min(d => d.Date.Date);
            pChartArea.AxisX.Minimum = minDate.ToOADate();

            DateTime maxDate = lStreamData.Max(d => d.Date.Date);
            pChartArea.AxisX.Maximum = maxDate.ToOADate();


            //set and configure the y-axis
            pChartArea.AxisY.Title = "Flow (cubic ft/sec)";
            pChartArea.AxisY.TitleFont = pAxisFont;

            //Add the survey dates for the site in question
            sGroupFields = " V.VisitID, V.SiteID, V.SampleDate, S.SiteID, S.SiteName";
            sCHaMPSiteName = String.Format("\"{0}\"", sCHaMPSiteName);
            sSQL = "SELECT " + sGroupFields +
                 " FROM (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) " +
                 " WHERE S.SiteName = " + sCHaMPSiteName +
                 " ORDER BY V.SampleDate";

            comFS = new OleDbCommand(sSQL, dbCon);
            dbRead = comFS.ExecuteReader();


            System.Windows.Forms.DataVisualization.Charting.Series pVisitsSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Visits");
            pVisitsSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            pVisitsSeries.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            pVisitsSeries.MarkerSize = 16;
            pVisitsSeries.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            pVisitsSeries.MarkerColor = Color.Transparent;
            pVisitsSeries.MarkerBorderColor = Color.Red;
            pVisitsSeries.SmartLabelStyle.Enabled = false;
            //pVisitsSeries.ToolTip = "#LABEL" + Environment.NewLine + "Date: #VALX";

            TupleList<DateTime, int> pVisitSampleDates = new TupleList<DateTime, int>();
            while (dbRead.Read())
            {
                DateTime pSampleDate = Convert.ToDateTime(dbRead[2]);
                int iVisitID = Convert.ToInt32(dbRead[0]);
                pVisitSampleDates.Add(pSampleDate, iVisitID);
            }
            dbRead.Close();

            

            if (lStreamData.Count > 0)
            {

                //create hydrograph series and set key properties
                System.Windows.Forms.DataVisualization.Charting.Series pHydrographSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Hydrograph");
                pHydrographSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                pHydrographSeries.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                pHydrographSeries.Color = Color.Blue;
                pHydrographSeries.BorderWidth = 2;
                //pHydrographSeries.ToolTip = "Flow: #VALY" + Environment.NewLine + "Date: #VALX";

                foreach (StreamFlowSample sample in lStreamData)
                {
                    DateTime x = sample.Date;

                    double y = sample.Flow;
                    if (y < 0)
                    {
                        y = 0;
                    }

                    for (int i = 0; i < pVisitSampleDates.Count; i++)
                    {
                        if (x == pVisitSampleDates[i].Item1)
                        {
                            //create visit data point and set key properties
                            System.Windows.Forms.DataVisualization.Charting.DataPoint pVisitPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(pVisitSampleDates[i].Item1.ToOADate(), y);
                            pVisitPoint.Label = "Visit " + pVisitSampleDates[i].Item2.ToString();
                            pVisitPoint.LabelAngle = -90;
                            //pVisitPoint.ToolTip = "#LABEL" + Environment.NewLine + "Date: #VALX";

                            pVisitsSeries.Points.Add(pVisitPoint);
                        }
                    }

                    //pHydrographSeries.Points.AddXY(x, y);
                    System.Windows.Forms.DataVisualization.Charting.DataPoint pHydrographPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(x.ToOADate(), y);
                    //pHydrographPoint.Label = x.ToString("dd-MM-yyyy");
                    //pHydrographPoint.ToolTip = "Flow: #VALY" + Environment.NewLine + "Date: #VALX";
                    pHydrographSeries.Points.Add(pHydrographPoint);
                }

                msnChart.Series.Add(pHydrographSeries);
                msnChart.Series.Add(pVisitsSeries);

                //adjust legend
                System.Windows.Forms.DataVisualization.Charting.Legend pLegend = msnChart.Legends["Legend"];
                pLegend.AutoFitMinFontSize = 12;
                pLegend.DockedToChartArea = "ChartArea";
                pLegend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
                pLegend.IsDockedInsideChartArea = true;

                //set chart size
                pChartArea.Position.Y = pChartArea.Position.Bottom;
                pChartArea.Position.Height = 80;
                pChartArea.Position.Width = 99;

                //enable scroll and zoom
                pChartArea.CursorX.IsUserEnabled = true;
                pChartArea.CursorX.IsUserSelectionEnabled = true;
                pChartArea.CursorY.IsUserEnabled = true;
                pChartArea.CursorY.IsUserSelectionEnabled = true;

            }
            else
            {
                //MessageBox.Show(String.Format("There is not data for USGS gage number {0}.", sSiteNumber),
                //                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Information);

            }

        }

        private void msnChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cmsFigureOptions.Show(Cursor.Position);
            }
        }

        private void miSaveImage_Click(object sender, EventArgs e)
        {
            //TODO:save image
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Save Figure";
            frm.Filter = "PNG Files (*.png)|*.png";

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.IO.FileInfo fiExport = new System.IO.FileInfo(frm.FileName);
                    if (String.IsNullOrEmpty(frm.FileName) == false)
                    {
                        msnChart.SaveImage(frm.FileName, System.Drawing.Imaging.ImageFormat.Png);

                        if (MessageBox.Show("Do you want to browse to the folder containing the file created?", "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(fiExport.Directory.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
            
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            msnChart.ChartAreas["ChartArea"].AxisX.ScaleView.ZoomReset(0);
            msnChart.ChartAreas["ChartArea"].AxisY.ScaleView.ZoomReset(0);
        }



        private void msnChart_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint:
                    if (msnChart.Series.IndexOf("Hydrograph") != -1)
                    {
                        System.Windows.Forms.DataVisualization.Charting.DataPoint pPoint = msnChart.Series["Hydrograph"].Points[e.HitTestResult.PointIndex];
                        e.Text = String.Format("Flow: {0}{1}Date: {2}", pPoint.YValues[0].ToString("N2"),
                                                                        Environment.NewLine,
                                                                        DateTime.FromOADate(pPoint.XValue).ToString("dd-MM-yyyy"));
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
