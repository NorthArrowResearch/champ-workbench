using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
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
            using (OleDbConnection DBCon = new OleDbConnection(m_sDBCon))
            {
                // Open the database and use a transaction for this entire RBT result file.
                // If anything goes wrong with any metric then the whole file is abandoned
                // and no changes are stored to the database.
                DBCon.Open();
                OleDbTransaction dbTrans = DBCon.BeginTransaction();

                try
                {
                    // Insert the result file record and retrieve the unique ID that represents that result.
                    nResultID = InsertResultRecord(ref dbTrans, sResultFilePath, ref xmlResults);
                    if (nResultID > 0)
                    {
                        ScavengeVisitMetrics(ref dbTrans, ref xmlResults, nResultID);
                        ScavengeTierMetrics(ref dbTrans, ref xmlResults, 1, 4, 5, nResultID); // Tier 1 is Metric Group ID LookupListItem = 4 and the values (e.g. Slow/Pool) are stored in ListID = 5
                        ScavengeTierMetrics(ref dbTrans, ref xmlResults, 2, 5, 11, nResultID); // Tier 2 is Metric Group ID LookupListItem = 5 and the values (e.g. rapid) are stored in ListID = 11
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

            using (OleDbConnection DBCon = new OleDbConnection(m_sDBCon))
            {
                DBCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT MetricID, Title, CMMetricID, RBTResultXMLTag FROM Metric_Definitions WHERE (TypeID = @TypeID) AND (CMMetricID Is Not Null) AND (RBTResultXMLTag Is Not Null)", DBCon);
                dbCom.Parameters.AddWithValue("@TypeID", nMetricTypeID);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
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
        private int InsertResultRecord(ref OleDbTransaction dbTrans, string sResultFile, ref XmlDocument xmlResults)
        {
            int nResultID = 0;

            XmlNode nodVersion = xmlResults.SelectSingleNode("//rbt_results//meta_data//rbt_version");
            if (nodVersion is XmlNode && !string.IsNullOrEmpty(nodVersion.InnerText))
            {
                XmlNode nodCreated = xmlResults.SelectSingleNode("//rbt_results/meta_data//date_time_created");
                DateTime dtCreated;
                if (nodCreated is XmlNode && !string.IsNullOrEmpty(nodCreated.InnerText) && DateTime.TryParse(nodCreated.InnerText, out dtCreated))
                {
                    XmlNode nodVisitID = xmlResults.SelectSingleNode("//rbt_results//metric_results//visitid");
                    int nVisitID;
                    if (nodVisitID is XmlNode && !string.IsNullOrEmpty(nodVisitID.InnerText) && int.TryParse(nodVisitID.InnerText, out nVisitID))
                    {
                        OleDbCommand dbCom = new OleDbCommand("INSERT INTO Metric_Results (ResultFile, ModelVersion, VisitID, RunDateTime, ScavengeTypeID)" +
                            " VALUES (@ResultFile, @ModelVersion, @VisitID, @RBTRunDateTime, @ScavengeTypeID)", dbTrans.Connection, dbTrans);

                        dbCom.Parameters.AddWithValue("@ResultFile", sResultFile);
                        dbCom.Parameters.AddWithValue("@ModelVersion", nodVersion.InnerText);
                        dbCom.Parameters.AddWithValue("@VisitID", nVisitID);
                        dbCom.Parameters.AddWithValue("@RunDateTime", dtCreated.ToString());
                        dbCom.Parameters.AddWithValue("@ScavengeTypeID", m_nRBTScavengeTypeID);

                        dbCom.ExecuteNonQuery();

                        dbCom = new OleDbCommand("SELECT @@Identity FROM Metric_Results", dbTrans.Connection, dbTrans);
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
        private int ScavengeVisitMetrics(ref OleDbTransaction dbTrans, ref XmlDocument xmlResults, int nResultID)
        {
            List<ScavengeMetric> lVisitMetrics = GetMetrics(m_nVisitMetricTypeID);
            if (lVisitMetrics.Count < 1)
                return 0;

            OleDbCommand dbCom = new OleDbCommand("INSERT INTO Metric_VisitMetrics (ResultID, MetricID, MetricValue) VALUES (@ResultID, @MetricID, @MetricValue)", dbTrans.Connection, dbTrans);
            OleDbParameter pResultID = dbCom.Parameters.AddWithValue("@ResultID", nResultID);
            OleDbParameter pMetricID = dbCom.Parameters.Add("@MetricID", OleDbType.Integer);
            OleDbParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", OleDbType.Double);

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
        private int ScavengeTierMetrics(ref OleDbTransaction dbTrans, ref XmlDocument xmlResults, int nTier, int nMetricGroupID, int nLookupListIDTierValues, int nResultID)
        {
            List<ScavengeMetric> lVisitMetrics = GetMetrics(nMetricGroupID);
            if (lVisitMetrics.Count < 1)
                return 0;

            // The metric definition XPaths for tier metrics contain a wildcard that needs to be replaced when searching the XML results.
            string sTierWildCard = string.Format("%%TIER{0}_NAME%%", nTier);

            // Build a dictionary of the tier values ("rapid", "Beaver Pool", Off Channel etc) for the specified Metric Group.
            // These will be substituted for the wildcard string above.
            Dictionary<string, int> dTierValues = new Dictionary<string, int>();
            OleDbCommand comTierValues = new OleDbCommand("SELECT ItemID, Title FROM LookupListItems WHERE ListID = @ListID", dbTrans.Connection, dbTrans);
            comTierValues.Parameters.AddWithValue("@ListID", nLookupListIDTierValues);
            OleDbDataReader dbRead = comTierValues.ExecuteReader();
            while (dbRead.Read())
                dTierValues.Add(dbRead.GetString(dbRead.GetOrdinal("Title")), dbRead.GetInt32(dbRead.GetOrdinal("ItemID")));

            // Prepare the query to insert the tier metric value
            OleDbCommand dbCom = new OleDbCommand("INSERT INTO Metric_TierMetrics (ResultID, MetricID, TierID, MetricValue) VALUES (@ResultID, @MetricID, @TierID, @MetricValue)", dbTrans.Connection, dbTrans);
            OleDbParameter pResultID = dbCom.Parameters.AddWithValue("@ResultID", nResultID);
            OleDbParameter pMetricID = dbCom.Parameters.Add("@MetricID", OleDbType.Integer);
            OleDbParameter pTierID = dbCom.Parameters.Add("@TierID", OleDbType.Integer);
            OleDbParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", OleDbType.Double);

            int nMetricsScavenged = 0;
            foreach (ScavengeMetric aMetric in lVisitMetrics)
            {
                foreach (string sTierName in dTierValues.Keys)
                {
                    // Tier metric XPaths have wildcards that need to be replaced with the tier value name (e.g. "rapid")
                    string sXPath = aMetric.XPath.Replace(sTierWildCard, sTierName);
                    pMetricID.Value = dTierValues[sTierName];

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

        public void ScavengeLogFile(string sDBCon, int nResultID, String sLogFile, String sResultFilePath)
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

            using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("INSERT INTO LogFiles (ResultID, Status, VisitID, LogfilePath, ResultFilePath, MetaDataInfo) VALUES (@ResultID, @Status, @VisitID, @LogFilePath, @ResultFilePath, @MetaDataInfo)", dbCon);

                if (nResultID > 0)
                    dbCom.Parameters.AddWithValue("ResultID", nResultID);
                else
                    dbCom.Parameters.AddWithValue("ResultID", DBNull.Value);

                OleDbParameter pStatus = dbCom.Parameters.Add("Status", OleDbType.VarChar);
                pStatus.Value = DBNull.Value;
                XmlNode nodStatus = xmlR.SelectSingleNode("rbt/status");
                if (nodStatus is XmlNode)
                    if (nodStatus is XmlNode && !string.IsNullOrEmpty(nodStatus.InnerText))
                        pStatus.Value = nodStatus.InnerText;

                OleDbParameter pVisitID = dbCom.Parameters.Add("VisitID", OleDbType.Integer);
                pVisitID.Value = DBNull.Value;
                XmlNode nodTargetVisit = xmlR.SelectSingleNode("rbt/target_visit");
                if (nodTargetVisit is XmlNode && !string.IsNullOrEmpty(nodTargetVisit.InnerText))
                {
                    int nVisitID;
                    if (int.TryParse(nodTargetVisit.InnerText, out nVisitID))
                        pVisitID.Value = nVisitID;
                }

                dbCom.Parameters.AddWithValue("LogFilePath", sLogFile);

                OleDbParameter pResultFile = dbCom.Parameters.Add("ResultFilePath", OleDbType.VarChar);
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
                OleDbParameter pMeta = dbCom.Parameters.Add("MetaDataInfo", OleDbType.VarChar);
                pMeta.Value = DBNull.Value;
                if (xMeta is XmlNode)
                {
                    if (!string.IsNullOrEmpty(xMeta.InnerXml))
                    {
                        pMeta.Value = xMeta.InnerXml;
                        pMeta.Size = xMeta.InnerXml.Length;
                    }
                }
                dbCom.ExecuteNonQuery();
                //
                // Get the ID of this log file entry
                //
                dbCom = new OleDbCommand("SELECT @@Identity FROM LogFiles", dbCon);
                int nLogID = (int)dbCom.ExecuteScalar();
                if (nLogID > 0)
                {
                    //
                    // Now insert all the status messages and errors/warnings
                    //
                    dbCom = new OleDbCommand("INSERT INTO LogMessages (LogID, MessageType, LogSeverity, SourceVisitID, TargetVisitID, LogDateTime, LogMessage, LogException, LogSolution)" +
                                                                    " VALUES (@LogID, @MessageType, @MessageSeverity, @SourceVisitID, @TargetVisitID, @LogDateTime, @LogMessage, @LogException, @LogSolution)", dbCon);
                    dbCom.Parameters.AddWithValue("LogID", nLogID);
                    OleDbParameter pMessageType = dbCom.Parameters.Add("MessageType", OleDbType.VarChar);
                    OleDbParameter pMessageSeverity = dbCom.Parameters.Add("MessageSeverity", OleDbType.VarChar);
                    OleDbParameter pSourceVisitID = dbCom.Parameters.Add("SourceVisitID", OleDbType.BigInt);
                    OleDbParameter pTargetVisitID = dbCom.Parameters.Add("TargetVisitID", OleDbType.BigInt);
                    OleDbParameter pLogDateTime = dbCom.Parameters.Add("LogDateTime", OleDbType.Date);
                    OleDbParameter pLogMessage = dbCom.Parameters.Add("LogMessage", OleDbType.VarChar);
                    OleDbParameter pLogException = dbCom.Parameters.Add("LogException", OleDbType.VarChar);
                    OleDbParameter pLogSolution = dbCom.Parameters.Add("LogSolution", OleDbType.VarChar);

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


        private void Scavenge_ChangeDetection(ref OleDbTransaction dbTrans, XmlNode xmlTopNode, int nResultID)
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
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbTrans.Connection, dbTrans);
                try
                {
                    dbCom.ExecuteNonQuery();

                    int nChangeDetectionID = 0;
                    dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_ChangeDetection", dbTrans.Connection, dbTrans);
                    OleDbDataReader dbRdr = dbCom.ExecuteReader();
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

        private void PopulateTable_BudgetSegegration(ref OleDbTransaction dbTrans, XmlNode xmlTopNode, int nChangeDetectionID)
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

                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbTrans.Connection, dbTrans);
                    dbCom.ExecuteNonQuery();

                    dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_BudgetSegregations", dbTrans.Connection, dbTrans);
                    OleDbDataReader dbRdr = dbCom.ExecuteReader();
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


        private void PopulateTable_BudgetSegragationValues(OleDbTransaction dbTrans, XmlNode xmlBudgetNode, int nBudgetSegragationID, string sMaskValueName)
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
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbTrans.Connection, dbTrans);
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
