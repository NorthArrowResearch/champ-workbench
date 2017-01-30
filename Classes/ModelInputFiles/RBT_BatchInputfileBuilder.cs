using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public class RBTBatchInputfileBuilder : BatchInputFileBuilderBase
    {
        public const string m_sDefaultRBTInputXMLFileName = "rbt_input.xml";

        RBTConfig m_RBTConfig;
        RBTOutputs m_RBTOutputs;

        private bool m_bCalculateMetrics;
        private bool m_bChangeDetection;
        private bool m_bMakeDEMOrthogonal;
        private bool m_bIncludeOtherVisits;
        private bool m_bForcePrimary;
        private bool m_bRequiresWSTIN;

        public RBTBatchInputfileBuilder(string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sOutputFolder, ref Dictionary<int, string> dVisits, string sInputFileName, RBTConfig rbtConfig, RBTOutputs rbtOutput,
             Boolean bCalculateMetrics, Boolean bChangeDetection, Boolean bMakeDEMOrthogonal, bool bIncludeOtherVisits, bool bForcePrimary, bool bRequireWSTIN, bool bClearOtherBatches)
            : base(CHaMPWorkbench.Properties.Settings.Default.ModelType_RBT, sDBCon, sBatchName, bMakeOnlyBatch, sMonitoringDataFolder, sOutputFolder, ref dVisits, sInputFileName)
        {
            m_RBTConfig = rbtConfig;
            m_RBTOutputs = rbtOutput;

            m_bCalculateMetrics = bCalculateMetrics;
            m_bChangeDetection = bChangeDetection;
            m_bMakeDEMOrthogonal = bMakeDEMOrthogonal;
            m_bIncludeOtherVisits = bIncludeOtherVisits;
            m_bForcePrimary = bForcePrimary;
            m_bRequiresWSTIN = bRequireWSTIN;
        }

        private System.IO.DirectoryInfo AddVisitToSite(ref Classes.Site theSite, System.IO.DirectoryInfo dParentTopoFolder, int nVisitID, bool bTarget, bool bForcePrimary)
        {
            System.IO.DirectoryInfo dVisitTopoFolder = null;

            using (SQLiteConnection conVisit = new SQLiteConnection(DBCon))
            {
                conVisit.Open();

                dsData.CHAMP_ChannelUnits.Clear();
                dsData.CHaMP_Segments.Clear();

                RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter taVisits = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
                taVisits.Connection = conVisit;
                taVisits.FillByVisitID(dsData.CHAMP_Visits, nVisitID);

                RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit = dsData.CHAMP_Visits.First<RBTWorkbenchDataSet.CHAMP_VisitsRow>();

                // Only proceed and add the visit if the associated protocol possesses topo data
                if (rVisit.IsProtocolIDNull() || ProtocolHasTopoData(rVisit.ProtocolID))
                {
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
                        BatchVisit theVisit = new BatchVisit(rVisit, dSurveyGDB.FullName, dTopoTIN.FullName, dWSTIN.FullName, bTarget, bTarget, bTarget, bTarget, bForcePrimary);

                        theSite.AddVisit(theVisit);
                    }
                }
            }
            return dVisitTopoFolder;
        }

        public override int Run(out List<string> lExceptionMessages)
        {
            using (SQLiteConnection conVisits = new SQLiteConnection(DBCon))
            {
                conVisits.Open();

                // This query retrieves all visits for the site. The target visit always comes first.
                SQLiteCommand dbTargetVisits = new SQLiteCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, S.UTMZone, V.VisitYear, V.VisitID=@VisitID AS IsTarget" +
                    " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                    " WHERE (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null) AND V.SiteID IN (SELECT SiteID FROM CHaMP_Visits WHERE VisitID = @VisitID)" +
                    " ORDER BY  V.VisitID=@VisitID, V.SampleDate", conVisits);

                SQLiteParameter pVisitID = dbTargetVisits.Parameters.Add("@VisitID", System.Data.DbType.Int64);

                foreach (BatchInputFileBuilderBase.BatchVisits aVisit in Visits)
                {
                    Site theSite = null;
                    System.IO.FileInfo dInputFile = null;
                    bool bContinue = true;
                    try
                    {
                        pVisitID.Value = aVisit.VisitID;
                        SQLiteDataReader dbRead = dbTargetVisits.ExecuteReader();
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

                                System.IO.DirectoryInfo dVisitTopoFolder = AddVisitToSite(ref theSite, MonitoringDataFolder, aVisit.VisitID, true, m_bForcePrimary);
                                if (dVisitTopoFolder is System.IO.DirectoryInfo)
                                {
                                    // If got to here then the data paths were retrieved and point to real data that exist.
                                    dInputFile = Classes.DataFolders.RBTInputFile(OutputFolder.FullName, dVisitTopoFolder, InputFileName);
                                }
                                else
                                    aVisit.ExceptionMessage = string.Format("Visit ID {0} is missing the survey geodatabase or one of the TINs.", aVisit.VisitID);
                            }
                            else
                                AddVisitToSite(ref theSite, MonitoringDataFolder, nVisitID, false, m_bForcePrimary);

                            bContinue = m_bIncludeOtherVisits;
                        }
                        dbRead.Close();

                        if (dInputFile is System.IO.FileInfo)
                        {
                            dInputFile.Directory.Create();
                            XmlTextWriter xmlInput;
                            CreateFile(dInputFile.FullName, out xmlInput);

                            xmlInput.WriteStartElement("sites");
                            theSite.WriteToXML(xmlInput, MonitoringDataFolder.FullName, m_bRequiresWSTIN);
                            xmlInput.WriteEndElement(); // sites

                            // Write the end of the file
                            CloseFile(ref xmlInput, dInputFile.Directory.FullName);
 
                            aVisit.InputFile = dInputFile.FullName;
                        }
                    }
                    catch (Exception ex)
                    {
                        aVisit.ExceptionMessage = ex.Message;
                    }
                }
            }

            GenerateBatchDBRecord();

            return GetResults(out lExceptionMessages);
        }

        private void CreateFile(string sRBTInputFilePath, out XmlTextWriter xmlInput)
        {
            // Ensure that the directory exists
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sRBTInputFilePath));

            xmlInput = new System.Xml.XmlTextWriter(sRBTInputFilePath, System.Text.Encoding.UTF8);
            xmlInput.Formatting = System.Xml.Formatting.Indented;
            xmlInput.Indentation = 4;
            xmlInput.WriteStartElement("rbt");

            xmlInput.WriteStartElement("metadata");
            xmlInput.WriteStartElement("created");
            xmlInput.WriteElementString("tool", System.Reflection.Assembly.GetExecutingAssembly().FullName);
            xmlInput.WriteElementString("date", DateTime.Now.ToString());
            xmlInput.WriteEndElement(); // created
            xmlInput.WriteEndElement(); // metadata
        }

        private void CloseFile(ref XmlTextWriter xmlInput, String sOutputFolder)
        {
            m_RBTOutputs.WriteToXML(xmlInput, sOutputFolder);
            m_RBTConfig.WriteToXML(xmlInput);

            xmlInput.WriteEndElement(); // rbt

            xmlInput.Close();
        }
    }
}
