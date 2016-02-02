using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.RBT.Batches
{
    public partial class frmRandomBatch : Form
    {
        private OleDbConnection m_dbCon;

        public frmRandomBatch(ref OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void frmRandomBatch_Load(object sender, EventArgs e)
        {
            try
            {
                using (OleDbCommand dbCom = new OleDbCommand("SELECT B.ID, B.BatchName, Count(R.[BatchID]) AS Expr1 FROM RBT_Batches AS B INNER JOIN RBT_BatchRuns AS R ON B.ID = R.BatchID GROUP BY B.ID, B.BatchName ORDER BY B.BatchName", m_dbCon))
                {
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                        cboBatch.Items.Add(new RBTBatch(dbRead.GetInt32(0), string.Format("{0} ({1} runs)", dbRead.GetString(1), dbRead.GetInt32(2)), dbRead.GetInt32(2)));
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

            if (cboBatch.Items.Count > 0)
                cboBatch.SelectedIndex = 0;
        }

        private class RBTBatch : ListItem
        {
            private int m_nRuns;

            public int Runs { get { return m_nRuns; } }

            public RBTBatch(int nBatchID, string sBatchName, int nRuns)
                : base(sBatchName, nBatchID)
            {
                m_nRuns = nRuns;
            }
        }

        private void cboBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBatch.SelectedItem is RBTBatch)
            {
                RBTBatch theBatch = (RBTBatch)cboBatch.SelectedItem;
                valSize.Value = Math.Min(valSize.Value, theBatch.Runs);
                valSize.Maximum = theBatch.Runs;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!(cboBatch.SelectedItem is RBTBatch))
            {
                MessageBox.Show("You must select an existing RBT batch to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }

            int nBatchID = ((RBTBatch)cboBatch.SelectedItem).Value;
            OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
            try
            {
                // Set all runs to not run. Optionally restrict this query to just the current batch.
                string sSQL = "UPDATE RBT_BatchRuns SET Run = False";
                if (rdoLeaveOtherBatches.Checked)
                    sSQL += " WHERE BatchID = @BatchID";

                OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon, dbTrans);

                if (rdoLeaveOtherBatches.Checked)
                    dbCom.Parameters.AddWithValue("@BatchID", nBatchID);

                dbCom.ExecuteNonQuery();

                sSQL = string.Format("UPDATE RBT_BatchRuns SET Run = True WHERE ID IN (SELECT TOP {0} ID from RBT_BatchRuns WHERE (BatchID = {1}) ORDER BY rnd(ID))", valSize.Value, nBatchID);
                dbCom = new OleDbCommand(sSQL, m_dbCon, dbTrans);
                dbCom.ExecuteNonQuery();

                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                Exception ex2 = new Exception("Error. No Database Changes Saved", ex);
                Classes.ExceptionHandling.NARException.HandleException(ex2);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
