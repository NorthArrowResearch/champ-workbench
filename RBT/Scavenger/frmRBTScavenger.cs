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
    public partial class frmRBTScavenger : Form
    {
        private System.Data.OleDb.OleDbConnection m_dbCon;
        private Classes.ResultScavengerBatch m_scavenger;

        public frmRBTScavenger(System.Data.OleDb.OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void frmRBTScavenger_Load(object sender, EventArgs e)
        {

            tTip.SetToolTip(txtFolder, "The path to the top level folder that will be scanned for RBT result files");
            tTip.SetToolTip(cmdBrowseFolder, "Browse to a new top level folder");
            tTip.SetToolTip(chkRecursive, "When checked the tool will search recursively through all sub-folders within the top level folder for RBT result files");
            tTip.SetToolTip(radAllFiles, "Choose this option to check all XML files to see if they are RBT result files. Only XML files with the RBT result file structure will be scavenged");
            tTip.SetToolTip(radMatch, "Choose this option to only scavenge files that match the following file name pattern");
            tTip.SetToolTip(txtMatch, "File name pattern that identifies RBT result files. Use asterix as a wildcard. File names must end with .xml");
            tTip.SetToolTip(chkEmptyDB, "Check this option to empty the database before running the scavenger tool. Unchecking this box will append any scavenged results to existing data in the database.");
            tTip.SetToolTip(cmdHelp, "Get help with this tool");
            tTip.SetToolTip(cmdOK, "Start the result file scavenger running");
            tTip.SetToolTip(cmdStop, "Cancel the current scavenger operation");

            if (!string.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder))
                if (System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder))
                    txtFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder;
        }

        private void UpdateControls()
        {
            txtLog.Enabled = chkLogs.Checked;
            txtMatch.Enabled = radMatch.Checked;

        }

        private void chkLogs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void radMatch_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolder.Text) || !System.IO.Directory.Exists(txtFolder.Text))
                MessageBox.Show("The top level folder does not represent a valid directory.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (radMatch.Checked)
                if (string.IsNullOrWhiteSpace(txtMatch.Text))
                    MessageBox.Show("You must provide a file name matching string (e.g. Result*.xml) or select \"All Files\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (chkLogs.Checked)
                if (string.IsNullOrWhiteSpace(txtLog.Text))
                    MessageBox.Show("You must provide a log file name pattern (e.g. log*.xml) or uncheck the log file option.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

            string sSearch = "*.xml";
            if (radMatch.Checked)
            {
                sSearch = txtMatch.Text;
            }

            m_scavenger = new Classes.ResultScavengerBatch(m_dbCon, txtFolder.Text, sSearch, chkRecursive.Checked, chkEmptyDB.Checked, txtLog.Text);

            prgBar.Visible = true;
            cmdOK.Enabled = false;
            cmdStop.Enabled = true;
            BackgroundWorker1.WorkerReportsProgress = true;
            BackgroundWorker1.WorkerSupportsCancellation = true;
            BackgroundWorker1.RunWorkerAsync();
        }


        // This event handler is where the actual work is done. 
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker object that raised this event. 
            BackgroundWorker worker = (BackgroundWorker)sender;

            // Assign the result of the computation 
            // to the Result property of the DoWorkEventArgs 
            // object. This is will be available to the  
            // RunWorkerCompleted eventhandler.

            e.Result = m_scavenger.Process(BackgroundWorker1, e);
        }

        // This event handler updates the progress. 
        private void backgroundWorker1_ProgressChanged(System.Object sender, ProgressChangedEventArgs e)
        {
            //resultLabel.Text = (e.ProgressPercentage.ToString() + "%")
            prgBar.Value = e.ProgressPercentage;
        }

        private void cancelAsyncButton_Click(System.Object sender, System.EventArgs e)
        {
            // Cancel the asynchronous operation. 
            this.BackgroundWorker1.CancelAsync();

            // Disable the Cancel button.
            cmdOK.Enabled = true;
            cmdStop.Enabled = false;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown. 
            if ((e.Error != null))
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled the  
                // operation. 
                // Note that due to a race condition in  
                // the DoWork event handler, the Cancelled 
                // flag may not have been set, even though 
                // CancelAsync was called.
                //lblProgress.Text = "Canceled"
            }
            else if ((e.Error != null))
            {
                MessageBox.Show(e.Error.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Finally, handle the case where the operation succeeded.
                //lblProgress.Text = e.Result.ToString()
                cmdCancel.Focus();
                MessageBox.Show("Completed sucessfully", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Disable the Cancel button.
            cmdOK.Enabled = true;
            cmdStop.Enabled = false;
        }

        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select the top level folder that you want to search for RBT results and log files.";
           
            if (!string.IsNullOrWhiteSpace(txtFolder.Text))
                if (System.IO.Directory.Exists(txtFolder.Text))
                    frm.SelectedPath = txtFolder.Text;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Directory.Exists(frm.SelectedPath))
                    txtFolder.Text = frm.SelectedPath;
            }
        }

    }
}