﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Windows.Forms.DataVisualization.Charting;

namespace CHaMPWorkbench.Data
{
    public partial class ucMetricPlot : UserControl
    {
        public string DBCon { get; set; }
        public long VisitID { get; set; }
        public naru.db.NamedObject Program { get; set; }

        private Dictionary<long, ModelResult> m_dModelResults;

        public event EventHandler SelectedPlotChanged;

        public string CurrentPlotTitle
        {
            get
            {
                string sPlotType = string.Empty;
                if (cboPlotTypes.SelectedItem is Classes.MetricPlotType)
                    sPlotType = ((Classes.MetricPlotType)cboPlotTypes.SelectedItem).Title;

                return sPlotType;
            }
        }

        public ucMetricPlot()
        {
            InitializeComponent();
        }

        private void ucMetricPlot_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DBCon))
                return;

            Classes.MetricPlotType.LoadPlotTypes(ref cboPlotTypes, DBCon);
            ModelResult.LoadModelResults(ref cboModelResults, DBCon, VisitID, out m_dModelResults);

            cboModelResults.SelectedIndexChanged += PlotChanged;
            cboPlotTypes.SelectedIndexChanged += PlotChanged;
        }

        private void PlotChanged(object sender, EventArgs e)
        {
            Classes.MetricPlotType thePlot = null;
            if (cboPlotTypes.SelectedItem is Classes.MetricPlotType)
                thePlot = cboPlotTypes.SelectedItem as Classes.MetricPlotType;
            else
                return;

            ModelResult theResult = null;
            if (cboModelResults.SelectedItem is ModelResult)
                theResult = cboModelResults.SelectedItem as ModelResult;
            else
                return;

            Dictionary<long, double> dXMetricValues = GetMetricValues(thePlot.XMetricID, theResult.ID);
            Dictionary<long, double> dYMetricValues = GetMetricValues(thePlot.YMetricID, theResult.ID);

            EventHandler handler = this.SelectedPlotChanged;
            if (handler != null)
                handler(this, e);
        }

        private Dictionary<long, double> GetMetricValues(long nMetricID, long nResultID)
        {
            Dictionary<long, double> dResults = new Dictionary<long, double>();
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT ResultID, MetricValue FROM Metric_VisitMetrics WHERE (MetricID = @MetricID) AND (MetricValue IS NOT NULL)", dbCon);
                dbCom.Parameters.AddWithValue("@MetricID", nMetricID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    dResults.Add(dbRead.GetInt64(dbRead.GetOrdinal("ResultID")), dbRead.GetDouble(dbRead.GetOrdinal("MetricValue")));
            }

            return dResults;
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboModelResults.SelectedItem is ModelResult && cboPlotTypes.SelectedItem is Classes.MetricPlotType)
            {
                // TODO replot graph
                UpdatePlot(cboPlotTypes.SelectedItem as Classes.MetricPlotType, cboModelResults.SelectedItem as ModelResult);
            }
        }

        private void UpdatePlot(Classes.MetricPlotType thePlot, ModelResult theResult)
        {
            chtData.Series.Clear();


            Series watershedSeries = chtData.Series.Add(theResult.Watershed);
            watershedSeries.ChartType = SeriesChartType.Point;
            foreach (ModelResult aResult in m_dModelResults.Values)
            {
                if (aResult.MetricValues.ContainsKey(thePlot.XMetricID) && aResult.MetricValues.ContainsKey(thePlot.YMetricID))
                    watershedSeries.Points.AddXY(aResult.MetricValues[thePlot.XMetricID], aResult.MetricValues[thePlot.YMetricID]);
            }

            if (m_dModelResults.ContainsKey(theResult.ID) && theResult.MetricValues.ContainsKey(thePlot.XMetricID) && theResult.MetricValues.ContainsKey(thePlot.YMetricID))
            {
                // Add the specific result for the target visit
                Series visitSeries = chtData.Series.Add(string.Format("Visit {0}", theResult.VisitID));
                visitSeries.ChartType = SeriesChartType.Point;
                visitSeries.Points.AddXY(theResult.MetricValues[thePlot.XMetricID], theResult.MetricValues[thePlot.YMetricID]);

                visitSeries.Color = Color.Red;
                visitSeries.MarkerSize = 10;
            }

            ChartArea pChartArea = chtData.ChartAreas[0];
            if (chtData.Titles.Count < 1)
                chtData.Titles.Add("ChartTitle");
            chtData.Titles[0].Text = thePlot.Title;

            pChartArea.AxisX.Title = thePlot.XMetric;
            pChartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            pChartArea.AxisX.MajorTickMark.LineColor = Color.Black;
            pChartArea.AxisX.MinorTickMark.Enabled = true;
            pChartArea.AxisX.RoundAxisValues();

            pChartArea.AxisY.Title = thePlot.YMetric;
            pChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            pChartArea.AxisY.MajorTickMark.LineColor = Color.Black;
            pChartArea.AxisY.MinorTickMark.Enabled = true;

            //enable scroll and zoom
            pChartArea.CursorX.IsUserEnabled = true;
            pChartArea.CursorX.IsUserSelectionEnabled = true;
            pChartArea.CursorY.IsUserEnabled = true;
            pChartArea.CursorY.IsUserSelectionEnabled = true;
        }

        # region HelperClasses

        private class ModelResult
        {
            public long ID { get; internal set; }
            public string ModelVersion { get; internal set; }
            public string ScavengeType { get; internal set; }
            public DateTime RunDateTime { get; internal set; }
            public bool HasDateTime { get; set; }

            public long WatershedID { get; internal set; }
            public string Watershed { get; internal set; }
            public long SiteID { get; internal set; }
            public string Site { get; internal set; }
            public long VisitID { get; internal set; }

            public Dictionary<long, double> MetricValues { get; internal set; }

            public void SetMetricValue(long nMetricID, double fValue)
            {
                MetricValues[nMetricID] = fValue;
            }

            public string Title
            {
                get
                {
                    if (HasDateTime)
                        return string.Format("Version {0} on {1} status {2} ({3})", ModelVersion, RunDateTime, ScavengeType, ID);
                    else
                        return string.Format("Version {0} status {1} ({2})", ModelVersion, ScavengeType, ID);
                }
            }

            public override string ToString()
            {
                return Title;
            }

            public ModelResult(long nID, string sModelVersion, DateTime dtRunDateTime, string sScavengeType, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitID)
            {
                RunDateTime = dtRunDateTime;
                HasDateTime = true;

                Init(nID, sModelVersion, sScavengeType, nWatershedID, sWatershedName, nSiteID, sSiteName, nVisitID);
            }

            public ModelResult(long nID, string sModelVersion, string sScavengeType, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitID)
            {
                Init(nID, sModelVersion, sScavengeType, nWatershedID, sWatershedName, nSiteID, sSiteName, nVisitID);
            }

            private void Init(long nID, string sModelVersion, string sScavengeType, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitID)
            {
                ID = nID;
                ModelVersion = sModelVersion;
                HasDateTime = false;
                ScavengeType = sScavengeType;

                WatershedID = nWatershedID;
                Watershed = sWatershedName;
                SiteID = nSiteID;
                Site = sSiteName;
                VisitID = nVisitID;

                MetricValues = new Dictionary<long, double>();
            }

            public static void LoadModelResults(ref ComboBox cbo, string sDBCon, long VisitID, out Dictionary<long, ModelResult> dModelResults)
            {
                cbo.Items.Clear();
                dModelResults = new Dictionary<long, ModelResult>();

                using (SQLiteConnection conResults = new SQLiteConnection(sDBCon))
                {
                    conResults.Open();
                    SQLiteCommand comResults = new SQLiteCommand("SELECT R.ResultID AS ResultID, ModelVersion, RunDateTime, L.Title AS ScavengeType, R.VisitID AS VisitID, SiteName, S.SiteID AS SiteID, W.WatershedID AS WatershedID, WatershedName" +
" FROM LookupListItems AS L INNER JOIN (CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN (Metric_Results AS R INNER JOIN CHAMP_Visits AS V ON R.VisitID = V.VisitID) ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID) ON L.ItemID = R.ScavengeTypeID" +
" WHERE (((W.WatershedID) In (SELECT SS.WatershedID FROM CHAMP_Sites AS SS INNER JOIN CHAMP_Visits AS VV ON SS.SiteID = VV.SiteID WHERE (((VV.VisitID)=@VisitID)))))" +
" ORDER BY R.RunDateTime DESC", conResults);
                    comResults.Parameters.AddWithValue("@VisitID", VisitID);

                    using (SQLiteConnection conMetricValues = new SQLiteConnection(sDBCon))
                    {
                        conMetricValues.Open();
                        SQLiteCommand comMetricValues = new SQLiteCommand("SELECT MetricID, MetricValue FROM Metric_VisitMetrics WHERE (ResultID = @ResultID) AND (MetricValue IS NOT NULL)", conMetricValues);
                        SQLiteParameter pResultID = comMetricValues.Parameters.Add("@ResultID", DbType.Int64);
                        SQLiteDataReader rdResults = comResults.ExecuteReader();
                        while (rdResults.Read())
                        {
                            long nResultID = rdResults.GetInt64(rdResults.GetOrdinal("ResultID"));

                            ModelResult theResult = null;
                            if (rdResults.IsDBNull(rdResults.GetOrdinal("RunDateTime")))
                                theResult = new ModelResult(nResultID
                                    , rdResults.GetString(rdResults.GetOrdinal("ModelVersion"))
                                    , rdResults.GetString(rdResults.GetOrdinal("ScavengeType"))
                                    , rdResults.GetInt64(rdResults.GetOrdinal("WatershedID"))
                                    , rdResults.GetString(rdResults.GetOrdinal("WatershedName"))
                                    , rdResults.GetInt64(rdResults.GetOrdinal("SiteID"))
                                    , rdResults.GetString(rdResults.GetOrdinal("SiteName"))
                                    , rdResults.GetInt64(rdResults.GetOrdinal("VisitID"))


                                    );
                            else
                                theResult = new ModelResult(nResultID
                                    , rdResults.GetString(rdResults.GetOrdinal("ModelVersion"))
                                    , rdResults.GetDateTime(rdResults.GetOrdinal("RunDateTime"))
                                    , rdResults.GetString(rdResults.GetOrdinal("ScavengeType"))
                                    , rdResults.GetInt64(rdResults.GetOrdinal("WatershedID"))
                                    , rdResults.GetString(rdResults.GetOrdinal("WatershedName"))
                                    , rdResults.GetInt64(rdResults.GetOrdinal("SiteID"))
                                    , rdResults.GetString(rdResults.GetOrdinal("SiteName"))
                                    , rdResults.GetInt64(rdResults.GetOrdinal("VisitID"))
                                    );

                            // Now add the metric values to this result
                            pResultID.Value = nResultID;
                            SQLiteDataReader rdMetricValues = comMetricValues.ExecuteReader();
                            while (rdMetricValues.Read())
                                theResult.SetMetricValue(rdMetricValues.GetInt64(rdMetricValues.GetOrdinal("MetricID")), rdMetricValues.GetDouble(rdMetricValues.GetOrdinal("MetricValue")));
                            rdMetricValues.Close();

                            dModelResults.Add(theResult.ID, theResult);

                            if (theResult.VisitID == VisitID)
                                cbo.Items.Add(theResult);
                        }
                    }

                    if (cbo.Items.Count > 0)
                        cbo.SelectedIndex = 0;
                }
            }
        }

        #endregion
    }
}
