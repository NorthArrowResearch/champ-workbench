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

        public int Run(string sFolder, string sFileName, Dictionary<string, Experimental.Philip.frmMetricScraper.MetricSchema> MetricSchemas, bool bUseXMLModelVersion, string sModelVersion, long nScavengeTypeID)
        {
            int nFilesProcessed = 0;
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                // Load the tier 1 and tier 2 types. The keys in the dict below are Look-up list ID 2 and the 
                // arguments to the method are corresponding ListIDs for each set of tier types (5 = Tier 1, 11 = Tier 2)
                Dictionary<long, Dictionary<string, long>> dTierTypes = new Dictionary<long, Dictionary<string, long>>();
                dTierTypes[4] = LoadTierTypes(5);
                dTierTypes[5] = LoadTierTypes(11);

                List<long> lVisitIDs = LoadVisitIDs();

                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    SQLiteCommand comResult = new SQLiteCommand("INSERT INTO Metric_Results (ResultFile, ModelVersion, VisitID, RunDateTime, ScavengeTypeID) VALUES (@ResultFile, @ModelVersion, @VisitID, @RunDateTime, 1)", dbTrans.Connection, dbTrans);
                    SQLiteParameter pResultFile = comResult.Parameters.Add("ResultFile", System.Data.DbType.String);
                    SQLiteParameter pModelVersion = comResult.Parameters.Add("ModelVersion", System.Data.DbType.String);
                    SQLiteParameter pVisitID = comResult.Parameters.Add("VisitID", System.Data.DbType.Int64);
                    SQLiteParameter pRunDateTime = comResult.Parameters.Add("RunDateTime", System.Data.DbType.DateTime);
                    comResult.Parameters.AddWithValue("ScavengeTypeID", nScavengeTypeID);

                    SQLiteCommand comResultID = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);

                    XmlDocument xmlDoc = new XmlDocument();
                    foreach (string xmlFile in System.IO.Directory.GetFiles(sFolder, sFileName, System.IO.SearchOption.AllDirectories))
                    {
                        xmlDoc.Load(xmlFile);

                        XmlNode nodVisitID = xmlDoc.SelectSingleNode("/TopoMetrics/Meta/VisitID");
                        XmlNode nodRunDate = xmlDoc.SelectSingleNode("/TopoMetrics/Meta/DateCreated");

                        if (bUseXMLModelVersion)
                        {
                            XmlNode nodVersion = xmlDoc.SelectSingleNode("/TopoMetrics/Meta/Version");
                            sModelVersion = nodVersion.InnerText;
                        }
                    
                        if (nodVisitID is XmlNode && nodRunDate is XmlNode && !string.IsNullOrEmpty(sModelVersion))
                        {
                            long nVisitID = long.Parse(nodVisitID.InnerText);

                            // Can only scrape metrics if the visit already exists in the database
                            if (!lVisitIDs.Contains<long>(nVisitID))
                                continue;

                            pResultFile.Value = xmlFile;
                            pModelVersion.Value = sModelVersion;
                            pVisitID.Value = nVisitID;
                            pRunDateTime.Value = DateTime.Parse(nodRunDate.InnerText);
                            comResult.ExecuteNonQuery();
                            long nResultID = (long)comResultID.ExecuteScalar();


                            foreach (string sMetricSchemaName in MetricSchemas.Keys)
                            {
                                switch (MetricSchemas[sMetricSchemaName].MetricTypeID)
                                {
                                    case 3:
                                        ScrapeVisitMetrics(nResultID, MetricSchemas[sMetricSchemaName].MetricDefs, ref xmlDoc, ref dbTrans);
                                        break;

                                    case 6:
                                        ScrapeChannelUnitMetrics(nResultID, nVisitID, MetricSchemas[sMetricSchemaName].MetricDefs, ref xmlDoc, ref dbTrans);
                                        break;

                                    default:
                                        ScrapeTierMetrics(nResultID, dTierTypes[MetricSchemas[sMetricSchemaName].MetricTypeID], MetricSchemas[sMetricSchemaName].MetricDefs, ref xmlDoc, ref dbTrans);
                                        break;
                                }
                            }

                            nFilesProcessed++;
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

        private void ScrapeVisitMetrics(long nResultID, Dictionary<string, MetricDef> MetricDefs, ref XmlDocument xmlDoc, ref SQLiteTransaction dbTrans)
        {
            SQLiteCommand comVisitMetrics = new SQLiteCommand("INSERT INTO Metric_VisitMetrics (ResultID, MetricID, MetricValue) VALUES (@ResultID, @MetricID, @MetricValue)", dbTrans.Connection, dbTrans);
            comVisitMetrics.Parameters.AddWithValue("ResultID", nResultID);
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

        private void ScrapeChannelUnitMetrics(long nResultID, long nVisitID, Dictionary<string, MetricDef> MetricDefs, ref XmlDocument xmlDoc, ref SQLiteTransaction dbTrans)
        {
            SQLiteCommand comInsert = new SQLiteCommand("INSERT INTO Metric_ChannelUnitMetrics (ResultID, MetricID, ChannelUnitID, ChannelUnitNumber, MetricValue) VALUES (@ResultID, @MetricID, @ChannelUnitID, @ChannelUnitNumber, @MetricValue)", dbTrans.Connection, dbTrans);
            comInsert.Parameters.AddWithValue("ResultID", nResultID);
            SQLiteParameter pMetricID = comInsert.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pChannelUnitID = comInsert.Parameters.Add("ChannelUnitID", System.Data.DbType.Int64);
            SQLiteParameter pMetricChannelUnitNumber = comInsert.Parameters.Add("ChannelUnitNumber", System.Data.DbType.Int64);

            SQLiteParameter pMetricValue = comInsert.Parameters.Add("MetricValue", System.Data.DbType.Double);

            //using (SQLiteConnection dbCon = new SQLiteConnection(dbTrans.Connection.ConnectionString))
            //{
            //    dbCon.Open();

            SQLiteCommand comCUID = new SQLiteCommand("SELECT ID AS ChannelUnitID FROM CHaMP_ChannelUnits WHERE (VisitID = @VisitID) AND (ChannelUnitNumber = @ChannelUnitNumber)", dbTrans.Connection, dbTrans);
            comCUID.Parameters.AddWithValue("VisitID", nVisitID);
            SQLiteParameter pChannelUnitNumber = comCUID.Parameters.Add("ChannelUnitNumber", System.Data.DbType.Int64);

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
                        // Retrieve the channel unit number from the CHaMP_ChannelUnits table
                        pChannelUnitNumber.Value = nChannelUnitNumber;
                        object objCUID = comCUID.ExecuteScalar();
                        if (objCUID != null && objCUID != DBNull.Value)
                        {
                            double fMetricValue;
                            if (!string.IsNullOrEmpty(nodMetric.InnerText) && double.TryParse(nodMetric.InnerText, out fMetricValue))
                                pMetricValue.Value = fMetricValue;
                            else
                                pMetricValue.Value = DBNull.Value;

                            pChannelUnitID.Value = (long)objCUID;
                            pMetricChannelUnitNumber.Value = nChannelUnitNumber;
                            pMetricID.Value = metric.ID;
                            comInsert.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void ScrapeTierMetrics(long nResultID, Dictionary<string, long> dTierTypes, Dictionary<string, MetricDef> MetricDefs, ref XmlDocument xmlDoc, ref SQLiteTransaction dbTrans)
        {
            SQLiteCommand comInsert = new SQLiteCommand("INSERT INTO Metric_TierMetrics (ResultID, MetricID, TierID, MetricValue) VALUES (@ResultID, @MetricID, @TierID, @MetricValue)", dbTrans.Connection, dbTrans);
            comInsert.Parameters.AddWithValue("ResultID", nResultID);
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
                    XmlNode nodTier = nodMetric.ParentNode.SelectSingleNode("Name");
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
