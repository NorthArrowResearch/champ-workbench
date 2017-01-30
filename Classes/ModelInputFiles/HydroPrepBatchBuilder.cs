using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public class HydroPrepBatchBuilder : BatchInputFileBuilderBase
    {
        private string TemporaryFolder { get; set; }

        public HydroPrepBatchBuilder(string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sInputFolder, ref Dictionary<int, string> dVisits, string sInputFileName, string sTempFolder)
            : base(CHaMPWorkbench.Properties.Settings.Default.ModelType_HydroPrep, sDBCon, sBatchName, bMakeOnlyBatch, sMonitoringDataFolder, sInputFolder, ref dVisits, sInputFileName)
        {
            TemporaryFolder = sTempFolder;
        }

        public override int Run(out List<string> lExceptionMessages)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                // Query to retrieve watershed name, site name and field season
                dbCon.Open();
                SQLiteCommand comVisit = new SQLiteCommand("SELECT W.WatershedName, S.SiteName, V.VisitYear FROM " +
                    " (CHAMP_Watersheds AS W INNER JOIN CHAMP_Sites AS S ON W.WatershedID = S.WatershedID) INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID" +
                    " WHERE (V.VisitID = @VisitID)", dbCon);

                SQLiteParameter pVisitID = comVisit.Parameters.Add("@VisitID", System.Data.DbType.Int64);

                // Loop over all the visits
                foreach (BatchVisits aVisit in Visits)
                {
                    System.IO.DirectoryInfo dVisit = null;
                    if (DataFolders.Visit(MonitoringDataFolder, aVisit.VisitID, out dVisit))
                    {
                        System.IO.DirectoryInfo dSurveyGDB = null;
                        if (DataFolders.SurveyGDB(MonitoringDataFolder, aVisit.VisitID, out dSurveyGDB))
                        {
                            // Retrieve the additional visit information
                            pVisitID.Value = aVisit.VisitID;
                            SQLiteDataReader dbRead = comVisit.ExecuteReader();
                            if (dbRead.Read())
                            {
                                System.IO.DirectoryInfo dOutputFolder = DataFolders.HydroPrepFolder(OutputFolder.FullName, dVisit);
                                // Build all the input files here.
                                XmlNode nodTopLevel;
                                XmlDocument xmlDoc = CreateInputXMLDoc("hydro_prep", out nodTopLevel);

                                XmlNode nodSurveyGDB = xmlDoc.CreateElement("surveygdb");
                                nodSurveyGDB.InnerText = dSurveyGDB.FullName;
                                nodTopLevel.AppendChild(nodSurveyGDB);

                                XmlNode nodOutputFolder = xmlDoc.CreateElement("output_folder");
                                nodOutputFolder.InnerText = dOutputFolder.FullName;
                                nodTopLevel.AppendChild(nodOutputFolder);

                                XmlNode nodTempFolder = xmlDoc.CreateElement("temp_folder");
                                nodTempFolder.InnerText = TemporaryFolder;
                                nodTopLevel.AppendChild(nodTempFolder);
                                XmlNode nodSite = xmlDoc.CreateElement("site");
                                nodSite.InnerText = dbRead.GetString(dbRead.GetOrdinal("SiteName"));
                                nodTopLevel.AppendChild(nodSite);

                                XmlNode nodWatershed = xmlDoc.CreateElement("watershed");
                                nodWatershed.InnerText = dbRead.GetString(dbRead.GetOrdinal("WatershedName"));
                                nodTopLevel.AppendChild(nodWatershed);

                                XmlNode nodVisit = xmlDoc.CreateElement("visit");
                                nodVisit.InnerText = aVisit.VisitID.ToString();
                                nodTopLevel.AppendChild(nodVisit);

                                XmlNode nodFieldSeason = xmlDoc.CreateElement("field_season");
                                nodFieldSeason.InnerText = dbRead.GetInt16(dbRead.GetOrdinal("VisitYear")).ToString();
                                nodTopLevel.AppendChild(nodFieldSeason);

                                dOutputFolder.Create();
                                aVisit.InputFile = System.IO.Path.Combine(dOutputFolder.FullName, InputFileName);
                                xmlDoc.Save(aVisit.InputFile);
                            }
                            dbRead.Close();
                        }
                    }
                }
            }

            GenerateBatchDBRecord();

            return GetResults(out lExceptionMessages);
        }
    }
}
