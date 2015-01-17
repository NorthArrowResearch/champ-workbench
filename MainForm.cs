using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CHaMPWorkbench.RBTInputFile;
using System.Deployment.Application;

namespace CHaMPWorkbench
{
    public partial class MainForm : Form
    {

        private System.Data.OleDb.OleDbConnection m_dbCon;

        public MainForm()
        {
            InitializeComponent();

            string sPath = GetDatabasePathFromConnectionString(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
            if (!String.IsNullOrWhiteSpace(sPath))
            {
                if (System.IO.File.Exists(sPath))
                {
                    m_dbCon = new System.Data.OleDb.OleDbConnection(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
                    m_dbCon.Open();
                }
            }
        }

        private string GetDatabasePathFromConnectionString(string sConnectionString)
        {
            string sPath = "";
            if (!String.IsNullOrWhiteSpace(sConnectionString))
            {
                string sKey = "Source=";
                int nStart = sConnectionString.IndexOf(sKey);
                if (nStart > 0)
                {
                    int nEnd = sConnectionString.IndexOf(";", nStart);
                    if (nEnd > nStart)
                    {
                        sPath = sConnectionString.Substring(nStart + sKey.Length, nEnd - nStart - sKey.Length);
                        if (!System.IO.File.Exists(sPath))
                            sPath = string.Empty;
                    }
                }

            }
            return sPath;
        }

        private void UpdateMenuItemStatus(ToolStripItemCollection aMenu)
        {
            foreach (ToolStripItem subMenu in aMenu)
            {
                if (subMenu is ToolStripMenuItem)
                //if we get the desired object type.
                {
                    if (((ToolStripMenuItem)subMenu).HasDropDownItems) // if subMenu has children
                    {
                        if (subMenu.Name != "aboutToolStripMenuItem")
                            UpdateMenuItemStatus(((ToolStripMenuItem)subMenu).DropDownItems); // Call recursive Method.
                    }
                    else // Do the desired operations here.
                    {
                        switch (subMenu.Name)
                        {
                            case "optionsToolStripMenuItem":
                                break; // do nothing. Always enabled.

                            case "openDatabaseToolStripMenuItem":
                                break; // do nothing. Always enabled.

                            case "exitToolStripMenuItem":
                                break; // do nothing. Always enabled.

                            case "closeDatabaseToolStripMenuItem":
                                subMenu.Enabled = m_dbCon != null;
                                break;

                            default:
                                subMenu.Enabled = m_dbCon is System.Data.OleDb.OleDbConnection;
                                break;
                        }
                    }
                }

                // Now update the tool status strip
                if (m_dbCon is System.Data.OleDb.OleDbConnection)
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                    tssDatabasePath.Text = oCon.DataSource;
                }
                else
                    tssDatabasePath.Text = string.Empty;
            }
        }

        private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select " + CHaMPWorkbench.Properties.Resources.MyApplicationNameLong + " Database";
            dlg.Filter = "Access Databases (*.mdb, *.accdb)|*.mdb;*.accdb|All Files (*.*)|*.*";

            if (m_dbCon is System.Data.OleDb.OleDbConnection)
            {
                System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
                dlg.FileName = System.IO.Path.GetFileName(oCon.DataSource);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.DBConnection))
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
                    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
                    dlg.FileName = System.IO.Path.GetFileName(oCon.DataSource);
                }
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (m_dbCon is System.Data.OleDb.OleDbConnection)
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                    if (string.Compare(dlg.FileName, oCon.DataSource, true) == 0)
                        return;
                    else
                        m_dbCon.Close();
                }

                String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + dlg.FileName);

                try
                {
                    Console.WriteLine("Attempting to open database: " + sDB);
                    m_dbCon = new System.Data.OleDb.OleDbConnection(sDB);
                    m_dbCon.Open();
                    CHaMPWorkbench.Properties.Settings.Default.DBConnection = sDB;
                    CHaMPWorkbench.Properties.Settings.Default.Save();
                    UpdateMenuItemStatus(menuStrip1.Items);
                }
                catch (Exception ex)
                {
                    m_dbCon = null;
                    MessageBox.Show("Error opening database: " + sDB, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void closeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_dbCon = null;
            GC.Collect();
            UpdateMenuItemStatus(menuStrip1.Items);
        }

        private void individualFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dbCon != null)
            {
                frmRBTInputSingle frm = new frmRBTInputSingle(m_dbCon);
            }
        }

        private void batchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dbCon != null)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(m_dbCon);
                frm.ShowDialog();
            }
        }

        private void scavengeVisitInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmScavengeVisitTopoInfo2 frm = new Data.frmScavengeVisitTopoInfo2(m_dbCon);
            frm.ShowDialog();
        }

        private void selectBatchesToRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelectRBTBatches frm = new frmSelectRBTBatches(m_dbCon);
            frm.ShowDialog();
        }

        private void runRBTConsoleBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RBT.frmRunBatches frm = new RBT.frmRunBatches(m_dbCon);
            frm.ShowDialog();
        }

        private void scavengeRBTResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHaMPWorkbench.frmRBTScavenger rbt = new frmRBTScavenger(m_dbCon);
            rbt.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOptions frm = new frmOptions();
            frm.ShowDialog();
        }

        private void singleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRBTInputSingle frm = new frmRBTInputSingle(m_dbCon);
            frm.ShowDialog();
        }

        private void batchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmRBTInputBatch frm = new frmRBTInputBatch(m_dbCon);
            frm.ShowDialog();
        }

        private void scavengeVisitTopoDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmScavengeVisitTopoInfo2 frm = new Data.frmScavengeVisitTopoInfo2(m_dbCon);
            frm.ShowDialog();
        }

        private void unpackMonitoringData7ZipArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmDataUnPacker frm = new Data.frmDataUnPacker();
            frm.ShowDialog();
        }

        private void aboutTheCHaMPWorkbenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout(m_dbCon);
            frm.ShowDialog();
        }

        private void cHaMPWorkbenchWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(CHaMPWorkbench.Properties.Resources.WebSiteURL);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dbCon is System.Data.OleDb.OleDbConnection)
                    if (m_dbCon.State == ConnectionState.Open)
                        m_dbCon.Close();
            }
            catch (Exception ex)
            {
                // Do nothing. Let the application quitting try to release DB connection & resoures.
            }

            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateMenuItemStatus(menuStrip1.Items);
        }

        private void scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmImportCHaMPInfo frm = new Data.frmImportCHaMPInfo(m_dbCon);
            frm.ShowDialog();
        }

        private void prepareDatabaseForDeploymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmClearDatabase frm = new Data.frmClearDatabase(m_dbCon);
            frm.ShowDialog();
        }

        private void aboutExperimentalToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This menu contains tools that are still under development, or only intended for a select number of people. Developers should" +
                " place experimental tools under their own name until they are robust and tested.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void hydroModelInputGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Kelly.frmHydroModelInputs frm = new Experimental.Kelly.frmHydroModelInputs(m_dbCon);
            frm.ShowDialog();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;
            Cursor.Current = Cursors.WaitCursor;

            if ((ApplicationDeployment.IsNetworkDeployed))
            {
                ApplicationDeployment AD = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = AD.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time.\n\nPlease check your network connection, or try again later. Error: " + dde.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong);
                    return;
                }

                if ((info.UpdateAvailable))
                {
                    bool doUpdate = true;

                    if ((!info.IsUpdateRequired))
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if ((!(System.Windows.Forms.DialogResult.OK == dr)))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " + "version to version " + info.MinimumRequiredVersion.ToString() + ". The application will now install the update and restart.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if ((doUpdate))
                    {
                        try
                        {
                            AD.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application.\n\nPlease check your network connection, or try again later.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("The application is not deployed over the internet and therefore cannot be updated automatically.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }

        private void openDatabaseInAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dbCon is System.Data.OleDb.OleDbConnection)
            {
                string sPath = GetDatabasePathFromConnectionString(m_dbCon.ConnectionString);
                if (!string.IsNullOrWhiteSpace(sPath))
                    System.Diagnostics.Process.Start(sPath);
            }
        }

        private void testXPathReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Philip.frmTestXPath frm = new Experimental.Philip.frmTestXPath(m_dbCon);
            frm.ShowDialog();
        }

        private void queueBridgeCreekBatchesRBTRunsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Philip.frmBridgeBatchRuns frm = new Experimental.Philip.frmBridgeBatchRuns(m_dbCon);
            frm.ShowDialog();
        }

        private void findVisitByIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmFindVisitByID frm = new Data.frmFindVisitByID(m_dbCon);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.VisitToCreateInputFile > 0)
                {
                    frmRBTInputSingle frmInput = new frmRBTInputSingle(m_dbCon, frm.VisitToCreateInputFile);
                    frm.ShowDialog();
                }
            }
        }

        private void extractRBTErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Kelly.frmExtractRBTErrors frm = new Experimental.Kelly.frmExtractRBTErrors(m_dbCon);
            frm.ShowDialog();
        }

        private void generateBatchHabitatProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Habitat.frmHabitatBatch frm = new Habitat.frmHabitatBatch(m_dbCon);
            frm.ShowDialog();
        }
    }
}
