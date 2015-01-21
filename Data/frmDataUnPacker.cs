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

namespace CHaMPWorkbench.Data
{
    public partial class frmDataUnPacker : Form
    {
        public frmDataUnPacker()
        {
            InitializeComponent();
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
                string sHydroInputs = "";
                string sHydroResults = "";
                string sOutputFolder = "";

                if (chkHydroInputs.Checked)
                    sHydroInputs = txtHydroInputs.Text;

                if (chkHydroResults.Checked)
                    sHydroResults = txtHydroResults.Text;

                if (rdoDifferent.Checked)
                    sOutputFolder = txtOutputFolder.Text;

                int nCount = Process(txtFolder.Text, txtSurvey1.Text, txtSurvey2.Text, txtSurvey3.Text, txtTopoTIN.Text, txtWSTIN.Text, sHydroInputs,sHydroResults, sOutputFolder);
                MessageBox.Show("Processed " + nCount.ToString("#,##0"), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private int Process(string sTopLevelFolder, string sSurvey1, string sSurvey2, string sSurvey3, string sTopoTIN, string sWSTIN, string sHydroInputs, string sHydroResults, string sOutputFolder)
        {
            int nUnpackedCount = 0;
            DialogResult eResult = DialogResult.Yes;
            foreach (string aFolder in Directory.GetDirectories(sTopLevelFolder, "*", SearchOption.AllDirectories))
            {
                Debug.WriteLine("Folder: " + aFolder);
                bool bFound = false;
                string sSurveyZip = string.Empty;
                string sTopoTINZip = string.Empty;
                string sWSTINZip = string.Empty;

                string[] sFiles = Directory.GetFiles(aFolder, sSurvey1);
                if (sFiles.Count() > 0)
                {
                    sSurveyZip = sFiles[0];
                }
                else
                {
                    sFiles = Directory.GetFiles(aFolder, sSurvey2);
                    if (sFiles.Count() > 0)
                    {
                        sSurveyZip = sFiles[0];
                    }
                    else
                    {
                        sFiles = Directory.GetFiles(aFolder, sSurvey3);
                        if (sFiles.Count() > 0)
                        {
                            sSurveyZip = sFiles[0];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sSurveyZip))
                {
                    sFiles = Directory.GetFiles(aFolder, sTopoTIN);
                    if (sFiles.Count() > 0)
                    {
                        sTopoTINZip = sFiles[0];
                        sFiles = Directory.GetFiles(aFolder, sWSTIN);
                        if (sFiles.Count() > 0)
                        {
                            sWSTINZip = sFiles[0];

                            String sZipSoftware = CHaMPWorkbench.Properties.Settings.Default.ZipPath;
                            if (String.IsNullOrWhiteSpace(sZipSoftware) || !System.IO.File.Exists(sZipSoftware))
                            {
                                MessageBox.Show("The path to the 7 Zip software is not valid. You can specify the path to the 7 Zip software under Tools\\Options '" + sZipSoftware + ".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return 0;
                            }
                            else
                            {
                                string sOutputPath = "";
                                if (!string.IsNullOrEmpty(sOutputFolder))
                                {
                                    sOutputPath = System.IO.Path.GetDirectoryName(sSurveyZip).Replace(sTopLevelFolder, sOutputFolder);
                                    System.IO.Directory.CreateDirectory(sOutputPath);
                                }

                                UnZipArchive(sSurveyZip, sZipSoftware, sOutputPath);
                                UnZipArchive(sTopoTINZip, sZipSoftware, sOutputPath);
                                UnZipArchive(sWSTINZip, sZipSoftware, sOutputPath);
                                nUnpackedCount += 1;
                            }
                        }
                    }
                }
            }

            return nUnpackedCount;
        }

        private string UnZipArchive(string sFilePath, string sZipSoftware, string sOutputFolder)
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
            
            if (sOutputFolder.Contains(" "))
            {
                sOptions += string.Format(" -o\"{0}\"",sOutputFolder);
            }
            else
            {
                sOptions += string.Format(" -o{0}", sOutputFolder);
            }

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(sZipSoftware, sOptions);
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = info;
            p.Start();
            p.WaitForExit();

            string sResult = p.StandardOutput.ReadToEnd();
            return sResult;
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
    }
}
