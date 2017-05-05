using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Experimental.Philip
{
    public partial class frmTestXPath : Form
    {
        public frmTestXPath()
        {
            InitializeComponent();
        }

        private void UpdateControls()
        {
            chkCHaMPMetricID.Enabled = rdoSelect.Checked;
            chkChaMPUse.Enabled = rdoSelect.Checked;
            chkIsActive.Enabled = rdoSelect.Checked;
        }

        private void frmTestXPath_Load(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void rdoSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            string sFilePath = "";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "RBT Result XML File";
            dlg.Filter = "RBT Result XML Files (*.xml)|*.xml";

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.LastResultsFile) &&
                System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.LastResultsFile))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(CHaMPWorkbench.Properties.Settings.Default.LastResultsFile);
                dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(CHaMPWorkbench.Properties.Settings.Default.LastResultsFile);
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtResultFile.Text = dlg.FileName;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtResultFile.Text) || !System.IO.File.Exists(txtResultFile.Text))
            {
                MessageBox.Show("You must select a valid RBT result XML file to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            
            List<string> lExceptions = new List<string>();

            string sWhereClause = "";
            if (rdoSelect.Checked)
            {
                if (chkCHaMPMetricID.Checked)
                    sWhereClause = " (CMMetricID IS NOT NULL) ";

                if (chkChaMPUse.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(sWhereClause))
                        sWhereClause += " AND ";

                    sWhereClause += " (IsCHaMP <> 0) ";
                }

                if (chkIsActive.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(sWhereClause))
                        sWhereClause += " AND ";

                    sWhereClause += " (IsActive <> 0) ";
                }
            }

            try
            {
                Experimental.Philip.TestXPath theTester = new Experimental.Philip.TestXPath(txtResultFile.Text);
                int nProcessed = theTester.RunTest(ref lExceptions, sWhereClause);
                if (lExceptions.Count < 1)
                    MessageBox.Show("All active metrics possess valid XPath values.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    string sErrors = "";
                    foreach (string s in lExceptions)
                        sErrors += s;

                    Clipboard.SetText(sErrors);
                    MessageBox.Show(nProcessed.ToString() + " metrics processed. " + lExceptions.Count.ToString() + " errors encountered and copied to the clipboard.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }
    }
}
