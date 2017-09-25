using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public abstract class MetricInstance
    {
        public const string MODEL_VERSION_METRIC_NAME = "ModelVersion";
        public const string GENERATION_DATE_METRIC_NAME = "GenerationDate";

        public long InstanceID { get; internal set; }
        public long VisitID { get; internal set; }
        public string ModelVersion { get; internal set; }
        public Dictionary<long, double?> Metrics { get; internal set; }
        public DateTime GenerationDate { get; internal set; }

        public MetricInstance(long nInstanceID, long nVisitID, string sModelVersion)
        {
            InstanceID = nInstanceID;
            VisitID = nVisitID;
            ModelVersion = sModelVersion;
            Metrics = new Dictionary<long, double?>();
        }

        public virtual List<GeoOptix.API.Model.MetricValueModel> GetAPIMetricInstance(ref Data.Metrics.Upload.SchemaDefinitionWorkbench schemaDef)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = new List<GeoOptix.API.Model.MetricValueModel>();
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel(MODEL_VERSION_METRIC_NAME, ModelVersion));
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel(GENERATION_DATE_METRIC_NAME, GenerationDate.ToString("o")));

            foreach (Data.MetricDefinitions.MetricDefinitionBase metricDef in schemaDef.MetricsByID.Values)
            {
                if (Metrics.ContainsKey(metricDef.ID) && Metrics[metricDef.ID].HasValue)
                {
                    if (metricDef.DataTypeID == 10023)
                    {
                        string sFormat = "0";
                        if (metricDef.Precision.HasValue)
                            sFormat = string.Format("0:0.{0}", new string('0', Convert.ToInt32(metricDef.Precision.Value)));

                        string sMetricValue = Metrics[metricDef.ID].Value.ToString(sFormat);
                        metricValues.Add(new GeoOptix.API.Model.MetricValueModel(metricDef.Name, sMetricValue));
                    }
                }
            }

            return metricValues;
        }

        public class MetricInstanceValue : naru.db.NamedObject
        {
            public double? MetricValue { get; internal set; }

            public MetricInstanceValue(long nID, string sName, double? fMetricValue)
                : base(nID, sName)
            {
                MetricValue = fMetricValue;
            }
        }
    }

    public class MetricVisitInstance : MetricInstance
    {
        public MetricVisitInstance(long nInstanceID, long nVisitID, string sModelVersion)
            : base(nInstanceID, nVisitID, sModelVersion)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <returns>Dictionary keyed by VisitID to a list of metric instances within this batch</returns>
        public static Dictionary<long, List<MetricInstance>> LoadVisitMetrics(CHaMPData.MetricBatch batch)
        {
            Dictionary<long, List<MetricInstance>> instances = new Dictionary<long, List<MetricInstance>>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_Instances WHERE BatchID = @BatchID", dbCon))
                {
                    dbCom.Parameters.AddWithValue("BatchID", batch.ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        instances[dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))] = new List<MetricInstance>() {
                            new MetricVisitInstance(dbRead.GetInt64(dbRead.GetOrdinal("InstanceID")), dbRead.GetInt64(dbRead.GetOrdinal("VisitID")), naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion"))
                        };
                    }
                    dbRead.Close();
                }

                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_VisitMetrics WHERE InstanceID = @InstanceID", dbCon))
                {
                    SQLiteParameter pInstanceID = dbCom.Parameters.Add("@InstanceID", System.Data.DbType.Int64);

                    // Now load all the metric values for this instance.
                    foreach (List<MetricInstance> instanceList in instances.Values)
                    {
                        foreach (MetricInstance instance in instanceList)
                        {
                            pInstanceID.Value = instance.InstanceID;
                            SQLiteDataReader dbRead = dbCom.ExecuteReader();
                            while (dbRead.Read())
                                instance.Metrics[dbRead.GetInt64(dbRead.GetOrdinal("MetricID"))] = naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue");
                            dbRead.Close();
                        }
                    }
                }
            }

            return instances;
        }
    }

    public class MetricTierInstance : MetricInstance
    {
        public long TierID { get; internal set; }
        public string TierName { get; internal set; }
        public ushort TierLevel { get; internal set; }

        public MetricTierInstance(ushort TierLevel, long nInstanceID, long nVisitID, string sModelVersion, long nTierID, string sTierName)
        : base(nInstanceID, nVisitID, sModelVersion)
        {
            TierID = nTierID;
            TierName = sTierName;
        }

        public static Dictionary<long, List<MetricInstance>> LoadTierMetrics(ushort TierLevel, CHaMPData.MetricBatch batch)
        {
            Dictionary<long, List<MetricInstance>> instances = new Dictionary<long, List<MetricInstance>>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT I.InstanceID AS InstanceID, VisitID, ModelVersion, TierID, L.Title AS TierName" +
                    " FROM Metric_Instances I" +
                        " INNER JOIN Metric_TierMetrics T ON I.InstanceID = T.InstanceID" +
                        " INNER JOIN LookupListItems L ON T.TierID = L.ItemID" +
                    " WHERE BatchID = @BatchID" +
                    " GROUP BY I.InstanceID, VisitID, ModelVersion, TierID, TierName", dbCon))
                {
                    dbCom.Parameters.AddWithValue("BatchID", batch.ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        long visitID = dbRead.GetInt64(dbRead.GetOrdinal("VisitID"));
                        if (!instances.ContainsKey(visitID))
                            instances[visitID] = new List<MetricInstance>();

                        instances[visitID].Add(new MetricTierInstance(TierLevel, dbRead.GetInt64(dbRead.GetOrdinal("InstanceID")), dbRead.GetInt64(dbRead.GetOrdinal("VisitID")), naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")
                            , dbRead.GetInt64(dbRead.GetOrdinal("TierID")), dbRead.GetString(dbRead.GetOrdinal("TierName"))));
                    }
                    dbRead.Close();
                }


                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_TierMetrics WHERE (InstanceID = @InstanceID) AND (TierID = @TierID)", dbCon))
                {
                    SQLiteParameter pInstanceID = dbCom.Parameters.Add("InstanceID", System.Data.DbType.Int64);
                    SQLiteParameter pTierID = dbCom.Parameters.Add("TierID", System.Data.DbType.Int64);

                    // Now load all the metric values for this instance.
                    foreach (List<MetricInstance> instanceList in instances.Values)
                    {
                        foreach (MetricInstance instance in instanceList)
                        {
                            pInstanceID.Value = instance.InstanceID;
                            pTierID.Value = ((MetricTierInstance)instance).TierID;
                            SQLiteDataReader dbRead = dbCom.ExecuteReader();
                            while (dbRead.Read())
                                instance.Metrics[dbRead.GetInt64(dbRead.GetOrdinal("MetricID"))] = naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue");
                            dbRead.Close();
                        }
                    }
                }
            }
            return instances;
        }

        public override List<GeoOptix.API.Model.MetricValueModel> GetAPIMetricInstance(ref Data.Metrics.Upload.SchemaDefinitionWorkbench schemaDef)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = base.GetAPIMetricInstance(ref schemaDef);
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel(string.Format("Tier{0}", TierLevel), TierName));
            return metricValues;
        }
    }

    public class MetricChannelUnitInstance : MetricInstance
    {
        public long ChannelUnitNumber { get; internal set; }

        public string Tier1Name { get; internal set; }
        public string Tier2Name { get; internal set; }

        public MetricChannelUnitInstance(long nInstanceID, long nVisitID, string sModelVersion, long nChannelUnitNumber, string sTier1Name, string sTier2Name)
        : base(nInstanceID, nVisitID, sModelVersion)
        {
            ChannelUnitNumber = nChannelUnitNumber;
            Tier1Name = sTier1Name;
            Tier2Name = sTier2Name;
        }

        public static Dictionary<long, List<MetricInstance>> LoadChannelUnitMetricsMetrics(CHaMPData.MetricBatch batch)
        {
            Dictionary<long, List<MetricInstance>> instances = new Dictionary<long, List<MetricInstance>>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT I.InstanceID AS InstanceID, I.VisitID, ModelVersion, M.ChannelUnitNumber AS ChannelUnitNumber, C.Tier1, C.Tier2" +
                    " FROM Metric_Instances I" +
                    " INNER JOIN Metric_ChannelUnitMetrics M ON I.InstanceID = M.InstanceID" +
                    " INNER JOIN CHaMP_ChannelUnits C ON I.VisitID = C.VisitID AND C.ChannelUnitNumber = M.ChannelUnitNumber" +
                    " WHERE BatchID = @BatchID" +
                    " GROUP BY I.InstanceID, I.VisitID, ModelVersion, M.ChannelUnitNumber, C.Tier1, C.Tier2", dbCon))
                {
                    dbCom.Parameters.AddWithValue("BatchID", batch.ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        long visitID = dbRead.GetInt64(dbRead.GetOrdinal("VisitID"));
                        if (!instances.ContainsKey(visitID))
                            instances[visitID] = new List<MetricInstance>();

                        instances[visitID].Add(new MetricChannelUnitInstance(dbRead.GetInt64(dbRead.GetOrdinal("InstanceID")), dbRead.GetInt64(dbRead.GetOrdinal("VisitID")), naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")
                            , dbRead.GetInt64(dbRead.GetOrdinal("ChannelUnitNumber")), dbRead.GetString(dbRead.GetOrdinal("Tier1")), dbRead.GetString(dbRead.GetOrdinal("Tier1"))));
                    }
                    dbRead.Close();
                }

                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_ChannelUnitMetrics WHERE (InstanceID = @InstanceID) AND (ChannelUnitNumber = @ChannelUnitNumber)", dbCon))
                {
                    SQLiteParameter pInstanceID = dbCom.Parameters.Add("InstanceID", System.Data.DbType.Int64);
                    SQLiteParameter pChannelUnitNumber = dbCom.Parameters.Add("ChannelUnitNumber", System.Data.DbType.Int64);

                    // Now load all the metric values for this instance.
                    foreach (List<MetricInstance> instanceList in instances.Values)
                    {
                        foreach (MetricInstance instance in instanceList)
                        {
                            pInstanceID.Value = instance.InstanceID;
                            pChannelUnitNumber.Value = ((MetricChannelUnitInstance)instance).ChannelUnitNumber;
                            SQLiteDataReader dbRead = dbCom.ExecuteReader();
                            while (dbRead.Read())
                                instance.Metrics[dbRead.GetInt64(dbRead.GetOrdinal("MetricID"))] = naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue");
                            dbRead.Close();
                        }
                    }
                }
            }
            return instances;
        }

        public override List<GeoOptix.API.Model.MetricValueModel> GetAPIMetricInstance(ref Data.Metrics.Upload.SchemaDefinitionWorkbench schemaDef)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = base.GetAPIMetricInstance(ref schemaDef);
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel("ChannelUnitNumber", ChannelUnitNumber.ToString()));
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel("Tier1", Tier1Name));
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel("Tier1", Tier2Name));
            return metricValues;
        }
    }
}
