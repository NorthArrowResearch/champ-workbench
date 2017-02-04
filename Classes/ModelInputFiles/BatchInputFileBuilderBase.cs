using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public abstract class BatchInputFileBuilderBase
    {
        // This is the database ID stored in the Workbench LookupListItems table for the corresponding
        // model type. e.g. RBT = 15, GUT = 16. These values should be passed to the constructor and
        // must match the values in the Workbench database.
        private int ModelTypeID;

        public string DBCon { get; internal set; }
        public string BatchName { get; internal set; }
        public bool MakeOnlyBatch { get; internal set; }
        public System.IO.DirectoryInfo MonitoringDataFolder { get; internal set; }
        public System.IO.DirectoryInfo OutputFolder { get; internal set; }
        public string InputFileName { get; internal set; }

        protected List<BatchInputFileBuilderBase.BatchVisits> Visits;

        // This list loads from the software settings. It helps distinguish visits that
        // have topo data from those that dont'
        private List<int> ProtocolsWithTopoData;

        public bool ProtocolHasTopoData(int nProtocolID)
        {
            return ProtocolsWithTopoData.Contains<int>(nProtocolID);
        }

        protected int GetResults(out List<string> lExceptionMessages)
        {
            int nSuccessful = 0;
            lExceptionMessages = new List<string>();

            foreach (BatchVisits aVisit in Visits)
            {
                if (string.IsNullOrEmpty(aVisit.ExceptionMessage))
                    nSuccessful++;
                else
                    lExceptionMessages.Add(aVisit.ExceptionMessage);
            }

            return nSuccessful;
        }
        
        public BatchInputFileBuilderBase(int nModelTypeID, string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sOutputFolder, ref Dictionary<long, string> dVisits, string sInputFileName)
        {
            ModelTypeID = nModelTypeID;
            DBCon = sDBCon;
            BatchName = sBatchName;
            MakeOnlyBatch = bMakeOnlyBatch;

            MonitoringDataFolder = new System.IO.DirectoryInfo(sMonitoringDataFolder);
            if (!MonitoringDataFolder.Exists)
                throw new Exception("The monitoring data folder must already exist.");

            OutputFolder = new System.IO.DirectoryInfo(sOutputFolder);

            Visits = new List<BatchVisits>();
            foreach (int nVisitID in dVisits.Keys)
                Visits.Add(new BatchVisits(nVisitID, dVisits[nVisitID]));

            if (string.IsNullOrEmpty(sInputFileName))
                throw new Exception("The input file name cannot be empty.");
            else
            {
                if (!sInputFileName.ToLower().EndsWith(".xml"))
                    throw new Exception("The input XML file name must end with .xml");
            }
            InputFileName = sInputFileName;

            // Load the protocols that possess topodata. This is used later when determining when
            // a visit should be processed by calling ProtcolHasTopoData
            ProtocolsWithTopoData = new List<int>();
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.ProtocolIDsWithTopoData))
            {
                foreach (string sProtocolID in CHaMPWorkbench.Properties.Settings.Default.ProtocolIDsWithTopoData.Split(','))
                {
                    int nProtocolID = 0;
                    if (int.TryParse(sProtocolID, out nProtocolID))
                        ProtocolsWithTopoData.Add(nProtocolID);
                }
            }
        }

        protected System.Xml.XmlDocument CreateInputXMLDoc(string sTopLevelNode, out XmlNode nodTopLevel)
        {
            // Build all the input files here.
            XmlDocument xmlDoc = new XmlDocument();

            nodTopLevel = xmlDoc.CreateElement(sTopLevelNode);
            xmlDoc.AppendChild(nodTopLevel);

            //Create an XML declaration. 
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.InsertBefore(xmldecl, nodTopLevel);

            // Write Metadata group
            XmlNode nodMetadata = xmlDoc.CreateElement("metadata");
            nodTopLevel.AppendChild(nodMetadata);

            XmlNode nodCreated = xmlDoc.CreateElement("created");
            nodMetadata.AppendChild(nodCreated);

            XmlNode nodTool = xmlDoc.CreateElement("tool");
            nodTool.InnerText = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            nodCreated.AppendChild(nodTool);

            XmlNode nodDate = xmlDoc.CreateElement("date");
            nodDate.InnerText = DateTime.Now.ToString();
            nodCreated.AppendChild(nodDate);

            return xmlDoc;
        }

        protected void GenerateBatchDBRecord()
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Model_Batches (BatchName) VALUES (@BatchName)", dbTrans.Connection, dbTrans);
                    dbCom.Parameters.AddWithValue("@BatchName", BatchName);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbCon, dbTrans);
                    object objBatchID = dbCom.ExecuteScalar();
                    if (objBatchID != null && objBatchID is long)
                    {
                        dbCom = new SQLiteCommand("INSERT INTO Model_BatchRuns (BatchID, ModelTypeID, PrimaryVisitID, Summary, InputFile) VALUES (@BatchID, @ModelTypeID, @PrimaryVisitID, @Summary, @InputFile)", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("@BatchID", (long)objBatchID);
                        dbCom.Parameters.AddWithValue("@ModelTypeID", ModelTypeID);
                        SQLiteParameter pVisitID = dbCom.Parameters.Add("@PrimaryVisitID", System.Data.DbType.Int64   );
                        SQLiteParameter pSummary = dbCom.Parameters.Add("@Summary",  System.Data.DbType.String);
                        SQLiteParameter pInputFile = dbCom.Parameters.Add("@InputFile",  System.Data.DbType.String);

                        foreach (BatchVisits aVisit in Visits)
                        {
                            if (!string.IsNullOrEmpty(aVisit.InputFile) && System.IO.File.Exists(aVisit.InputFile))
                            {
                                pVisitID.Value = aVisit.VisitID;

                                if (string.IsNullOrEmpty(aVisit.Description))
                                    pSummary.Value = DBNull.Value;
                                else
                                {
                                    pSummary.Value = string.Format("{0}", aVisit.Description);
                                    pSummary.Size = aVisit.Description.Length;
                                }

                                pInputFile.Value = aVisit.InputFile;
                                pInputFile.Size = aVisit.InputFile.Length;

                                dbCom.ExecuteNonQuery();
                            }
                        }

                        if (MakeOnlyBatch)
                        {
                            // Make all existing RBT batches set to NOT run
                            dbCom = new SQLiteCommand("UPDATE Model_BatchRuns SET Run = 0 WHERE (BatchID <> @BatchID) AND (ModelTypeID = @ModelTypeID)", dbCon, dbTrans);
                            dbCom.Parameters.AddWithValue("BatchID", (long)objBatchID);
                            dbCom.Parameters.AddWithValue("ModelTypeID", ModelTypeID);
                            dbCom.ExecuteNonQuery();
                        }
                    }

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                }
            }

            // Generate the batch here.

            // Generate the batch runs here.
        }

        /// <summary>
        /// All derived classes must implement a Run method that actually generates the input files.
        /// </summary>
        public abstract int Run(out List<string> lExceptionMessages);

        /// <summary>
        /// This class represents a successfully generated model input file on disk.
        /// Create and store a list of these objects then insert them into the database.
        /// </summary>
        public class BatchVisits
        {
            public int VisitID { get; internal set; }
            public string InputFile { get; set; }
            public string Description { get; set; }

            // Stores the eception or message iIf anything goes wrong creating this visit input file
            public string ExceptionMessage { get; set; }

            public BatchVisits(int nVisitID, string sDescription)
            {
                System.Diagnostics.Debug.Assert(nVisitID > 0, "The visit ID must be provided.");
                VisitID = nVisitID;
                InputFile = string.Empty;
                Description = sDescription;
            }

            public BatchVisits(int nVisitID, string sInputFile, string sDescription)
            {
                System.Diagnostics.Debug.Assert(nVisitID > 0, "The visit ID must be provided.");
                VisitID = nVisitID;

                System.Diagnostics.Debug.Assert(System.IO.File.Exists(sInputFile), "The input file should already be created and exist byt this point.");
                InputFile = sInputFile;

                Description = sDescription;
            }
        }
    }
}
