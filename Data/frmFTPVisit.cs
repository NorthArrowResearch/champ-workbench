using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CHaMPWorkbench.Data
{
    public partial class frmFTPVisit : Form
    {
        private int m_nVisitID;
        private string m_sRemoteRoot;
        private string m_sLocalRoot;
        private const string m_sFTPTopLevelFolder = "ftp://ftp.geooptix.com/ByYear";

        private List<string> m_sFiles;
        private string m_sCurrentFile;
        private bool m_bOverwrite;
        private bool m_bCreateFolders;
        private StringBuilder m_sProgress;

        public frmFTPVisit(int nVisitID, string sRemoteFolder, string sLocalFolder)
        {
            InitializeComponent();
            m_nVisitID = nVisitID;

            m_sLocalRoot = sLocalFolder;
            m_sRemoteRoot = sRemoteFolder;
            m_sCurrentFile = string.Empty;
            m_bOverwrite = false;
            m_bCreateFolders = true;
        }

        private void frmFTPVisit_Load(object sender, EventArgs e)
        {
            TreeNode nodParent = treFiles.Nodes.Add(string.Format("Visit ID {0}", m_nVisitID));
            TreeNode nodHydro = nodParent.Nodes.Add("Hydro");
            nodHydro.Nodes.Add("HydroModelInputs.zip");
            nodHydro.Nodes.Add("HydroModelResults.zip");

            TreeNode nodTopo = nodParent.Nodes.Add("Topo");
            nodTopo.Nodes.Add("MapImages.zip");
            nodTopo.Nodes.Add("SurveyGDB.zip");
            nodTopo.Nodes.Add("TIN.zip");
            nodTopo.Nodes.Add("TopoToolbarResults.xml");
            nodTopo.Nodes.Add("WettedSurfaceTIN.zip");

            TreeNode nodRBT = nodParent.Nodes.Add("RBTOutputs");
            nodRBT.Nodes.Add("LogFile.xml");
            nodRBT.Nodes.Add("RBTOutput.zip");
            nodRBT.Nodes.Add("Results.xml");

            nodParent.ExpandAll();
            treFiles.CheckBoxes = true;

            grpProgress.Visible = false;
            treFiles.Height = treFiles.Height + grpProgress.Height;
            //this.Height = this.Height - grpProgress.Height;

            backgroundWorker1.WorkerReportsProgress = true;
            chkCreateDir.Checked = m_bCreateFolders;
            checkBox1.Checked = m_bOverwrite;
            txtRemote.Text = m_sRemoteRoot;
            txtLocalFolder.Text = m_sLocalRoot;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < m_sFiles.Count; i++)
            {
                try
                {
                    m_sProgress.AppendFormat("{0}Downloading {1}...", Environment.NewLine, m_sFiles[i]);
                    backgroundWorker1.ReportProgress(100 * (i / m_sFiles.Count));
                    
                    FTPFile(m_sFiles[i]);
                    m_sProgress.Append(" success");
                    backgroundWorker1.ReportProgress(100 * (i / m_sFiles.Count));
                }
                catch (Exception ex)
                {
                    m_sProgress.AppendFormat("{2}{0}, {1}", m_sFiles[i], ex.Message,Environment.NewLine);
                }

                backgroundWorker1.ReportProgress(100 * (i / m_sFiles.Count));
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            txtProgress.Text = m_sProgress.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (MessageBox.Show("Process complete. Do you want to explore the local, download folder?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                case System.Windows.Forms.DialogResult.Cancel:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return;
                case System.Windows.Forms.DialogResult.Yes:
                    System.Diagnostics.Process.Start(txtLocalFolder.Text);
                    break;

                default:
                    // No. Do nothing
                    break;
            }
        }

        private void GetCheckedFiles(ref string sPathSoFar, ref TreeNode aNode)
        {
            string sRelativePath = string.Empty;

            if (aNode.Parent is TreeNode)
                sRelativePath = System.IO.Path.Combine(sPathSoFar, aNode.Text);

            if (sRelativePath.Contains("."))
            {
                if (aNode.Checked)
                    m_sFiles.Add(sRelativePath);
            }
            else
            {
                // folder. Process children.
                TreeNode aChildNode;
                foreach (TreeNode aChild in aNode.Nodes)
                {
                    aChildNode = aChild;
                    GetCheckedFiles(ref sRelativePath, ref aChildNode);
                }
            }
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/ms229711%28v=vs.110%29.aspx
        /// </summary>
        /// <param name="sRelativePath"></param>
        private void FTPFile(string sRelativePath)
        {
            string sFTPFile = System.IO.Path.Combine(m_sRemoteRoot, sRelativePath).Replace("\\", "/");
            string sLocalFile = System.IO.Path.Combine(m_sLocalRoot, sRelativePath);

            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sLocalFile)))
            {
                if (System.IO.File.Exists(sLocalFile))
                {
                    if (m_bOverwrite)
                    {
                        System.IO.File.Delete(sLocalFile);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                if (m_bCreateFolders)
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sLocalFile));
                }
                else
                {
                    return;
                }
            }

            // Get the object used to communicate with the server.
            System.Net.FtpWebRequest request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(sFTPFile);
            request.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new System.Net.NetworkCredential("anonymous", CHaMPWorkbench.Properties.Settings.Default.UserEmail);
            System.Net.FtpWebResponse response = (System.Net.FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            using (var fileStream = File.Create(sLocalFile))
            {
                responseStream.CopyTo(fileStream);
            }

            response.Close();
        }

        private void treFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode theNode = e.Node;
            CheckUncheckChildren(ref theNode);
        }

        private void CheckUncheckChildren(ref TreeNode aNode)
        {
            TreeNode aChildNode;
            foreach (TreeNode aChild in aNode.Nodes)
            {
                aChildNode = aChild;
                aChildNode.Checked = aNode.Checked;
                CheckUncheckChildren(ref aChildNode);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            grpProgress.Visible = true;
            treFiles.Height -= grpProgress.Height;
             m_sLocalRoot = txtLocalFolder.Text;
   
            m_sFiles = new List<string>();
            foreach (TreeNode aNode in treFiles.Nodes)
            {
                TreeNode aChildNode = aNode;
                string sPath = "";
                GetCheckedFiles(ref sPath, ref aChildNode);
            }

            m_sProgress = new StringBuilder(string.Format("Attempting to download {0} files...", m_sFiles.Count));
            txtProgress.Text = m_sProgress.ToString();
            backgroundWorker1.RunWorkerAsync();
        }

        private void cmdBrowseLocal_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLocalFolder.Text = frm.SelectedPath;
            }
        }
    }
}
