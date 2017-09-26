using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data.Metrics.Upload
{
    public partial class frmMetricUpload : Form
    {
        frmKeystoneCredentials frmCredentials = null;
        MetricUploader uploader;

        public frmMetricUpload()
        {
            InitializeComponent();
        }

        private void frmMetricUpload_Load(object sender, EventArgs e)
        {
            chkLog_CheckedChanged(null, null);
            txtLog.Text = System.IO.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), string.Format("{0:yyyMMdd_HHmmss}_metricupload.log", DateTime.Now));
            chkLog.Checked = true;
        }

        private bool ValidateForm()
        {
            if (!ucBatch.Validate())
                return false;

            if (chkLog.Checked)
            {
                if (string.IsNullOrEmpty(txtLog.Text))
                {
                    MessageBox.Show("You must specify a log file path or uncheck the option to generate a log file.", "Invalid Log File Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmdBrowseLog.Select();
                    return false;
                }
            }

            return true;
        }

        private void cboOK_Click(object sender, EventArgs e)
        {
            try
            {
                pgrProgress.Value = 0;
                txtMessages.Text = string.Empty;
                cmdOK.Enabled = false;
                cmdCancel.Text = "Cancel";
                uploader = new MetricUploader(bgWorker, ucBatch.SelectedProgram, chkLog.Checked ? txtLog.Text : string.Empty);

                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (frmCredentials == null)
                    frmCredentials = new frmKeystoneCredentials();

                if (frmCredentials.ShowDialog() != DialogResult.OK)
                    return;

                uploader.Run(ucBatch.SelectedBatches, frmCredentials.UserName, frmCredentials.Password);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtMessages.Text = uploader.Messages.ToString();
            txtMessages.SelectionStart = txtMessages.Text.Length;
            txtMessages.ScrollToCaret();
            pgrProgress.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdOK.Enabled = true;
            cmdCancel.Text = "Close";
            uploader = null;
        }

        private void chkLog_CheckedChanged(object sender, EventArgs e)
        {
            txtLog.Enabled = chkLog.Checked;
            cmdBrowseLog.Enabled = chkLog.Checked;
        }

        private void cmdBrowseLog_Click(object sender, EventArgs e)
        {
            naru.ui.Textbox.BrowseSaveFile(ref txtLog, "Metric Upload Log File", "Comma Separated Value files (*.csv)|*.csv");
        }
    }
}
