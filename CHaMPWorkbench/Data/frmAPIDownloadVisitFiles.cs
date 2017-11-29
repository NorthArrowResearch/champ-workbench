using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CHaMPWorkbench.CHaMPData;
using System.Net;

namespace CHaMPWorkbench.Data
{
    public partial class frmAPIDownloadVisitFiles : Form
    {
        private Dictionary<long, BindingList<VisitWithFiles>> Visits;
        private Dictionary<long, CHaMPData.Program> Programs;
        private Dictionary<long, GeoOptix.API.ApiHelper> APIHelpers;

        private string m_sCurrentFile;
        private bool m_bOverwrite;
        private bool m_bCreateFolders;
        private StringBuilder m_sProgress;

        private frmKeystoneCredentials CredentialsForm;

        public string UserName { get; internal set; }
        public string Password { get; internal set; }

        private DirectoryInfo TopLevelLocalFolder { get; set; }

        private Dictionary<string, string> _checkedNamesPaths;

        public int FileCount
        {
            get
            {
                int nFiles = 0;
                foreach (KeyValuePair<long, BindingList<VisitWithFiles>> kvp in Visits)
                    foreach (VisitWithFiles aVisit in kvp.Value)
                        nFiles += aVisit.FilesAndFolders.Count;

                return nFiles;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lVisits"></param>
        public frmAPIDownloadVisitFiles(List<VisitBasic> lVisits)
        {
            InitializeComponent();

            lblSelectedVisits.Text = String.Format("With {0} selected visits", lVisits.Count);

            Visits = new Dictionary<long, BindingList<VisitWithFiles>>();
            _checkedNamesPaths = new Dictionary<string, string>();
            Programs = CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString);
            APIHelpers = new Dictionary<long, GeoOptix.API.ApiHelper>();

            foreach (KeyValuePair<long, CHaMPData.Program> kvp in Programs)
            {
                Visits[kvp.Key] = new BindingList<VisitWithFiles>();
            }
                

            foreach (VisitBasic aVisit in lVisits)
            {
                List<APIFileFolder> visitfilefolders = APIFileFolder.Load(naru.db.sqlite.DBCon.ConnectionString, aVisit.ID);
                Visits[aVisit.ProgramID].Add(new VisitWithFiles(aVisit, visitfilefolders, Programs[aVisit.ProgramID]));
            }

            m_sCurrentFile = string.Empty;
            m_bOverwrite = false;
            m_bCreateFolders = true;
        }

        private void frmFTPVisit_Load(object sender, EventArgs e)
        {
            TreeNode treParent = treFiles.Nodes.Add("File / Folder Types");
            TreeNode nodFiles = treParent.Nodes.Add("Files");
            TreeNode nodFolders = treParent.Nodes.Add("Visit Folders");
            TreeNode nodFieldFolders = treParent.Nodes.Add("Field Folders");

            // Now we need to untangle the unique values
            List<Tuple<string, APIFileFolder.APIFileFolderType>> ffCombinations = Visits.SelectMany(v => v.Value).SelectMany(k => k.FilesAndFolders)
                .Select(r => new Tuple<string, APIFileFolder.APIFileFolderType>(r.Name, r.GetAPIFileFolderType)).Distinct().ToList();

            foreach (Tuple<string, APIFileFolder.APIFileFolderType> tupComb in ffCombinations)
                switch (tupComb.Item2)
                {
                    case APIFileFolder.APIFileFolderType.FILE:
                        nodFiles.Nodes.Add(tupComb.Item1).Tag = tupComb.Item2;
                        break;
                    case APIFileFolder.APIFileFolderType.FIELDFOLDER:
                        nodFieldFolders.Nodes.Add(tupComb.Item1).Tag = tupComb.Item2;
                        break;
                    case APIFileFolder.APIFileFolderType.FOLDER:
                        nodFolders.Nodes.Add(tupComb.Item1).Tag = tupComb.Item2;
                        break;
                }

            treParent.ExpandAll();

            treFiles.CheckBoxes = true;

            grpProgress.Visible = false;
            treFiles.Height = treFiles.Height + grpProgress.Height;

            backgroundWorker1.WorkerReportsProgress = true;
            chkCreateDir.Checked = m_bCreateFolders;
            chkOverwrite.Checked = m_bOverwrite;

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder) &&
                System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder))
                txtLocalFolder.Text = CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder;
        }

        /// <summary>
        /// Do the actual work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int nFileCounter = 0;
            int nTotalFiles = FileCount;

            foreach (KeyValuePair<long, BindingList<VisitWithFiles>> kvp in Visits)
                foreach (VisitWithFiles aVisit in kvp.Value)
                {
                    string visitURL = string.Format(@"{0}/visits/{1}", Programs[kvp.Key].API, aVisit.ID);
                    GeoOptix.API.ApiHelper api = new GeoOptix.API.ApiHelper(visitURL, APIHelpers[aVisit.ProgramID].AuthToken);

                    foreach (APIFileFolder ff in aVisit.FilesAndFolders.Where(ff => _checkedNamesPaths.ContainsKey(ff.Name)).ToList())
                    {
                        try
                        {
                            string sRelativePath = System.IO.Path.Combine(aVisit.VisitFolderRelative, _checkedNamesPaths[ff.Name]);
                            APIDownload(ff, api, TopLevelLocalFolder.FullName, sRelativePath, nFileCounter, nTotalFiles);
                        }
                        catch (Exception ex)
                        {
                            m_sProgress.AppendFormat("{2}{0}, {1}", ff.Name, ex.Message, Environment.NewLine);
                        }

                        nFileCounter += 1;
                        double fRatio = Math.Min(100.0 * (double)nFileCounter / (double)nTotalFiles, 100);
                        backgroundWorker1.ReportProgress(Convert.ToInt32(fRatio));

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

        /// <summary>
        /// Sort through all the visit files and download what we need
        /// </summary>
        /// <param name="aVisit"></param>
        /// <param name="sPathSoFar"></param>
        /// <param name="aNode"></param>
        private void GetCheckedFiles(string sPathSoFar, TreeNode aNode)
        {
            string sRelativePath = string.Empty;

            if (aNode.Parent is TreeNode)
                sRelativePath = Path.Combine(sPathSoFar, aNode.Text);

            // This is a node leaf (meaning it's a file or folder we have to do somethign with
            if (aNode.Nodes.Count == 0)
            {
                if (aNode.Checked)
                    _checkedNamesPaths[aNode.Text] = sRelativePath;
            }
            // This is a node branch so we need to recurse
            else
            {
                foreach (TreeNode aChild in aNode.Nodes)
                    GetCheckedFiles(sRelativePath, aChild);
            }
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/ms229711%28v=vs.110%29.aspx
        /// </summary>
        /// <param name="sRelativePath"></param>
        private void APIDownload(APIFileFolder ffilefolder, GeoOptix.API.ApiHelper api, string sRootLocalFolder, string sRelativePath, int nFileCounter, int nTotalFiles)
        {
            FileInfo filefolderpath = new FileInfo(Path.Combine(sRootLocalFolder, sRelativePath));
            // Get the object used to communicate with the server.
            switch (ffilefolder.GetAPIFileFolderType)
            {
                case APIFileFolder.APIFileFolderType.FILE:
                    APIDownloadfile(ffilefolder, filefolderpath, api, sRelativePath);
                    break;

                case APIFileFolder.APIFileFolderType.FOLDER:
                    m_sProgress.AppendFormat("{0}Downloading Folder {1}...", Environment.NewLine, sRelativePath);
                    foreach (APIFileFolder ff in VisitWithFiles.GetFolderFiles(ffilefolder, api))
                        APIDownloadfile(ff, new FileInfo(Path.Combine(filefolderpath.FullName, ff.Name)), api, sRelativePath);
                    break;

                case APIFileFolder.APIFileFolderType.FIELDFOLDER:
                    m_sProgress.AppendFormat("{0}Downloading Field Folder {1}...", Environment.NewLine, sRelativePath);
                    foreach (APIFileFolder ff in VisitWithFiles.GetFieldFolderFiles(ffilefolder, api))
                        APIDownloadfile(ff, new FileInfo(Path.Combine(filefolderpath.FullName, ff.Name)), api, sRelativePath);
                    break;

            }

            m_sProgress.Append(" success");
        }


        /// <summary>
        /// GeoOptix doesn't handle file downloads so this should do it.
        /// </summary>
        /// <param name="url"></param>
        public void APIDownloadfile(APIFileFolder ff, FileInfo fiLocalFile, GeoOptix.API.ApiHelper api, string sRelativePath)
        {
            m_sProgress.AppendFormat("{0}Downloading {1}...", Environment.NewLine, sRelativePath);

            if (fiLocalFile.Directory.Exists && fiLocalFile.Exists)
            {
                if (m_bOverwrite)
                    fiLocalFile.Delete();
                else
                {
                    m_sProgress.AppendFormat("{0}Skipping existing {1}...", Environment.NewLine, sRelativePath);
                    return;
                }
            }
            else
            {
                if (m_bCreateFolders)
                    fiLocalFile.Directory.Create();
                else
                {
                    m_sProgress.AppendFormat("{0}No folder {1}...", Environment.NewLine, sRelativePath);
                    return;
                }
            }

            using (WebClient wc = new WebClient())
            {
                wc.Headers["Authorization"] = "Bearer " + api.AuthToken.AccessToken;
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new Uri(string.Format("{0}?Download", ff.URL)), fiLocalFile.FullName);
            }

        }

        /// <summary>
        /// This is an example of how we can track progress for individual file download. Maybe a second progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar.Value = e.ProgressPercentage;
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
            m_bOverwrite = chkOverwrite.Checked;
            m_bCreateFolders = chkCreateDir.Checked;

            if (string.IsNullOrEmpty(txtLocalFolder.Text) || !System.IO.Directory.Exists(txtLocalFolder.Text))
            {
                MessageBox.Show("You must specifcy the top level local folder where you want to download data.", "Missing Top Level Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }

            if (!naru.web.CheckForInternetConnection())
            {
                MessageBox.Show("Check that you are currently connected to the Internet and try again.", "No Internet Connection.", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            UserName = CredentialsForm.UserName;
            Password = CredentialsForm.Password;

            foreach(KeyValuePair<long, CHaMPData.Program> kvp in Programs)
                APIHelpers[kvp.Key] = new GeoOptix.API.ApiHelper(kvp.Value.API, Programs[kvp.Key].Keystone,
                    Properties.Settings.Default.GeoOptixClientID,
                    Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(),
                    UserName, Password);

            // Get alll the checked folders and files to download
            TopLevelLocalFolder = new System.IO.DirectoryInfo(txtLocalFolder.Text);
            foreach (TreeNode aNode in treFiles.Nodes)
                GetCheckedFiles("", aNode);

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


        private class VisitWithFiles : VisitBasic
        {
            public BindingList<VisitBasic> visits { get; internal set; }
            CHaMPData.Program theProg;

            public List<APIFileFolder> FilesAndFolders;

            public FileInfo Destination { get; internal set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="aVisit"></param>
            public VisitWithFiles(VisitBasic aVisit, List<APIFileFolder> allfilefolders, CHaMPData.Program program) : base(aVisit, naru.db.DBState.Unchanged)
            {
                FilesAndFolders = allfilefolders;

                theProg = program;
            }

            public List<string> GetNames { get { return FilesAndFolders.Select(ff => ff.Name).ToList(); } }

            /// <summary>
            /// Get the list of files inside the folder using GeoOptix
            /// </summary>
            /// <param name="ff">APIFileFolder FOLDER to look into</param>
            /// <param name="api">Api helper to use for this operation</param>
            /// <returns></returns>
            public static List<APIFileFolder> GetFolderFiles(APIFileFolder ff, GeoOptix.API.ApiHelper api)
            {
                List<APIFileFolder> retVal = new List<APIFileFolder>();
                GeoOptix.API.ApiResponse<GeoOptix.API.Model.FileSummaryModel[]> filelist = api.GetFiles(ff.Name);

                if (filelist.Payload == null) return retVal;

                foreach (GeoOptix.API.Model.FileSummaryModel file in filelist.Payload)
                    retVal.Add(new APIFileFolder(file.Name, file.Url, ff.Name, true, false, naru.db.DBState.New));

                return retVal;
            }

            /// <summary>
            /// Get the list of field files inside the field folders
            /// </summary>
            /// <param name="ff">APIFileFolder FIELD FOLDER to look into</param>
            /// <param name="api">Api helper to use for this operation</param>
            /// <returns></returns>
            public static List<APIFileFolder> GetFieldFolderFiles(APIFileFolder ff, GeoOptix.API.ApiHelper api)
            {
                List<APIFileFolder> retVal = new List<APIFileFolder>();
                GeoOptix.API.ApiResponse<GeoOptix.API.Model.FileSummaryModel[]> filelist = api.GetFieldFiles(ff.Name);

                if (filelist.Payload == null) return retVal;

                foreach (GeoOptix.API.Model.FileSummaryModel file in filelist.Payload)
                    retVal.Add(new APIFileFolder(file.Name, file.Url, ff.Name, true, true, naru.db.DBState.New));

                return retVal;
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            CHaMPWorkbench.OnlineHelp.FormHelp(this.Name);
        }

    }
}