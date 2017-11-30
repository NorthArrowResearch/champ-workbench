using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using CHaMPWorkbench.CHaMPData;
using System.Net;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CHaMPWorkbench.Data.APIFiles
{
    public partial class frmAPIDownloadVisitFiles : Form
    {
        private Dictionary<long, BindingList<VisitWithFiles>> Visits;
        private Dictionary<long, CHaMPData.Program> Programs;
        private Dictionary<long, GeoOptix.API.ApiHelper> APIHelpers;

        private BackgroundWorker jobWorker;

        private ConcurrentQueue<Job> cq = new ConcurrentQueue<Job>();
        private int _totalJobs;

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
                Visits[kvp.Key] = new BindingList<VisitWithFiles>();

            foreach (VisitBasic aVisit in lVisits)
            {
                List<APIFileFolder> visitfilefolders = APIFileFolder.Load(naru.db.sqlite.DBCon.ConnectionString, aVisit.ID);
                Visits[aVisit.ProgramID].Add(new VisitWithFiles(aVisit, visitfilefolders, Programs[aVisit.ProgramID]));
            }

            // Now hook up asynchronous job eventhandlers to the right connectors
            // We do the unassign first to make sure we don't get two handles 
            // -= can safely be called even when it doesn't exist
            Job.Logger -= loggerhandler;
            Job.AddNewJob -= jobhandler;
            Job.IncrementJobs -= jobincrement;
            Job.BootWorker -= workerbooter;

            Job.Logger += loggerhandler;
            Job.AddNewJob += jobhandler;
            Job.IncrementJobs += jobincrement;
            Job.BootWorker += workerbooter;

            jobWorker = new BackgroundWorker();

            jobWorker.WorkerReportsProgress = true;
            jobWorker.WorkerSupportsCancellation = true;

            jobWorker.DoWork += new DoWorkEventHandler(jobWorker_DoWork);
            jobWorker.ProgressChanged += new ProgressChangedEventHandler(jobWorker_ProgressChanged);
            jobWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(jobWorker_DownloadCompleted);

        }

        // here are the handlers corresponding tot he events for Job above
        private void loggerhandler(object somesender, string msg) { AppendText(txtProgress, msg); }
        private void jobhandler(object somesender, Job job) { cq.Enqueue(job); }
        private void jobincrement() { _totalJobs++; }
        private void workerbooter() { if (jobWorker != null && !jobWorker.IsBusy) jobWorker.RunWorkerAsync(); }


        /// <summary>
        /// Form load method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFTPVisit_Load(object sender, EventArgs e)
        {
            TreeNode treParent = treFiles.Nodes.Add("File / Folder Types");
            TreeNode nodFiles = treParent.Nodes.Add("Files");
            TreeNode nodFolders = treParent.Nodes.Add("Visit Folders");
            TreeNode nodFieldFolders = treParent.Nodes.Add("Field Folders");

            // reset the overall status
            _totalJobs = 0;

            // Now we need to untangle the unique values to build the tree
            List<Tuple<string, APIFileFolder.APIFileFolderType>> ffCombinations = Visits.SelectMany(v => v.Value).SelectMany(k => k.FilesAndFolders)
                .Select(r => new Tuple<string, APIFileFolder.APIFileFolderType>(r.Name, r.GetAPIFileFolderType)).Distinct().ToList();

            // For each unique value in each of the 3 types build us a nice looking tree
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

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder) &&
                Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder))
                txtLocalFolder.Text = CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder;
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

        private void setControlsEnabled(bool val)
        {
            // Add this to the jobs needing doing
            SetEnabled(cmdOK, val);
            SetEnabled(chkCreateDir, val);
            SetEnabled(chkOverwrite, val);
        }

        /// <summary>
        /// The "Ok" button does a lot of the work of this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            grpProgress.Visible = true;
            _totalJobs = 0;
            txtProgress.Text = "";

            setControlsEnabled(false);

            if (string.IsNullOrEmpty(txtLocalFolder.Text) || !System.IO.Directory.Exists(txtLocalFolder.Text))
            {
                MessageBox.Show("You must specify the top level local folder where you want to download data.", "Missing Top Level Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                setControlsEnabled(true);
                return;
            }

            if (!naru.web.CheckForInternetConnection())
            {
                MessageBox.Show("Check that you are currently connected to the Internet and try again.", "No Internet Connection.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                setControlsEnabled(true);
                return;
            }

            if (CredentialsForm == null)
                CredentialsForm = new frmKeystoneCredentials();

            // We need to ask for Sitka API permissions before continuing
            DialogResult eCredentialsResult = CredentialsForm.ShowDialog();
            switch (eCredentialsResult)
            {
                case DialogResult.Cancel:
                    //this.DialogResult = DialogResult.Cancel;
                    setControlsEnabled(true);
                    return;
            }

            UserName = CredentialsForm.UserName;
            Password = CredentialsForm.Password;

            // Now go create some low-level API helpers: one for each program
            foreach (KeyValuePair<long, CHaMPData.Program> kvp in Programs)
                APIHelpers[kvp.Key] = new GeoOptix.API.ApiHelper(kvp.Value.API, Programs[kvp.Key].Keystone,
                    Properties.Settings.Default.GeoOptixClientID,
                    Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(),
                    UserName, Password);

            // Get alll the checked folders and files to download
            TopLevelLocalFolder = new System.IO.DirectoryInfo(txtLocalFolder.Text);
            foreach (TreeNode aNode in treFiles.Nodes)
                GetCheckedFiles("", aNode);

            txtProgress.Text = String.Format("Attempting to download {0} files...", FileCount);

            // Clear the Queue
            cq = new ConcurrentQueue<Job>();

            // Loop over all visit lists, one per program
            foreach (KeyValuePair<long, BindingList<VisitWithFiles>> kvp in Visits)
            {
                foreach (VisitWithFiles aVisit in kvp.Value)
                {
                    // We need an API helper for each visit, however, they reuse the API token from their respective programs
                    string visitURL = string.Format(@"{0}/visits/{1}", Programs[kvp.Key].API, aVisit.ID);
                    GeoOptix.API.ApiHelper api = new GeoOptix.API.ApiHelper(visitURL, APIHelpers[aVisit.ProgramID].AuthToken);

                    // Now go try to download every type of thing we can currently see (files and two kinds of folders)
                    foreach (APIFileFolder ff in aVisit.FilesAndFolders.Where(ff => _checkedNamesPaths.ContainsKey(ff.Name)).ToList())
                    {
                        //APIDownload(ff, api, TopLevelLocalFolder.FullName, Path.Combine(aVisit.VisitFolderRelative, _checkedNamesPaths[ff.Name]));
                        string sRelativePath = Path.Combine(aVisit.VisitFolderRelative, _checkedNamesPaths[ff.Name]);

                        FileInfo filefolderpath = new FileInfo(Path.Combine(TopLevelLocalFolder.FullName, sRelativePath));
                        // Get the object used to communicate with the server.
                        switch (ff.GetAPIFileFolderType)
                        {
                            case APIFileFolder.APIFileFolderType.FILE:
                                cq.Enqueue(new DownloadJob(ff, filefolderpath, api, sRelativePath, chkCreateDir.Checked, chkOverwrite.Checked));
                                break;

                            case APIFileFolder.APIFileFolderType.FOLDER:
                                cq.Enqueue(new GetFolderFilesJob(ff, filefolderpath, api, sRelativePath, chkCreateDir.Checked, chkOverwrite.Checked));
                                break;

                            case APIFileFolder.APIFileFolderType.FIELDFOLDER:
                                cq.Enqueue(new GetFieldFolderFilesJob(ff, filefolderpath, api, sRelativePath, chkCreateDir.Checked, chkOverwrite.Checked));
                                break;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Cancel all the queues when we drop out of this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            // clear the queue
            //if (jobWorker != null && jobWorker.IsBusy)  jobWorker.CancelAsync();
            //cq = new ConcurrentQueue<Job>();
        }

        /// <summary>
        /// Browse handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdBrowseLocal_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            if (frm.ShowDialog() == DialogResult.OK)
                txtLocalFolder.Text = frm.SelectedPath;
        }

        /// <summary>
        /// This is the asynchronous job handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Job localJob;
            List<Task> workerlist = new List<Task>();
            while (cq.TryDequeue(out localJob))
            {
                localJob.ProgressChanged += wc_DownloadProgressChanged;
                try
                {
                    Task.WaitAll(localJob.Run());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EXCEPTION");
                }
                // Report something having changed.
                jobWorker.ReportProgress(0);
            }
        }

        /// <summary>
        /// Handle the event of something being changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int jobsleft = _totalJobs - cq.Count();
            SetText(lblOverallProgress, String.Format("{0}/{1}", jobsleft, _totalJobs));
            int totalProg = (int)(100 * jobsleft / (double)_totalJobs);
            SetValue(progressOverall, totalProg);
        }

        /// <summary>
        /// When the job worker is complete we show an alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobWorker_DownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (MessageBox.Show("Process complete. Do you want to explore the local, download folder?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                case DialogResult.Cancel:
                    DialogResult = DialogResult.Cancel;
                    return;
                case DialogResult.Yes:
                    Process.Start(txtLocalFolder.Text);
                    break;

                default:
                    // No. Do nothing
                    break;
            }
            progressFile.Value = 100;
            lblProgress2.Text = "File";
            lblOverallProgress.Text = "...";
            setControlsEnabled(true);
        }


        /// <summary>
        /// Just a nice little progress method to update the progress bars
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SetValue(progressFile, e.ProgressPercentage);

            // Getting the name of the thing we care about is surprisingly difficult
            string name = ((Uri)((TaskCompletionSource<object>)e.UserState).Task.AsyncState).AbsolutePath;
            string downloadProgress = string.Format("{2} {0} MB / {1} MB",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"), name.ToString());

            SetText(lblProgress2, downloadProgress);
        }

        #region Asynchronous UI helper functions

        delegate void SetEnabledCallback(Control ctl, bool val);
        private void SetEnabled(Control ctl, bool val)
        {
            if (ctl.InvokeRequired)
            {
                SetEnabledCallback d = new SetEnabledCallback(SetEnabled);
                Invoke(d, new object[] { ctl, val });
            }
            else
                ctl.Enabled = val;
        }

        delegate void SetTextCallback(Control ctl, string text);
        private void SetText(Control ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { ctl, text });
            }
            else
                ctl.Text = text;
        }


        delegate void AppendTextCallback(TextBox ctl, string text);
        private void AppendText(TextBox ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                AppendTextCallback d = new AppendTextCallback(AppendText);
                Invoke(d, new object[] { ctl, text });
            }
            else
                ctl.AppendText(text);
        }

        delegate void SetValueCallback(ProgressBar ctl, int value);
        private void SetValue(ProgressBar ctl, int value)
        {
            if (ctl.InvokeRequired)
            {
                SetValueCallback d = new SetValueCallback(SetValue);
                Invoke(d, new object[] { ctl, value });
            }
            else
                ctl.Value = value;
        }


        #endregion
    }
}