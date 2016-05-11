using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.Xml.Linq;

namespace CHaMPWorkbench.Experimental.James
{
    public partial class frmEnterPostGCD_QAQC_Record : Form
    {

        private OleDbConnection m_dbCon;
        //private string[] m_sReasonsForFlag = {"", "Outlier Metric"};
        private string[] m_sErrorTypes = { "", "Rod Height Bust", "Datum Shift", "Other"};
        private string[] m_sErrorDEMs = { "", "NewVisit", "OldVisit", "Both", "Unknown"};

        public frmEnterPostGCD_QAQC_Record(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
            LoadVisits(m_dbCon);
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

        private void LoadVisits(OleDbConnection dbCon)
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            //dgvGCD_Review.DataSource = null;

            string sGroupFields = "ID,NewVisitID,OldVisitID,FlagReason,ValidResults,ErrorType,ErrorDEM,Comments,EnteredBy,DateModified,Processed";

            string sSQL = "SELECT " + sGroupFields  +
                " FROM GCD_Review" +
                " ORDER BY DateModified";

            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            OleDbDataAdapter daGCD_Reivew = new OleDbDataAdapter(dbCom);
            DataTable dtGCD_Review = new DataTable();
            try
            {
                daGCD_Reivew.Fill(dtGCD_Review);
                dgvGCD_Review.DataSource = dtGCD_Review.AsDataView();
                //dgvGCD_Review.Refresh();

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            }
            catch(Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            string sSQL = "UPDATE GCD_Review" +
                             " SET ID= @id," +
                             " NewVisitID = @new_visit_id," +
                             " OldVisitID = @old_visit_id," +
                             " FlagReason = @flag_reason," +
                             " ValidResults = @valid_results," +
                             " ErrorType = @error_type," +
                             " ErrorDEM = @error_dem," +
                             " Comments = @comments," +
                             " EnteredBy = @entered_by," +
                             " DateModified = @date_modified," +
                             " Processed = @processed" +
                             " WHERE ID = @id AND NewVisitID = @new_visit_id AND OldVisitID = @old_visit_id";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            dbCom.Parameters.Add(new OleDbParameter("@id", valBudgetSegregationID.Value));
            dbCom.Parameters.Add(new OleDbParameter("@new_visit_id",  valNewVisitID.Value));
            dbCom.Parameters.Add(new OleDbParameter("@old_visit_id", valOldVisitID.Value));
            dbCom.Parameters.Add(new OleDbParameter("@flag_reason", txtReasonForFlag.Text));
            dbCom.Parameters.Add(new OleDbParameter("@valid_results", GetBooleanValidGCD_Results()));
            dbCom.Parameters.Add(new OleDbParameter("@error_type", cboErrorType.SelectedItem.ToString()));
            dbCom.Parameters.Add(new OleDbParameter("@error_dem", cboErrorDEM.SelectedItem.ToString()));
            dbCom.Parameters.Add(new OleDbParameter("@comments", txtComments.Text));
            dbCom.Parameters.Add(new OleDbParameter("@entered_by", txtEnteredBy.Text));
            dbCom.Parameters.Add(new OleDbParameter("@date_modified", DateTime.Now.ToString()));
            dbCom.Parameters.Add(new OleDbParameter("@processed", true));
            dbCom.ExecuteNonQuery();
            LoadVisits(m_dbCon);

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
                    string sSQL_Statement = "SELECT ID,NewVisitID,OldVisitID,FlagReason,ValidResults,ErrorType,ErrorDEM,Comments,EnteredBy,DateModified " +
                        "FROM GCD_Review " +
                        "ORDER BY DateModified";


                    int nExported = aws.Run(ref m_dbCon, sSQL_Statement, fiExport);

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

        //private void cmdGetStreamData_Click(object sender, EventArgs e)
        //{
        //    if (String.IsNullOrEmpty(txtSite.Text) == false & String.IsNullOrEmpty(txtWatershed.Text) == false)
        //    {
        //        Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(m_dbCon, txtSite.Text, txtWatershed.Text);
        //        frm.ShowDialog();
        //    }
        //    else
        //    {
        //        //Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(m_dbCon);
        //        //frm.ShowDialog();
        //    }
        //}

        private void PopulateFormInfo(DataGridViewRow drv)
        {
            if (drv != null)
            {
                
                valBudgetSegregationID.Value = System.Convert.ToDecimal(drv.Cells[11].Value);
                valNewVisitID.Value = System.Convert.ToDecimal(drv.Cells[12].Value);
                valOldVisitID.Value = System.Convert.ToDecimal(drv.Cells[13].Value);

                txtSite.Text = GetSiteName(m_dbCon, valNewVisitID.Value.ToString());
                txtWatershed.Text = GetWatershedName(m_dbCon, valNewVisitID.Value.ToString());
                txtNewVisitDate.Text = Convert.ToDateTime(GetVisitDate(m_dbCon, "CHAMP_Visits", "SampleDate", "VisitID", valNewVisitID.Value.ToString())).ToString("MM/dd/yyyy");
                txtOldVisitDate.Text = Convert.ToDateTime(GetVisitDate(m_dbCon, "CHAMP_Visits", "SampleDate", "VisitID", valOldVisitID.Value.ToString())).ToString("MM/dd/yyyy");

                //PopulateComboBox(cboReaonForFlag, drv, drv.Cells[13].Value.ToString(), m_sReasonsForFlag);
                txtReasonForFlag.Text = drv.Cells[14].Value.ToString();
                if ((Boolean)drv.Cells[15].Value == true)
                {
                    rdoResultsValidTrue.Checked = true;
                }
                else if ((Boolean)drv.Cells[15].Value == false)
                {
                    rdoResultsValidFalse.Checked = true;
                }
                PopulateComboBox(cboErrorType, drv, drv.Cells[16].Value.ToString(), m_sErrorTypes);
                PopulateComboBox(cboErrorDEM, drv, drv.Cells[17].Value.ToString(), m_sErrorDEMs);
                txtComments.Text = drv.Cells[18].Value.ToString();
                txtEnteredBy.Text = drv.Cells[19].Value.ToString();
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
               string sVisitID = dr["NewVisitID"].ToString();
               string sVisitYear = GetVisitDate(m_dbCon, "CHAMP_Visits", "VisitYear", "VisitID", sVisitID);
               string sWatershedName = GetWatershedName(m_dbCon, sVisitID);
               string sSiteName = GetSiteName(m_dbCon, sVisitID);                

               string sTopoFolder = RetrieveVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder, sVisitYear, sWatershedName, sSiteName, sVisitID);
               string sFTPFolder = "ftp://" + RetrieveVisitFolder("ftp.geooptix.com/ByYear", sVisitYear, sWatershedName, sSiteName, sVisitID).Replace("\\", "/");
               Data.frmFTPVisit frmNewVisitData = new Data.frmFTPVisit(Convert.ToInt16(sVisitID), sFTPFolder, sTopoFolder);
               frmNewVisitData.ShowDialog();
  
               //Old Visit

               //Get parameters for Old Visit to feed into RetreiveVisitFolder
               sVisitID = dr["OldVisitID"].ToString();
               sVisitYear = GetVisitDate(m_dbCon, "CHAMP_Visits", "VisitYear", "VisitID", sVisitID);               
               sTopoFolder = RetrieveVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder, sVisitYear, sWatershedName, sSiteName, sVisitID);
               sFTPFolder = "ftp://" + RetrieveVisitFolder("ftp.geooptix.com/ByYear", sVisitYear, sWatershedName, sSiteName, sVisitID).Replace("\\", "/");
               Data.frmFTPVisit frmOldVisitData = new Data.frmFTPVisit(Convert.ToInt16(sVisitID), sFTPFolder, sTopoFolder);
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

        private string GetVisitDate(OleDbConnection dbCon, string sTableName, string sGetFieldName, string sWhereFieldName, string sValue)
        {
            string sSQL = String.Format("SELECT {0}" +
                 " FROM {1}" +
                 " WHERE {2} = @value", sGetFieldName, sTableName, sWhereFieldName);
            string sReturnValue = GetSingleValue(dbCon, sSQL, sValue);
            return sReturnValue;
        }

        private string GetWatershedName(OleDbConnection dbCon, string sValue)
        {
            string sSQL = "SELECT W.WatershedName " +
                          " FROM (CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID)" +
                          " WHERE V.VisitID = @value";
            string sReturnValue = GetSingleValue(dbCon, sSQL, sValue);
            return sReturnValue;
        }

        private string GetSiteName(OleDbConnection dbCon, string sValue)
        {
            string sSQL = "SELECT S.SiteName " +
                          " FROM (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID)" +
                          " WHERE V.VisitID = @value";
            string sReturnValue = GetSingleValue(dbCon, sSQL, sValue);
            return sReturnValue;
        }

        private string GetSingleValue(OleDbConnection dbCon, string sSQL, string sValue)
        {
            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            dbCom.Parameters.Add(new OleDbParameter("@value", sValue));
            dbCom.ExecuteNonQuery();
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            string sReturnValue = "";
            while (dbRead.Read())
            {
                sReturnValue = dbRead[0].ToString();
            }
            dbRead.Close();
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

                string sWatershedName = (string)r["WatershedName"];
                string sSiteName = (string)r["SiteName"];

                if (String.IsNullOrEmpty(sSiteName) == false & String.IsNullOrEmpty(sWatershedName) == false)
                {
                    //Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(m_dbCon, sSiteName, sWatershedName);
                    //frm.ShowDialog();
                }
                else
                {
                    //Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(m_dbCon);
                    //frm.ShowDialog();
                }

            }
        }
    }
}
