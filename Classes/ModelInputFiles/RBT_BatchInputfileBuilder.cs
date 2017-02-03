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

        public RBTBatchInputfileBuilder(string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sOutputFolder, ref Dictionary<long, string> dVisits, string sInputFileName, RBTConfig rbtConfig, RBTOutputs rbtOutput,
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

        public override int Run(out List<string> lExceptionMessages)
        {
            using (SQLiteConnection conVisits = new SQLiteConnection(DBCon))
            {
                conVisits.Open();

                // This query retrieves all visits at the same site as a specified target visit. The target visit always comes first.
                SQLiteCommand dbTargetVisits = new SQLiteCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, S.UTMZone, V.VisitYear, V.VisitID=@VisitID AS IsTarget" +
                    " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                    " WHERE (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null) AND V.SiteID IN (SELECT SiteID FROM CHaMP_Visits WHERE VisitID = @VisitID)" +
                    " ORDER BY  V.VisitID=@VisitID, V.SampleDate", conVisits);
                SQLiteParameter pVisitID = dbTargetVisits.Parameters.Add("@VisitID", System.Data.DbType.Int64);

                foreach (BatchInputFileBuilderBase.BatchVisits aVisit in Visits)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode nodTopLevel = xmlDoc.CreateElement("rbt");
                    xmlDoc.AppendChild(nodTopLevel);

                    //Create an XML declaration. 
                    XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
                    xmlDoc.InsertBefore(xmldecl, nodTopLevel);

                    XmlNode nodSites = xmlDoc.CreateElement("sites");
                    nodTopLevel.AppendChild(nodSites);

                    // Prepare the site XML node.
                    XmlNode nodSite = null;

                    // Prepare the RBT input file path. This will only get set if the target visit has topo data.
                    System.IO.FileInfo dInputFile = null;

                    pVisitID.Value = aVisit.VisitID;
                    SQLiteDataReader dbRead = dbTargetVisits.ExecuteReader();
                    while (dbRead.Read())
                    {
                        CHaMPData.Visit visitAtSite = CHaMPData.Visit.Load(DBCon, dbRead.GetInt64(dbRead.GetOrdinal("VisitID")));

                        // If the site node is null then no visits have been added yet. Therefore the first one is the target visit.
                        if (nodSite == null)
                        {
                            System.IO.DirectoryInfo diVisit = null;
                            if (Classes.DataFolders.Visit(MonitoringDataFolder, visitAtSite.ID, out diVisit))
                            {
                                dInputFile = Classes.DataFolders.RBTInputFile(OutputFolder.FullName, diVisit, InputFileName);

                                nodSite = visitAtSite.Site.CreateXMLNode(ref xmlDoc);
                                nodSites.AppendChild(nodSite);

                                // Add the target visit
                                XmlNode nodVisit = visitAtSite.CreateXMLNode(ref xmlDoc, MonitoringDataFolder, m_bRequiresWSTIN, m_bCalculateMetrics, m_bChangeDetection, true);
                                if (nodVisit is XmlNode)
                                    nodSite.AppendChild(nodVisit);
                                else
                                {
                                    // Quit this target visit if the target visit failed to generate an XML tag.
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // this is another visit to the same site as the target visit. Add it, but turn off metrics etc.
                            XmlNode nodVisit = visitAtSite.CreateXMLNode(ref xmlDoc, MonitoringDataFolder, false, false, visitAtSite.IsPrimary, visitAtSite.IsPrimary);
                            if (nodVisit is XmlNode)
                                nodSite.AppendChild(nodVisit);
                        }
                    }

                    // Outputs XML node
                    nodTopLevel.AppendChild(m_RBTOutputs.CreateXMLNode(ref xmlDoc, OutputFolder.FullName));

                    // Model configuration XML node
                    nodTopLevel.AppendChild(m_RBTConfig.CreateXMLNode(ref xmlDoc));
                    
                    System.IO.Directory.CreateDirectory(dInputFile.DirectoryName);
                    xmlDoc.Save(dInputFile.FullName);
                }
            }

            GenerateBatchDBRecord();

            return GetResults(out lExceptionMessages);
        }
    }
}
