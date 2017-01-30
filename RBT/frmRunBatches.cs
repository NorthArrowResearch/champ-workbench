using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.RBT
{
    public partial class frmRunBatches : Form
    {
        public frmRunBatches()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRBTConsole.Text) || !System.IO.File.Exists(txtRBTConsole.Text))
            {
                MessageBox.Show("The RBT Console path is not valid. Go to Tools\\Options to set the correct path to the RBT Console executable (rbtconsole.exe).", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            System.Diagnostics.ProcessWindowStyle eWindow = (System.Diagnostics.ProcessWindowStyle)((ListItem)cboWindowStyle.SelectedItem).Value;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            Classes.RBTBatchEngine rbt = new Classes.RBTBatchEngine(txtRBTConsole.Text, eWindow);
            rbt.Run(chkScavengeResults.Checked, chkScavengeLog.Checked);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void frmRunBatches_Load(object sender, EventArgs e)
        {
            int nHidden = cboWindowStyle.Items.Add(new ListItem("Hidden", (int)System.Diagnostics.ProcessWindowStyle.Hidden));
            int nNormal = cboWindowStyle.Items.Add(new ListItem("Normal", (int)System.Diagnostics.ProcessWindowStyle.Normal));
            cboWindowStyle.SelectedIndex = nHidden;

            if (string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.RBTConsole) || !System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.RBTConsole))
            {
                MessageBox.Show("The RBT Console path is not valid. Go to Tools\\Options to set the correct path to the RBT Console executable (rbtconsole.exe).", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            else
                txtRBTConsole.Text = CHaMPWorkbench.Properties.Settings.Default.RBTConsole;

            int nRuns = 0;
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                SQLiteCommand dbCom = new SQLiteCommand("SELECT Count(Model_BatchRuns.Run) AS CountOfRun" +
                " FROM Model_Batches RIGHT JOIN Model_BatchRuns ON Model_Batches.ID = Model_BatchRuns.BatchID" +
                " WHERE (Model_BatchRuns.Run <> 0)", dbCon);

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

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog frm = new OpenFileDialog();
            frm.Title = "RBT Console Path";
            if (!String.IsNullOrWhiteSpace(txtRBTConsole.Text) && System.IO.File.Exists(txtRBTConsole.Text))
            {
                frm.InitialDirectory = System.IO.Path.GetDirectoryName(txtRBTConsole.Text);
                frm.FileName = System.IO.Path.GetFileName(txtRBTConsole.Text);
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtRBTConsole.Text = frm.FileName;
        }
    }
}
