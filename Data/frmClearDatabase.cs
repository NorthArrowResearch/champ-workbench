using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class frmClearDatabase : Form
    {
        public frmClearDatabase()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Classes.ClearDatabase clr = new Classes.ClearDatabase();
            if (chkRBTBatches.Checked)
                clr.AddSQLStatementToClear("DELETE FROM Model_Batches", "Model batches cleared");

            if (chkRBTLogs.Checked)
                clr.AddSQLStatementToClear("DELETE FROM LogFiles", "RBT log files cleared");

            if (chkRBTMetrics.Checked)
            {
                clr.AddSQLStatementToClear("DELETE FROM Metric_Results WHERE ScavengeTypeID <> 2", "RBT metrics (normalized) cleared");
            }

            if (chkManulMetrics.Checked)
            {
                DialogResult eResult = MessageBox.Show("Are you sure that you want to empty the database of all manual, validation metric data?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (eResult)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        clr.AddSQLStatementToClear("DELETE FROM Metric_Results WHERE ScavengeTypeID = 2", "RBT validation metrics (normalized) cleared");
                        break;

                    case System.Windows.Forms.DialogResult.No:
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                        return;

                    case System.Windows.Forms.DialogResult.Cancel:
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        return;
                }
            }

            if (chkWatersheds.Checked)
                clr.AddSQLStatementToClear("DELETE FROM CHaMP_Watersheds", "CHaMP information cleared");

            if (clr.TableCount > 0)
            {
                try
                {
                    List<string> lMessages = new List<string>();
                    List<string> lErrors = new List<string>();
                    clr.DoClear(ref lMessages, ref lErrors);

                    string sMessage = "Process Complete.";

                    if (lErrors.Count > 0)
                    {
                        sMessage = "Process Complete with errors.";
                        lMessages.Add(" ");
                        lMessages.Add("The following errors occurred:");
                        lMessages.AddRange(lErrors);
                    }

                    frmToolResults frm = new frmToolResults("CHaMP Information Cleared", sMessage, ref lMessages);
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
            else
            {
                MessageBox.Show("No tables to process.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
