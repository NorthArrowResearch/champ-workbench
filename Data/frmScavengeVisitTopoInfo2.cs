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
        private OleDbConnection m_dbCon;

        public frmScavengeVisitTopoInfo2(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
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
                ScavengeProperties theResult = ScavengeVisitTopoInfo(m_dbCon, txtMonitoringDataFolder.Text, chkSetNull.Checked);
                Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(string.Format("{0} topo folders found, of which {1} are within visit ID folders, of which {2} contain topo data files. {3} hydraulic model CSV result files found.",
                    theResult.TopoFolders, theResult.WithVisitID, theResult.WithVisitFiles, theResult.WithHydro)
                    , CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor.Current = System.Windows.Forms.Cursors.Default;
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private ScavengeProperties ScavengeVisitTopoInfo(OleDbConnection dbCon, string sMonitoringDataFolder, bool bSetMissingDataNULL)
        {
            if (dbCon.State == ConnectionState.Closed)
                dbCon.Open();

            if (bSetMissingDataNULL)
                ClearTopoFields();

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

            string[] sAllTopoFolders = System.IO.Directory.GetDirectories(sMonitoringDataFolder, "Topo", SearchOption.AllDirectories);
            ScavengeProperties theResult = new ScavengeProperties();


            foreach (string sTopoFolder in sAllTopoFolders)
            {
                theResult.TopoFolders += 1;
                System.Diagnostics.Debug.WriteLine(sTopoFolder);

                System.IO.DirectoryInfo dTopo = new System.IO.DirectoryInfo(sTopoFolder);
                string[] sVisitFolderParts = dTopo.Parent.Name.Split('_');

                if (sVisitFolderParts.Count<string>() == 2)
                {
                    int nVisitID = 0;
                    if (int.TryParse(sVisitFolderParts[1], out nVisitID))
                    {
                        theResult.WithVisitID += 1;

                        // Got a VisitID, now look for the topo data underneath
                        RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit = ds.CHAMP_Visits.FindByVisitID(nVisitID);
                        if (rVisit is RBTWorkbenchDataSet.CHAMP_VisitsRow)
                        {
                            // The final path written to the table is the middle of the path, after the monitoring data folder and before the survey GDB
                            rVisit.Folder = dTopo.FullName.Substring(sMonitoringDataFolder.Length);
                            rVisit.SetSurveyGDBNull();
                            rVisit.SetTopoTINNull();
                            rVisit.SetWSTINNull();
                            string sSurveyGDB, sTopoTIN, sWSTIN;

                            if (LookForTopoFiles(dTopo.FullName, out sSurveyGDB, out sTopoTIN, out sWSTIN))
                            {
                                theResult.WithVisitFiles += 1;
                                rVisit.SurveyGDB = System.IO.Path.GetFileName(sSurveyGDB);
                                rVisit.TopoTIN = System.IO.Path.GetFileName(sTopoTIN);
                                rVisit.WSTIN = System.IO.Path.GetFileName(sWSTIN);
                            }

                            // Now look for the hydraulic model artifacts
                            rVisit.SetHydraulicModelCSVNull();
                            System.IO.DirectoryInfo[] dAllHydro = dTopo.Parent.GetDirectories("Hydro");
                            if (dAllHydro.Count<System.IO.DirectoryInfo>() == 1)
                            {
                                System.IO.DirectoryInfo[] dAllArtifacts = dAllHydro[0].GetDirectories("artifacts");
                                if (dAllArtifacts.Count<System.IO.DirectoryInfo>() == 1)
                                {
                                    // got hydro!
                                    System.IO.FileInfo[] dAllCSVFiles = dAllArtifacts[1].GetFiles("*.csv");
                                    if (dAllCSVFiles.Count<System.IO.FileInfo>() == 1)
                                    {
                                        rVisit.HydraulicModelCSV = dAllCSVFiles[1].FullName.Substring(dTopo.FullName.Length);
                                        theResult.WithHydro += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            daVisits.Update(ds.CHAMP_Visits);
            return theResult;
        }

        private void ClearTopoFields()
        {
            OleDbCommand dbCom = new OleDbCommand("UPDATE CHaMP_Visits SET Folder = NULL, SurveyGDB = NULL, TopoTIN = NULL, WSTIN = NULL, HydraulicModelCSV = NULL", m_dbCon);
            dbCom.ExecuteNonQuery();
        }

        private bool LookForTopoFiles(string sVisitTopoFolder, out string sSurveyGDB, out string sTopoTIN, out string sWSTIN)
        {
            sSurveyGDB = "";
            String[] sGDBs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "*orthog*.gdb");
            if (sGDBs.Count<String>() > 0)
            {
                if (sGDBs.Count<String>() == 1)
                    sSurveyGDB = System.IO.Path.Combine(sVisitTopoFolder, sGDBs[0]);
                else
                {
                    // multiple orthog GDBs
                }
            }
            else
            {
                String[] sAnyGDBs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "*.gdb");
                if (sAnyGDBs.Count<String>() > 0)
                {
                    if (sAnyGDBs.Count<String>() == 1)
                        sSurveyGDB = System.IO.Path.Combine(sVisitTopoFolder, sAnyGDBs[0]);
                    else
                    {
                        // multiple orthog GDBs
                        System.Diagnostics.Debug.Print("Warning: Skipping multiple GDBs in " + sVisitTopoFolder);
                    }
                }
            }
            System.Diagnostics.Debug.Print(sSurveyGDB);

            // Topo TIN
            sTopoTIN = "";
            String[] sTopoDirs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "tin*");
            if (sTopoDirs.Count<String>() > 0)
                sTopoTIN = sTopoDirs[0];

            // WS TIN
            sWSTIN = "";
            String[] sWSDirs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "ws*");
            if (sWSDirs.Count<String>() > 0)
                sWSTIN = sWSDirs[0];

            return (!String.IsNullOrWhiteSpace(sSurveyGDB) && !String.IsNullOrWhiteSpace(sTopoTIN) && !String.IsNullOrWhiteSpace(sWSTIN));
        }

        private class ScavengeProperties
        {
            public int TopoFolders { get; set; }
            public int WithVisitID { get; set; }
            public int WithVisitFiles { get; set; }
            public int WithHydro { get; set; }
        }
    }
}
