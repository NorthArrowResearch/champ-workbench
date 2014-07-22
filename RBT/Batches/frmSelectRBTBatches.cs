using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench
{
    public partial class frmRunRBT : Form
    {
        private OleDbConnection m_dbCon;

        public frmRunRBT(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            using (OleDbCommand dbUpdateBatch = new OleDbCommand("UPDATE RBT_Batches SET Run = ? WHERE ID = ?", m_dbCon))
            {
                OleDbParameter pBatchRun = dbUpdateBatch.Parameters.Add("BatchRun", OleDbType.Boolean);
                OleDbParameter pBatchID = dbUpdateBatch.Parameters.Add("BatchID", OleDbType.Integer);

                using (OleDbCommand dbUpdateRun = new OleDbCommand("UPDATE RBT_BatchRuns SET Run = ? WHERE ID = ?", m_dbCon))
                {
                    OleDbParameter pBatchRunRun = dbUpdateRun.Parameters.Add("BatchRun", OleDbType.Boolean);
                    OleDbParameter pRunID = dbUpdateRun.Parameters.Add("BatchID", OleDbType.Integer);

                    foreach (TreeNode nodRoot in treBatches.Nodes)
                    {
                        foreach (TreeNode nodBatch in nodRoot.Nodes)
                        {
                            int nBatchID;
                            if (Int32.TryParse((string) nodBatch.Tag, out nBatchID))
                            {
                                pBatchRun.Value = nodBatch.Checked;
                                pBatchID.Value = nBatchID;
                                dbUpdateBatch.ExecuteNonQuery();
                            }

                            foreach (TreeNode nodRun in nodBatch.Nodes)
                            {
                                int nRunID;
                                if (Int32.TryParse((string)nodRun.Tag, out nRunID))
                                {
                                    pBatchRunRun.Value = nodRun.Checked;
                                    pRunID.Value = nRunID;
                                    dbUpdateRun.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void frmRunRBT_Load(object sender, EventArgs e)
        {
            treBatches.CheckBoxes = true;
            LoadTree();
        }

        private void LoadTree()
        {
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            //using (OleDbCommand dbBatches = new OleDbCommand("SELECT ID, BatchName, Run FROM RBT_Batches ORDER BY CreatedOn DESC", m_dbCon))
            //{
            //    using (OleDbCommand dbRuns = new OleDbCommand("SELECT ID, BatchID, Summary, Run FROM RBT_BatchRuns WHERE BatchID = ?", m_dbCon))
            //    {
            //        OleDbParameter pBatchID = dbRuns.Parameters.Add("BatchID", OleDbType.Integer);

            //        OleDbDataReader dbRead = dbBatches.ExecuteReader();
            //        while (dbRead.Read())
            //        {
            //            pBatchID.Value


            //        }
            //    }
            //}

            treBatches.Nodes.Clear();
            TreeNode nodRoot = treBatches.Nodes.Add("RBT Batches and Runs");
            
            int nBatchID = 0;
            int nNewBatchID = -1;
         TreeNode nodBatch = null;
                TreeNode nodRun = null;
   
             using (OleDbCommand dbBatches = new OleDbCommand("SELECT R.BatchID, R.ID AS RunID, B.BatchName, B.Run AS BatchRun, R.Summary, R.Run" + 
                 " FROM RBT_Batches AS B Right JOIN RBT_BatchRuns AS R ON B.ID = R.BatchID" +
                 " WHERE R.Inputfile Is Not Null" +
                 " ORDER BY R.BatchID, B.CreatedOn DESC", m_dbCon))
            {


                 OleDbDataReader dbRead = dbBatches.ExecuteReader();
                 while (dbRead.Read())
                 {
                     if (System.Convert.IsDBNull(dbRead["BatchID"]))
                         nNewBatchID = -1;
                     else
                         nNewBatchID = (int) dbRead["BatchID"];

                     if (nNewBatchID != nBatchID)
                     {
                         // Add new parent tree node

                         String sBatchName = "Unknown Batch";
                         if (!System.Convert.IsDBNull(dbRead["BatchName"]))
                             sBatchName = (string) dbRead["BatchName"];

                         nodBatch = nodRoot.Nodes.Add(sBatchName);
                         nodBatch.Tag = "b";

                         if (!System.Convert.IsDBNull(dbRead["BatchID"]))
                             nodBatch.Tag = ((int) dbRead["BatchID"]).ToString();

                         if (!System.Convert.IsDBNull(dbRead["BatchRun"]))
                             nodBatch.Checked = (bool) dbRead["BatchRun"];

                         nBatchID = nNewBatchID;
                     }

                     if (nodBatch is TreeNode)
                     {
                         string sRun = "Unknown Run";
                         if (!System.Convert.IsDBNull(dbRead["Summary"]))
                             sRun = (string)dbRead["Summary"];

                         nodRun = nodBatch.Nodes.Add(sRun);
                         nodRun.Tag = ((int)dbRead["RunID"]).ToString();
                         nodRun.Checked = (bool)dbRead["Run"];
                     }
                 }

                 nodRoot.Expand();
             }

        }

        private void treBatches_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TreeNode)
                foreach (TreeNode cNode in e.Node.Nodes)
                    cNode.Checked = e.Node.Checked;

            if (e.Node.Parent is TreeNode)
            {
                bool bAllChecked = false;
                foreach (TreeNode nodsibling in e.Node.Parent.Nodes)
                    if (nodsibling.Checked)
                        bAllChecked = true;
            }
        }
    }
}
