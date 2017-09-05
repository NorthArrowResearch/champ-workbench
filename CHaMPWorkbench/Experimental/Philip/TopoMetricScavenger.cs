using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Xml;
using static CHaMPWorkbench.Experimental.Philip.frmMetricScraper;

namespace CHaMPWorkbench.Experimental.Philip
{
    public class TopoMetricScavenger
    {
        public TopoMetricScavenger()
        {

        }

        public int Run(string sFolder, string sFileName, Dictionary<string, Experimental.Philip.frmMetricScraper.MetricSchemaWithDefs> MetricSchemas, bool bUseXMLModelVersion, string sModelVersion, long nScavengeTypeID)
        {
            int nFilesProcessed = 0;
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                // Load the tier 1 and tier 2 types. The keys in the dict below are Look-up list ID 2 and the 
                // arguments to the method are corresponding ListIDs for each set of tier types (5 = Tier 1, 11 = Tier 2)
                Dictionary<long, Dictionary<string, long>> dTierTypes = new Dictionary<long, Dictionary<string, long>>();
                dTierTypes[5] = LoadTierTypes(5);
                dTierTypes[11] = LoadTierTypes(11);

                List<long> lVisitIDs = LoadVisitIDs();

                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    foreach (string sMetricSchemaName in MetricSchemas.Keys)
                    {
                        long batchID = GetBatchID(ref dbTrans, MetricSchemas[sMetricSchemaName].ID, nScavengeTypeID);

                        XmlDocument xmlDoc = new XmlDocument();
                        foreach (string xmlFile in System.IO.Directory.GetFiles(sFolder, sFileName, System.IO.SearchOption.AllDirectories))
                        {

                            xmlDoc.Load(xmlFile);

                            XmlNode nodVisitID = xmlDoc.SelectSingleNode("/*/Meta/VisitID");
                            XmlNode nodRunDate = xmlDoc.SelectSingleNode("/*/Meta/DateCreated");

                            if (bUseXMLModelVersion)
                            {
                                XmlNode nodVersion = xmlDoc.SelectSingleNode("/*/Meta/Version");
                                sModelVersion = nodVersion.InnerText;
                            }

                            if (nodVisitID is XmlNode && nodRunDate is XmlNode && !string.IsNullOrEmpty(sModelVersion))
                            {
                                long nVisitID = long.Parse(nodVisitID.InnerText);

                                // Can only scrape metrics if the visit already exists in the database
                                if (!lVisitIDs.Contains<long>(nVisitID))
                                    continue;

                                long nInstanceID = InsertMetricInstance(ref dbTrans, batchID, nVisitID, DateTime.Parse(nodRunDate.InnerText), sModelVersion);

                                switch (MetricSchemas[sMetricSchemaName].DatabaseTable.ToLower())
                                {
                                    case "metric_visitmetrics":
                                        ScrapeVisitMetrics(nInstanceID, MetricSchemas[sMetricSchemaName].MetricDefs, ref xmlDoc, ref dbTrans);
                                        break;

                                    case "metric_channelunitmetrics":
                                        ScrapeChannelUnitMetrics(nInstanceID, MetricSchemas[sMetricSchemaName].MetricDefs, ref xmlDoc, ref dbTrans);
                                        break;

                                    case "metric_tiermetrics":
                                        long nTierListID = 5;
                                        string sTierXMLTag = "Tier1";
                                        if (MetricSchemas[sMetricSchemaName].Name.Contains("2"))
                                        {
                                            sTierXMLTag = "Tier2";
                                            nTierListID = 11;
                                        }

                                        ScrapeTierMetrics(nInstanceID, dTierTypes[nTierListID], sTierXMLTag, MetricSchemas[sMetricSchemaName].MetricDefs, ref xmlDoc, ref dbTrans);
                                        break;

                                    default:
                                        throw new Exception(string.Format("Unhandled metric database type '{0}'", MetricSchemas[sMetricSchemaName].DatabaseTable));
                                }

                                nFilesProcessed++;
                            }
                        }
                    }

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }

            return nFilesProcessed;
        }

