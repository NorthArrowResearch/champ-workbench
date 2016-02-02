using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Experimental.Philip
{
    public partial class frmBridgeBatchRuns : Form
    {
        private OleDbConnection m_dbCon;

        public frmBridgeBatchRuns(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void frmBridgeBatchRuns_Load(object sender, EventArgs e)
        {
            try
            {
                OleDbCommand dbCom = new OleDbCommand("SELECT ID, BatchName FROM RBT_Batches WHERE BatchName IS NOT NULL", m_dbCon);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    cboBatches.Items.Add(new ListItem((string)dbRead["BatchName"], (int)dbRead["ID"]));
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

            if (cboBatches.Items.Count > 0)
                cboBatches.SelectedIndex = 0;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (cboBatches.SelectedItem is ListItem)
            {
                try
                {
                    OleDbCommand dbCom = new OleDbCommand(" UPDATE CHAMP_Visits INNER JOIN RBT_BatchRuns ON CHAMP_Visits.VisitID = RBT_BatchRuns.PrimaryVisitID SET RBT_BatchRuns.Run = 1 WHERE (((CHAMP_Visits.IsBridge)<>0) AND ((RBT_BatchRuns.BatchID)=[?]))", m_dbCon);
                    dbCom.Parameters.AddWithValue("ID", ((ListItem)cboBatches.SelectedItem).Value);
                    dbCom.ExecuteNonQuery();
                    MessageBox.Show("Process completed successfully.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
            else
                MessageBox.Show("No batch selected.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
