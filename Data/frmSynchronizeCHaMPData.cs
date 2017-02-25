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
    public partial class frmSynchronizeCHaMPData : Form
    {
        BindingList<CHaMPData.Program> Programs;
        BindingList<CHaMPData.Watershed> Watersheds;
        CHaMPData.DataSynchronizer syncEngine;

        public frmSynchronizeCHaMPData()
        {
            InitializeComponent();
        }

        private void frmSynchronizeCHaMPData_Load(object sender, EventArgs e)
        {
            // Only display programs that have an API defined.
            IEnumerable<CHaMPData.Program> allPrograms = CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString).Values;
            Programs = new BindingList<CHaMPData.Program>(allPrograms.Where<CHaMPData.Program>(x => !string.IsNullOrEmpty(x.API)).ToList<CHaMPData.Program>());
            lstPrograms.DataSource = Programs;
            lstPrograms.DisplayMember = "Name";
            lstPrograms.ValueMember = "ID";

            Watersheds = new BindingList<CHaMPData.Watershed>(CHaMPData.Watershed.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.Watershed>());
            lstWatersheds.DataSource = Watersheds;
            lstWatersheds.DisplayMember = "Name";
            lstWatersheds.ValueMember = "ID";

            // Hide the progress bar for now.
            ShowProgressGroup(false);

            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;

            // Construct the synchronization engine and subscribe to it's progress event
            syncEngine = new CHaMPData.DataSynchronizer();
            syncEngine.OnProgressUpdate += synchronizer_OnProgressUpdate;
        }

        private void ShowProgressGroup(bool bVisible)
        {
            if (grpProgress.Visible & !bVisible)
            {
                grpProgress.Visible = false;
                this.Height -= (grpProgress.Height + (grpProgress.Top - grpPrograms.Bottom));
            }
            else if (!grpProgress.Visible & bVisible)
            {
                grpProgress.Visible = true;
                this.Height += (grpProgress.Height + (grpProgress.Top - grpPrograms.Bottom));
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                ShowProgressGroup(true);
                pgrBar.Value = 0;
                cmdOK.Enabled = false;
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
            if (lstPrograms.CheckedItems.Count < 1)
            {
                MessageBox.Show("You must select at least one program to synchronize.");
                lstPrograms.Select();
                return false;
            }

            if (!naru.web.CheckForInternetConnection())
            {
                MessageBox.Show("Check that you are currently connected to the Internet and try again.", "No Internet Connection.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Build a list of the programs that are checked.
            List<CHaMPData.Program> checkedPrograms = new List<CHaMPData.Program>();
            foreach (CHaMPData.Program aProgram in lstPrograms.CheckedItems)
                checkedPrograms.Add(aProgram);

            // An empty list means that all watersheds will be processed
            Dictionary<long, CHaMPData.Watershed> checkedWatersheds = new Dictionary<long, CHaMPData.Watershed>();
            foreach (CHaMPData.Watershed aWatershed in lstWatersheds.CheckedItems)
                checkedWatersheds.Add(aWatershed.ID, aWatershed);

            try
            {
                syncEngine.Run(checkedPrograms, checkedWatersheds);
            }
            catch(Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblCurrentProcess.Text = syncEngine.CurrentProcess;
            pgrBar.Value = e.ProgressPercentage;
        }

        private void synchronizer_OnProgressUpdate(int value)
        {
            bgWorker.ReportProgress(value);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdCancel.Text = "Close";
            cmdCancel.Select();
        }

        private void AllNoneWatershedsClick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < lstWatersheds.Items.Count; i++)
                    lstWatersheds.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }
    }
}
