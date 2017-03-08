using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Windows.Forms.DataVisualization;

namespace CHaMPWorkbench.Experimental.James
{
    public partial class frmUSGS_StreamDataViewer : Form
    {
        private string DBConnection;

        private int m_iGageID;
        private USGS_StreamData m_USGS_StreamData;

        private long m_nInitialWatershedID;
        private long m_nInitialSiteID;

        public frmUSGS_StreamDataViewer(string sDBCon, long nSiteID, long nWatershedID)
        {
            InitializeComponent();
            this.msnChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.msnChart_GetToolTipText);
            //msnChart.GetToolTipText += new System.Windows.Forms.DataVisualization.Charting.ToolTipEventHandler(msnChart_GetToolTipText);

            DBConnection = sDBCon;
            m_nInitialSiteID = nSiteID;
            m_nInitialWatershedID = nWatershedID;

            m_USGS_StreamData = new USGS_StreamData(sDBCon, nSiteID);
            SetStreamGageBasedOnSite(m_USGS_StreamData);
        }

        private void frmUSGS_StreamDataViewer_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            // Use the public shared method to fill a combo box with ListItems (a simple custom class of string and IDs)
           naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cmbWatershed, DBConnection, "SELECT WatershedID, WatershedName FROM CHaMP_Watersheds ORDER BY WatershedName", m_nInitialWatershedID);

            // Fill the CHaMP sites. These will be filtered by the currently selected watershed and pre-select the current site.
            LoadCHaMPSiteCombo(m_nInitialSiteID);

            // Hook up the event that will cause the site combo to filter when the current watershed changes
            // Note: Only hook this up after the watershed and site have been selected the first time.
            cmbWatershed.SelectedIndexChanged += WatershedComboChanged;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
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
                long iGageID = Convert.ToInt64(txtUSGS_SiteNumber.Text);

                //Get the data
                List<StreamFlowSample> lStreamData = m_USGS_StreamData.GetUSGS_DischargeData(iGageID);
                if (lStreamData.Count > 1)
                {
                    naru.db.NamedObject aSite = cmbCHaMPSite.SelectedItem as naru.db.NamedObject;
                    //plot data
                    PlotStreamDataMicrosoftChart(m_USGS_StreamData.StreamData, aSite.ID, aSite.Name, iGageID);
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

  public class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }

        public Coordinate GetCoordinate(long nSiteID)
        {
            var coordinate = new Coordinate(0, 0);
            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnection))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT Latitude, Longitude FROM CHaMP_Sites WHERE (SiteID = @SiteID) AND (Latitude IS NOT NULL) AND (Longitude IS NOT NULL)", dbCon);
                dbCom.Parameters.Add(new SQLiteParameter("@SiteID", nSiteID));
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    coordinate = new Coordinate(Convert.ToDouble(dbRead[0]), Convert.ToDouble(dbRead[1]));
                }
                dbRead.Close();
            }
            return coordinate;
        }

        private void cmbCHaMPSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbUSGS_Gage.Items.Clear();

            if (cmbCHaMPSite.SelectedItem is naru.db.NamedObject)
            {
                Coordinate champCoordinate = GetCoordinate(((naru.db.NamedObject)cmbCHaMPSite.SelectedItem).ID);
                List<KeyValuePair<string, double>> lUSGS_GageSites = new List<KeyValuePair<string, double>>();

                using (SQLiteConnection dbCon = new SQLiteConnection(DBConnection))
                {
                    dbCon.Open();

                    SQLiteCommand dbCom = new SQLiteCommand ("SELECT GageID, Latitude, Longitude FROM USGS_Gages ORDER BY GageID", dbCon);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();

                    while (dbRead.Read())
                    {
                        string usgsGageNumber = dbRead[0].ToString();
                        Coordinate usgsCoordinate = new Coordinate(Convert.ToDouble(dbRead[1]), Convert.ToDouble(dbRead[2]));
                        double distance = Math.Round(Coordinate.Distance(champCoordinate, usgsCoordinate, Coordinate.UnitsOfLength.Kilometer), 2);
                        Coordinate.CardinalPoints compass = Coordinate.Bearing(champCoordinate, usgsCoordinate).ToCardinalMark();
                        lUSGS_GageSites.Add(new KeyValuePair<string, double>(String.Format("{0}; Distance: {1} Direction: {2}", usgsGageNumber, distance, compass), distance));
                    }
                    dbRead.Close();
                }

                //sort in ascending order by distance from champ site 
                var lUSGS_Sorted = lUSGS_GageSites.OrderBy(site => site.Value);
                foreach (KeyValuePair<string, double> usgsGage in lUSGS_Sorted)
                {
                    cmbUSGS_Gage.Items.Add(usgsGage.Key);
                }

                //Check if there is a pre-selected USGS gage, if so select that
                if (m_USGS_StreamData != null)
                {
                    m_USGS_StreamData.CheckCHaMP_SiteForAssociatedGage((cmbCHaMPSite.SelectedItem as naru.db.NamedObject).ID);
                    SetStreamGageBasedOnSite(m_USGS_StreamData);
                }
            }
        }

        private void SetStreamGageBasedOnSite(USGS_StreamData pUSGS_StreamData)
        {
            if (m_USGS_StreamData.SiteHasGageID == true)
            {
                //Set selected gage to the site gage id
                bool bFoundGage = false;
                char splitCharacter = (char)59;
                for (int i = 0; i < cmbUSGS_Gage.Items.Count; i++)
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

        private void PlotStreamDataMicrosoftChart(List<StreamFlowSample> lStreamData, long nSiteID, string sCHaMPSiteName, long iGageID)
        {
            string sUSGS_Description = string.Empty;
            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnection))
            {
                dbCon.Open();

                SQLiteCommand comFS = new SQLiteCommand("SELECT Description FROM USGS_Gages WHERE GageID = @GageID", dbCon);
                comFS.Parameters.AddWithValue("@GageID", iGageID);
                SQLiteDataReader dbRead = comFS.ExecuteReader();

                while (dbRead.Read())
                {
                    sUSGS_Description = Convert.ToString(dbRead[0]);
                }
                dbRead.Close();
            }

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
            pTitle.Name = "Title";
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
            TupleList<DateTime, long> pVisitSampleDates = new TupleList<DateTime, long>();
            System.Windows.Forms.DataVisualization.Charting.Series pVisitsSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Visits");
            pVisitsSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            pVisitsSeries.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            pVisitsSeries.MarkerSize = 16;
            pVisitsSeries.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            pVisitsSeries.MarkerColor = Color.Transparent;
            pVisitsSeries.MarkerBorderColor = Color.Red;
            pVisitsSeries.SmartLabelStyle.Enabled = false;
            //pVisitsSeries.ToolTip = "#LABEL" + Environment.NewLine + "Date: #VALX";

            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnection))
            {
                dbCon.Open();

                SQLiteCommand comFS = new SQLiteCommand ("SELECT VisitID, SampleDate FROM CHaMP_Visits WHERE SiteID = @SiteID ORDER BY SampleDate", dbCon);
                comFS.Parameters.AddWithValue("@SiteID", nSiteID);
                SQLiteDataReader dbRead = comFS.ExecuteReader();
                
                while (dbRead.Read())
                {
                    DateTime pSampleDate = Convert.ToDateTime(dbRead[1]);
                    long iVisitID = Convert.ToInt64(dbRead[0]);
                    pVisitSampleDates.Add(pSampleDate, iVisitID);
                }
                dbRead.Close();
            }

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


        /// <summary>
        /// Fill the sites combo box based on the selected watershed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatershedComboChanged(object sender, EventArgs e)
        {
            LoadCHaMPSiteCombo();
        }

        private void LoadCHaMPSiteCombo(long nSelectedSiteID = 0)
        {
            cmbCHaMPSite.Items.Clear();
            if (cmbWatershed.SelectedItem is naru.db.NamedObject)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                using (SQLiteConnection dbCon = new SQLiteConnection(DBConnection))
                {
                    dbCon.Open();
                    SQLiteCommand dbCom = new SQLiteCommand("SELECT SiteID, SiteName FROM CHaMP_Sites WHERE WatershedID = @WatershedID", dbCon);
                    dbCom.Parameters.AddWithValue("@WatershedID", ((naru.db.NamedObject)cmbWatershed.SelectedItem).ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        long nSiteID = dbRead.GetInt64(dbRead.GetOrdinal("SiteID"));
                        int nIndex = cmbCHaMPSite.Items.Add(new naru.db.NamedObject(nSiteID, dbRead.GetString(dbRead.GetOrdinal("SiteName"))));
                        if (nSiteID == nSelectedSiteID)
                            cmbCHaMPSite.SelectedIndex = nIndex;
                    }
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://waterdata.usgs.gov/nwis");
        }
    }
}
