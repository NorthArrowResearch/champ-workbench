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

        private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select " + CHaMPWorkbench.Properties.Resources.MyApplicationNameLong + " Database";
            dlg.Filter = "Access Databases (*.mdb, *.accdb)|*.mdb;*.accdb|All Files (*.*)|*.*";

            if (CHaMPWorkbench.Properties.Settings.Default.DBConnection != null)
            {
                //System. DBConnection dbCon = new DBConnection(CHaMPWorkbench.Properties.Settings.Default.DBConnection );
                //dlg.InitialDirectory =  
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + dlg.FileName);
                try
                {
                    Console.WriteLine("Attempting to open database: " + sDB);
                    m_dbCon = new System.Data.OleDb.OleDbConnection(sDB);
                    m_dbCon.Open();
                    CHaMPWorkbench.Properties.Settings.Default.DBConnection = sDB;
                    CHaMPWorkbench.Properties.Settings.Default.Save();
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
            frmScavengeVisitInfo frm = new frmScavengeVisitInfo(m_dbCon);
            frm.ShowDialog();
        }

        private void selectBatchesToRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRunRBT frm = new frmRunRBT(m_dbCon);
            frm.ShowDialog();
        }

        private void runRBTConsoleBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            using (System.Data.OleDb.OleDbCommand dbCom = new System.Data.OleDb.OleDbCommand("SELECT Count(RBT_BatchRuns.Run) AS CountOfRun" +
                " FROM RBT_Batches RIGHT JOIN RBT_BatchRuns ON RBT_Batches.ID = RBT_BatchRuns.BatchID" +
                " WHERE (((RBT_BatchRuns.Run)=True)) OR (((RBT_Batches.Run)=True))", m_dbCon))
            {
                int nRuns = (int) dbCom.ExecuteScalar();

                if (nRuns < 1)
                {
                    MessageBox.Show("There are no runs queued to run. Use the Select Batch Runs tool to queue batch runs for processing.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (MessageBox.Show("Are you sure that you want to run the RBT on the " + nRuns.ToString("#,##0") + " RBT runs that are queued?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Classes.RBTBatchEngine rbt = new Classes.RBTBatchEngine(m_dbCon, CHaMPWorkbench.Properties.Settings.Default.RBTConsole);
                        rbt.Run();
                    }
                }
            }
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
            frmScavengeVisitInfo frm = new frmScavengeVisitInfo(m_dbCon);
            frm.ShowDialog();
        }

        private void unpackMonitoringData7ZipArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmDataUnPacker frm = new Data.frmDataUnPacker();
            frm.ShowDialog();
        }

        private void aboutTheCHaMPWorkbenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }
    }
}
