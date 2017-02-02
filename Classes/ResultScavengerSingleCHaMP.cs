using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    /// <summary>
    /// This class scavenges results from a single RBT result file but puts then 
    /// in the Workbench database using the new normalized table structure
    /// (See the class ResultScavengerSingle for the old code that puts the metrics
    /// in the old structure).
    /// </summary>
    public class ResultScavengerSingleCHaMP
    {
        private string m_sDBCon;

        /// <summary>
        /// These constants correspond to the LookupListItem IDs in the 
        /// Workbench database for "Scavenge Types" and "Metric Types"
        /// </summary>
        private const int m_nRBTScavengeTypeID = 1;

        private const int m_nVisitMetricTypeID = 3;
        private const int m_nTier1MetricTypeID = 4;
        private const int m_nTier2MetricTypeID = 5;
        private const int m_nChannelUnitTypeID = 6;

        public ResultScavengerSingleCHaMP(string sDBCon)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(sDBCon), "The database connection string cannot be empty.");
            m_sDBCon = sDBCon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sResultFilePath">Full path to existing RBT result XML file containing metric values</param>
        /// <returns>Metric_Results.ResultID for this result file</returns>
        public int ScavengeResultFile(string sResultFilePath)
        {
            if (string.IsNullOrEmpty(sResultFilePath) || !System.IO.File.Exists(sResultFilePath))
            {
                Exception ex = new Exception("the rbt result file does not exist.");
                ex.Data["result file"] = sResultFilePath;
                throw ex;
            }

            XmlDocument xmlResults = new XmlDocument();

            try
            {
                xmlResults.Load(sResultFilePath);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error loading RBT result XML file into XMLDocument object.", ex);
                ex2.Data["Result File"] = sResultFilePath;
                throw ex2;
            }

            int nResultID = 0;
            using (SQLiteConnection DBCon = new SQLiteConnection(m_sDBCon))
            {
                // Open the database and use a transaction for this entire RBT result file.
                // If anything goes wrong with any metric then the whole file is abandoned
                // and no changes are stored to the database.
                DBCon.Open();
                SQLiteTransaction dbTrans = DBCon.BeginTransaction();

                try
                {
                    // Insert the result file record and retrieve the unique ID that represents that result.
                    int nVisitID = 0;
                    nResultID = InsertResultRecord(ref dbTrans, sResultFilePath, ref xmlResults, out nVisitID);
                    if (nResultID > 0)
                    {
                        ScavengeVisitMetrics(ref dbTrans, ref xmlResults, nResultID);
                        ScavengeTierMetrics(ref dbTrans, ref xmlResults, 1, 4, 5, nResultID); // Tier 1 is Metric Group ID LookupListItem = 4 and the values (e.g. Slow/Pool) are stored in ListID = 5
                        ScavengeTierMetrics(ref dbTrans, ref xmlResults, 2, 5, 11, nResultID); // Tier 2 is Metric Group ID LookupListItem = 5 and the values (e.g. rapid) are stored in ListID = 11
                        ScavengeChannelUnitrMetrics(ref dbTrans, ref xmlResults, nVisitID, nResultID);
                        Scavenge_ChangeDetection(ref dbTrans, xmlResults, nResultID);
                        dbTrans.Commit();
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("Skipping result. Failed to insert result ID: " + sResultFilePath);
                    }

                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw ex;
                }
            }

            return nResultID;
        }

        /// <summary>
        /// Retrieves all metric definitions that have the argument TypeID, have an XPath defined, and are also tied to CHaMP monitoring IDs
        /// </summary>
        /// <param name="nMetricTypeID">Workbench LookupListItemID of the particular type of metrics to retrieve. See constants at top of this file.</param>
        /// <returns></returns>
        private List<ScavengeMetric> GetMetrics(int nMetricTypeID)
        {
            List<ScavengeMetric> lMetrics = new List<ScavengeMetric>();

            using (SQLiteConnection DBCon = new SQLiteConnection(m_sDBCon))
            {
                DBCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT MetricID, Title, CMMetricID, RBTResultXMLTag FROM Metric_Definitions WHERE (TypeID = @TypeID) AND (CMMetricID Is Not Null) AND (RBTResultXMLTag Is Not Null)", DBCon);
                dbCom.Parameters.AddWithValue("@TypeID", nMetricTypeID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    lMetrics.Add(new ScavengeMetric(dbRead.GetInt32(dbRead.GetOrdinal("MetricID")),
                        dbRead.GetInt32(dbRead.GetOrdinal("CMMetricID")),
                        dbRead.GetString(dbRead.GetOrdinal("Title")),
                        dbRead.GetString(dbRead.GetOrdinal("RBTResultXMLTag"))));
                }
            }

            return lMetrics;
        }

        /// <summary>
        /// Inserts the record that represents this RBT result file.
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="sResultFile">Full path to the RBT result file.</param>
        /// <param name="xmlResults">XML document object representing the result file. This is used to get some basic properties of the RBT run.</param>
        /// <returns>The ID of the RBT result file record. This is used as the foreign key for inserting metrics.</returns>
        /// <remarks>
        /// This method only saves the record if the RBT version, run date time and VisitID values can be obtained from the result XML file.</remarks>
        private int InsertResultRecord(ref SQLiteTransaction dbTrans, string sResultFile, ref XmlDocument xmlResults, out int nVisitID)
        {
            int nResultID = 0;
            nVisitID = 0;

            XmlNode nodVersion = xmlResults.SelectSingleNode("//rbt_results//meta_data//rbt_version");
            if (nodVersion is XmlNode && !string.IsNullOrEmpty(nodVersion.InnerText))
            {
                XmlNode nodCreated = xmlResults.SelectSingleNode("//rbt_results/meta_data//date_time_created");
                DateTime dtCreated;
                if (nodCreated is XmlNode && !string.IsNullOrEmpty(nodCreated.InnerText) && DateTime.TryParse(nodCreated.InnerText, out dtCreated))
                {
                    XmlNode nodVisitID = xmlResults.SelectSingleNode("//rbt_results//metric_results//visitid");
                    if (nodVisitID is XmlNode && !string.IsNullOrEmpty(nodVisitID.InnerText) && int.TryParse(nodVisitID.InnerText, out nVisitID))
                    {
                        SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Results (ResultFile, ModelVersion, VisitID, RunDateTime, ScavengeTypeID)" +
                            " VALUES (@ResultFile, @ModelVersion, @VisitID, @RBTRunDateTime, @ScavengeTypeID)", dbTrans.Connection, dbTrans);

                        dbCom.Parameters.AddWithValue("@ResultFile", sResultFile);
                        dbCom.Parameters.AddWithValue("@ModelVersion", nodVersion.InnerText);
                        dbCom.Parameters.AddWithValue("@VisitID", nVisitID);
                        dbCom.Parameters.AddWithValue("@RunDateTime", dtCreated.ToString());
                        dbCom.Parameters.AddWithValue("@ScavengeTypeID", m_nRBTScavengeTypeID);

                        dbCom.ExecuteNonQuery();

                        dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                        object objResultID = dbCom.ExecuteScalar();
                        if (objResultID != null && objResultID != DBNull.Value && objResultID is int)
                        {
                            nResultID = (int)objResultID;
                        }
                    }
                }
            }

            return nResultID;
        }

        /// <summary>
        /// Performs the actual retrieval of metric values from the result XML file and inserts them into the database
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="xmlResults">RBT result XML document</param>
        /// <param name="nResultID">The parent ResultID that represents the XML result file record in Metric_Results</param>
        private int ScavengeVisitMetrics(ref SQLiteTransaction dbTrans, ref XmlDocument xmlResults, int nResultID)
        {
            List<ScavengeMetric> lVisitMetrics = GetMetrics(m_nVisitMetricTypeID);
            if (lVisitMetrics.Count < 1)
                return 0;

            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_VisitMetrics (ResultID, MetricID, MetricValue) VALUES (@ResultID, @MetricID, @MetricValue)", dbTrans.Connection, dbTrans);
            SQLiteParameter pResultID = dbCom.Parameters.AddWithValue("@ResultID", nResultID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("@MetricID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", System.Data.DbType.Double);

            int nMetricsScavenged = 0;
            foreach (ScavengeMetric aMetric in lVisitMetrics)
            {
                XmlNode metricNode = xmlResults.SelectSingleNode(aMetric.XPath);
                if (metricNode is XmlNode)
                {
                    pMetricID.Value = aMetric.MetricID;
                    double fMetricValue;
                    if (!string.IsNullOrEmpty(metricNode.InnerText) && double.TryParse(metricNode.InnerText, out fMetricValue))
                        pMetricValue.Value = fMetricValue;
                    else
                        pMetricValue.Value = DBNull.Value;

                    nMetricsScavenged += dbCom.ExecuteNonQuery();
                }
            }

            return nMetricsScavenged;
        }

        /// <summary>
        /// Performs the actual retrieval of metric values from the result XML file and inserts them into the database
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="xmlResults">RBT result XML document</param>
        /// <param name="nMetricGroupID">The Workbench ID for either Tier1 or Tier2. See LookupListID = 2</param>
        /// <param name="nResultID">The parent ResultID that represents the XML result file record in Metric_Results</param>
        private int ScavengeTierMetrics(ref SQLiteTransaction dbTrans, ref XmlDocument xmlResults, int nTier, int nMetricGroupID, int nLookupListIDTierValues, int nResultID)
        {
            List<ScavengeMetric> lVisitMetrics = GetMetrics(nMetricGroupID);
            if (lVisitMetrics.Count < 1)
                return 0;

            // The metric definition XPaths for tier metrics contain a wildcard that needs to be replaced when searching the XML results.
            string sWildCard = string.Format("%%TIER{0}_NAME%%", nTier);

            // Build a dictionary of the tier values ("rapid", "Beaver Pool", Off Channel etc) for the specified Metric Group.
            // These will be substituted for the wildcard string above.
            Dictionary<string, int> dTierValues = new Dictionary<string, int>();
            SQLiteCommand comTierValues = new SQLiteCommand("SELECT ItemID, Title FROM LookupListItems WHERE ListID = @ListID", dbTrans.Connection, dbTrans);
            comTierValues.Parameters.AddWithValue("@ListID", nLookupListIDTierValues);
            SQLiteDataReader dbRead = comTierValues.ExecuteReader();
            while (dbRead.Read())
                dTierValues.Add(dbRead.GetString(dbRead.GetOrdinal("Title")), dbRead.GetInt32(dbRead.GetOrdinal("ItemID")));

            // Prepare the query to insert the tier metric value
            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_TierMetrics (ResultID, MetricID, TierID, MetricValue) VALUES (@ResultID, @MetricID, @TierID, @MetricValue)", dbTrans.Connection, dbTrans);
            SQLiteParameter pResultID = dbCom.Parameters.AddWithValue("@ResultID", nResultID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("@MetricID", System.Data.DbType.Int64);
            SQLiteParameter pTierID = dbCom.Parameters.Add("@TierID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", System.Data.DbType.Double);

            int nMetricsScavenged = 0;
            foreach (ScavengeMetric aMetric in lVisitMetrics)
            {
                foreach (string sTierName in dTierValues.Keys)
                {
                    // Tier metric XPaths have wildcards that need to be replaced with the tier value name (e.g. "rapid")
                    string sXPath = aMetric.XPath.Replace(sWildCard, string.Format("'{0}'", sTierName));
                    pTierID.Value = dTierValues[sTierName];

                    XmlNode metricNode = xmlResults.SelectSingleNode(sXPath);
                    if (metricNode is XmlNode)
                    {
                        pMetricID.Value = aMetric.MetricID;
                        double fMetricValue;
                        if (!string.IsNullOrEmpty(metricNode.InnerText) && double.TryParse(metricNode.InnerText, out fMetricValue))
                            pMetricValue.Value = fMetricValue;
                        else
                            pMetricValue.Value = DBNull.Value;

                        nMetricsScavenged += dbCom.ExecuteNonQuery();
                    }
                }
            }

            return nMetricsScavenged;
        }

        /// <summary>
        /// Performs the actual retrieval of metric values from the result XML file and inserts them into the database
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="xmlResults">RBT result XML document</param>
        /// <param name="nMetricGroupID">The Workbench ID for either Tier1 or Tier2. See LookupListID = 2</param>
        /// <param name="nResultID">The parent ResultID that represents the XML result file record in Metric_Results</param>
        private int ScavengeChannelUnitrMetrics(ref SQLiteTransaction dbTrans, ref XmlDocument xmlResults, int nVisitID, int nResultID)
        {
            // channel unit metrics are LookupList ItemID 6 (cm.org GroupTypeID = 2)
            List<ScavengeMetric> lVisitMetrics = GetMetrics(6);
            if (lVisitMetrics.Count < 1)
                return 0;

            // Build a dictionary of the channel units for this visit. Key is channel unit number (crew defined) to value of ChannelUnitID (workbench DB ID)
            Dictionary<int, int> dChannelUnits = new Dictionary<int, int>();
            SQLiteCommand comTierValues = new SQLiteCommand("SELECT C.ID AS ChannelUnitID, C.ChannelUnitNumber" +
                " FROM CHAMP_Visits AS V INNER JOIN (CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS C ON S.SegmentID = C.SegmentID) ON V.VisitID = S.VisitID" +
                " WHERE (V.VisitID = @VisitID) ORDER BY C.ChannelUnitNumber", dbTrans.Connection, dbTrans);
            comTierValues.Parameters.AddWithValue("@VisitID", nVisitID);
            SQLiteDataReader dbRead = comTierValues.ExecuteReader();
            while (dbRead.Read())
                dChannelUnits.Add(dbRead.GetInt32(dbRead.GetOrdinal("ChannelUnitNumber")), dbRead.GetInt32(dbRead.GetOrdinal("ChannelUnitID")));

            // Prepare the query to insert the tier metric value
            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_ChannelUnitMetrics (ResultID, MetricID, ChannelUnitID, ChannelUnitNumber, MetricValue) VALUES (@ResultID, @MetricID, @ChannelUnitID, @ChannelUnitNumber, @MetricValue)", dbTrans.Connection, dbTrans);
            SQLiteParameter pResultID = dbCom.Parameters.AddWithValue("@ResultID", nResultID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("@MetricID", System.Data.DbType.Int64);
            SQLiteParameter pChannelUnitID = dbCom.Parameters.Add("@ChannelUnitID", System.Data.DbType.Int64);
            SQLiteParameter pChannelUnitNumber = dbCom.Parameters.Add("@ChannelUnitNumber", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", System.Data.DbType.Double);

            int nMetricsScavenged = 0;
            foreach (ScavengeMetric aMetric in lVisitMetrics)
            {
                foreach (int nChannelUnitNumber in dChannelUnits.Keys)
                {
                    // The metric definition XPaths for channel unit metrics contain a wildcard that needs to be replaced when searching the XML results.
                    string sWildCard = string.Format("%%CHANNEL_UNIT_NUMBER%%", nChannelUnitNumber);

                    // Tier metric XPaths have wildcards that need to be replaced with the tier value name (e.g. "rapid")
                    string sXPath = aMetric.XPath.Replace(sWildCard, string.Format("'{0}'", nChannelUnitNumber));
                    pChannelUnitID.Value = dChannelUnits[nChannelUnitNumber];
                    pChannelUnitNumber.Value = nChannelUnitNumber;

                    XmlNode metricNode = xmlResults.SelectSingleNode(sXPath);
                    if (metricNode is XmlNode)
                    {
                        pMetricID.Value = aMetric.MetricID;
                        double fMetricValue;
                        if (!string.IsNullOrEmpty(metricNode.InnerText) && double.TryParse(metricNode.InnerText, out fMetricValue))
                            pMetricValue.Value = fMetricValue;
                        else
                            pMetricValue.Value = DBNull.Value;

                        nMetricsScavenged += dbCom.ExecuteNonQuery();
                    }
                }
            }

            return nMetricsScavenged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDBCon"></param>
        /// <param name="nResultID"></param>
        /// <param name="sLogFile"></param>
        /// <param name="sResultFilePath"></param>
        /// <param name="nBatchRunID">Optional identifier for the batch run that generated this log</param>
        /// <remarks>The BatchRunID was added on 5 Aug 2016 and requested by Carol. It will be absent when a
        /// log file is scavenged from disk. But it should be present when the log file was generated as part
        /// of a workbench batch run. This is helpful for SFR to trace back what purpose / model was being done
        /// when a log file was generated.</remarks>
        public void ScavengeLogFile(string sDBCon, int nResultID, String sLogFile, String sResultFilePath, int nBatchRunID = 0)
        {
            if (!string.IsNullOrEmpty(sLogFile) && !System.IO.File.Exists(sLogFile))
                return;

            XmlDocument xmlR = new XmlDocument();

            try
            {
                xmlR.Load(sLogFile);


            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error loading RBT log file into XMLDocument object.", ex);
                ex2.Data["Log File"] = sLogFile;
                throw ex2;
            }

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO LogFiles (ResultID, Status, VisitID, LogfilePath, ResultFilePath, MetaDataInfo, DateRun, ModelVersion, BatchRunID) VALUES (@ResultID, @Status, @VisitID, @LogFilePath, @ResultFilePath, @MetaDataInfo, @DateRun, @ModelVersion, @BatchRunID)", dbCon);

                if (nResultID > 0)
                    dbCom.Parameters.AddWithValue("ResultID", nResultID);
                else
                    dbCom.Parameters.AddWithValue("ResultID", DBNull.Value);

                SQLiteParameter pStatus = dbCom.Parameters.Add("Status", System.Data.DbType.String);
                pStatus.Value = DBNull.Value;
                XmlNode nodStatus = xmlR.SelectSingleNode("rbt/status");
                if (nodStatus is XmlNode)
                    if (nodStatus is XmlNode && !string.IsNullOrEmpty(nodStatus.InnerText))
                        pStatus.Value = nodStatus.InnerText;

                SQLiteParameter pVisitID = dbCom.Parameters.Add("VisitID", System.Data.DbType.Int64);
                pVisitID.Value = DBNull.Value;
                XmlNode nodTargetVisit = xmlR.SelectSingleNode("rbt/target_visit");
                if (nodTargetVisit is XmlNode && !string.IsNullOrEmpty(nodTargetVisit.InnerText))
                {
                    int nVisitID;
                    if (int.TryParse(nodTargetVisit.InnerText, out nVisitID))
                        pVisitID.Value = nVisitID;
                }

                dbCom.Parameters.AddWithValue("LogFilePath", sLogFile);

                SQLiteParameter pResultFile = dbCom.Parameters.Add("ResultFilePath", System.Data.DbType.String);
                if (string.IsNullOrEmpty(sResultFilePath))
                {
                    pResultFile.Value = DBNull.Value;
                }
                else
                {
                    pResultFile.Value = sResultFilePath;
                    pResultFile.Size = sResultFilePath.Length;
                }

                XmlNode xMeta = xmlR.SelectSingleNode("rbt/meta_data");
                SQLiteParameter pMeta = dbCom.Parameters.Add("MetaDataInfo", System.Data.DbType.String);
                pMeta.Value = DBNull.Value;
                if (xMeta is XmlNode)
                {
                    if (!string.IsNullOrEmpty(xMeta.InnerXml))
                    {
                        pMeta.Value = xMeta.InnerXml;
                        pMeta.Size = xMeta.InnerXml.Length;
                    }
                }

                XmlNode xDateRun = xmlR.SelectSingleNode("rbt/meta_data/date_time_created");
                SQLiteParameter pDateRun = dbCom.Parameters.Add("DateRun", System.Data.DbType.String);
                pDateRun.Value = DBNull.Value;
                DateTime dtDaterun;
                if (xDateRun is XmlNode && !string.IsNullOrEmpty(xDateRun.InnerText) && DateTime.TryParse(xDateRun.InnerText, out dtDaterun))
                {
                    pDateRun.Value = dtDaterun;
                    pDateRun.Size = xDateRun.InnerXml.Length;
                }

                XmlNode xModelVersion = xmlR.SelectSingleNode("rbt/meta_data/rbt_version");
                SQLiteParameter pModelVersion = dbCom.Parameters.Add("ModelVersion",  System.Data.DbType.String);
                pModelVersion.Value = DBNull.Value;
                if (xModelVersion is XmlNode)
                {
                    if (!string.IsNullOrEmpty(xModelVersion.InnerXml))
                    {
                        pModelVersion.Value = xModelVersion.InnerXml;
                        pModelVersion.Size = xModelVersion.InnerXml.Length;
                    }
                }

                SQLiteParameter pBatchID = dbCom.Parameters.Add("BatchRunID", System.Data.DbType.Int64);
                pBatchID.Value = DBNull.Value;
                if (nBatchRunID > 0)
                    pBatchID.Value = nBatchRunID;

                dbCom.ExecuteNonQuery();
                //
                // Get the ID of this log file entry
                //
                dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbCon);
                int nLogID = (int)dbCom.ExecuteScalar();
                if (nLogID > 0)
                {
                    //
                    // Now insert all the status messages and errors/warnings
                    //
                    dbCom = new SQLiteCommand("INSERT INTO LogMessages (LogID, MessageType, LogSeverity, SourceVisitID, TargetVisitID, LogDateTime, LogMessage, LogException, LogSolution)" +
                                                                    " VALUES (@LogID, @MessageType, @MessageSeverity, @SourceVisitID, @TargetVisitID, @LogDateTime, @LogMessage, @LogException, @LogSolution)", dbCon);
                    dbCom.Parameters.AddWithValue("LogID", nLogID);
                    SQLiteParameter pMessageType = dbCom.Parameters.Add("MessageType", System.Data.DbType.String);
                    SQLiteParameter pMessageSeverity = dbCom.Parameters.Add("MessageSeverity", System.Data.DbType.String);
                    SQLiteParameter pSourceVisitID = dbCom.Parameters.Add("SourceVisitID", System.Data.DbType.Int64);
                    SQLiteParameter pTargetVisitID = dbCom.Parameters.Add("TargetVisitID", System.Data.DbType.Int64);
                    SQLiteParameter pLogDateTime = dbCom.Parameters.Add("LogDateTime",  System.Data.DbType.DateTime);
                    SQLiteParameter pLogMessage = dbCom.Parameters.Add("LogMessage", System.Data.DbType.String);
                    SQLiteParameter pLogException = dbCom.Parameters.Add("LogException", System.Data.DbType.String);
                    SQLiteParameter pLogSolution = dbCom.Parameters.Add("LogSolution", System.Data.DbType.String);

                    foreach (XmlNode MessageNode in xmlR.SelectNodes("rbt/messages/message"))
                    {
                        XmlAttribute att = MessageNode.Attributes["severity"];
                        pMessageSeverity.Value = DBNull.Value;
                        if (att is XmlAttribute)
                        {
                            if (!string.IsNullOrEmpty(att.InnerText))
                            {
                                if (!string.IsNullOrEmpty(att.InnerText))
                                {
                                    pMessageSeverity.Value = att.InnerText;
                                    pMessageSeverity.Size = att.InnerText.Length;
                                }
                            }
                        }

                        att = MessageNode.Attributes["type"];
                        pMessageType.Value = DBNull.Value;
                        if (att is XmlAttribute)
                        {
                            if (!string.IsNullOrEmpty(att.InnerText))
                            {
                                if (!string.IsNullOrEmpty(att.InnerText))
                                {
                                    pMessageType.Value = att.InnerText;
                                    pMessageType.Size = att.InnerText.Length;
                                }
                            }
                        }

                        att = MessageNode.Attributes["time"];
                        pLogDateTime.Value = DBNull.Value;
                        if (att is XmlAttribute)
                        {
                            if (!string.IsNullOrEmpty(att.InnerText))
                            {
                                DateTime aTime = default(DateTime);
                                if (DateTime.TryParse(att.InnerText, out aTime))
                                {
                                    pLogDateTime.Value = aTime;
                                }
                            }
                        }

                        XmlNode aChildNode = MessageNode.SelectSingleNode("description");
                        pLogMessage.Value = DBNull.Value;
                        if (aChildNode is XmlNode)
                        {
                            if (!string.IsNullOrEmpty(aChildNode.InnerText))
                            {
                                pLogMessage.Value = aChildNode.InnerText.Trim();
                                //pLogMessage.Size = aChildNode.InnerText.Length;
                            }
                        }

                        aChildNode = MessageNode.SelectSingleNode("exception");
                        pLogException.Value = DBNull.Value;
                        if (aChildNode is XmlNode)
                        {
                            if (!string.IsNullOrEmpty(aChildNode.InnerText))
                            {
                                pLogException.Value = aChildNode.InnerText.Trim();
                                //pLogException.Size = aChildNode.InnerText.Length;
                            }
                        }

                        aChildNode = MessageNode.SelectSingleNode("solution");
                        pLogSolution.Value = DBNull.Value;
                        if (aChildNode is XmlNode)
                        {
                            if (!string.IsNullOrEmpty(aChildNode.InnerText))
                            {
                                pLogSolution.Value = aChildNode.InnerText.Trim();
                                //pLogSolution.Size = aChildNode.InnerText.Length;
                            }
                        }

                        aChildNode = MessageNode.SelectSingleNode("source_visit");
                        pSourceVisitID.Value = DBNull.Value;
                        if (aChildNode is XmlNode)
                        {
                            if (!string.IsNullOrEmpty(aChildNode.InnerText))
                            {
                                long nVisitID = 0;
                                if (long.TryParse(aChildNode.InnerText, out nVisitID))
                                    pSourceVisitID.Value = nVisitID;
                            }
                        }

                        aChildNode = MessageNode.SelectSingleNode("target_visit");
                        pTargetVisitID.Value = DBNull.Value;
                        if (aChildNode is XmlNode)
                        {
                            if (!string.IsNullOrEmpty(aChildNode.InnerText))
                            {
                                long nVisitID = 0;
                                if (long.TryParse(aChildNode.InnerText, out nVisitID))
                                    pTargetVisitID.Value = nVisitID;
                            }
                        }

                        dbCom.ExecuteNonQuery();
                    }
                }
            }
        }


        private void Scavenge_ChangeDetection(ref SQLiteTransaction dbTrans, XmlNode xmlTopNode, int nResultID)
        {

            foreach (XmlNode dodNode in xmlTopNode.SelectNodes("/rbt_results/metric_results/change_detection_results/dod"))
            {
                string sSQL = null;
                sSQL = "INSERT INTO Metric_ChangeDetection (ResultID, NewVisit, NewfieldSeason, NewVisitID, OldVisit, OldFieldSeason, OldVisitID, Epoch, ThresholdType, Threshold, SpatialCoherence";

                sSQL += ") VALUES (" + nResultID.ToString();

                AddStringValue(ref sSQL, dodNode, "./new_visit_name");
                AddNumericValue(ref sSQL, dodNode, "./new_visit_year");
                AddNumericValue(ref sSQL, dodNode, "./new_visit_id");
                AddStringValue(ref sSQL, dodNode, "./old_visit_name");
                AddNumericValue(ref sSQL, dodNode, "./old_visit_year");
                AddNumericValue(ref sSQL, dodNode, "./old_visit_id");

                XmlAttribute xmlAtt = dodNode.Attributes["epoch"];
                if (xmlAtt is XmlAttribute && !string.IsNullOrEmpty(xmlAtt.InnerText))
                {
                    sSQL += ", '" + xmlAtt.InnerText.Replace("'", "") + "'";
                }
                else
                {
                    sSQL += ", NULL";
                }

                xmlAtt = dodNode.Attributes["type"];
                if (xmlAtt is XmlAttribute && !string.IsNullOrEmpty(xmlAtt.InnerText))
                {
                    sSQL += ", '" + xmlAtt.InnerText.Replace("'", "") + "'";
                }
                else
                {
                    sSQL += ", NULL";
                }

                xmlAtt = dodNode.Attributes["threshold"];
                if (xmlAtt is XmlAttribute && !String.IsNullOrWhiteSpace(xmlAtt.InnerText))
                {
                    sSQL += ", " + xmlAtt.InnerText;
                }
                else
                {
                    sSQL += ", NULL";
                }

                xmlAtt = dodNode.Attributes["spatial_coherence"];
                if (xmlAtt is XmlAttribute && !String.IsNullOrWhiteSpace(xmlAtt.InnerText))
                {
                    sSQL += ", " + xmlAtt.InnerText;
                }
                else
                {
                    sSQL += ", NULL";
                }

                sSQL += ")";
                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbTrans.Connection, dbTrans);
                try
                {
                    dbCom.ExecuteNonQuery();

                    int nChangeDetectionID = 0;
                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    SQLiteDataReader dbRdr = dbCom.ExecuteReader();
                    if (dbRdr.Read())
                    {
                        if (!System.Convert.IsDBNull(dbRdr[0]))
                        {
                            nChangeDetectionID = (int)dbRdr[0];
                            PopulateTable_BudgetSegegration(ref dbTrans, dodNode, nChangeDetectionID);
                        }
                    }
                    dbRdr.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inserting change detection");
                }
            }
        }

        private void PopulateTable_BudgetSegegration(ref SQLiteTransaction dbTrans, XmlNode xmlTopNode, int nChangeDetectionID)
        {
            //("./change_detection/dod")
            foreach (XmlNode aBudgetSegNode in xmlTopNode.ChildNodes)
            {

                if (aBudgetSegNode.Name.ToLower().Contains("site") || aBudgetSegNode.Name.ToLower().Contains("tier") || aBudgetSegNode.Name.ToLower().Contains("bankfull"))
                {
                    string sSQL = "INSERT INTO Metric_BudgetSegregations (";
                    sSQL += "ChangeDetectionID" + ", Mask";

                    sSQL += ") VALUES (";
                    sSQL += nChangeDetectionID.ToString() + ", ";
                    sSQL += "'" + aBudgetSegNode.Name.Replace("'", "") + "'";
                    sSQL += ")";

                    SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbTrans.Connection, dbTrans);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    SQLiteDataReader dbRdr = dbCom.ExecuteReader();
                    if (dbRdr.Read())
                    {
                        if (!System.Convert.IsDBNull(dbRdr[0]))
                        {
                            int nBudgetSegragationID = (int)dbRdr[0];

                            if (aBudgetSegNode.Name.ToLower().Contains("site"))
                            {
                                PopulateTable_BudgetSegragationValues(dbTrans, aBudgetSegNode, nBudgetSegragationID, "site");
                            }
                            else
                            {
                                foreach (XmlNode aChannelUnitTypeNode in aBudgetSegNode.ChildNodes)
                                {
                                    PopulateTable_BudgetSegragationValues(dbTrans, aChannelUnitTypeNode, nBudgetSegragationID, aChannelUnitTypeNode.Name);
                                }
                            }

                        }
                    }
                    dbRdr.Close();
                }
            }
        }


        private void PopulateTable_BudgetSegragationValues(SQLiteTransaction dbTrans, XmlNode xmlBudgetNode, int nBudgetSegragationID, string sMaskValueName)
        {
            string sSQL = "INSERT INTO Metric_BudgetSegregationValues (BudgetID" + ", MaskValueName" + ", RawAreaErosion" + ", RawAreaDeposition" + ", ThresholdAreaErosion" + ", ThresholdAreaDeposition" + ", AreaDetectableChange" + ", AreaOfInterestRaw" + ", PercentAreaOfInterestDetectableChange" + ", RawVolumeErosion" + ", ThresholdVolumeErosion" + ", ErrorVolumeErosion" + ", ThresholdPercentErosion" + ", RawVolumeDeposition" + ", ThresholdVolumeDeposition" + ", ErrorVolumeDeposition" + ", ThresholdPercentDeposition" + ", RawVolumeDifference" + ", ThresholdedVolumeDifference" + ", ErrorVolumeDifference" + ", VolumeDifferencePercent" + ", AverageDepthErosionRaw" + ", AverageDepthErosionThreshold" + ", AverageDepthErosionError" + ", AverageDepthErosionPercent" + ", AverageDepthDepositionRaw" + ", AverageDepthDepositionThreshold" + ", AverageDepthDepositionError" + ", AverageDepthDepositionPercent" + ", AverageThicknessDifferenceAOIRaw" + ", AverageThicknessDifferenceAOIThresholded" + ", AverageThicknessDifferenceAOIError" + ", AverageThicknessDifferenceAOIPercent" + ", AverageNetThicknessDifferenceAOIRaw" + ", AverageNetThicknessDifferenceAOIThresholded" + ", AverageNetThicknessDifferenceAOIError" + ", AverageNetThicknessDifferenceAOIPercent" + ", AverageThicknessDifferenceADCThresholded" + ", AverageThicknessDifferenceADCError" + ", AverageThicknessDifferenceADCPercent" + ", AverageNetThicknessDifferenceADCThresholded" + ", AverageNetThicknessDifferenceADCError" + ", AverageNetThicknessDifferenceADCPercent" + ", PercentErosionRaw" + ", PercentErosionThresholded" + ", PercentDepositionRaw" + ", PercentDepositionThresholded" + ", PercentImbalanceRaw" + ", PercentImbalanceThresholded" + ", PercentNetVolumeRatioRaw" + ", PercentNetVolumeRatioThresholded" + ") VALUES (" + nBudgetSegragationID;

            sSQL += ", '" + sMaskValueName.Replace("'", "") + "'";
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_area_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_area_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_area_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_area_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "area_detectable_change");
            AddNumericValue(ref sSQL, xmlBudgetNode, "area_of_interest_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_area_of_interest_detectable_change");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_volume_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_volume_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "error_volume_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_percent_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_volume_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_volume_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "error_volume_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_percent_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_volume_difference");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_volume_difference");
            AddNumericValue(ref sSQL, xmlBudgetNode, "error_volume_difference");
            AddNumericValue(ref sSQL, xmlBudgetNode, "volume_difference_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_adc_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_adc_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_adc_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_adc_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_adc_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_adc_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_erosion_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_erosion_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_deposition_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_deposition_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_imbalance_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_imbalance_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_net_volume_ratio_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_net_volume_ratio_thresholded");

            sSQL += ")";

            try
            {
                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbTrans.Connection, dbTrans);
                dbCom.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting budget segregation values " + nBudgetSegragationID.ToString());
            }
        }


        private void AddStringValue(ref string sSQL, XmlNode xmlTopNode, string xmlTag)
        {
            XmlNode xmlNode = xmlTopNode.SelectSingleNode(xmlTag);
            if (xmlNode is XmlNode)
            {
                string sValue = xmlNode.InnerText;
                if (string.IsNullOrEmpty(sValue))
                {
                    sSQL += ", NULL";
                }
                else
                {
                    sSQL += ", '" + sValue.Replace("'", "''") + "'";
                }
            }
            else
            {
                sSQL += ", NULL";
            }

        }


        private void AddNumericValue(ref string sSQL, XmlNode xmlTopNode, string xmlTag)
        {
            string sValue = "";
            XmlNode xmlNode = xmlTopNode.SelectSingleNode(xmlTag);
            if (xmlNode is XmlNode)
            {
                sValue = xmlNode.InnerText;
            }

            double fValue = 0;
            if (double.TryParse(sValue, out fValue) && !double.IsNaN(fValue))
            {
                sSQL += ", " + fValue.ToString();
            }
            else
            {
                sSQL += ", NULL";
            }
        }

        /// <summary>
        /// This class is used to keep a list of all metrics in memory that need processing from an RBT result XML file.
        /// </summary>
        /// <remarks>Typically a list of these metrics is loaded for just a particular TypeID (visit or channel unit tier 1 level etc)</remarks>
        private class ScavengeMetric
        {
            public int MetricID { get; internal set; }
            public int CMMetricID { get; internal set; }
            public string Name { get; internal set; }
            public string XPath { get; internal set; }

            public ScavengeMetric(int nMetricID, int nCMMetricID, string sName, string sXPath)
            {
                MetricID = nMetricID;
                CMMetricID = nCMMetricID;
                Name = sName;
                XPath = sXPath;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
