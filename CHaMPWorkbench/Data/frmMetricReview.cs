using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data
{
    public partial class frmMetricReview : Form
    {
        public string DBCon { get; internal set; }
        private List<CHaMPData.VisitBasic> Visits { get; set; }
        public naru.db.NamedObject Program { get; internal set; }

        public frmMetricReview(string sDBCon, List<CHaMPData.VisitBasic> lVisits, naru.db.NamedObject theProgram)
        {
            InitializeComponent();
            DBCon = sDBCon;
            Visits = lVisits;
            ucMetricGrid1.VisitIDs = lVisits;
            ucMetricGrid1.DBCon = sDBCon;
            ucMetricGrid1.ProgramID = theProgram.ID;

            ucMetricPlot1.DBCon = sDBCon;
            ucMetricPlot1.HighlightedVisitID = 1;
            ucMetricPlot1.VisitIDs = lVisits.Select(n => n.ID).ToList<long>();
            ucMetricPlot1.Program = theProgram;

            ucUserFeedback1.DBCon = sDBCon;

            Program = theProgram;
        }

        private void frmMetricReview_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} Metric Review", Program.Name);

            splitContainer2.SplitterDistance = this.Width - 295;

            ucMetricGrid1.SelectedVisitChanged += HandleSelectedVisitChangedInGrid;
            ucMetricPlot1.SelectedPlotChanged += HandleSelectedPlotChanged;

            HandleSelectedVisitChangedInGrid(null, null);
            HandleSelectedPlotChanged(null, null);
        }

        public void HandleSelectedVisitChangedInGrid(object sender, EventArgs e)
        {
            // TODO: the user changed the selected visit in the grid view.
            ucUserFeedback1.SelectVisit(ucMetricGrid1.SelectedVisit);
            ucMetricPlot1.HighlightedVisitID = ucMetricGrid1.SelectedVisit;
        }

        public void HandleSelectedPlotChanged(object sender, EventArgs e)
        {
            ucUserFeedback1.ItemReviewed = string.Format("Plot: {0}", ucMetricPlot1.CurrentPlotTitle);
        }

        private void metricDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Metrics.frmMetricDownload frm = new Metrics.frmMetricDownload();
            frm.ShowDialog();
        }
    }
}
