﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CHaMPWorkbench.Data
{
    public partial class frmDataUnPacker : Form
    {
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
            String sZipSoftware = CHaMPWorkbench.Properties.Settings.Default.ZipPath;
            if (String.IsNullOrWhiteSpace(sZipSoftware) || !System.IO.File.Exists(sZipSoftware))
            {
                MessageBox.Show("The path to the 7 Zip software is not valid. You can specify the path to the 7 Zip software under Tools\\Options '" + sZipSoftware + ".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.RunWorkerAsync();

                //                int nCount = Process(txtFolder.Text, txtSurvey1.Text, txtSurvey2.Text, txtSurvey3.Text, txtTopoTIN.Text, txtWSTIN.Text, sHydroInputs,sHydroResults, sOutputFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void frmDataUnPacker_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder) &&
                System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder))
                txtFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder;

            UpdateControls(sender, e);
        }

        //private int Process(string sTopLevelFolder, string sSurvey1, string sSurvey2, string sSurvey3, string sTopoTIN, string sWSTIN, string sHydroInputs, string sHydroResults, string sOutputFolder)
        //{

        //    return nUnpackedCount;
        //}

        private string UnZipArchive(string sFilePath, string sOutputFolder)
        {
            string sOptions;

            if (sFilePath.Contains(" "))
            {
                sOptions = " x \"" + sFilePath + "\"";
            }
            else
            {
                sOptions = " x " + sFilePath;
            }

            // If no output folder is specified then simply unzip into the same folder as the archive
            if (string.IsNullOrEmpty(sOutputFolder))
                sOutputFolder = Path.GetDirectoryName(sFilePath);
            sOutputFolder = sOutputFolder.Trim();
            System.IO.Directory.CreateDirectory(sOutputFolder);

            if (sOutputFolder.Contains(" "))
            {
                sOptions += string.Format(" -o\"{0}\"", sOutputFolder);
            }
            else
            {
                sOptions += string.Format(" -o{0}", sOutputFolder);
            }

            sOptions += " -aoa";

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(CHaMPWorkbench.Properties.Settings.Default.ZipPath, sOptions);
            info.UseShellExecute = false;
            // info.RedirectStandardOutput = true;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = info;
            p.Start();
            //p.WaitForExit();

            //p.StandardOutput.ReadToEnd();
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select Top Level Output Data Folder";

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
            m_Status = "Building index of zip archives. This may take several minutes before progress bar commences...";
            backgroundWorker1.ReportProgress(0);
            string[] sAllZipFiles = Directory.GetFiles(m_sTopLevelFolder, "*.zip", SearchOption.AllDirectories);
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
            lblStatus.Text = m_Status;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdOK.Enabled = true;
            cmdCancel.Enabled = true;
            lblStatus.Text = string.Format("Process complete. {0} zip files processed.", m_nFilesProcessed.ToString("#,##0"));
            Cursor.Current = System.Windows.Forms.Cursors.Default;
            //MessageBox.Show("Processed " + m_nFilesProcessed.ToString("#,##0"), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
