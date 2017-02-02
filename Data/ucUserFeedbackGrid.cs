using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class ucUserFeedbackGrid : UserControl
    {
        public string DBCon { get; set; }
        public List<naru.db.NamedObject> VisitIDs { get; set; }

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
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string sVisitIDs = string.Empty;
                    if (VisitIDs != null && VisitIDs.Count > 0)
                        sVisitIDs = string.Format("WHERE V.VisitID IN ({0})", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                    string sSQL = string.Format("SELECT L.*, W.WatershedName AS WatershedName, S.SiteName as SiteName, LI.Title AS QualityRating FROM LogFeedback L" +
                        " INNER JOIN LookupListItems LI ON L.QualityRatingID = LI.ItemID" +
                        " LEFT JOIN CHaMP_Watersheds W ON L.WatershedID = W.WatershedID" +
                        " LEFT JOIN CHaMP_Sites S ON L.SiteID = S.SiteID" +
                        " LEFT JOIN CHaMP_Visits V ON L.VisitID = V.VisitID" +
                        " {0} ORDER BY AddedOn DESC", sVisitIDs);

                    SQLiteDataAdapter da = new SQLiteDataAdapter(sSQL, dbCon);
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
                            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                            {
                                dbCon.Open();
                                SQLiteCommand dbCom = new SQLiteCommand("DELETE FROM LogFeedback WHERE LogID = @LogID", dbCon);
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

        private void grdData_MouseClick(object sender, MouseEventArgs e)
        {
            var hti = grdData.HitTest(e.X, e.Y);
            grdData.ClearSelection();
            if (hti.RowY > 1 && hti.ColumnX > 0)
                grdData.Rows[hti.RowIndex].Selected = true;
        }
    }
}
