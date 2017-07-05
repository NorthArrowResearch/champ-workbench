using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class ucMetricReviewPlot : UserControl
    {
        public string DBCon { get; set; }
        public naru.db.NamedObject Program { get; set; }
        public List<long> VisitIDs { get; set; }
        private long m_nHighlightedVisitID;

        public long HighlightedVisitID
        {
            get { return m_nHighlightedVisitID; }
            set
            {
                m_nHighlightedVisitID = value;
                List<long> lHighlightedVisit = new List<long>();
                lHighlightedVisit.Add(m_nHighlightedVisitID);
                UpdatePlot(lHighlightedVisit);
            }
        }

        public event EventHandler SelectedPlotChanged;
        public event EventHandler SelectedSchemaChanged;

        public CHaMPData.MetricSchema SelectedSchema
        {
            get
            {
                if (cboMetricSchemas.SelectedItem is CHaMPData.MetricSchema)
                    return (CHaMPData.MetricSchema)cboMetricSchemas.SelectedItem;
                else
                    return null;
            }
        }

        public string CurrentPlotTitle
        {
            get
            {
                string sPlotType = string.Empty;
                if (cboXAxis.SelectedItem is naru.db.NamedObject && cboYAxis.SelectedItem is naru.db.NamedObject)
                    sPlotType = string.Format("{0} by {1}", cboXAxis.SelectedItem, cboYAxis.SelectedItem);

                return sPlotType;
            }
        }

        public ucMetricReviewPlot()
        {
            InitializeComponent();
        }

        private void ucMetricReviewPlot_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DBCon) || Program == null)
                return;

            // Metric Schema Dropdown 
            Dictionary<long, CHaMPData.MetricSchema> dMetricSchemas = CHaMPData.MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString);
            cboMetricSchemas.DataSource = new naru.ui.SortableBindingList<CHaMPData.MetricSchema>(dMetricSchemas.Values.Where<CHaMPData.MetricSchema>(x => x.ProgramID == Program.ID).ToList<CHaMPData.MetricSchema>());
            cboMetricSchemas.DisplayMember = "Name";
            cboMetricSchemas.ValueMember = "ID";
            cboMetricSchemas.SelectedIndexChanged += SelectedSchemaChanged;

            // Basic, unchanging plot configuration
            ChartArea pChartArea = chtData.ChartAreas[0];
            pChartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            pChartArea.AxisX.MajorTickMark.LineColor = Color.Black;
            pChartArea.AxisX.MinorTickMark.Enabled = true;

            pChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            pChartArea.AxisY.MajorTickMark.LineColor = Color.Black;
            pChartArea.AxisY.MinorTickMark.Enabled = true;

            //enable scroll and zoom
            pChartArea.CursorX.IsUserEnabled = true;
            pChartArea.CursorX.IsUserSelectionEnabled = true;
            pChartArea.CursorY.IsUserEnabled = true;
            pChartArea.CursorY.IsUserSelectionEnabled = true;
        }

        private void UpdatePlot(List<long> theVisits)
        {
            if (string.IsNullOrEmpty(DBCon) || cboXAxis.SelectedItem == null || cboYAxis.SelectedItem == null || theVisits == null)
                return;

            Series visitSeries = null;
            if (theVisits.Count == 1)
            {
                visitSeries = null;

                foreach (Series aSeries in chtData.Series)
                {
                    if (aSeries.Name.StartsWith("Visit"))
                    {
                        visitSeries = aSeries;
                        visitSeries.Name = string.Format("Visit {0}", theVisits[0].ToString());
                        visitSeries.Points.Clear();
                        break;
                    }
                }

                if (visitSeries == null)
                    visitSeries = chtData.Series.Add(string.Format("Visit {0}", theVisits[0].ToString()));

                visitSeries.Color = Color.Red;
                visitSeries.MarkerSize = 10;
            }
            else
                visitSeries = chtData.Series.Add("All Visits");

            visitSeries.ChartType = SeriesChartType.Point;

            //using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            //{
            //    dbCon.Open();

            //    // Note the "TOP 1" statement to just get the most recent metric value from the latest result inserted
            //    SQLiteCommand dbCom = new SQLiteCommand("SELECT VM.MetricValue FROM Metric_VisitMetrics VM" +
            //        " INNER JOIN (SELECT ResultID, RunDateTime FROM Metric_Results WHERE VisitID = @VisitID) MR ON VM.ResultID = MR.ResultID" +
            //        " WHERE(VM.MetricValue IS NOT NULL) AND(VM.MetricID = @MetricID) ORDER BY MR.RunDateTime DESC", dbCon);
            //    SQLiteParameter pVisitID = dbCom.Parameters.Add("VisitID", DbType.Int64);
            //    SQLiteParameter pMetricID = dbCom.Parameters.Add("MetricID", DbType.Int64);

            //    double fXMetricValue, fYMetricValue = 0;

            //    foreach (int nVisitID in theVisits)
            //    {
            //        pVisitID.Value = nVisitID;

            //        if (GetMetricValueFromScalar(ref dbCom, ref pMetricID, ((naru.db.NamedObject)cboXAxis.SelectedItem).ID, out fXMetricValue) &&
            //            GetMetricValueFromScalar(ref dbCom, ref pMetricID, ((naru.db.NamedObject)cboYAxis.SelectedItem).ID, out fYMetricValue))
            //        {
            //            visitSeries.Points.AddXY(fXMetricValue, fYMetricValue);
            //        }
            //    }
            //}

            ChartArea pChartArea = chtData.ChartAreas[0];
            if (chtData.Titles.Count < 1)
                chtData.Titles.Add("ChartTitle");
            chtData.Titles[0].Text = CurrentPlotTitle;

            pChartArea.AxisX.Title = ((naru.db.NamedObject)cboXAxis.SelectedItem).ToString();
            pChartArea.AxisX.RoundAxisValues();

            pChartArea.AxisY.Title = ((naru.db.NamedObject)cboYAxis.SelectedItem).ToString();
        }

        private bool GetMetricValueFromScalar(ref SQLiteCommand dbCom, ref SQLiteParameter pMetric, long nMetricID, out double fMetricValue)
        {
            fMetricValue = 0;
            bool bResult = false;

            pMetric.Value = nMetricID;
            object objMetricValue = dbCom.ExecuteScalar();
            if (objMetricValue != null && objMetricValue != DBNull.Value)
            {
                fMetricValue = (double)objMetricValue;
                bResult = true;
            }

            return bResult;
        }

        private void cboPlotTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboXAxis.SelectedIndex = -1;
            cboYAxis.SelectedIndex = -1;

            if (cboPlotTypes.SelectedItem is Classes.MetricPlotType)
            {
                SelectMetricInCombobox(ref cboXAxis, ((Classes.MetricPlotType)cboPlotTypes.SelectedItem).XMetricID);
                SelectMetricInCombobox(ref cboYAxis, ((Classes.MetricPlotType)cboPlotTypes.SelectedItem).YMetricID);
            }


        }

        private void cboSchemaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cboMetricSchemas.SelectedItem is CHaMPData.MetricSchema))
                return;

            string sMetricSQL = string.Format("SELECT D.MetricID, D.Title FROM Metric_Definitions D INNER JOIN Metric_Schema_Definitions P ON D.MetricID = P.MetricID" +
                " WHERE (P.SchemaID = {0}) GROUP BY D.MetricID, D.Title ORDER BY D.Title", SelectedSchema.ID);

            // Reload the metrics into combos
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboXAxis, DBCon, sMetricSQL);
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboYAxis, DBCon, sMetricSQL);

            // Load the plot types. Do this after the X and Y combos to ensure they update when the initial plot type is selected
            Classes.MetricPlotType.LoadPlotTypes(ref cboPlotTypes, DBCon, SelectedSchema.ID);

            // Raise the generic event that the schema has changed so that the parent form can tell the other user controls
            EventHandler handler = this.SelectedSchemaChanged;
            if (handler != null)
                handler(this, e);
        }

        private void SelectMetricInCombobox(ref ComboBox cbo, long nMetricID)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (((naru.db.NamedObject)cbo.Items[i]).ID == nMetricID)
                {
                    cbo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void MetricCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            chtData.Series.Clear();
            UpdatePlot(VisitIDs);
            if (HighlightedVisitID > 0)
            {
                List<long> lHighlightedVisit = new List<long>();
                lHighlightedVisit.Add(HighlightedVisitID);
                UpdatePlot(lHighlightedVisit);
            }

            // Tell parent that the plot type has changed.
            EventHandler handler = this.SelectedPlotChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}
