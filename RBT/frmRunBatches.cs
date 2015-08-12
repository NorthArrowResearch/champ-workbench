using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.RBT
{
    public partial class frmRunBatches : Form
    {
        private OleDbConnection m_dbCon;

        public frmRunBatches(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.RBTConsole) || !System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.RBTConsole))
            {
                MessageBox.Show("The RBT Console path is not valid. Go to Tools\\Options to set the correct path to the RBT Console executable (rbtconsole.exe).", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            System.Diagnostics.ProcessWindowStyle eWindow = (System.Diagnostics.ProcessWindowStyle)((ListItem)cboWindowStyle.SelectedItem).Value;

            Classes.RBTBatchEngine rbt = new Classes.RBTBatchEngine(m_dbCon, CHaMPWorkbench.Properties.Settings.Default.RBTConsole, eWindow);
            rbt.Run(chkScavengeResults.Checked, chkScavengeLog.Checked);
        }

        private void frmRunBatches_Load(object sender, EventArgs e)
        {
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            int nHidden = cboWindowStyle.Items.Add(new ListItem("Hidden", (int)System.Diagnostics.ProcessWindowStyle.Hidden));
            int nNormal = cboWindowStyle.Items.Add(new ListItem("Normal", (int)System.Diagnostics.ProcessWindowStyle.Normal));
            cboWindowStyle.SelectedIndex = nHidden;
         
            int nRuns=0;
            using (System.Data.OleDb.OleDbCommand dbCom = new System.Data.OleDb.OleDbCommand("SELECT Count(RBT_BatchRuns.Run) AS CountOfRun" +
                " FROM RBT_Batches RIGHT JOIN RBT_BatchRuns ON RBT_Batches.ID = RBT_BatchRuns.BatchID" +
                " WHERE (RBT_BatchRuns.Run <> 0) OR (RBT_Batches.Run <> 0)", m_dbCon))
            {
                nRuns = (int)dbCom.ExecuteScalar();
             }

            if (nRuns < 1)
            {
                MessageBox.Show("There are no runs queued to run. Use the Select Batch Runs tool to queue batch runs for processing.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            else
                lblMessage.Text = "Are you sure that you want to run the RBT on the " + nRuns.ToString("#,##0") + " RBT runs that are queued?";


        }
    }
}
