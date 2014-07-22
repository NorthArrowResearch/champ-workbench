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
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            txtOptions.Text = CHaMPWorkbench.Properties.Settings.Default.RBTConsole;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtOptions.Text) || System.IO.File.Exists(txtOptions.Text))
                CHaMPWorkbench.Properties.Settings.Default.RBTConsole = txtOptions.Text;





            CHaMPWorkbench.Properties.Settings.Default.Save();
        }
    }
}
