using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CHaMPWorkbench.RBTInputFile;

namespace CHaMPWorkbench
{
    public partial class MainForm : Form
    {

        private System.Data.OleDb.OleDbConnection m_dbCon;

        public MainForm()
        {
            InitializeComponent();

            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.DBConnection))
            {
                m_dbCon = new System.Data.OleDb.OleDbConnection(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
                m_dbCon.Open();
            }
        }

        private void UpdateMenuItemStatus(ToolStripItemCollection aMenu)
        {
            foreach (ToolStripItem subMenu in aMenu)
            {
                if (subMenu is ToolStripMenuItem)
                //if we get the desired object type.
                {
                    if (((ToolStripMenuItem) subMenu).HasDropDownItems) // if subMenu has children
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
                dlg.InitialDirectory =  System.IO.Path.GetDirectoryName( oCon.DataSource);
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
                frm.ShowDialog();
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
            frmScavengeVisitTopoInfo frm = new frmScavengeVisitTopoInfo(m_dbCon);
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
            frmScavengeVisitTopoInfo frm = new frmScavengeVisitTopoInfo(m_dbCon);
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
       Data.frmScavengeVisitInfo frm = new Data.frmScavengeVisitInfo(m_dbCon);
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
    }
}
