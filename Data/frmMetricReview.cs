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

        public frmMetricReview(string sDBCon, List<ListItem> lVisits)
        {
            InitializeComponent();
            DBCon = sDBCon;
            Visits = lVisits;
            ucMetricGrid1.VisitIDs = lVisits;
            ucMetricGrid1.DBCon = sDBCon;

            ucMetricPlot1.DBCon = sDBCon;
            ucMetricPlot1.VisitID = 1;
        }
    }
}
