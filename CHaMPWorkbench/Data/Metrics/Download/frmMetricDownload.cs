using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data.Metrics
{
    public partial class frmMetricDownload : Form
    {
        List<CHaMPData.VisitBasic> Visits { get; set; }
        naru.ui.SortableBindingList<CHaMPData.MetricSchema> MetricSchemas;

        private MetricDownloader downloadEngine;
        private frmKeystoneCredentials CredentialsForm;

        public frmMetricDownload(List<CHaMPData.VisitBasic> lVisits)
        {
            InitializeComponent();
            Visits = lVisits;
            lblCurrentProcess.Text = string.Empty;

            downloadEngine = new MetricDownloader(naru.db.sqlite.DBCon.ConnectionString);
            downloadEngine.OnProgressUpdate += downloader_OnProgressUpdate;
        }

        private void frmMetricDownload_Load(object sender, EventArgs e)
        {
            Dictionary<long, CHaMPData.MetricSchema> dMetricSchemas = CHaMPData.MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString);
            lstMetricSchemas.DataSource = new naru.ui.SortableBindingList<CHaMPData.MetricSchema>(dMetricSchemas.Values.ToList<CHaMPData.MetricSchema>());
            lstMetricSchemas.DisplayMember = "NameWithProgram";
            lstMetricSchemas.ValueMember = "ID";

            cmdCancel.DialogResult = DialogResult.Cancel;
        }

        private void downloader_OnProgressUpdate(int value)
        {
            bgWorker.ReportProgress(value);
        }

        void ShowProgressControls(bool bShow)
        {
            groupBox1.Visible = bShow;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            if (CredentialsForm == null)
                CredentialsForm = new frmKeystoneCredentials();

            DialogResult eCredentialsResult = CredentialsForm.ShowDialog();
            switch (eCredentialsResult)
            {
                case DialogResult.Cancel:
                    this.DialogResult = DialogResult.Cancel;
                    return;
            }

            try
            {
                pgrProgress.Value = 0;
                txtProgress.Text = string.Empty;
                lblCurrentProcess.Text = string.Empty;
                cmdOK.Enabled = false;
                cmdCancel.Text = "Cancel";
                cmdCancel.DialogResult = DialogResult.None;
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                cmdOK.Enabled = true;
            }
        }

        private bool ValidateForm()
        {
            if (lstMetricSchemas.CheckedItems.Count < 1)
            {
                MessageBox.Show("You must select at least one metric schema to continue.", "No Metric Schemas Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<CHaMPData.MetricSchema> schemas = new List<CHaMPData.MetricSchema>();
                foreach (CHaMPData.MetricSchema schema in lstMetricSchemas.CheckedItems)
                    schemas.Add(schema);

                downloadEngine.Run(Visits, schemas, bgWorker, CredentialsForm.UserName,  CredentialsForm.Password);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblCurrentProcess.Text = downloadEngine.CurrentProcess;
            txtProgress.Text = downloadEngine.ErrorMessages.ToString();
            pgrProgress.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdOK.Enabled = true;
            cmdCancel.Text = "Close";
            cmdCancel.DialogResult = DialogResult.Cancel;
            cmdCancel.Select();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstMetricSchemas.Items.Count; i++)
            {
                lstMetricSchemas.SetItemChecked(i, ((ToolStripMenuItem)sender).Name.ToLower().Contains("all"));
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                bgWorker.CancelAsync();
                lblCurrentProcess.Text = "User cancelled process";
                cmdCancel.Text = "Close";
                cmdCancel.DialogResult = DialogResult.Cancel;
                cmdOK.Enabled = true;
            }
            catch(Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }
    }
}
