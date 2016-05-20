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
    public partial class frmGCD_MetricsViewer : Form
    {
        private string DBConnection;
        private DataTable m_dtGCD_Results;

        //database table variables
        const int m_iFirstGCDMetricIndex = 17;

        //charting variables
        System.Windows.Forms.DataVisualization.Charting.ChartArea m_pChartArea;
        const string m_sSeriesName = "GCD Analysis";

        public frmGCD_MetricsViewer(string sDBCon)
        {
            InitializeComponent();
            this.msnChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.msnChart_GetToolTipText);
            //msnChart.GetToolTipText += new System.Windows.Forms.DataVisualization.Charting.ToolTipEventHandler(msnChart_GetToolTipText);

            m_pChartArea = msnChart.ChartAreas["ChartArea"];

            DBConnection = sDBCon;

        }

        private void frmGCD_AnalysisWatershed_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            // Fill the CHaMP sites. These will be filtered by the currently selected watershed and pre-select the current site.
            LoadCHaMPSiteCombo();
            

            // Hook up the event that will cause the site combo to filter when the current watershed changes
            // Note: Only hook this up after the watershed and site have been selected the first time.
            cmbWatershed.SelectedIndexChanged += WatershedComboChanged;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            using (OleDbConnection dbCon = new OleDbConnection(DBConnection))
            {
                dbCon.Open();
                try
                {

                    string sGroupFields = @"V.SiteID,S.SiteName,W.WatershedID,W.WatershedName,CD.NewVisitID,V.SampleDate AS NewSampleDate,CD.NewFieldSeason,CD.OldVisitID,V2.SampleDate As OldSampleDate,
		                    CD.OldFieldSeason,V.PanelName As PanelName,V.CategoryName As CategoryName,CD.Epoch,CD.ThresholdType,CD.Threshold,CD.SpatialCoherence,MBSV.MaskValueName,
		                    MBSV.RawAreaErosion,MBSV.RawAreaDeposition,MBSV.ThresholdAreaErosion,MBSV.ThresholdAreaDeposition,MBSV.RawVolumeErosion,MBSV.RawVolumeDeposition,MBSV.ThresholdVolumeErosion,
		                    MBSV.ThresholdVolumeDeposition,MBSV.RawVolumeDifference,MBSV.ThresholdPercentErosion,MBSV.ThresholdPercentDeposition,MBSV.ErrorVolumeErosion,MBSV.ErrorVolumeDeposition,
		                    MBSV.ErrorVolumeDifference,MBSV.AreaDetectableChange,MBSV.AreaOfInterestRaw,MBSV.PercentAreaOfInterestDetectableChange,MBSV.ThresholdedVolumeDifference,MBSV.VolumeDifferencePercent,
		                    MBSV.AverageDepthErosionRaw,MBSV.AverageDepthErosionThreshold,MBSV.AverageDepthErosionError,MBSV.AverageDepthErosionpercent,MBSV.AverageDepthDepositionRaw,	MBSV.AverageDepthDepositionThreshold,
		                    MBSV.AverageDepthDepositionError,MBSV.AverageDepthDepositionpercent,MBSV.AverageThicknessDifferenceAOIRaw,MBSV.AverageThicknessDifferenceAOIThresholded,MBSV.AverageThicknessDifferenceAOIError,
		                    MBSV.AverageThicknessDifferenceAOIpercent,MBSV.AverageNetThicknessDifferenceAOIRaw,MBSV.AverageNetThicknessDifferenceAOIThresholded,MBSV.AverageNetThicknessDifferenceAOIError,
		                    MBSV.AverageNetThicknessDifferenceAOIpercent,MBSV.AverageNetThicknessDifferenceADCError,MBSV.AverageNetThicknessDifferenceADCThresholded,MBSV.AverageNetThicknessDifferenceADCpercent,
		                    MBSV.AverageThicknessDifferenceADCError,MBSV.AverageThicknessDifferenceADCThresholded,MBSV.AverageThicknessDifferenceADCpercent,MBSV.PercentErosionRaw,MBSV.PercentErosionThresholded,
		                    MBSV.PercentDepositionRaw,MBSV.PercentDepositionThresholded,MBSV.PercentImbalanceRaw,MBSV.PercentImbalanceThresholded,MBSV.PercentNetVolumeRatioRaw,MBSV.PercentNetVolumeRatioThresholded";

                    string sSQL = "SELECT " + sGroupFields +
                        @" FROM (((((
	                           CHAMP_Visits AS V 
	                           INNER JOIN 
	                           Metric_ChangeDetection As CD 
	                           ON V.VisitID = CD.NewVisitID)
	                           INNER JOIN CHAMP_Visits AS V2
	                           ON CD.OldVisitID = V2.VisitID)
	                           INNER JOIN
	                           CHAMP_Sites AS S 
	                           ON V.SiteID = S.SiteID)
	                           INNER JOIN
	                           CHAMP_Watersheds AS W 
	                           ON S.WatershedID = W.WatershedID)
	                           INNER JOIN
	                           Metric_BudgetSegregations AS MBS
	                           ON CD.ChangeDetectionID = MBS.ChangeDetectionID)
	                           INNER JOIN
	                           Metric_BudgetSegregationValues AS MBSV
	                           ON MBS.BudgetID = MBSV.BudgetID";

                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);                
                    OleDbDataReader dbRead = dbCom.ExecuteReader();

                    m_dtGCD_Results = new DataTable();
                    m_dtGCD_Results.Load(dbRead);
                    dbRead.Close();

                    if (m_dtGCD_Results.Rows.Count > 0)
                    {
                        //only load watersheds that have gcd metrics
                        var pWatershedsWithData = m_dtGCD_Results.AsEnumerable().Select(row => new
                        {
                            WatershedID = row.Field<int>("WatershedID"),
                            WatershedName = row.Field<string>("WatershedName")
                        }).Distinct();
                        cmbWatershed.Items.Insert(0, new ListItem("", 0));
                        foreach (var pWatershed in pWatershedsWithData)
                        {
                            cmbWatershed.Items.Add(new ListItem(pWatershed.WatershedName, pWatershed.WatershedID));
                        }

                        //load x and y axis combo box with gcd metric parameters
                        int iColumnIndex = 0;
                        foreach (DataColumn column in m_dtGCD_Results.Columns)
                        {
                            if (iColumnIndex >= m_iFirstGCDMetricIndex)
                            {
                                cmbXaxis.Items.Add(column.ColumnName);
                                cmbYaxis.Items.Add(column.ColumnName);
                            }
                            iColumnIndex += 1;
                        }

                        //load mask combo box
                        var pMaskNames = (from DataRow row in m_dtGCD_Results.Rows
                                          select (string)row["MaskValueName"]).Distinct();
                        cmbMask.Items.Add("");
                        foreach (string sMaskName in pMaskNames)
                        {
                            cmbMask.Items.Add(sMaskName);
                        }

                        //load years combo box
                        int iMaxYear = m_dtGCD_Results.AsEnumerable().Max(row => (int)row["NewFieldSeason"]);
                        int iMinYear = m_dtGCD_Results.AsEnumerable().Min(row => (int)row["OldFieldSeason"]);
                        cmbNewYear.Items.Add("");
                        cmbOldYear.Items.Add("");
                        while (iMaxYear >= iMinYear)
                        {
                            cmbNewYear.Items.Add(iMaxYear.ToString());
                            cmbOldYear.Items.Add(iMaxYear.ToString());
                            iMaxYear -= 1;
                        }

                        this.cmbNewYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
                        this.cmbOldYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);

                        iMaxYear = m_dtGCD_Results.AsEnumerable().Max(row => (int)row["NewFieldSeason"]);
                        //load interval combo box
                        cmbInterval.Items.Add("");
                        while (iMaxYear != iMinYear)
                        {
                            int iInterval = iMaxYear - iMinYear;
                            cmbInterval.Items.Add(iInterval);
                            iMaxYear -= 1;
                        }
                    }
                    else
                    {
                        MessageBox.Show(String.Format("There are not gcd results in the connected database.{0}{0}Please load data via the RBT Scavanger tool to in order to utilize this tool.", Environment.NewLine),
                                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private System.Windows.Forms.DataVisualization.Charting.Series PlotData(string sXaxisParameter, string sYaxisParameter, int iWatershedID, string sMaskName = "", int iInterval = 0, int iNewYear = 0, int iOldYear = 0)
        {

            //Clear annotations and lines
            msnChart.Annotations.Clear();
            msnChart.Titles.Clear();
            msnChart.Series.Clear();
            foreach (var pSeries in msnChart.Series)
            {
                pSeries.Points.Clear();
            }

            //todo enable highligting the selected site
            ////Add the survey dates for the site in question
            //TupleList<DateTime, int> pVisitSampleDates = new TupleList<DateTime, int>();
            //System.Windows.Forms.DataVisualization.Charting.Series pVisitsSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Visits");
            //pVisitsSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            //pVisitsSeries.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            //pVisitsSeries.MarkerSize = 16;
            //pVisitsSeries.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            //pVisitsSeries.MarkerColor = Color.Transparent;
            //pVisitsSeries.MarkerBorderColor = Color.Red;
            //pVisitsSeries.SmartLabelStyle.Enabled = false;
            //pVisitsSeries.ToolTip = "#LABEL" + Environment.NewLine + "Date: #VALX";

            DataRow[] results = m_dtGCD_Results.Select();
            Console.Write(String.Format("Total Count: {0}{1}", results.Length, Environment.NewLine));
            if (iWatershedID != 0)
            {
                results = (from DataRow row in results
                           where (int)row["WatershedID"] == iWatershedID
                           select row).ToArray();
            }
            Console.Write(String.Format("Watershed Count: {0}{1}", results.Length, Environment.NewLine));
            if (String.IsNullOrEmpty(sMaskName) == false)
            {
                results = (from DataRow row in results
                           where (string)row["MaskValueName"] == sMaskName
                           select row).ToArray();
            }
            Console.Write(String.Format("Mask Count: {0}{1}", results.Length, Environment.NewLine));
            if (iInterval != 0)
            {
                results = (from DataRow row in results
                           where ((int)row["NewFieldSeason"] - (int)row["OldFieldSeason"]) == iInterval
                           select row).ToArray();
            }
            Console.Write(String.Format("Interval Count: {0}{1}", results.Length, Environment.NewLine));
            if (iNewYear != 0 && iOldYear != 0)
            {
                results = (from DataRow row in results
                           where (int)row["NewFieldSeason"] == iNewYear && (int)row["OldFieldSeason"] == iOldYear
                           select row).ToArray();
            }
            Console.Write(String.Format("Field Season Count: {0}{1}", results.Length, Environment.NewLine));

            System.Windows.Forms.DataVisualization.Charting.Series pGCDSeries = new System.Windows.Forms.DataVisualization.Charting.Series();

            if (results.Length > 0)
            {

                //set and configure the title
                System.Windows.Forms.DataVisualization.Charting.Title pTitle = new System.Windows.Forms.DataVisualization.Charting.Title();
                pTitle.Name = "Title";
                //pTitle.Text = String.Format("Watershed: {0}", sWatershedName);
                System.Drawing.Font pTitleFont = new System.Drawing.Font("Verdana", 12f, FontStyle.Bold);
                pTitle.Font = pTitleFont;
                pTitle.DockedToChartArea = "ChartArea";
                pTitle.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
                pTitle.IsDockedInsideChartArea = false;
                msnChart.Titles.Add(pTitle);

                //set min max of x axis
                m_pChartArea.AxisX.Minimum = 0;
                double dMaxX = results.Max(row => (double)row[sXaxisParameter]);
                m_pChartArea.AxisX.Maximum = dMaxX + (dMaxX * .1);

                //set and configure the x-axis labeling
                m_pChartArea.AxisX.LabelStyle.Format = "{0.00}";
                m_pChartArea.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
                System.Drawing.Font pAxisFont = new System.Drawing.Font("Verdana", 10f, FontStyle.Bold);
                m_pChartArea.AxisX.TitleFont = pAxisFont;
                m_pChartArea.AxisX.Title = sXaxisParameter;



                //set min max of y axis
                m_pChartArea.AxisY.Minimum = 0;
                double dMaxY = results.Max(row => (double)row[sYaxisParameter]);
                m_pChartArea.AxisY.Maximum = dMaxY + (dMaxY * .1);

                //set and configure the y-axis
                m_pChartArea.AxisY.LabelStyle.Format = "{0.00}";
                m_pChartArea.AxisY.Title = sYaxisParameter;
                m_pChartArea.AxisY.TitleFont = pAxisFont;
                m_pChartArea.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;

                //create hydrograph series and set key properties
                pGCDSeries = new System.Windows.Forms.DataVisualization.Charting.Series(m_sSeriesName);
                pGCDSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                pGCDSeries.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
                pGCDSeries.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                pGCDSeries.Color = Color.Blue;

                foreach (DataRow row in results)
                {
                    double x = (double)row[sXaxisParameter];
                    double y = (double)row[sYaxisParameter];

                    System.Windows.Forms.DataVisualization.Charting.DataPoint pGCDPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(x, y);
                    pGCDPoint.SetCustomProperty("SiteID", row["SiteID"].ToString());
                    pGCDPoint.SetCustomProperty("SiteName", row["SiteName"].ToString());
                    pGCDPoint.SetCustomProperty("NewVisitID", row["NewVisitID"].ToString());
                    pGCDPoint.SetCustomProperty("NewSampleDate", row["NewSampleDate"].ToString()); 
                    pGCDPoint.SetCustomProperty("OldVisitID", row["OldVisitID"].ToString());
                    pGCDPoint.SetCustomProperty("OldSampleDate", row["OldSampleDate"].ToString());
                    pGCDPoint.SetCustomProperty("MaskValueName", row["MaskValueName"].ToString());

                    pGCDSeries.Points.Add(pGCDPoint);
                }

                msnChart.Series.Add(pGCDSeries);

                //adjust legend
                System.Windows.Forms.DataVisualization.Charting.Legend pLegend = msnChart.Legends["Legend"];
                pLegend.AutoFitMinFontSize = 12;
                pLegend.DockedToChartArea = "ChartArea";
                pLegend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
                pLegend.IsDockedInsideChartArea = true;

                //set chart size
                m_pChartArea.Position.Y = m_pChartArea.Position.Bottom;
                m_pChartArea.Position.Height = 95;
                m_pChartArea.Position.Width = 99;

                //enable scroll and zoom
                m_pChartArea.CursorX.IsUserEnabled = true;
                m_pChartArea.CursorX.IsUserSelectionEnabled = true;
                m_pChartArea.CursorY.IsUserEnabled = true;
                m_pChartArea.CursorY.IsUserSelectionEnabled = true;

            }
            else
            {
                MessageBox.Show(String.Format("There are not gcd results in the connected database for the current query.{0}{0}Please review the entered parameters.", Environment.NewLine),
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

            return pGCDSeries;
        }

        private void msnChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cmsFigureOptions.Show(Cursor.Position);
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Windows.Forms.DataVisualization.Charting.HitTestResult pHitTest = this.msnChart.HitTest(e.X, e.Y);


                //switch (pHitTest.ChartElementType)
                //{
                //    case System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint:
                //        if (msnChart.Series.IndexOf(m_sSeriesName) != -1)
                //        {
                //            System.Windows.Forms.DataVisualization.Charting.DataPoint pPoint = msnChart.Series[m_sSeriesName].Points[pHitTest.PointIndex];
                //            string sSiteName = pPoint.GetCustomProperty("SiteName");
                //            string sNewVisitID = pPoint.GetCustomProperty("NewVisitID");
                //            string sNewSampleDate = pPoint.GetCustomProperty("NewSampleDate");
                //            sNewSampleDate = DateTime.Parse(sNewSampleDate).ToString("MM/dd/yyyy");
                //            string sOldVisitID = pPoint.GetCustomProperty("OldVisitID");
                //            string sOldSampleDate = pPoint.GetCustomProperty("OldSampleDate");
                //            sOldSampleDate = DateTime.Parse(sOldSampleDate).ToString("MM/dd/yyyy");
                            
                //        }
                //        break;
                //    default:
                //        break;
                //}


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
            m_pChartArea.AxisX.ScaleView.ZoomReset(0);
            m_pChartArea.AxisY.ScaleView.ZoomReset(0);
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

        private void LoadCHaMPSiteCombo(int nSelectedSiteID = 0)
        {
            cmbSite.Items.Clear();
            if (cmbWatershed.SelectedItem is ListItem)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                using (OleDbConnection dbCon = new OleDbConnection(DBConnection))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = new OleDbCommand("SELECT SiteID, SiteName FROM CHaMP_Sites WHERE WatershedID = @WatershedID", dbCon);
                    dbCom.Parameters.AddWithValue("@WatershedID", ((ListItem)cmbWatershed.SelectedItem).Value);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    cmbSite.Items.Insert(0, "");
                    while (dbRead.Read())
                    {
                        int nSiteID = dbRead.GetInt32(dbRead.GetOrdinal("SiteID"));
                        int nIndex = cmbSite.Items.Add(new ListItem(dbRead.GetString(dbRead.GetOrdinal("SiteName")), nSiteID));
                        if (nSiteID == nSelectedSiteID)
                            cmbSite.SelectedIndex = nIndex;
                    }
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        private void cmdGetData_Click(object sender, EventArgs e)
        {
            //Check that a watershed, x-axis parameter, y-axis parameter have all been selected
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            if (cmbXaxis.SelectedItem == null || cmbYaxis.SelectedItem == null)
            {
                MessageBox.Show("Please select a parameter for both axis viat the drop-down menus.",
                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }

            if ((cmbNewYear.SelectedItem == "" && cmbOldYear.SelectedItem != "") || (cmbNewYear.SelectedItem != "" && cmbOldYear.SelectedItem == ""))
            {
                MessageBox.Show("Please select a parameter for both timespan parameters in timespan the drop-down menus.",
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
               return;
            }

            int iWatershedID = 0;
            if (cmbWatershed.SelectedItem != null)
            {
                if (cmbWatershed.SelectedItem is ListItem)
                {
                    ListItem pSelectedWatershed = (ListItem)cmbWatershed.SelectedItem;
                    iWatershedID = pSelectedWatershed.Value;
                }
            }
            string sXaxisParameter = cmbXaxis.GetItemText(cmbXaxis.SelectedItem);
            string sYaxisParameter = cmbYaxis.GetItemText(cmbYaxis.SelectedItem);
            string sMaskName = cmbMask.GetItemText(cmbMask.SelectedItem);
            int iInterval = 0;
            if (cmbInterval.GetItemText(cmbInterval.SelectedItem) != "" && cmbInterval.GetItemText(cmbInterval.SelectedItem) != null)
            {
                iInterval = Convert.ToInt32(cmbInterval.GetItemText(cmbInterval.SelectedItem));
            }
            int iNewYear = 0;
            if (cmbNewYear.GetItemText(cmbNewYear.SelectedItem) != "" && cmbNewYear.GetItemText(cmbNewYear.SelectedItem) != null)
            {
                iNewYear = Convert.ToInt32(cmbNewYear.GetItemText(cmbNewYear.SelectedItem));
            }
            int iOldYear = 0;
            if (cmbOldYear.GetItemText(cmbOldYear.SelectedItem) != "" && cmbOldYear.GetItemText(cmbOldYear.SelectedItem) != null)
            {
                iOldYear = Convert.ToInt32(cmbOldYear.GetItemText(cmbOldYear.SelectedItem));
            }

            System.Windows.Forms.DataVisualization.Charting.Series pGCD_Series = PlotData(sXaxisParameter, sYaxisParameter, iWatershedID, sMaskName, iInterval, iNewYear, iOldYear);
            if (pGCD_Series.Points.Count > 0)
            {
                if (chkHighlightSite.Checked == true)
                {
                    if (cmbSite.SelectedItem != null)
                    {
                        if (cmbSite.SelectedItem is ListItem)
                        {
                            ListItem pSelectedSite = (ListItem)cmbSite.SelectedItem;
                            int iHighlightSiteID = pSelectedSite.Value;

                            dgvVisits.Rows.Clear();
                            foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint pPoint in pGCD_Series.Points)
                            {
                                int iPointSiteID = Convert.ToInt32(pPoint.GetCustomProperty("SiteID"));
                                if (iPointSiteID == iHighlightSiteID)
                                {
                                    pPoint.MarkerColor = Color.Red;
                                    pPoint.MarkerSize = 12;
                                    dgvVisits.Rows.Add();
                                    dgvVisits.Rows[dgvVisits.Rows.Count - 1].Cells["NewVisitID"].Value = pPoint.GetCustomProperty("NewVisitID");
                                    string sNewSampleDate = pPoint.GetCustomProperty("NewSampleDate");
                                    sNewSampleDate = DateTime.Parse(sNewSampleDate).ToString("MM/dd/yyyy");
                                    dgvVisits.Rows[dgvVisits.Rows.Count - 1].Cells["NewSampleDate"].Value = sNewSampleDate;
                                    dgvVisits.Rows[dgvVisits.Rows.Count - 1].Cells["OldVisitID"].Value = pPoint.GetCustomProperty("OldVisitID");
                                    string sOldSampleDate = pPoint.GetCustomProperty("OldSampleDate");
                                    sOldSampleDate = DateTime.Parse(sOldSampleDate).ToString("MM/dd/yyyy");
                                    dgvVisits.Rows[dgvVisits.Rows.Count - 1].Cells["OldSampleDate"].Value = sOldSampleDate;
                                    dgvVisits.Rows[dgvVisits.Rows.Count - 1].Cells["MaskValueName"].Value = pPoint.GetCustomProperty("MaskValueName");

                                }
                            }
                        }
                    }
                }
            }
        }

        private void cmbCHaMPSite_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void msnChart_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint:
                    if (msnChart.Series.IndexOf(m_sSeriesName) != -1)
                    {
                        System.Windows.Forms.DataVisualization.Charting.DataPoint pPoint = msnChart.Series[m_sSeriesName].Points[e.HitTestResult.PointIndex];
                        string sSiteName = pPoint.GetCustomProperty("SiteName");
                        string sNewVisitID = pPoint.GetCustomProperty("NewVisitID");
                        string sNewSampleDate = pPoint.GetCustomProperty("NewSampleDate");
                        sNewSampleDate = DateTime.Parse(sNewSampleDate).ToString("MM/dd/yyyy");
                        string sOldVisitID = pPoint.GetCustomProperty("OldVisitID");
                        string sOldSampleDate = pPoint.GetCustomProperty("OldSampleDate");
                        sOldSampleDate = DateTime.Parse(sOldSampleDate).ToString("MM/dd/yyyy");
                        string sMaskValue = pPoint.GetCustomProperty("MaskValueName");
                        e.Text = String.Format("{0} {1}New Visit ID: {2} Sample Date: {3}{1}Old Visit ID: {4} Sample Date: {5}{1}Mask: {6}", sSiteName,
                                                                                                                                              Environment.NewLine,
                                                                                                                                              sNewVisitID,
                                                                                                                                              sNewSampleDate,
                                                                                                                                              sOldVisitID,
                                                                                                                                              sOldSampleDate,
                                                                                                                                              sMaskValue);
                    }
                    break;
                default:
                    break;
            }
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cmbNewYear.SelectedItem != "" && cmbNewYear.SelectedItem != null) && (cmbOldYear.SelectedItem != "" && cmbOldYear.SelectedItem != null))
            {

                int iMaxYear = Convert.ToInt32(cmbNewYear.GetItemText(cmbNewYear.SelectedItem));
                int iMinYear = Convert.ToInt32(cmbOldYear.GetItemText(cmbOldYear.SelectedItem));
                if (iMaxYear > iMinYear)
                {
                    cmbInterval.Items.Clear();
                    cmbInterval.Items.Add("");
                    while (iMaxYear != iMinYear)
                    {
                        int iInterval = iMaxYear - iMinYear;
                        cmbInterval.Items.Add(iInterval);
                        iMaxYear -= 1;
                    }
                }
            }

        }

        private void dgvVisits_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (e.RowIndex > -1)
            {
                DataGridViewRow drv = dgvVisits.Rows[e.RowIndex];
                if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0)
                {
                    dgvVisits.Rows[e.RowIndex].Selected = true;
                    cmsGCD_Visit.Show(Cursor.Position);
                }
            }

        }

        private bool CreateGCD_ReviewRecord(DataGridViewRow drv)
        {
            bool bSuccess = false;
            using (OleDbConnection dbCon = new OleDbConnection(DBConnection))
            {
                
                dbCon.Open();
                OleDbTransaction dbTrans = dbCon.BeginTransaction();
                try
                {

                    string sSQL = "INSERT INTO GCD_Review" +
                         " (NewVisitID, OldVisitID, MaskValueName, FlagReason, ValidResults, ErrorType, ErrorDEM, Comments, EnteredBy, DateModified, Processed) " +
                         " VALUES (@NewVisitID, @OldVisitID, @MaskValueName, @FlagReason, @ValidResults, @ErrorType, @ErrorDEM, @Comments, @EnteredBy, @DateModified, @Processed)";

                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbTrans.Connection, dbTrans);
                    dbCom.Parameters.AddWithValue("@NewVisitID", drv.Cells["NewVisitID"].Value);
                    dbCom.Parameters.AddWithValue("@OldVisitID", drv.Cells["OldVisitID"].Value);
                    dbCom.Parameters.AddWithValue("@MaskValueName", drv.Cells["MaskValueName"].Value);
                    dbCom.Parameters.AddWithValue("@FlagReason", "GCD Metric Review");
                    dbCom.Parameters.AddWithValue("@ValidResults", false);
                    dbCom.Parameters.AddWithValue("@ErrorType", "");
                    dbCom.Parameters.AddWithValue("@ErrorDEM", "");
                    dbCom.Parameters.AddWithValue("@Comments", "");
                    dbCom.Parameters.AddWithValue("@EnteredBy", "Workbench User");
                    dbCom.Parameters.AddWithValue("@DateModified", DateTime.Now.ToString());
                    dbCom.Parameters.AddWithValue("@Processed", false);
                    dbCom.ExecuteNonQuery();

                    dbTrans.Commit();
                    dbCon.Close();
                    bSuccess = true;
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    dbTrans.Rollback();
                    if (ex.ErrorCode == -2147467259)
                    {
                        MessageBox.Show(String.Format("Record was not added to the GCD Review table.{3}{3}The combination of New Visit ID: {0}, Old Visit ID: {1}, Mask: {2} is already in the GCD Review table.", drv.Cells["NewVisitID"].Value, drv.Cells["OldVisitID"].Value, drv.Cells["MaskValueName"].Value, Environment.NewLine),
                                        CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                    else
                    {
                        Classes.ExceptionHandling.NARException.HandleException(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
                if (bSuccess == true)
                {
                    MessageBox.Show(String.Format("Record successfully added to GCD Review table.{3}{3}New Visit ID: {0}{3}Old Visit ID: {1}{3}Mask: {2}", drv.Cells["NewVisitID"].Value, drv.Cells["OldVisitID"].Value, drv.Cells["MaskValueName"].Value, Environment.NewLine),
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
            }
            return bSuccess;
        }

        private void gcdRunReviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iIndex = dgvVisits.SelectedRows[0].Index;
            DataGridViewRow drv = dgvVisits.Rows[iIndex];
            CreateGCD_ReviewRecord(drv);
        }



        private void createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iIndex = dgvVisits.SelectedRows[0].Index;
            DataGridViewRow drv = dgvVisits.Rows[iIndex];
            bool bSuccess = CreateGCD_ReviewRecord(drv);
            if (bSuccess == true)
            {
                OleDbConnection dbCon = new OleDbConnection(DBConnection);
                Experimental.James.frmEnterPostGCD_QAQC_Record frm = new Experimental.James.frmEnterPostGCD_QAQC_Record(dbCon);
                frm.ShowDialog();
            }
        }

    }
}