        private long GetBatchID(ref SQLiteTransaction dbTrans, long schemaID, long scavengeTypeID)
        {
            long nBatchID = 0;

            if (scavengeTypeID == 1)
            {
                // AWS automation run. See if there is an existing batch ID to reuse
                SQLiteCommand dbCom = new SQLiteCommand("SELECT BatchID FROM Metric_Batches WHERE (SchemaID = @SchemaID) AND (ScavengeTypeID = @ScavengeTypeID)", dbTrans.Connection, dbTrans);
                dbCom.Parameters.AddWithValue("SchemaID", schemaID);
                dbCom.Parameters.AddWithValue("ScavengeTypeID", scavengeTypeID);
                nBatchID = naru.db.sqlite.SQLiteHelpers.GetScalarID(ref dbCom);
            }

            if (nBatchID < 1)
            {
                try
                {
                    SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Batches (SchemaID, ScavengeTypeID, Title) VALUES (@SchemaID, @ScavengeTypeID, 'API Download')", dbTrans.Connection, dbTrans);
                    dbCom.Parameters.AddWithValue("SchemaID", schemaID);
                    dbCom.Parameters.AddWithValue("ScavengeTypeID", scavengeTypeID);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    nBatchID = (long)dbCom.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inserting new metric batch.", ex);
                }
            }
            return nBatchID;
        }

        /// <summary>
        /// Delete existing metrics for this schema THAT HAVE DOWNLOAD SCAVENGE TYPE
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="nBatchID">Batch that represents this schema and scavenge type</param>
        /// <param name="visitID">The visit that is being downloaded and needs to have its metrics cleared</param>
        private void DeleteExistingMetrics(ref SQLiteTransaction dbTrans, long nBatchID, long visitID)
        {
            SQLiteCommand dbCom = new SQLiteCommand("DELETE FROM Metric_Instances WHERE (BatchID = @BatchID) AND (VisitID = @VisitID)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("BatchID", nBatchID);
            dbCom.Parameters.AddWithValue("VisitID", visitID);
            dbCom.ExecuteNonQuery();
        }

        private long InsertMetricInstance(ref SQLiteTransaction dbTrans, long batchID, long visitID, DateTime? dbMetricsCreatedOn, string sModelVersion)
        {
            string sMetricsCalculatedOn = string.Empty;

            // Delete existing metric instance and associated values.
            DeleteExistingMetrics(ref dbTrans, batchID, visitID);

            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Instances (BatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn)" +
                " VALUES (@BatchID, @VisitID, @ModelVersion, @MetricsCalculatedOn, NULL)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("BatchID", batchID);
            dbCom.Parameters.AddWithValue("VisitID", visitID);
            naru.db.sqlite.SQLiteHelpers.AddDateTimeParameterN(ref dbCom, dbMetricsCreatedOn, "MetricsCalculatedOn");
            naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, sModelVersion, "ModelVersion");
            dbCom.ExecuteNonQuery();

            dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
            long instanceID = (long)dbCom.ExecuteScalar();
            return instanceID;
        }

        private void ScrapeVisitMetrics(long nInstanceID, Dictionary<string, MetricDef> MetricDefs, ref XmlDocument xmlDoc, ref SQLiteTransaction dbTrans)
        {
            SQLiteCommand comVisitMetrics = new SQLiteCommand("INSERT INTO Metric_VisitMetrics (InstanceID, MetricID, MetricValue) VALUES (@InstanceID, @MetricID, @MetricValue)", dbTrans.Connection, dbTrans);
            comVisitMetrics.Parameters.AddWithValue("InstanceID", nInstanceID);
            SQLiteParameter pMetricID = comVisitMetrics.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = comVisitMetrics.Parameters.Add("MetricValue", System.Data.DbType.Double);

            foreach (MetricDef metric in MetricDefs.Values)
            {
                XmlNode nodMetric = xmlDoc.SelectSingleNode(metric.XPath);
                if (nodMetric is XmlNode)
                {
                    pMetricID.Value = metric.ID;
                    double fMetricValue;
                    if (!string.IsNullOrEmpty(nodMetric.InnerText) && double.TryParse(nodMetric.InnerText, out fMetricValue))
                        pMetricValue.Value = fMetricValue;
                    else
                        pMetricValue.Value = DBNull.Value;

                    comVisitMetrics.ExecuteNonQuery();
                }
                else
                    Console.WriteLine("Failed to find metric with XPath {0}", metric.XPath);
            }
        }

