using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Xml;
using System.Xml.Linq;
using System.Data;

namespace CHaMPWorkbench.Experimental.James
{
    public partial class frmEnterPostGCD_QAQC_Record : Form
    {
        private string[] m_sErrorTypes = { "", "None", "Rod Height Bust", "Datum Shift", "Other" };
        private string[] m_sErrorDEMs = { "Niether", "NewVisit", "OldVisit", "Both", "Unknown" };

        //table name
        const string m_sTableName = "LogGCDReview";

        //table column names
        const string m_sFieldName_NewVisitID = "NewVisitID";
        const string m_sFieldName_OldVisitID = "OldVisitID";
        const string m_sFieldName_SiteID = "SiteID";
        const string m_sFieldName_SiteName = "SiteName";
        const string m_sFieldName_WatershedID = "WatershedID";
        const string m_sFieldName_WatershedName = "WatershedName";
        const string m_sFieldName_MaskValue = "MaskValueName";
        const string m_sFieldName_FlagReason = "FlagReason";
        const string m_sFieldName_ValidResults = "ValidResults";
        const string m_sFieldName_ErrorType = "ErrorType";
        const string m_sFieldName_ErrorDEM = "ErrorDEM";
        const string m_sFieldName_Comments = "Comments";
        const string m_sFieldName_EnteredBy = "EnteredBy";
        const string m_sFieldName_DateModified = "DateModified";
        const string m_sFieldName_Processed = "Processed";


        public frmEnterPostGCD_QAQC_Record()
        {
            InitializeComponent();
            LoadVisits();
        }

        private void frmEnterPostGCD_QAQC_Record_Load(object sender, EventArgs e)
        {
            //if there are rows in table get selected row and populate form
            int iSelectedCellCount = dgvGCD_Review.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (iSelectedCellCount == 1)
            {
                DataGridViewRow drv = dgvGCD_Review.Rows[dgvGCD_Review.CurrentCell.RowIndex];
                PopulateFormInfo(drv);
            }
        }

