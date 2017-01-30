using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.UserQueries
{
 
    public partial class frmManageQueries : Form
    {
        private string DBCon { get; set; }
   
        // Intialized as false and set to true whenever any query is added, edited or deleted
        public bool UserQueriesChanged { get; internal set;}

        public frmManageQueries(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;
            UserQueriesChanged = false;
        }

        private void frmManageQueries_Load(object sender, EventArgs e)
        {
            LoadData();

        }

        private void LoadData()
        {
            grdData.AutoGenerateColumns = false;
            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdData.MultiSelect = false;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT User_Queries.QueryID, User_Queries.Title, Left([QueryText],50) AS QueryText, User_Queries.CreatedOn FROM User_Queries ORDER BY Title", dbCon);
                DataTable ta = new DataTable();
                da.Fill(ta);

                grdData.DataSource = ta;
            }

            grdData_SelectionChanged(null, null);
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmQueryProperties frm = new frmQueryProperties(DBCon, 0);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UserQueriesChanged = true;
                LoadData();
            }
        }

        private void cmdProperties_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count > 0)
            {
                DataRowView selRow = (DataRowView)grdData.SelectedRows[0].DataBoundItem;

                frmQueryProperties frm = new frmQueryProperties(DBCon, (int)selRow.Row["QueryID"]);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UserQueriesChanged = true;
                    LoadData();
                }
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count > 0)
            {
                DataRowView selRow = (DataRowView)grdData.SelectedRows[0].DataBoundItem;


                switch (MessageBox.Show("Are you sure that you want to delete the selected user query?", "Confirm Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    case System.Windows.Forms.DialogResult.No:
                        return;

                    case System.Windows.Forms.DialogResult.Cancel:
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        return;
                        break;
                }

                try
                {
                    using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                    {
                        dbCon.Open();

                        SQLiteCommand dbCom = new SQLiteCommand("DELETE FROM User_Queries WHERE QueryID = @QueryID", dbCon);
                        dbCom.Parameters.AddWithValue("@ID", selRow.Row["QueryID"]);
                        dbCom.ExecuteNonQuery();

                        MessageBox.Show("User query deleted successfully.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UserQueriesChanged = true;
                        LoadData();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Deleting User Query", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void grdData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cmdProperties_Click(sender, e);
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            cmdDelete.Enabled = grdData.SelectedRows.Count == 1;
            cmdProperties.Enabled = cmdDelete.Enabled;
        }
    }
}
