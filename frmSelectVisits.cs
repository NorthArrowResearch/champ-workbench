using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench
{
    public partial class frmSelectVisits : Form
    {

        public int VisitsToSelect { get; internal set; }

        public frmSelectVisits(int nVisits = 0)
        {
            InitializeComponent();
            valVisits.Value = nVisits;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            VisitsToSelect = (int) valVisits.Value;
        }
    }
}
