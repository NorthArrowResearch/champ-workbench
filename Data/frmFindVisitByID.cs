﻿using System;
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
        private string m_sBatchName;

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
            m_sBatchName = DateTime.Now.ToShortDateString();

            OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Watersheds.WatershedName, CHAMP_Watersheds.WatershedID, CHAMP_Sites.SiteName, CHAMP_Sites.SiteID, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear, CHAMP_Visits.HitchName, CHAMP_Visits.CrewName, CHAMP_Visits.IsPrimary, CHAMP_Watersheds.Folder AS WatershedFolder, CHAMP_Sites.Folder AS SiteFolder, CHAMP_Visits.Folder AS VisitFolder FROM (CHAMP_Watersheds INNER JOIN CHAMP_Sites ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID) INNER JOIN CHAMP_Visits ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID WHERE VisitID = ?", m_dbCon);
            dbCom.Parameters.AddWithValue("VisitID", valVisitID.Value);
            try
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                if (dbRead.Read())
                {
                    string sVisitID =  GetSafeIntValue(dbRead, "VisitID");
                    string sFieldSeason = GetSafeIntValue(dbRead, "VisitYear");

                    txtResult.Text = "Watershed: " + GetSafeIntValue(dbRead, "WatershedID") + ", " + GetSafeStringValue(dbRead, "WatershedName");
                    txtResult.Text += Environment.NewLine + "Site: " + GetSafeIntValue(dbRead, "SiteID") + ", " + GetSafeStringValue(dbRead, "SiteName");
                    txtResult.Text += Environment.NewLine + "Visit: " + sVisitID;
                    txtResult.Text += Environment.NewLine + "Field Season: " + sFieldSeason;
                    txtResult.Text += Environment.NewLine + "Hitch Name: " + GetSafeIntValue(dbRead, "HitchName");
                    txtResult.Text += Environment.NewLine + "Crew: " + GetSafeIntValue(dbRead, "CrewName");

                    m_sBatchName += ", VisitID " + sVisitID + ", Field Season " + sFieldSeason;

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
            cmdRBTBatch.Enabled = !string.IsNullOrEmpty(txtOutputPath.Text) && System.IO.Directory.Exists(txtOutputPath.Text) && System.IO.File.Exists(RBTInputFile);
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

        private string RBTInputFile
        {
            get
            {
                string sResult = "";
                if (!string.IsNullOrEmpty(txtOutputPath.Text) && System.IO.Directory.Exists(txtOutputPath.Text))
                {
                    sResult = System.IO.Path.Combine(txtOutputPath.Text, "rbt_input.xml");
                    if (!System.IO.File.Exists(sResult))
                        sResult = "";
                }
                return sResult;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
            try
            {
                OleDbCommand dbCom = new OleDbCommand("INSERT INTO RBT_Batches (BatchName, Run) Values (?, 1)", m_dbCon, dbTrans);
                dbCom.Parameters.AddWithValue("BatchName", m_sBatchName);
                dbCom.ExecuteNonQuery();

                dbCom = new OleDbCommand("SELECT @@Identity", m_dbCon, dbTrans);
                int nBatchID = (int)dbCom.ExecuteScalar();

                if (nBatchID > 0)
                {
                    dbCom = new OleDbCommand("INSERT INTO RBT_BatchRuns (BatchID, Summary, Run, InputFile) VALUES (?, ?, 1, ?)", m_dbCon, dbTrans);
                    dbCom.Parameters.AddWithValue("BatchID", nBatchID);

                    OleDbParameter pSummary = dbCom.Parameters.AddWithValue("Summary", m_sBatchName);
                    pSummary.Size = m_sBatchName.Length;

                    OleDbParameter pInputFile = dbCom.Parameters.AddWithValue("InputFile", RBTInputFile);
                  
                    dbCom.ExecuteNonQuery();
                    dbTrans.Commit();

                    MessageBox.Show("RBT batch run created with the name '" + m_sBatchName + "'.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    dbTrans.Rollback();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                MessageBox.Show("Error creating RBT batch run: " + ex.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}