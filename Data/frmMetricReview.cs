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
        private List<ListItem> Visits { get; set; }
        public ListItem Program { get; internal set; }

        public frmMetricReview(string sDBCon, List<ListItem> lVisits, ListItem theProgram)
        {
            InitializeComponent();
            DBCon = sDBCon;
            Visits = lVisits;
            ucMetricGrid1.VisitIDs = lVisits;
            ucMetricGrid1.DBCon = sDBCon;
            ucMetricGrid1.ProgramID = theProgram.Value;

            ucMetricPlot1.DBCon = sDBCon;
            ucMetricPlot1.VisitID = 1;

            ucUserFeedback1.DBCon = sDBCon;

            Program = theProgram;
        }

        private void frmMetricReview_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} Metric Review", Program.Text);

            splitContainer2.SplitterDistance = this.Width - 295;

            ucMetricGrid1.SelectedVisitChanged += HandleSelectedVisitChangedInGrid;
        }

        public void HandleSelectedVisitChangedInGrid(object sender, EventArgs e)
        {
            // TODO: the user changed the selected visit in the grid view.
            ucUserFeedback1.SelectVisit(ucMetricGrid1.SelectedVisit);
        }
    }
}
