using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Models
{
    public partial class frmScavengeMetrics : Form
    {
        OpenFileDialog frmOpen;
        private string WorkbenchCon { get; set; }

        public frmScavengeMetrics(string sWorkbenchCon)
        {
            InitializeComponent();
            WorkbenchCon = sWorkbenchCon;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSourceDB.Text) && System.IO.File.Exists(txtSourceDB.Text))
            {
                frmOpen.InitialDirectory = System.IO.Path.GetDirectoryName(txtSourceDB.Text);
                frmOpen.FileName = System.IO.Path.GetFileNameWithoutExtension(txtSourceDB.Text);
            }

            if (frmOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSourceDB.Text = frmOpen.FileName;
            }
        }

        private void frmScavengeMetrics_Load(object sender, EventArgs e)
        {
            frmOpen = new OpenFileDialog();
            frmOpen.Title = "CHaMP Program Metrics Export Database";
            frmOpen.Filter = "Access Databases (*.mdb)| *.mdb";
            frmOpen.CheckFileExists = true;
            frmOpen.AddExtension = true;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtSourceDB.Text) || !System.IO.File.Exists(txtSourceDB.Text))
            {
                MessageBox.Show("You must specify a CHaMP Program Metrics database export to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowse.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtModelVersion.Text))
            {
                MessageBox.Show("You must specify a model version before you can continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtModelVersion.Select();
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature Not Implemented in this version of the CHaMP Workbench", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;

            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            //Classes.CHaMPMetricScavenger theScavenger = new Classes.CHaMPMetricScavenger(WorkbenchCon);

            try
            {
                List<string> lMessages;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                //lMessages = theScavenger.Run(txtModelVersion.Text, "", txtSourceDB.Text, chkClear.Checked);

                frmToolResults frm = new frmToolResults("Scavenge CHaMP Metrics", "Process complete.", ref lMessages);
                frm.ShowDialog();

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }
    }
}
