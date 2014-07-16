using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        private void createInputFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (m_dbCon != null)
            {
                frmRBTRun frm = new frmRBTRun(m_dbCon);
                frm.ShowDialog();
            }
        }
    }
}
