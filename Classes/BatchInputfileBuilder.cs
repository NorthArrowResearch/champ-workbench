using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class BatchInputfileBuilder : InputFileBuilder
    {
        private OleDbConnection m_dbCon;
        private List<int> m_lVisitIDs;

        public BatchInputfileBuilder(OleDbConnection dbCon, List<int> lVisitIDs, Classes.Config rbtConfig, Classes.Outputs rbtOutputs)
            : base(rbtConfig, rbtOutputs)
        {
            m_lVisitIDs = lVisitIDs;
            m_dbCon = dbCon;
        }

        public static string GetVisitTopoFolder(String sParentTopoDataFolder, int nFieldSeason, string sWatershedName, string sSiteName, int nVisitID)
        {
            string sPath = sPath = System.IO.Path.Combine(nFieldSeason.ToString(), sWatershedName, sSiteName, string.Format("VISIT_{0:0000}\\Topo", nVisitID));

            if (!string.IsNullOrEmpty(sParentTopoDataFolder) && System.IO.Directory.Exists(sParentTopoDataFolder))
                sPath = System.IO.Path.Combine(sParentTopoDataFolder, sPath);

            return sPath;
        }

        public String Run(String sBatchName, String sDefaultInputFileName, String sParentTopoDataFolder, Boolean bCalculateMetrics, Boolean bChangeDetection, Boolean bMakeDEMOrthogonal, bool bIncludeOtherVisits, bool bForcePrimary, bool bRequireWSTIN)
        {
            OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
            int nSuccess = 0;
            string sResult;
            try
            {
                OleDbCommand dbInsert = new OleDbCommand("INSERT INTO RBT_Batches (BatchName, Run) Values (?, 1)", m_dbCon, dbTrans);
                dbInsert.Parameters.AddWithValue("BatchName", sBatchName);
                dbInsert.ExecuteNonQuery();

                dbInsert = new OleDbCommand("SELECT @@Identity", m_dbCon, dbTrans);
                long nBatchID = (int)dbInsert.ExecuteScalar();

                dbInsert = new OleDbCommand("INSERT INTO RBT_BatchRuns (BatchID, Summary, Inputfile, PrimaryVisitID) Values (?, ?, ?, ?)", m_dbCon, dbTrans);
                dbInsert.Parameters.AddWithValue("BatchID", nBatchID);
                OleDbParameter pSummary = dbInsert.Parameters.Add("Summary", OleDbType.VarChar);
                OleDbParameter pInputfile = dbInsert.Parameters.Add("InputFile", OleDbType.VarChar);
                OleDbParameter pPrimaryVisitID = dbInsert.Parameters.Add("VisitID", OleDbType.Integer);




                foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit in m_ds.CHAMP_Visits)
                {
                    if (rVisit.IsFolderNull() || rVisit.IsSurveyGDBNull())
                        continue;

                    string sVisitTopofolder = System.IO.Path.Combine(sParentTopoDataFolder, rVisit.Folder);
                    string sOutputfolder = System.IO.Path.Combine(this.m_Outputs.OutputFolder, rVisit.Folder);
                    string sInputFile = System.IO.Path.Combine(sOutputfolder, sDefaultInputFileName);
                    sInputFile = System.IO.Path.ChangeExtension(sInputFile, "xml");

                    //pSummary.Value = DBNull.Value; //rVisit.VisitYear.ToString() + ", " + rVisit.;
                    pInputfile.Value = sInputFile;
                    pPrimaryVisitID.Value = rVisit.VisitID;

                    if (System.IO.Directory.Exists(sVisitTopofolder))
                    {
                        XmlTextWriter xmlInput;
                        CreateFile(sInputFile, out xmlInput);

                        Site theSite = new Site(rVisit.CHAMP_SitesRow);

                        Visit v = new Visit(rVisit, bCalculateMetrics, bChangeDetection, bMakeDEMOrthogonal, m_Config.Mode == Classes.Config.RBTModes.Hydraulic_Model_Preparation, bForcePrimary);
                        theSite.AddVisit(v);

                        if (bIncludeOtherVisits)
                        {
                            foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rOtherVisit in m_ds.CHAMP_Visits)
                            {
                                if (rOtherVisit.SiteID == rVisit.SiteID && rOtherVisit.VisitID != rVisit.VisitID)
                                {
                                    Visit vOther = new Visit(rOtherVisit, false, false, bMakeDEMOrthogonal, false, bForcePrimary);
                                    theSite.AddVisit(vOther);
                                    //vOther.WriteToXML(ref xmlInput, sVisitTopofolder);
                                }
                            }
                        }
                        pSummary.Value = theSite.NameForDatabaseBatch;

                        xmlInput.WriteStartElement("sites");
                        theSite.WriteToXML(xmlInput, sParentTopoDataFolder, bRequireWSTIN);
                        xmlInput.WriteEndElement(); // sites

                        // Write the end of the file
                        CloseFile(ref xmlInput, sOutputfolder);

                        dbInsert.ExecuteNonQuery();
                        nSuccess += 1;
                    }
                }

                dbTrans.Commit();
                sResult = nSuccess.ToString("#,##0") + " input files generated successfully.";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                sResult = nSuccess.ToString("#,##0") + " input files were generated successfully, but then an error occurred and none of the records were stored in the workbench database. The error was: " + ex.Message;
            }

            return sResult;
        }
    }
}
