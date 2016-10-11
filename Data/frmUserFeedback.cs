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
    public partial class frmUserFeedback : Form
    {
        public frmUserFeedback(string sDBCon)
        {
            InitializeComponent();
            ucUserFeedback1.DBCon = sDBCon;
        }
    }
}
