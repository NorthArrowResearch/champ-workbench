using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace CHaMPWorkbench.Data
{
    public partial class frmScavengeVisitTopoInfo2 : Form
    {
        private enum SearchTypes
        {
            Directory,
            File
        }

        private OleDbConnection m_dbCon;
        private string m_sStatus;
        private ScavengeProperties m_Props;

        public frmScavengeVisitTopoInfo2(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
            m_sStatus = "";
        }

        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select the top level data containing the monitoring topo data. i.e. it should contain a subfolder for each field season.";

            if (!String.IsNullOrWhiteSpace(txtMonitoringDataFolder.Text) && System.IO.Directory.Exists(txtMonitoringDataFolder.Text))
                dlg.SelectedPath = txtMonitoringDataFolder.Text;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtMonitoringDataFolder.Text = dlg.SelectedPath;
        }

        private void frmScavengeVisitTopoInfo2_Load(object sender, EventArgs e)
        {
            txtMonitoringDataFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMonitoringDataFolder.Text) || (!System.IO.Directory.Exists(txtMonitoringDataFolder.Text)))
            {
                MessageBox.Show("You must provide the path to the top level folder containing the monitoring data.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                cmdCancel.Enabled = false;
                cmdOK.Enabled = false;
                grpStatus.Visible = true;

                ScavengeProperties theResult = new ScavengeProperties();// = ScavengeVisitTopoInfo(m_dbCon, txtMonitoringDataFolder.Text, chkSetNull.Checked);

                backgroundWorker1.WorkerReportsProgress = true;
                //backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                Cursor.Current = System.Windows.Forms.Cursors.Default;
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearTopoFields()
        {
            OleDbCommand dbCom = new OleDbCommand("UPDATE CHaMP_Visits SET Folder = NULL, SurveyGDB = NULL, TopoTIN = NULL, WSTIN = NULL, HydraulicModelCSV = NULL", m_dbCon);
            dbCom.ExecuteNonQuery();
        }

        private class ScavengeProperties
        {
            public int TopoFolders { get; set; }
            public int WithVisitID { get; set; }
            public int WithVisitFiles { get; set; }
            public int WithHydro { get; set; }
        }

        private bool LookForMatchingItems(string sContainingFolderPath, string sPatternList, SearchTypes eType, out string sResult)
        {
            sResult = "";
            foreach (string aPattern in sPatternList.Split(';'))
            {
                String[] sMatches;
                if (eType == SearchTypes.Directory)
                {
                    sMatches = System.IO.Directory.GetDirectories(sContainingFolderPath, aPattern);
                }
                else
                {
                    sMatches = System.IO.Directory.GetFiles(sContainingFolderPath, aPattern);
                }

                if (sMatches.Count<String>() == 1)
                {
                    sResult = sMatches[0].Substring(sContainingFolderPath.Length + 1);
                    break;
                }
            }

            return !string.IsNullOrEmpty(sResult);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {      
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            if (chkSetNull.Checked)
                ClearTopoFields();

            string sMonitoringDataFolder = txtMonitoringDataFolder.Text;

            RBTWorkbenchDataSet ds = new RBTWorkbenchDataSet();

            RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter daWatersheds = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            daWatersheds.Connection = m_dbCon;
            daWatersheds.Fill(ds.CHAMP_Watersheds);

            RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter daSites = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            daSites.Connection = m_dbCon;
            daSites.Fill(ds.CHAMP_Sites);

            RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter daVisits = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
            daVisits.Connection = m_dbCon;
            daVisits.Fill(ds.CHAMP_Visits);

            m_sStatus = "Indexing Topo folders under the parent folder. This may take a moment...";
            backgroundWorker1.ReportProgress(0);
            string[] sAllTopoFolders = System.IO.Directory.GetDirectories(sMonitoringDataFolder, "Topo", SearchOption.AllDirectories);
            
            m_Props = new ScavengeProperties();
            foreach (string sTopoFolder in sAllTopoFolders)
            {
                if (backgroundWorker1.CancellationPending)
                    return;

                m_Props.TopoFolders += 1;
                m_sStatus = sTopoFolder;
                backgroundWorker1.ReportProgress((int) (100 * ((double) m_Props.TopoFolders / sAllTopoFolders.Count<string>())));

                System.Diagnostics.Debug.WriteLine(sTopoFolder);

                System.IO.DirectoryInfo dTopo = new System.IO.DirectoryInfo(sTopoFolder);
                string[] sVisitFolderParts = dTopo.Parent.Name.Split('_');

                if (sVisitFolderParts.Count<string>() == 2)
                {
                    int nVisitID = 0;
                    if (int.TryParse(sVisitFolderParts[1], out nVisitID))
                    {
                        m_Props.WithVisitID += 1;

                        // Got a VisitID, now look for the topo data underneath
                        RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit = ds.CHAMP_Visits.FindByVisitID(nVisitID);
                        if (rVisit is RBTWorkbenchDataSet.CHAMP_VisitsRow)
                        {
                            // The final path written to the table is the middle of the path, after the monitoring data folder and before the survey GDB
                            rVisit.Folder = dTopo.FullName.Substring(sMonitoringDataFolder.Length + 1);
                            string sResult;

                            if (LookForMatchingItems(dTopo.FullName, "*orthog*.gdb;*.gdb", SearchTypes.Directory, out sResult))
                            {
                                rVisit.SurveyGDB = sResult;
                                m_Props.WithVisitFiles+=1;
                        }
                            else
                                rVisit.SetSurveyGDBNull();

                            if (LookForMatchingItems(dTopo.FullName, "tin*", SearchTypes.Directory, out sResult))
                                rVisit.TopoTIN = sResult;
                            else
                                rVisit.SetTopoTINNull();

                            if (LookForMatchingItems(dTopo.FullName, "ws*", SearchTypes.Directory, out sResult))
                                rVisit.WSTIN = sResult;
                            else
                                rVisit.SetWSTINNull();
                            
                            // Now look for the hydraulic model artifacts
                            rVisit.SetHydraulicModelCSVNull();
                            System.IO.DirectoryInfo[] dAllHydro = dTopo.Parent.GetDirectories("Hydro");
                            if (dAllHydro.Count<System.IO.DirectoryInfo>() == 1)
                            {
                                //System.IO.DirectoryInfo[] dAllArtifacts = dAllHydro[0].GetDirectories("artifacts");
                                //if (dAllArtifacts.Count<System.IO.DirectoryInfo>() == 1)
                                {
                                    // got hydro!
                                    System.IO.FileInfo[] dAllCSVFiles = dAllHydro[0].GetFiles("dem_grid*.csv");
                                    if (dAllCSVFiles.Count<System.IO.FileInfo>() == 1)
                                    {
                                        rVisit.HydraulicModelCSV = System.IO.Path.GetFileName(dAllCSVFiles[0].FullName);
                                        m_Props.WithHydro += 1;
                                    }
                                }
                            }
                             
                        }
                    }
                }
            }

            m_sStatus = "Storing file and folder paths to the workbench database...";
            backgroundWorker1.ReportProgress(100);
            daVisits.Update(ds.CHAMP_Visits);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgrProgress.Value = e.ProgressPercentage;
            lblStatus.Text = m_sStatus;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = System.Windows.Forms.Cursors.Default;
            cmdCancel.Enabled = true;
            cmdOK.Enabled = true;
            lblStatus.Text = string.Format("{0} topo folders processed, of which {1} are within visit ID folders with {2} survey GDBs and {3} hydraulic model CSV results.",
                m_Props.TopoFolders, m_Props.WithVisitID, m_Props.WithVisitFiles, m_Props.WithHydro);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
        }
    }
}