        private void LoadVisits()
        {
            if (string.IsNullOrEmpty(naru.db.sqlite.DBCon.ConnectionString))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sGroupFields = @"G.NewVisitID,G.OldVisitID,S.SiteID,S.SiteName,W.WatershedID,W.WatershedName,G.MaskValueName,G.FlagReason,G.ValidResults,G.ErrorType,G.ErrorDEM,G.Comments,G.EnteredBy,G.DateModified,G.Processed";

            string sSQL = "SELECT " + sGroupFields +
                            @" FROM (((
	                           LogGCDReview AS G 
	                           INNER JOIN 
	                           CHAMP_Visits As V 
	                           ON G.NewVisitID = V.VisitID)
	                           INNER JOIN 
                               CHAMP_Sites AS S
	                           ON V.SiteID = S.SiteID)
	                           INNER JOIN
	                           CHAMP_Watersheds AS W 
	                           ON S.WatershedID = W.WatershedID)
                               ORDER BY G.DateModified";

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);
                SQLiteDataAdapter daGCD_Review = new SQLiteDataAdapter(dbCom);
                DataTable dtGCD_Review = new DataTable();

                try
                {
                    daGCD_Review.Fill(dtGCD_Review);
                    dgvGCD_Review.DataSource = dtGCD_Review.AsDataView();

                    //hide site and watershed ids
                    dgvGCD_Review.Columns["SiteID"].Visible = false;
                    dgvGCD_Review.Columns["WatershedID"].Visible = false;

                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            if (cboErrorDEM.SelectedItem == null)
            {
                MessageBox.Show("Please select a value from the Error DEM drop-down menu.",
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (cboErrorType.SelectedItem == null && String.IsNullOrEmpty(cboErrorType.Text) == true)
            {
                MessageBox.Show("Please select a value or type a custom value in the Error Type drop-down menu.",
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(naru.db.sqlite.DBCon.ConnectionString))
                return;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                bool bSuccess = false;
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();
                try
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    string sSQL = @"UPDATE LogGCDReview
                                    SET NewVisitID = @new_visit_id,
                                    OldVisitID = @old_visit_id,
                                    MaskValueName = @mask_value_name,
                                    FlagReason = @flag_reason,
                                    ValidResults = @valid_results,
                                    ErrorType = @error_type,
                                    ErrorDEM = @error_dem,
                                    Comments = @comments,
                                    EnteredBy = @entered_by,
                                    DateModified = @date_modified,
                                    Processed = @processed
                                    WHERE NewVisitID = @new_visit_id AND OldVisitID = @old_visit_id AND MaskValueName = @mask_value_name";

                    string sErrorType = "";
                    if (cboErrorType.SelectedItem == null)
                    {
                        sErrorType = cboErrorType.Text;
                    }
                    else if (cboErrorType.SelectedItem != null)
                    {
                        sErrorType = cboErrorType.GetItemText(cboErrorType.SelectedItem);
                    }

                    SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbTrans.Connection, dbTrans);
                    dbCom.Parameters.AddWithValue("@new_visit_id", valNewVisitID.Value);
                    dbCom.Parameters.AddWithValue("@old_visit_id", valOldVisitID.Value);
                    dbCom.Parameters.AddWithValue("@mask_value_name", txtMask.Text);
                    dbCom.Parameters.AddWithValue("@flag_reason", txtReasonForFlag.Text);
                    dbCom.Parameters.AddWithValue("@valid_results", GetBooleanValidGCD_Results());
                    dbCom.Parameters.AddWithValue("@error_type", sErrorType);
                    dbCom.Parameters.AddWithValue("@error_dem", cboErrorDEM.SelectedItem.ToString());
                    dbCom.Parameters.AddWithValue("@comments", txtComments.Text);
                    dbCom.Parameters.AddWithValue("@entered_by", txtEnteredBy.Text);
                    dbCom.Parameters.AddWithValue("@date_modified", DateTime.Now.ToString());
                    dbCom.Parameters.AddWithValue("@processed", true);
                    dbCom.ExecuteNonQuery();

                    dbTrans.Commit();
                    bSuccess = true;
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
                if (bSuccess == true)
                {
                    MessageBox.Show("Record was successfully updated.",
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
            }

            LoadVisits();
        }

        private void cmdOutputToJSON_Click(object sender, EventArgs e)
        {

            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Save CHaMP Data For AWS";
            frm.Filter = "JSON Files (*.json)|*.json";

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Classes.AWSExporter aws = new Classes.AWSExporter();
                    System.IO.FileInfo fiExport = new System.IO.FileInfo(frm.FileName);

                    string sSQL_Statement = "SELECT NewVisitID, OldVisitID, MaskValueName, FlagReason, ValidResults, ErrorType, ErrorDEM, Comments, EnteredBy, DateModified, Processed " +
                        "FROM LogGCDReview " +
                        "ORDER BY DateModified";

                    int nExported = aws.Run(sSQL_Statement, fiExport);

                    if (MessageBox.Show(string.Format("{0:#,##0} records exported to file. Do you want to browse to the file created?", nExported), "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(fiExport.Directory.FullName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void PopulateComboBox(ComboBox cbo, DataGridViewRow row, string sValue, string[] sStandardValues)
        {
            cbo.Items.Clear();
            for (int i = 0; i < sStandardValues.Length; i++)
            {
                if (String.Compare(sStandardValues[i], sValue) != 0)
                {
                    cbo.Items.Add(sStandardValues[i]);
                }
                else
                {
                    cbo.Items.Add(sValue);
                    cbo.SelectedItem = sValue;
                }
            }
        }

        private void PopulateFormInfo(DataGridViewRow drv)
        {
            if (drv != null)
            {
                valNewVisitID.Value = System.Convert.ToDecimal(drv.Cells[m_sFieldName_NewVisitID].Value);
                valOldVisitID.Value = System.Convert.ToDecimal(drv.Cells[m_sFieldName_OldVisitID].Value);

                txtSite.Text = (string)drv.Cells[m_sFieldName_SiteName].Value;
                txtWatershed.Text = (string)drv.Cells[m_sFieldName_WatershedName].Value;
                txtNewVisitDate.Text = Convert.ToDateTime(GetVisitDate("CHAMP_Visits", "SampleDate", "VisitID", valNewVisitID.Value.ToString())).ToString("MM/dd/yyyy");
                txtOldVisitDate.Text = Convert.ToDateTime(GetVisitDate("CHAMP_Visits", "SampleDate", "VisitID", valOldVisitID.Value.ToString())).ToString("MM/dd/yyyy");
                txtMask.Text = (string)drv.Cells[m_sFieldName_MaskValue].Value;

                txtReasonForFlag.Text = drv.Cells[m_sFieldName_FlagReason].Value.ToString();
                if ((Boolean)drv.Cells[m_sFieldName_ValidResults].Value == true)
                {
                    rdoResultsValidTrue.Checked = true;
                }
                else if ((Boolean)drv.Cells[m_sFieldName_ValidResults].Value == false)
                {
                    rdoResultsValidFalse.Checked = true;
                }
                PopulateComboBox(cboErrorType, drv, drv.Cells[m_sFieldName_ErrorType].Value.ToString(), m_sErrorTypes);
                PopulateComboBox(cboErrorDEM, drv, drv.Cells[m_sFieldName_ErrorDEM].Value.ToString(), m_sErrorDEMs);
                txtComments.Text = drv.Cells[m_sFieldName_Comments].Value.ToString();
                txtEnteredBy.Text = drv.Cells[m_sFieldName_EnteredBy].Value.ToString();
                if ((Boolean)drv.Cells[m_sFieldName_Processed].Value == true)
                {
                    chkProcessed.Checked = true;
                }
                else if ((Boolean)drv.Cells[m_sFieldName_Processed].Value == false)
                {
                    chkProcessed.Checked = false;
                }
            }
        }

        private Boolean GetBooleanValidGCD_Results()
        {
            Boolean bResult = false;
            if (rdoResultsValidTrue.Checked)
            {
                bResult = true;
            }
            else if (rdoResultsValidFalse.Checked)
            {
                bResult = false;
            }
            return bResult;
        }


        private DataRow RetrieveVisitInfo()
        {
            DataRow r = null;
            if (dgvGCD_Review.SelectedRows.Count == 1)
            {
                DataRowView drv = (DataRowView)dgvGCD_Review.SelectedRows[0].DataBoundItem;
                r = drv.Row;
            }
            return r;
        }

        private string RetrieveVisitFolder(string sParentFolder, string sVisitYear, string sWatershedName, string sSiteName, string sVisitID)
        {
            string sPath = string.Empty;
            sPath = System.IO.Path.Combine(sParentFolder, sVisitYear);
            sPath = System.IO.Path.Combine(sPath, sWatershedName);
            sPath = System.IO.Path.Combine(sPath, sSiteName);
            sPath = System.IO.Path.Combine(sPath, string.Format("VISIT_{0}", sVisitID));
            sPath = sPath.Replace(" ", "");
            return sPath;
        }

        private void downloadTopoAndHydroDataFromCmorgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = RetrieveVisitInfo();
            if (dr is DataRow)
            {
                //Get parameters for New Visit to feed into RetreiveVisitFolder
                long newVisitID = (long)dr[m_sFieldName_NewVisitID];
                CHaMPData.VisitBasic newVisit = CHaMPData.VisitBasic.Load(newVisitID);
                List<CHaMPData.VisitBasic> visits = new List<CHaMPData.VisitBasic>();
                visits.Add(newVisit);

                //Get parameters for Old Visit to feed into RetreiveVisitFolder
                long oldVisitID = (long)dr[m_sFieldName_OldVisitID];
                CHaMPData.VisitBasic oldVisit = CHaMPData.VisitBasic.Load(newVisitID);
                visits.Add(oldVisit);
                Data.frmAPIDownloadVisitFiles frmOldVisitData = new Data.frmAPIDownloadVisitFiles(visits);
                frmOldVisitData.ShowDialog();
            }
        }

        private void dgvGCD_Review_CellClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                DataGridViewRow drv = dgvGCD_Review.Rows[e.RowIndex];
                PopulateFormInfo(drv);
                if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0)
                {
                    dgvGCD_Review.Rows[e.RowIndex].Selected = true;
                    cmsGCD_Visit.Show(Cursor.Position);
                }
            }
        }

        private string GetVisitDate(string sTableName, string sGetFieldName, string sWhereFieldName, string sValue)
        {
            string sSQL = String.Format("SELECT {0} FROM {1} WHERE {2} = @value", sGetFieldName, sTableName, sWhereFieldName);
            string sReturnValue = GetSingleValue(sSQL, sValue);
            return sReturnValue;
        }

        private string GetWatershedName(string sValue)
        {
            string sSQL = "SELECT W.WatershedName " +
                          " FROM (CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID)" +
                          " WHERE V.VisitID = @value";
            string sReturnValue = GetSingleValue(sSQL, sValue);
            return sReturnValue;
        }

        private string GetSiteName(string sValue)
        {
            string sSQL = "SELECT S.SiteName " +
                          " FROM (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID)" +
                          " WHERE V.VisitID = @value";
            string sReturnValue = GetSingleValue(sSQL, sValue);
            return sReturnValue;
        }

        private string GetSingleValue(string sSQL, string sValue)
        {
            string sReturnValue = "";
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);
                dbCom.Parameters.Add(new SQLiteParameter("@value", sValue));
                dbCom.ExecuteNonQuery();
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    sReturnValue = dbRead[0].ToString();
                }
                dbRead.Close();
            }

            return sReturnValue;
        }

        private void exploreSiteLevelUSGSStreamGageDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvGCD_Review.SelectedRows.Count > 1)
            {
                MessageBox.Show("USGS stream gage data can only be explored for one site at a time. Please select only one record from the table.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dgvGCD_Review.SelectedRows.Count == 1)
            {
                DataRowView drv = (DataRowView)dgvGCD_Review.SelectedRows[0].DataBoundItem;
                //DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;

                long iWatershedID = (long)r[m_sFieldName_WatershedID];
                long iSiteID = (long)r[m_sFieldName_SiteID];

                if (iSiteID != null && iWatershedID != null)
                {
                    if (iSiteID > 0 && iWatershedID > 0)
                    {
                        Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(iSiteID, iWatershedID);
                        frm.ShowDialog();
                    }
                }

            }
        }
    }
}
