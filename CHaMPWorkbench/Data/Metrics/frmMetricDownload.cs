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

            downloadEngine = new MetricDownloader(naru.db.sqlite.DBCon.ConnectionString);
            downloadEngine.OnProgressUpdate +=  downloader_OnProgressUpdate;
        }

        private void frmMetricDownload_Load(object sender, EventArgs e)
        {
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstMetricSchemas, naru.db.sqlite.DBCon.ConnectionString, "SELECT SchemaID, S.Title || ' (' || P.Title || ')' AS Title FROM Metric_Schemas S INNER JOIN LookupPrograms P ON S.ProgramID = P.ProgramID ORDER BY Title", true);
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
                //ShowProgressGroup(true);
                //pgrBar.Value = 0;
                cmdOK.Enabled = false;
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                cmdOK.Enabled = true;
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<CHaMPData.MetricSchema> schemas = new List<CHaMPData.MetricSchema>();

                foreach (CHaMPData.MetricSchema schema in lstMetricSchemas.CheckedItems)
                    schemas.Add(schema);

                downloadEngine.Run(Visits, schemas, CredentialsForm.UserName, CredentialsForm.Password);
            }
            catch(Exception ex)
            {

            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblCurrentProcess.Text = downloadEngine.CurrentProcess;
           progressBar1.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdCancel.Text = "Close";
            cmdCancel.Select();
        }
    }
}
