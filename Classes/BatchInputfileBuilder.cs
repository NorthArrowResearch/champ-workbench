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

        public static string GetVisitFolder(String sParentFolder, int nFieldSeason, string sWatershedName, string sSiteName, int nVisitID)
        {
            string sPath = sPath = System.IO.Path.Combine(nFieldSeason.ToString(), sWatershedName, sSiteName, string.Format("VISIT_{0:0000}\\Topo", nVisitID));

            if (!string.IsNullOrEmpty(sParentFolder) && System.IO.Directory.Exists(sParentFolder))
                sPath = System.IO.Path.Combine(sParentFolder, sPath);

            return sPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nVisitID"></param>
        /// <param name="sSurveyGDBPath"></param>
        /// <param name="sTopoTIN"></param>
        /// <param name="sWSETIN"></param>
        /// <returns>True if all the topo data were found for this visit.</returns>
        private Boolean FindVisitTopoData(string sVisitTopoFolder, out string sSurveyGDBPath, out string sTopoTIN, out string sWSETIN)
        {
            bool bResult = false;

            if (Classes.FileSystem.LookForMatchingItems(sVisitTopoFolder, "*orthog*.gdb;*.gdb", Classes.FileSystem.SearchTypes.Directory, out sSurveyGDBPath))
            {
                bResult &= Classes.FileSystem.LookForMatchingItems(sVisitTopoFolder, "tin*", Classes.FileSystem.SearchTypes.Directory, out sTopoTIN);
                bResult &= Classes.FileSystem.LookForMatchingItems(sVisitTopoFolder, "ws*", Classes.FileSystem.SearchTypes.Directory, out sWSETIN);
            }
            else
            {
                sTopoTIN = string.Empty;
                sWSETIN = string.Empty;
            }
       
            return bResult;
        }

        private string AddVisitToSite(ref Classes.Site theSite, string sParentTopoFolder, int nVisitID, bool bTarget, bool bForcePrimary)
        {
            string sVisitTopoFolder = string.Empty;

            using (OleDbConnection dbCon = new OleDbConnection(m_dbCon.ConnectionString))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, S.UTMZone, V.VisitID, V.VisitYear, V.IsPrimary, V.HitchName, V.CrewName, V.SampleDate" +
                " FROM (CHAMP_Watersheds AS W INNER JOIN CHAMP_Sites AS S ON W.WatershedID = S.WatershedID) INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID" +
                " WHERE (V.VisitID = @VisitID) AND (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null)", dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", nVisitID);

                OleDbDataReader dbRead = dbCom.ExecuteReader();
                if (!dbRead.Read())
                    throw new Exception(string.Format("Failed to find visit {0} information", nVisitID));

                string sPath = GetVisitFolder(sParentTopoFolder, (Int16)dbRead["VisitYear"], (string)dbRead["WatershedName"], (string)dbRead["SiteName"], (int)dbRead["VisitID"]);
                if (System.IO.Directory.Exists(sPath))
                {
                    string sSurveyGDBPath, sTopoTIN, sWSETIN;
                    if (FindVisitTopoData(sParentTopoFolder, out sSurveyGDBPath, out sTopoTIN, out sWSETIN))
                    {
                        sVisitTopoFolder = System.IO.Path.GetDirectoryName(sSurveyGDBPath);

                        bool bPrimary = (bool)dbRead["IsPrimary"];

                        string sHitchName = string.Empty;
                        if (dbRead["HitchName"] != DBNull.Value)
                            sHitchName = (string)dbRead["HitchName"];

                        string sCrewName = string.Empty;
                        if (dbRead["CrewName"] != DBNull.Value)
                            sCrewName = (string)dbRead["CrewName"];

                        DateTime dSampleDate = DateTime.Now;
                        if (dbRead["SampleDate"] != DBNull.Value)
                            dSampleDate = (DateTime)dbRead["SampleDate"];

                        Visit theVisit = new Visit(nVisitID, sHitchName, sCrewName, (int)dbRead["VisitYear"], sSurveyGDBPath, sTopoTIN, sWSETIN, dSampleDate, bTarget, bPrimary || bForcePrimary);
                        theSite.AddVisit(theVisit);
                    }
                }
                dbRead.Close();
            }

            return sVisitTopoFolder;
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

                using (OleDbConnection conVisits = new OleDbConnection(m_dbCon.ConnectionString))
                {
                    conVisits.Open();

                    // This query retrieves all visits for the site. The target visit always comes first.
                    OleDbCommand dbTargetVisits = new OleDbCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, S.UTMZone, V.VisitYear, V.VisitID=@VisitID AS IsTarget" +
                        " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                        " WHERE (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null) AND V.SiteID IN (SELECT SiteID FROM CHaMP_Visits WHERE VisitID = @VisitID)" +
                        " ORDER BY  V.VisitID=@VisitID, V.SampleDate", conVisits);

                    OleDbParameter pVisitID = dbTargetVisits.Parameters.Add("@VisitID", OleDbType.Integer);

                    string sInputFile = string.Empty;
                    Site theSite = null;
                    bool bContinue = true;
                    foreach (int nVisitID in m_lVisitIDs)
                    {
                        pVisitID.Value = nVisitID;
                        OleDbDataReader dbRead = dbTargetVisits.ExecuteReader();
                        while (dbRead.Read() && bContinue)
                        {
                            if (theSite == null)
                            {
                                Watershed theWatershed = new Watershed(0, (string)dbRead["WatershedName"]);
                                string sUTMZone = string.Empty;
                                if (dbRead["UTMZone"] != DBNull.Value)
                                    sUTMZone = (string) dbRead["UTMZone"];

                                theSite = new Site(0, (string)dbRead["SiteName"], sUTMZone, ref theWatershed);

                                string sVisitTopoFolder = AddVisitToSite(ref theSite, sParentTopoDataFolder, nVisitID, true, bForcePrimary);
                                if (!string.IsNullOrEmpty(sVisitTopoFolder) && System.IO.Directory.Exists(sVisitTopoFolder))
                                {
                                    // If got to here then the data paths were retrieved and point to real data that exist.
                                    sInputFile = sVisitTopoFolder.Replace(sParentTopoDataFolder, this.m_Outputs.OutputFolder);
                                    sInputFile = System.IO.Path.GetDirectoryName(sInputFile);
                                    sInputFile = System.IO.Path.Combine(sInputFile, sDefaultInputFileName);
                                    sInputFile = System.IO.Path.ChangeExtension(sInputFile, "xml");
                                }
                            }
                            else
                                AddVisitToSite(ref theSite, sParentTopoDataFolder, nVisitID, false, bForcePrimary);

                            bContinue = bIncludeOtherVisits;
                        }
                        dbRead.Close();
                    }
                }
                //pSummary.Value = theSite.NameForDatabaseBatch;

                //System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sInputFile);

                //xmlInput.WriteStartElement("sites");
                //theSite.WriteToXML(xmlInput, sParentTopoDataFolder, bRequireWSTIN);
                //xmlInput.WriteEndElement(); // sites

                //// Write the end of the file
                //CloseFile(ref xmlInput, sOutputfolder);

                //dbInsert.ExecuteNonQuery();
                //nSuccess += 1;

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
