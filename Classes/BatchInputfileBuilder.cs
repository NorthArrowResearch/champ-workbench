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

        private RBTWorkbenchDataSet dsData;

        public BatchInputfileBuilder(OleDbConnection dbCon, List<int> lVisitIDs, Classes.Config rbtConfig, Classes.Outputs rbtOutputs)
            : base(rbtConfig, rbtOutputs)
        {
            m_lVisitIDs = lVisitIDs;
            m_dbCon = dbCon;

            dsData = new RBTWorkbenchDataSet();

            RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter taWatersheds = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            taWatersheds.Connection = m_dbCon;
            taWatersheds.Fill(dsData.CHAMP_Watersheds);

            RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter taSites = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            taSites.Connection = m_dbCon;
            taSites.Fill(dsData.CHAMP_Sites);
        }

        public static string GetVisitFolder(String sParentFolder, int nFieldSeason, string sWatershedName, string sSiteName, int nVisitID)
        {
            string sPath = sPath = System.IO.Path.Combine(nFieldSeason.ToString(), sWatershedName, sSiteName, string.Format("VISIT_{0}\\Topo", nVisitID));
            sPath = sPath.Replace(" ", "");

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

            string sGDBFolder = System.IO.Path.Combine(sVisitTopoFolder, "SurveyGDB");
            string sTopoTinFolder = System.IO.Path.Combine(sVisitTopoFolder, "TIN");
            string sWSTinFolder = System.IO.Path.Combine(sVisitTopoFolder, "WettedSurfaceTIN");

            if (bResult = Classes.FileSystem.LookForMatchingItems(sGDBFolder, "*orthog*.gdb;*.gdb", Classes.FileSystem.SearchTypes.Directory, out sSurveyGDBPath))
            {
                bResult &= Classes.FileSystem.LookForMatchingItems(sTopoTinFolder, "tin*", Classes.FileSystem.SearchTypes.Directory, out sTopoTIN);
                bResult &= Classes.FileSystem.LookForMatchingItems(sWSTinFolder, "ws*", Classes.FileSystem.SearchTypes.Directory, out sWSETIN);
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

            using (OleDbConnection conVisit = new OleDbConnection(m_dbCon.ConnectionString))
            {
                conVisit.Open();

                dsData.CHAMP_ChannelUnits.Clear();
                dsData.CHaMP_Segments.Clear();

                RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter taVisits = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
                taVisits.Connection = conVisit;
                taVisits.FillByVisitID(dsData.CHAMP_Visits, nVisitID);

                RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit = dsData.CHAMP_Visits.First<RBTWorkbenchDataSet.CHAMP_VisitsRow>();
                RBTWorkbenchDataSet.CHAMP_SitesRow rSite = dsData.CHAMP_Sites.FindBySiteID(rVisit.SiteID);
                RBTWorkbenchDataSet.CHAMP_WatershedsRow rWatershed = dsData.CHAMP_Watersheds.FindByWatershedID(rSite.WatershedID);

                if (dsData.CHAMP_Visits.Rows.Count != 1)
                    throw new Exception(string.Format("Failed to find visit {0} information", nVisitID));

                string sPath = GetVisitFolder(sParentTopoFolder, rVisit.VisitYear, rWatershed.WatershedName, rSite.SiteName, rVisit.VisitID);
                if (System.IO.Directory.Exists(sPath))
                {
                    string sSurveyGDBPath, sTopoTIN, sWSETIN;
                    if (FindVisitTopoData(sPath, out sSurveyGDBPath, out sTopoTIN, out sWSETIN))
                    {
                        RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter taSegments = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
                        taSegments.Connection = conVisit;
                        taSegments.FillByVisitID(dsData.CHaMP_Segments, nVisitID);

                        RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter taUnits = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
                        taUnits.Connection = conVisit;
                        taUnits.FillByVisitID(dsData.CHAMP_ChannelUnits, nVisitID);

                        sVisitTopoFolder = System.IO.Path.GetDirectoryName(sSurveyGDBPath);
                        Visit theVisit = new Visit(rVisit, sSurveyGDBPath, sTopoTIN, sWSETIN, bTarget, bTarget, bTarget, bTarget, bForcePrimary);

                        theSite.AddVisit(theVisit);
                    }
                }
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

                dbInsert = new OleDbCommand("INSERT INTO RBT_BatchRuns (BatchID, Summary, InputFile, PrimaryVisitID) Values (@BatchID, @Summary, @InputFile, @PrimaryVisitID)", m_dbCon, dbTrans);
                dbInsert.Parameters.AddWithValue("@BatchID", nBatchID);
                OleDbParameter pSummary = dbInsert.Parameters.Add("@Summary", OleDbType.VarChar);
                OleDbParameter pInputfile = dbInsert.Parameters.Add("@InputFile", OleDbType.VarChar);
                OleDbParameter pPrimaryVisitID = dbInsert.Parameters.Add("@PrimaryVisitID", OleDbType.Integer);

                using (OleDbConnection conVisits = new OleDbConnection(m_dbCon.ConnectionString))
                {
                    conVisits.Open();

                    // This query retrieves all visits for the site. The target visit always comes first.
                    OleDbCommand dbTargetVisits = new OleDbCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, S.UTMZone, V.VisitYear, V.VisitID=@VisitID AS IsTarget" +
                        " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                        " WHERE (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null) AND V.SiteID IN (SELECT SiteID FROM CHaMP_Visits WHERE VisitID = @VisitID)" +
                        " ORDER BY  V.VisitID=@VisitID, V.SampleDate", conVisits);

                    OleDbParameter pVisitID = dbTargetVisits.Parameters.Add("@VisitID", OleDbType.Integer);

                    foreach (int nTargetVisitID in m_lVisitIDs)
                    {
                        Site theSite = null;
                        string sInputFile = string.Empty;
                        bool bContinue = true;

                        pVisitID.Value = nTargetVisitID;
                        OleDbDataReader dbRead = dbTargetVisits.ExecuteReader();
                        while (dbRead.Read() && bContinue)
                        {
                            int nVisitID = (int)dbRead["VisitID"];

                            if (theSite == null)
                            {
                                Watershed theWatershed = new Watershed(0, (string)dbRead["WatershedName"]);
                                string sUTMZone = string.Empty;
                                if (dbRead["UTMZone"] != DBNull.Value)
                                    sUTMZone = (string)dbRead["UTMZone"];

                                theSite = new Site(0, (string)dbRead["SiteName"], sUTMZone, ref theWatershed);

                                string sVisitTopoFolder = AddVisitToSite(ref theSite, sParentTopoDataFolder, nTargetVisitID, true, bForcePrimary);
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

                        if (!string.IsNullOrEmpty(sInputFile))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sInputFile));
                            XmlTextWriter xmlInput;
                            CreateFile(sInputFile, out xmlInput);

                            xmlInput.WriteStartElement("sites");
                            theSite.WriteToXML(xmlInput, sParentTopoDataFolder, bRequireWSTIN);
                            xmlInput.WriteEndElement(); // sites

                            // Write the end of the file
                            CloseFile(ref xmlInput, System.IO.Path.GetDirectoryName(sInputFile));

                            pSummary.Value = theSite.NameForDatabaseBatch;
                            pInputfile.Value = sInputFile.Replace("/", "\\");
                            pPrimaryVisitID.Value = nTargetVisitID;
                            dbInsert.ExecuteNonQuery();
                            nSuccess += 1;
                        }
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
