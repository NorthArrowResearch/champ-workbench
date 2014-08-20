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
                clr.AddTableToClear("RBT_Batches");

            if (chkRBTLogs.Checked)
                clr.AddTableToClear("LogFiles");

            if (chkRBTMetrics.Checked)
                clr.AddTableToClear("Metric_SiteMetrics");

            if (chkWatersheds.Checked)
                clr.AddTableToClear("CHaMP_Watersheds");

            if (clr.TableCount > 0)
            {
                string sSuccess = "";
                string sErrors = "";
                clr.DoClear(ref sSuccess, ref sErrors);

                string sMessage = "Process complete.";
                sMessage += " The following tables were cleared " + sSuccess;

                if (!string.IsNullOrWhiteSpace(sErrors))
                    sMessage += " Errors were encountered clearing the following tables: " + sErrors;

                MessageBox.Show(sMessage, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("No tables to process.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
