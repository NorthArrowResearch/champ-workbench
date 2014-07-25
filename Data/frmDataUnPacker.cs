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

            try
            {
                int nCount = Process(txtFolder.Text, txtSurvey1.Text, txtSurvey2.Text, txtSurvey3.Text, txtTopoTIN.Text, txtWSTIN.Text);
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
        }


        private int Process(string sTopLevelFolder, string sSurvey1, string sSurvey2, string sSurvey3, string sTopoTIN, string sWSTIN)
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
                            //
                            // Got a folder with a survey GDB, topo TIN and WS TIN
                            // proceed and unzip all three into the current folder
                            //
                            /*
#if DEBUG
                            if (eResult == DialogResult.Yes)
                            {
                                eResult = MessageBox.Show(sSurveyZip + "\n" + sTopoTINZip + "\n" + sWSTINZip,CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                if (eResult == DialogResult.Cancel)
                                {
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
#endif
                            */
                            UnZipArchive(sSurveyZip);
                            UnZipArchive(sTopoTINZip);
                            UnZipArchive(sWSTINZip);
                            nUnpackedCount += 1;
                        }
                    }
                }
            }

            return nUnpackedCount;

        }
        
        private void UnZipArchive(string sFilePath)
        {
            string s7Zip64 = "C:\\Program Files\\7-Zip\\7z.exe";
            string s7Zip32 = "C:\\Program Files (x86)\\7-Zip\\7z.exe";
            string s7Zip = null;
            string sOptions;

            if (File.Exists(s7Zip64))
            {
                s7Zip = "\"" + s7Zip64 + "\"";
            }
            else
            {
                s7Zip = "\"" + s7Zip32 + "\"";
            }

            if (sFilePath.Contains(" "))
            {
                sOptions = " x \"" + sFilePath + "\"";
            }
            else
            {
                sOptions = " x " + sFilePath;
            }

            if (Path.GetDirectoryName(sFilePath).Trim().Contains(" "))
            {
                sOptions += " -o\"" + Path.GetDirectoryName(sFilePath).Trim() + "\"";
            }
            else
            {
                sOptions += " -o" + Path.GetDirectoryName(sFilePath).Trim();
            }

            Debug.WriteLine(s7Zip);


            System.Diagnostics.Process.Start(s7Zip);

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(s7Zip, sOptions);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = info;
            p.Start();
            p.WaitForExit();
        }

    }
}
