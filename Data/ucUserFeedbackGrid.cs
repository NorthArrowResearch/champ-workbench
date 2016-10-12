using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class ucUserFeedbackGrid : UserControl
    {
        public string DBCon { get; set; }
        public List<ListItem> VisitIDs { get; set; }

        public ucUserFeedbackGrid()
        {
            InitializeComponent();
        }

        private void ucUserFeedbackGrid_Load(object sender, EventArgs e)
        {
            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdData.MultiSelect = false;
            grdData.Dock = DockStyle.Fill;
            grdData.ReadOnly = true;
            grdData.ContextMenuStrip = cmsUserFeedback;

            if (string.IsNullOrEmpty(DBCon))
                return;

            LoadData();

            grdData.Columns["LogID"].Visible = false;
            grdData.Columns["UserName"].HeaderText = "User Name";
            grdData.Columns["WatershedName"].HeaderText = "Watershed";
            grdData.Columns["SiteName"].HeaderText = "Site";
            grdData.Columns["ItemReviewed"].HeaderText = "Item Reviewed";
            grdData.Columns["QualityRating"].HeaderText = "Quality Rating";
            grdData.Columns["VisitID"].HeaderText = "Visit";
            grdData.Columns["ReviewedOn"].HeaderText = "Reviewed On";
            grdData.Columns["ReviewedOn"].DefaultCellStyle.Format = "dd MMM yyyy";
            grdData.Columns["AddedOn"].HeaderText = "Added On";
            grdData.Columns["AddedOn"].DefaultCellStyle.Format = "dd MMM yyyy";
        }

        private void LoadData()
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string sSQL = "SELECT F.LogID, F.UserName, LookupListItems.Title AS QualityRating, F.ItemReviewed, W.WatershedName, S.SiteName, F.VisitID, F.ReviewedOn, F.AddedOn" +
                        " FROM((CHAMP_Visits AS V RIGHT JOIN(CHAMP_Watersheds AS W RIGHT JOIN LogFeedback AS F ON W.WatershedID = F.WatershedID) ON V.VisitID = F.VisitID) LEFT JOIN CHAMP_Sites AS S ON F.SiteID = S.SiteID) INNER JOIN LookupListItems ON F.QualityRatingID = LookupListItems.ItemID";

                    if (VisitIDs is List<ListItem> && VisitIDs.Count > 0)
                        sSQL = string.Format("{0} WHERE F.VisitID IN ({1})", sSQL, string.Join(",", VisitIDs.Select(n => n.Value.ToString()).ToArray()));

                    sSQL += " ORDER BY F.AddedOn DESC";

                    OleDbDataAdapter da = new OleDbDataAdapter(sSQL, dbCon);
                    DataTable ta = new DataTable();
                    da.Fill(ta);
                    grdData.DataSource = ta;
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
            }
        }

        #region Context Menu Strip Items

        private void addUserFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogItemDetails();
        }

        private void editSelectedUserFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (grdData.SelectedRows.Count == 1)
            {
                DataRowView drv = (DataRowView)grdData.SelectedRows[0].DataBoundItem;
                DataRow dr = drv.Row;
                ShowLogItemDetails((int)dr["LogID"]);
            }
        }

        private void ShowLogItemDetails(int nLogID = 0)
        {
            try
            {
                frmUserFeedback frm = new frmUserFeedback(DBCon, nLogID);
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void deleteSelectedUserFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count == 1)
            {
                DataRowView drv = (DataRowView)grdData.SelectedRows[0].DataBoundItem;
                DataRow dr = drv.Row;
                switch (MessageBox.Show("Are you sure that you want to delete the selected user feedback item? This is permanent and deleted items cannot be recovered.", "Continue?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    case DialogResult.Cancel:
                        ((Form)this.TopLevelControl).DialogResult = DialogResult.Cancel;
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Yes:
                        try
                        {
                            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                            {
                                dbCon.Open();
                                OleDbCommand dbCom = new OleDbCommand("DELETE FROM LogFeedback WHERE LogID = @LogID", dbCon);
                                dbCom.Parameters.AddWithValue("LogID", (int)dr["LogID"]);
                                dbCom.ExecuteNonQuery();
                                LoadData();
                            }
                        }
                        catch (Exception ex)
                        {
                            Classes.ExceptionHandling.NARException.HandleException(ex);
                        }
                        break;
                }
            }
        }

        #endregion

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            editSelectedUserFeedbackToolStripMenuItem.Enabled = grdData.SelectedRows.Count == 1;
            deleteSelectedUserFeedbackToolStripMenuItem.Enabled = grdData.SelectedRows.Count == 1;
        }

        private void grdData_MouseDown(object sender, MouseEventArgs e)
        {
            var hti = grdData.HitTest(e.X, e.Y);
            grdData.ClearSelection();
            grdData.Rows[hti.RowIndex].Selected = true;
        }
    }
}
