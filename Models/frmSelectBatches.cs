using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench
{
    public partial class frmSelectBatches : Form
    {
        private string DBCon;
        private int ModelTypeID;
        private string ModelType;

        /// <summary>
        /// Create a new form that allows the user to select which batches are active for a particular model
        /// </summary>
        /// <param name="sDBCon">Database connection string</param>
        /// <param name="nModelTypeID">Identifies which model the form applies to. See LookupListID = 4 (e.g. RBT = 15, GUT = 16)</param>
        public frmSelectBatches(string sDBCon, int nModelTypeID)
        {
            DBCon = sDBCon;
            InitializeComponent();
            ModelTypeID = nModelTypeID;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                using (SQLiteCommand dbUpdateRun = new SQLiteCommand("UPDATE Model_BatchRuns SET Run = ? WHERE ID = ?", dbCon))
                {
                    SQLiteParameter pBatchRunRun = dbUpdateRun.Parameters.Add("BatchRun",  DbType.Boolean);
                    SQLiteParameter pRunID = dbUpdateRun.Parameters.Add("BatchID", DbType.Int64);

                    foreach (TreeNode nodRoot in treBatches.Nodes)
                    {
                        foreach (TreeNode nodBatch in nodRoot.Nodes)
                        {
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
            Cursor.Current = Cursors.Default;
        }

        private void frmRunRBT_Load(object sender, EventArgs e)
        {

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT Title FROM LookupListItems WHERE ItemID = @ModelTypeID", dbCon);
                dbCom.Parameters.AddWithValue("@ModelTypeID", ModelTypeID);
                ModelType = (string)dbCom.ExecuteScalar();
            }

            this.Text = string.Format("{0} Batches and Runs", ModelType.Trim());

            treBatches.CheckBoxes = true;
            LoadTree();
        }

        private void LoadTree()
        {
            Cursor.Current = Cursors.WaitCursor;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                
                treBatches.Nodes.Clear();
                TreeNode nodRoot = treBatches.Nodes.Add(string.Format("{0} Batches and Runs", ModelType.Trim()));

                int nBatchID = 0;
                int nNewBatchID = -1;
                TreeNode nodBatch = null;
                TreeNode nodRun = null;

                using (SQLiteCommand dbBatches = new SQLiteCommand("SELECT R.BatchID, R.ID AS RunID, B.BatchName, R.Summary, R.Run" +
                    " FROM Model_Batches AS B Right JOIN Model_BatchRuns AS R ON B.ID = R.BatchID" +
                    " WHERE (R.Inputfile Is Not Null) AND (R.ModelTypeID = @ModelTypeID)" +
                    " ORDER BY R.BatchID, B.CreatedOn DESC", dbCon))
                {
                    dbBatches.Parameters.AddWithValue("@ModelTypeID", ModelTypeID);
                    SQLiteDataReader dbRead = dbBatches.ExecuteReader();
                    while (dbRead.Read())
                    {
                        if (System.Convert.IsDBNull(dbRead["BatchID"]))
                            nNewBatchID = -1;
                        else
                            nNewBatchID = (int)dbRead["BatchID"];

                        if (nNewBatchID != nBatchID)
                        {
                            // Add new parent tree node

                            String sBatchName = "Unknown Batch";
                            if (!System.Convert.IsDBNull(dbRead["BatchName"]))
                                sBatchName = (string)dbRead["BatchName"];

                            nodBatch = nodRoot.Nodes.Add(sBatchName);
                            nodBatch.Tag = "b";

                            if (!System.Convert.IsDBNull(dbRead["BatchID"]))
                                nodBatch.Tag = ((int)dbRead["BatchID"]).ToString();

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

            Cursor.Current = Cursors.Default;
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

        private void cmdRandom_Click(object sender, EventArgs e)
        {
            RBT.Batches.frmRandomBatch frm = new RBT.Batches.frmRandomBatch(DBCon, ModelTypeID);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadTree();
            }
        }
    }
}
