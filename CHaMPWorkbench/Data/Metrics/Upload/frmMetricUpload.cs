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
        string Message { get; set; }

        public frmMetricUpload()
        {
            InitializeComponent();
        }

        private void frmMetricUpload_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            if (!ucBatch.Validate())
                return false;

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
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        public void UploaderMessage(object sender, EventArgs e)
        {

        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (frmCredentials == null)
                    frmCredentials = new frmKeystoneCredentials();

                if (frmCredentials.ShowDialog() != DialogResult.OK)
                    return;

                uploader = new MetricUploader(frmCredentials.UserName, frmCredentials.Password);
                uploader.OnProgressUpdate += uploader_OnProgressUpdate;

                uploader.Run(ucBatch.SelectedBatches, bgWorker);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void uploader_OnProgressUpdate(int value, string sMessage)
        {
            txtMessages.AppendText(Message + Environment.NewLine);
            bgWorker.ReportProgress(value);
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtMessages.AppendText(Message + Environment.NewLine);
            pgrProgress.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdOK.Enabled = true;
            cmdCancel.Text = "Close";
            uploader = null;
        }
    }
}
