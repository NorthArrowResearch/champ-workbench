using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class frmClearDatabase : Form
    {
        private OleDbConnection m_dbCon;

        public frmClearDatabase(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Classes.ClearDatabase clr = new Classes.ClearDatabase(m_dbCon);
            if (chkRBTBatches.Checked)
                clr.AddSQLStatementToClear("DELETE FROM RBT_Batches", "RBT batches cleared");

            if (chkRBTLogs.Checked)
                clr.AddSQLStatementToClear("DELETE FROM LogFiles", "RBT log files cleared");

            if (chkRBTMetrics.Checked)
            {
                clr.AddSQLStatementToClear("DELETE FROM Metric_SiteMetrics WHERE ScavengeTypeID <> 2", "RBT metrics cleared"); // Does not = validation data
                clr.AddSQLStatementToClear("DELETE FROM Metric_Results WHERE ScavengeTypeID <> 2", "RBT metrics (normalized) cleared"); // Does not = validation data (new, normalized tables)
            }

            if (chkManulMetrics.Checked)
            {
                DialogResult eResult = MessageBox.Show("Are you sure that you want to empty the database of all manual, validation metric data?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (eResult)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        clr.AddSQLStatementToClear("DELETE FROM Metric_SiteMetrics WHERE ScavengeTypeID = 2", "RBT validation metrics cleared"); // Is validation data
                        clr.AddSQLStatementToClear("DELETE FROM Metric_Results WHERE ScavengeTypeID = 2", "RBT validation metrics (normalized) cleared"); // Is validation data (and normalized)
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
                    string sSuccess = "";
                    string sErrors = "";
                    clr.DoClear(ref sSuccess, ref sErrors);

                    string sMessage = string.Format("Process complete. {0}.", sSuccess);

                    if (!string.IsNullOrWhiteSpace(sErrors))
                        sMessage += " Errors were encountered clearing the following tables: " + sErrors;

                    MessageBox.Show(sMessage, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
