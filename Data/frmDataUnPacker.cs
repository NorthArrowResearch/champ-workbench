using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CHaMPWorkbench.Data
{
    public partial class frmDataUnPacker : Form
    {
        private int m_nTotalFiles;
        private DateTime m_dtStartTime;
        private int m_nFilesProcessed;
        private string m_Status;
        private string m_sTopLevelFolder;

        public frmDataUnPacker()
        {
            InitializeComponent();
            m_nFilesProcessed = 0;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select Top Level Monitoring Data Folder";

            if (!string.IsNullOrEmpty(txtFolder.Text))
            {
                if (Directory.Exists(txtFolder.Text))
                {
                    frm.SelectedPath = txtFolder.Text;
                }
            }

            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = frm.SelectedPath;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFolder.Text))
            {
                MessageBox.Show("You must choose a top level folder that exists.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            else
            {
                if (!Directory.Exists(txtFolder.Text))
                {
                    MessageBox.Show("You must choose a top level folder that exists.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }


            Regex r = new Regex(@"^[a-z]:[\\/]\s*$", RegexOptions.IgnoreCase);
            Match m = r.Match(txtFolder.Text);
            if (m.Success)
            {
                DialogResult result = MessageBox.Show(string.Format("It is likely that the path you specified: \"{0}\" is the root of your drive. Are you sure you want to recursively unzip from this directory?", txtFolder.Text), "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
            }


            if (rdoDifferent.Checked)
            {
                if (string.IsNullOrEmpty(txtOutputFolder.Text) || !System.IO.Directory.Exists(txtOutputFolder.Text))
                {
                    MessageBox.Show("The top level output folder must be a valid directory.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            try
            {

                m_sTopLevelFolder = txtFolder.Text;
                cmdOK.Enabled = false;
                cmdCancel.Enabled = false;
                lblStatus.Visible = true;
                groupProgress.Visible = true;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void frmDataUnPacker_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) &&
                System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
                txtOutputFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder) &&
                System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder))
                txtFolder.Text = CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder;

            lblETA.Visible = false;
            lblStatus.Visible = false;
            UpdateControls(sender, e);
            groupProgress.Visible = false;
            rdoDifferent.Checked = true;
        }


        /// <summary>
        /// Unzip the entire contents of a Zip Archive into a specified directory
        /// </summary>
        /// <param name="inFile">Zip archive file</param>
        /// <param name="outDir"></param>
        public static void UnZipArchive(string inFile, string outDir)
        {
            try
            {
                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(inFile))
                {
                    System.IO.Directory.CreateDirectory(outDir);
                    zip.ExtractAll(outDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {

            }
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select Top Level Output Data Folder";
            cmdOK.Enabled = true;
            if (!string.IsNullOrEmpty(txtOutputFolder.Text))
            {
                if (Directory.Exists(txtOutputFolder.Text))
                {
                    frm.SelectedPath = txtOutputFolder.Text;
                }
            }

            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = frm.SelectedPath;
            }
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            lblDifferent.Enabled = rdoDifferent.Checked;
            txtOutputFolder.Enabled = lblDifferent.Enabled;
            cmdBrowseOutput.Enabled = lblDifferent.Enabled;

            lblHydroInputs.Enabled = chkHydroInputs.Checked;
            txtHydroInputs.Enabled = lblHydroInputs.Enabled;

            lblHydroResults.Enabled = chkHydroResults.Checked;
            txtHydroResults.Enabled = lblHydroResults.Enabled;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            m_dtStartTime = DateTime.Now;
            m_Status = "Building index of zip archives. This may take several minutes before progress bar commences...";
            backgroundWorker1.ReportProgress(0);
            string[] sAllZipFiles;
            try
            {
                sAllZipFiles = Directory.GetFiles(m_sTopLevelFolder, "*.zip", SearchOption.AllDirectories);
            }
            catch (Exception ex) {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                return;
            }
            
            m_nTotalFiles = sAllZipFiles.Count<string>();

            m_nFilesProcessed = 0;

            foreach (string sZipFile in sAllZipFiles)
            {
                m_nFilesProcessed += 1;
                m_Status = sZipFile;
                backgroundWorker1.ReportProgress((100 * m_nFilesProcessed) / sAllZipFiles.Count<string>());

                string aFolder = System.IO.Path.GetDirectoryName(sZipFile);
                Debug.WriteLine("Folder: " + aFolder);
                string sZipFileToProcess;

                // Check if overriding the output folder
                string sOutputPath = aFolder;
                if (rdoDifferent.Checked)
                    sOutputPath = aFolder.Replace(txtFolder.Text, txtOutputFolder.Text);

                // Survey GDB
                string[] sSurveyGDBPattern = { txtSurvey1.Text, txtSurvey2.Text, txtSurvey3.Text };
                if (LookupFile(aFolder, sSurveyGDBPattern, out sZipFileToProcess))
                    UnZipArchive(sZipFileToProcess, sOutputPath);

                // Topo TIN
                string[] sTINPattern = { txtTopoTIN.Text };
                if (LookupFile(aFolder, sTINPattern, out sZipFileToProcess))
                    UnZipArchive(sZipFileToProcess, sOutputPath);

                // WS TIN
                string[] sWSTINPattern = { txtWSTIN.Text };
                if (LookupFile(aFolder, sWSTINPattern, out sZipFileToProcess))
                    UnZipArchive(sZipFileToProcess, sOutputPath);

                // Hydro Model Inputs
                if (chkHydroInputs.Checked)
                {
                    string[] sHydroInputs = { txtHydroInputs.Text };
                    if (LookupFile(aFolder, sHydroInputs, out sZipFileToProcess))
                        UnZipArchive(sZipFileToProcess, sOutputPath);
                }

                if (chkHydroResults.Checked)
                {
                    string[] sHydroResults = { txtHydroResults.Text };
                    if (LookupFile(aFolder, sHydroResults, out sZipFileToProcess))
                        UnZipArchive(sZipFileToProcess, sOutputPath);
                }
            }
        }

        private bool LookupFile(string sContainingFolder, string[] sPatternList, out string sFile)
        {
            sFile = "";

            foreach (string sPattern in sPatternList)
            {
                string[] sFiles = Directory.GetFiles(sContainingFolder, sPattern);
                if (sFiles.Count() > 0)
                {
                    sFile = sFiles[0];
                    break;
                }
            }

            return !string.IsNullOrEmpty(sFile);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgrProgress.Value = e.ProgressPercentage;
            //lblETA.Visible = true;
            lblStatus.Visible = true;
            lblStatus.Text = m_Status;

            Single nPercent = 0;
            if (m_nFilesProcessed > 0 && m_nTotalFiles > 0)
                nPercent = (Single)m_nFilesProcessed / m_nTotalFiles;

            lblETA.Text = string.Format("Unzipped {0:#,##0} ({1:P0}) of {2:#,##0} archives.", m_nFilesProcessed, nPercent, m_nTotalFiles);

            if (100 * nPercent > 5)
            {
                // enough files to warrent an ETA)
                Single fSeconds = DateTime.Now.Subtract(m_dtStartTime).Seconds;
                if (fSeconds > 0)
                {
                    Single fRemaining = (fSeconds / 60) / nPercent;
                    string sUnits = "minute(s)";

                    if (fRemaining > 60)
                    {
                        sUnits = "hour(s)";
                        fRemaining = fRemaining / 60;
                    }
                    lblETA.Text = string.Format("{0} {1:0.0} {2} remaining.", lblETA.Text, fRemaining, sUnits);
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdOK.Enabled = false;
            cmdCancel.Enabled = true;
            lblStatus.Text = string.Format("Process complete. {0} zip files processed.", m_nFilesProcessed.ToString("#,##0"));
            Cursor.Current = System.Windows.Forms.Cursors.Default;
            //MessageBox.Show("Processed " + m_nFilesProcessed.ToString("#,##0"), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            cmdOK.Enabled = true;
        }
    }
}
