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

        private System.IO.DirectoryInfo AddVisitToSite(ref Classes.Site theSite, System.IO.DirectoryInfo dParentTopoFolder, int nVisitID, bool bTarget, bool bForcePrimary)
        {
            System.IO.DirectoryInfo dVisitTopoFolder = null;

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

                System.IO.DirectoryInfo dSurveyGDB = null;
                System.IO.DirectoryInfo dTopoTIN = null;
                System.IO.DirectoryInfo dWSTIN = null;

                if (Classes.DataFolders.SurveyGDBTopoTinWSTin(dParentTopoFolder, nVisitID, out dSurveyGDB, out dTopoTIN, out dWSTIN))
                {
                    RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter taSegments = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
                    taSegments.Connection = conVisit;
                    taSegments.FillByVisitID(dsData.CHaMP_Segments, nVisitID);

                    RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter taUnits = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
                    taUnits.Connection = conVisit;
                    taUnits.FillByVisitID(dsData.CHAMP_ChannelUnits, nVisitID);

                    Classes.DataFolders.Topo(dParentTopoFolder, nVisitID, out dVisitTopoFolder);
                    Visit theVisit = new Visit(rVisit, dSurveyGDB.FullName, dTopoTIN.FullName, dWSTIN.FullName, bTarget, bTarget, bTarget, bTarget, bForcePrimary);

                    theSite.AddVisit(theVisit);
                }
            }
            return dVisitTopoFolder;
        }

        public String Run(String sBatchName, String sDefaultInputFileName, System.IO.DirectoryInfo dParentTopoDataFolder, Boolean bCalculateMetrics, Boolean bChangeDetection, Boolean bMakeDEMOrthogonal, bool bIncludeOtherVisits, bool bForcePrimary, bool bRequireWSTIN)
        {
            OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
            int nSuccess = 0;
            string sResult;
            try
            {
                OleDbCommand dbInsert = new OleDbCommand("INSERT INTO RBT_Batches (BatchName) Values (?)", m_dbCon, dbTrans);
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
                        System.IO.FileInfo dInputFile = null;
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

                                System.IO.DirectoryInfo dVisitTopoFolder = AddVisitToSite(ref theSite, dParentTopoDataFolder, nTargetVisitID, true, bForcePrimary);
                                if (dVisitTopoFolder is System.IO.DirectoryInfo)
                                {
                                    // If got to here then the data paths were retrieved and point to real data that exist.
                                    dInputFile= Classes.DataFolders.RBTInputFile(this.m_Outputs.OutputFolder, dVisitTopoFolder,sDefaultInputFileName);
                                }
                            }
                            else
                                AddVisitToSite(ref theSite, dParentTopoDataFolder, nVisitID, false, bForcePrimary);

                            bContinue = bIncludeOtherVisits;
                        }
                        dbRead.Close();

                        if (dInputFile is System.IO.FileInfo)
                        {
                            dInputFile.Directory.Create();
                            XmlTextWriter xmlInput;
                            CreateFile(dInputFile.FullName, out xmlInput);

                            xmlInput.WriteStartElement("sites");
                            theSite.WriteToXML(xmlInput, dParentTopoDataFolder.FullName, bRequireWSTIN);
                            xmlInput.WriteEndElement(); // sites

                            // Write the end of the file
                            CloseFile(ref xmlInput,dInputFile.Directory.FullName);

                            pSummary.Value = theSite.NameForDatabaseBatch;
                            pInputfile.Value = dInputFile.FullName.Replace("/", "\\");
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
