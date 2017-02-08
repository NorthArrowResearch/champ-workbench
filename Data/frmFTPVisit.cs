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
        private BindingList<VisitWithFiles> Visits;
        private Dictionary<long, CHaMPData.Program> Programs;

        private string m_sCurrentFile;
        private bool m_bOverwrite;
        private bool m_bCreateFolders;
        private StringBuilder m_sProgress;

        private System.IO.DirectoryInfo TopLevelLocalFolder { get; set; }

        public int FileCount
        {
            get
            {
                int nFiles = 0;
                foreach (VisitWithFiles aVisit in Visits)
                    nFiles += aVisit.RelativesPaths.Count;
                return nFiles;
            }
        }

        public frmFTPVisit(List<CHaMPData.VisitBasic> lVisits)
        {
            InitializeComponent();

            Visits = new BindingList<VisitWithFiles>();
            foreach (CHaMPData.VisitBasic aVisit in lVisits)
                Visits.Add(new VisitWithFiles(aVisit));

            Programs = CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString);

            m_sCurrentFile = string.Empty;
            m_bOverwrite = false;
            m_bCreateFolders = true;
        }

        private void frmFTPVisit_Load(object sender, EventArgs e)
        {
            TreeNode nodParent = treFiles.Nodes.Add("Visit Files");
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
            lstVisits.Height = lstVisits.Height + grpProgress.Height;
            //this.Height = this.Height - grpProgress.Height;

            backgroundWorker1.WorkerReportsProgress = true;
            chkCreateDir.Checked = m_bCreateFolders;
            chkOverwrite.Checked = m_bOverwrite;

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder) &&
                System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder))
                txtLocalFolder.Text = CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder;

            lstVisits.DataSource = Visits;
            lstVisits.DisplayMember = "Name";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int nFileCounter = 0;
            int nTotalFiles = FileCount;

            foreach (VisitWithFiles aVisit in Visits)
            {
                foreach (string sFile in aVisit.RelativesPaths)
                {
                    nFileCounter += 1;
                    try
                    {
                        string sRelativePath = System.IO.Path.Combine(aVisit.VisitFolderRelative, sFile);
                        FTPFile(Programs[aVisit.ProgramID].FTPURL, TopLevelLocalFolder.FullName, sRelativePath, nFileCounter, nTotalFiles);
                    }
                    catch (Exception ex)
                    {
                        m_sProgress.AppendFormat("{2}{0}, {1}", sFile, ex.Message, Environment.NewLine);
                    }

                    backgroundWorker1.ReportProgress(100 * (nFileCounter / nTotalFiles));
                }
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

            cmdOK.Enabled = true;
        }

        private void GetCheckedFiles(ref VisitWithFiles aVisit, ref string sPathSoFar, ref TreeNode aNode)
        {
            string sRelativePath = string.Empty;

            if (aNode.Parent is TreeNode)
                sRelativePath = System.IO.Path.Combine(sPathSoFar, aNode.Text);

            if (sRelativePath.Contains("."))
            {
                if (aNode.Checked)
                {
                    aVisit.RelativesPaths.Add(sRelativePath);
                }
            }
            else
            {
                // folder. Process children.
                TreeNode aChildNode;
                foreach (TreeNode aChild in aNode.Nodes)
                {
                    aChildNode = aChild;
                    GetCheckedFiles(ref aVisit, ref sRelativePath, ref aChildNode);
                }
            }
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/ms229711%28v=vs.110%29.aspx
        /// </summary>
        /// <param name="sRelativePath"></param>
        private void FTPFile(string sFTPRoot, string sRootLocalFolder, string sRelativePath, int nFileCounter, int nTotalFiles)
        {
            string sFTPFile = System.IO.Path.Combine(sFTPRoot, "ByYear", sRelativePath).Replace("\\", "/");
            System.IO.FileInfo fiLocalFile = new System.IO.FileInfo(System.IO.Path.Combine(sRootLocalFolder, sRelativePath));

            if (fiLocalFile.Directory.Exists)
            {
                if (fiLocalFile.Exists)
                {
                    if (m_bOverwrite)
                    {
                        fiLocalFile.Delete();
                    }
                    else
                    {
                        m_sProgress.AppendFormat("{0}Skipping existing {1}...", Environment.NewLine, sRelativePath);
                        return;
                    }
                }
            }
            else
            {
                if (m_bCreateFolders)
                {
                    fiLocalFile.Directory.Create();
                }
                else
                {
                    m_sProgress.AppendFormat("{0}No folder {1}...", Environment.NewLine, sRelativePath);
                    return;
                }
            }

            m_sProgress.AppendFormat("{0}Downloading {1}...", Environment.NewLine, sRelativePath);
            backgroundWorker1.ReportProgress(100 * (nFileCounter / nTotalFiles));

            // Get the object used to communicate with the server.
            System.Net.FtpWebRequest request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(sFTPFile);
            request.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new System.Net.NetworkCredential("anonymous", CHaMPWorkbench.Properties.Settings.Default.UserEmail);
            System.Net.FtpWebResponse response = (System.Net.FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            using (var fileStream = File.Create(fiLocalFile.FullName))
            {
                responseStream.CopyTo(fileStream);
            }

            m_sProgress.Append(" success");
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
            lstVisits.Height -= grpProgress.Height;
            m_bOverwrite = chkOverwrite.Checked;
            m_bCreateFolders = chkCreateDir.Checked;

            if (string.IsNullOrEmpty(txtLocalFolder.Text) || !System.IO.Directory.Exists(txtLocalFolder.Text))
            {
                MessageBox.Show("You must specifcy the top level local folder where you want to download data.", "Missing Top Level Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }
            TopLevelLocalFolder = new System.IO.DirectoryInfo(txtLocalFolder.Text);

            for (int i = 0; i < Visits.Count; i++)
            {
                VisitWithFiles aVisit = Visits[i];

                foreach (TreeNode aNode in treFiles.Nodes)
                {
                    TreeNode aChildNode = aNode;
                    string sPath = aVisit.VisitFolderRelative;
                    GetCheckedFiles(ref aVisit, ref sPath, ref aChildNode);
                }
            }

            try
            {
                m_sProgress = new StringBuilder(string.Format("Attempting to download {0} files...", FileCount));
                txtProgress.Text = m_sProgress.ToString();
                cmdOK.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void cmdBrowseLocal_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLocalFolder.Text = frm.SelectedPath;
            }
        }

        private class VisitWithFiles : CHaMPData.VisitBasic
        {
            public BindingList<CHaMPData.VisitBasic> visits { get; internal set; }

            public List<string> RelativesPaths { get; internal set; }

            public string Source { get; internal set; }
            public System.IO.FileInfo Destination { get; internal set; }

            public VisitWithFiles(CHaMPData.VisitBasic aVisit) : base(aVisit)
            {
                RelativesPaths = new List<string>();
            }
        }
    }
}