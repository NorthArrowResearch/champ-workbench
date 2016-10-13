using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data
{
    public partial class ucMetricReviewPlot : UserControl
    {
        public string DBCon { get; set; }
        public ListItem Program { get; set; }
        public List<int> VisitIDs { get; set; }
        public int HighlightedVisitID { get; set; }

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

        public ucMetricReviewPlot()
        {
            InitializeComponent();
        }

        private void ucMetricReviewPlot_Load(object sender, EventArgs e)
        {
            chtData.Dock = DockStyle.Fill;

            if (string.IsNullOrEmpty(DBCon) || Program == null)
                return;

            // Load the plot types
            Classes.MetricPlotType.LoadPlotTypes(ref cboPlotTypes, DBCon, Program.Value);

            // Load the metrics for the current protocol
            string sProgramClause = string.Empty;
            if (Program != null)
                string.Format(" AND (P.ProgramID = {0}", Program.Value);

            string sMetricSQL = string.Format("SELECT D.MetricID, D.Title FROM Metric_Definitions D INNER JOIN Metric_Definition_Programs P ON D.MetricID = P.MetricID" +
                " WHERE(D.TypeID = 3) {0} GROUP BY D.MetricID, D.Title ORDER BY D.Title", sProgramClause);

            ListItem.LoadComboWithListItems(ref cboXAxis, DBCon, sMetricSQL);
            ListItem.LoadComboWithListItems(ref cboYAxis, DBCon, sMetricSQL);
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
            
            EventHandler handler = this.SelectedPlotChanged;
            if (handler != null)
                handler(this, e);
        }

        private void SelectMetricInCombobox(ref ComboBox cbo, int nMetricID)
        {
            for (int i = 0; i < cbo.Items.Count;i++)
            {
                if (((ListItem) cbo.Items[i]).Value == nMetricID)
                {
                    cbo.SelectedIndex = i;
                    return;
                }
            }
        }
    }
}