        private void ScrapeChannelUnitMetrics(long nInstanceID, Dictionary<string, MetricDef> MetricDefs, ref XmlDocument xmlDoc, ref SQLiteTransaction dbTrans)
        {
            SQLiteCommand comInsert = new SQLiteCommand("INSERT INTO Metric_ChannelUnitMetrics (InstanceID, MetricID, ChannelUnitNumber, MetricValue) VALUES (@InstanceID, @MetricID, @ChannelUnitNumber, @MetricValue)", dbTrans.Connection, dbTrans);
            comInsert.Parameters.AddWithValue("InstanceID", nInstanceID);
            SQLiteParameter pMetricID = comInsert.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pMetricChannelUnitNumber = comInsert.Parameters.Add("ChannelUnitNumber", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = comInsert.Parameters.Add("MetricValue", System.Data.DbType.Double);

            // Loop over each of the metrics defined in metric schema XML document
            foreach (MetricDef metric in MetricDefs.Values)
            {
                // The metric schema definitions contain channel unit number as a metric. Skip this metric
                if (metric.XPath.ToLower().EndsWith("channelunitnumber"))
                    continue;

                // Loop over each instance of the specific metric (i.e. will return list of "Volume" metric values for each channel unit)
                foreach (XmlNode nodMetric in xmlDoc.SelectNodes(metric.XPath))
                {
                    // Get the channel unit number from the sibling node to the metric
                    long nChannelUnitNumber = 0;
                    XmlNode nodCU = nodMetric.ParentNode.SelectSingleNode("ChannelUnitNumber");
                    if (nodCU is XmlNode && !string.IsNullOrEmpty(nodCU.InnerText) && long.TryParse(nodCU.InnerText, out nChannelUnitNumber))
                    {
                        double fMetricValue;
                        if (!string.IsNullOrEmpty(nodMetric.InnerText) && double.TryParse(nodMetric.InnerText, out fMetricValue))
                            pMetricValue.Value = fMetricValue;
                        else
                            pMetricValue.Value = DBNull.Value;

                        pMetricChannelUnitNumber.Value = nChannelUnitNumber;
                        pMetricID.Value = metric.ID;
                        comInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        private void ScrapeTierMetrics(long nInstanceID, Dictionary<string, long> dTierTypes, string sTierXMLTag, Dictionary<string, MetricDef> MetricDefs, ref XmlDocument xmlDoc, ref SQLiteTransaction dbTrans)
        {
            SQLiteCommand comInsert = new SQLiteCommand("INSERT INTO Metric_TierMetrics (InstanceID, MetricID, TierID, MetricValue) VALUES (@InstanceID, @MetricID, @TierID, @MetricValue)", dbTrans.Connection, dbTrans);
            comInsert.Parameters.AddWithValue("InstanceID", nInstanceID);
            SQLiteParameter pMetricID = comInsert.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pTierID = comInsert.Parameters.Add("TierID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = comInsert.Parameters.Add("MetricValue", System.Data.DbType.Double);

            // Loop over each of the metrics defined in metric schema XML document
            foreach (MetricDef metric in MetricDefs.Values)
            {
                // Loop over each instance of the specific metric (i.e. will return list of "Volume" metric values for each channel unit)
                foreach (XmlNode nodMetric in xmlDoc.SelectNodes(metric.XPath))
                {
                    // Get the tier type name from the sibling node to the metric
                    XmlNode nodTier = nodMetric.ParentNode.SelectSingleNode(sTierXMLTag);
                    if (nodTier is XmlNode && !string.IsNullOrEmpty(nodTier.InnerText))
                    {
                        if (dTierTypes.ContainsKey(nodTier.InnerText))
                        {
                            double fMetricValue;
                            if (!string.IsNullOrEmpty(nodMetric.InnerText) && double.TryParse(nodMetric.InnerText, out fMetricValue))
                                pMetricValue.Value = fMetricValue;
                            else
                                pMetricValue.Value = DBNull.Value;

                            pTierID.Value = dTierTypes[nodTier.InnerText];
                            pMetricID.Value = metric.ID;
                            comInsert.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private Dictionary<string, long> LoadTierTypes(long nListID)
        {
            Dictionary<string, long> result = new Dictionary<string, long>();
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT ItemID, Title FROM LookupListItems WHERE (ListID = @ListID) ORDER BY Title", dbCon);
                dbCom.Parameters.AddWithValue("ListID", nListID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    result[dbRead.GetString(1)] = dbRead.GetInt64(0);
            }
            return result;
        }

        private List<long> LoadVisitIDs()
        {
            List<long> result = new List<long>();
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT VisitID FROM CHaMP_Visits ORDER BY VisitID", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    result.Add(dbRead.GetInt64(0));
            }
            return result;
        }
    }
}
