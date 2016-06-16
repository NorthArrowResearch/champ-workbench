﻿using System;
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
    public partial class frmSelectVisitID : Form
    {
        private string DBCon { get; set; }
        private List<int> ExistingVisitIDs;

        public int SelectedVisitID
        {
            get { return (int) valVisitID.Value; }
        }

        public frmSelectVisitID(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;

        }

        private void frmSelectVisitID_Load(object sender, EventArgs e)
        {
            valVisitID.Select(0, valVisitID.Text.Length);

            ExistingVisitIDs = new List<int>();
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT VisitID FROM CHaMP_Visits", dbCon);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    ExistingVisitIDs.Add(dbRead.GetInt32(0));
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ExistingVisitIDs.Contains((int)valVisitID.Value))
            {
                MessageBox.Show("There is no existing visit in the database with that ID. You must choose an ID for an existing visit.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
        }
    }
}