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
    public partial class frmUserFeedbackGrid : Form
    {
        public frmUserFeedbackGrid(string sDBCon, List<CHaMPData.VisitBasic> lVisitIDS)
        {
            InitializeComponent();
            ucUserFeedbackGrid1.DBCon = sDBCon;
            ucUserFeedbackGrid1.VisitIDs = lVisitIDS;
        }

        private void frmUserFeedbackGrid_Load(object sender, EventArgs e)
        {
            ucUserFeedbackGrid1.Dock = DockStyle.Fill;
        }
    }
}
