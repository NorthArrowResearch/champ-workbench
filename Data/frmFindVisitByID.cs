using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;

namespace CHaMPWorkbench.Data
{
    public partial class frmFindVisitByID : Form
    {
        private OleDbConnection m_dbCon;

        public frmFindVisitByID(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private string GetSafeStringValue(OleDbDataReader dbRead, string sField)
        {
            if (dbRead[sField] != DBNull.Value)
                return (string)dbRead[sField];
            else
                return string.Empty;
        }

        private string GetSafeIntValue(OleDbDataReader dbRead, string sField)
        {
            if (dbRead[sField] != DBNull.Value)
                return dbRead[sField].ToString();
            else
                return string.Empty;
        }

        private void valVisitID_ValueChanged(object sender, EventArgs e)
        {
            txtResult.Text = string.Empty;


            OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Watersheds.WatershedName, CHAMP_Watersheds.WatershedID, CHAMP_Sites.SiteName, CHAMP_Sites.SiteID, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear, CHAMP_Visits.HitchName, CHAMP_Visits.CrewName, CHAMP_Visits.IsPrimary, CHAMP_Watersheds.Folder AS WatershedFolder, CHAMP_Sites.Folder AS SiteFolder, CHAMP_Visits.Folder AS VisitFolder FROM (CHAMP_Watersheds INNER JOIN CHAMP_Sites ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID) INNER JOIN CHAMP_Visits ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID WHERE VisitID = ?", m_dbCon);
            dbCom.Parameters.AddWithValue("VisitID", valVisitID.Value);
            try
            {

                OleDbDataReader dbRead = dbCom.ExecuteReader();
                if (dbRead.Read())
                {
                    txtResult.Text = "Watershed: " + GetSafeIntValue(dbRead, "WatershedID") + ", " + GetSafeStringValue(dbRead, "WatershedName");
                    txtResult.Text += Environment.NewLine + "Site: " + GetSafeIntValue(dbRead, "SiteID") + ", " + GetSafeStringValue(dbRead, "SiteName");
                    txtResult.Text += Environment.NewLine + "Visit: " + GetSafeIntValue(dbRead, "VisitID");
                    txtResult.Text += Environment.NewLine + "Field Season: " + GetSafeIntValue(dbRead, "VisitYear");
                    txtResult.Text += Environment.NewLine + "Hitch Name: " + GetSafeIntValue(dbRead, "HitchName");
                    txtResult.Text += Environment.NewLine + "Crew: " + GetSafeIntValue(dbRead, "CrewName");

                    txtSurveyPath.Text = GetPath(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder, dbRead);
                    txtOutputPath.Text = GetPath(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder, dbRead);
                }
                else
                    txtResult.Text = "No visit with this ID.";
            }
            catch (Exception ex)
            {
                txtResult.Text = "Error retrieving the visit information.";
            }
        }

        private void cmdCopySurveyData_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSurveyPath.Text);
        }

        private void cmdExploreSurveyData_Click(object sender, EventArgs e)
        {
            Process.Start(txtSurveyPath.Text);
        }

        private void cmdCopyOutput_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtOutputPath.Text);
        }

        private void cmdExplorerOutput_Click(object sender, EventArgs e)
        {
            Process.Start(txtOutputPath.Text);
             }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void txtOutputPath_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            cmdCopyOutput.Enabled = !string.IsNullOrEmpty(txtOutputPath.Text) && System.IO.Directory.Exists(txtOutputPath.Text);
            cmdExplorerOutput.Enabled = !string.IsNullOrEmpty(txtOutputPath.Text) && System.IO.Directory.Exists(txtOutputPath.Text);

            cmdCopySurveyData.Enabled = !string.IsNullOrEmpty(txtSurveyPath.Text) && System.IO.Directory.Exists(txtSurveyPath.Text);
            cmdExploreSurveyData.Enabled = !string.IsNullOrEmpty(txtSurveyPath.Text) && System.IO.Directory.Exists(txtSurveyPath.Text);

        }

        private void frmFindVisitByID_Load(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private string GetPath(string sParent, OleDbDataReader dbRead)
        {
            string sResult = string.Empty;
            string sFinalResult = string.Empty;

            if (!string.IsNullOrEmpty(sParent))
            {
                if (System.IO.Directory.Exists(sParent))
                {
                    sResult = sParent;
                    string sTemp = GetSafeIntValue(dbRead, "VisitYear");
                    sResult = System.IO.Path.Combine(sResult, sTemp);
                    if (System.IO.Directory.Exists(sResult))
                    {
                        sTemp = GetSafeStringValue(dbRead, "WatershedFolder");
                        sResult = System.IO.Path.Combine(sResult, sTemp);
                       
                        if (System.IO.Directory.Exists(sResult))
                        {
                            sTemp = GetSafeStringValue(dbRead, "SiteFolder");
                            sResult = System.IO.Path.Combine(sResult, sTemp);

                            if (System.IO.Directory.Exists(sResult))
                            {
                                sTemp = GetSafeStringValue(dbRead, "VisitFolder");
                                sResult = System.IO.Path.Combine(sResult, sTemp);
                                if (System.IO.Directory.Exists(sResult))
                                {
                                    sTemp = GetSafeStringValue(dbRead, "VisitFolder");
                                    sFinalResult = sResult;
                                }
                            }
                        }
                    }
                }
            }

            return sFinalResult;
        }
    }
}
