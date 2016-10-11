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
    public partial class frmMetricGrid : Form
    {
        public frmMetricGrid(string sDBCon, List<ListItem> lVisitIDs)
        {
            InitializeComponent();
            this.Text = "Metric Results";
            ucMetricGrid1.DBCon = sDBCon;
            ucMetricGrid1.VisitIDs = lVisitIDs;
            ucMetricGrid1.Dock = DockStyle.Fill;
        }
    }
}
