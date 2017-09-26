using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class MetricBatch : naru.db.NamedObject
    {
        public naru.db.NamedObject ScavengeType { get; internal set; }
        public naru.db.NamedObject Schema { get; internal set; }
        public naru.db.NamedObject Program { get; internal set; }
        public string DatabaseTable { get; internal set; }
        public long Visits { get; internal set; }
        public long Instances { get; internal set; }

        public bool Copy { get; set; }

        public MetricBatch(long nbatchID, long nScavengetTypeID, string sScavengetType, long nSchemaID, string sSchema,
            long nProgramID, string sProgram, string sDatabaseTable, long? nVisits, long? nInstances)
            : base(nbatchID, string.Format("{0} - {1} - {2}", sProgram, sScavengetType, sSchema))
        {
            Schema = new naru.db.NamedObject(nSchemaID, sSchema);
            ScavengeType = new naru.db.NamedObject(nScavengetTypeID, sScavengetType);
            Program = new naru.db.NamedObject(nProgramID, sProgram);
            DatabaseTable = sDatabaseTable;
            Copy = false;

            Visits = 0;
            if (nVisits.HasValue)
                Visits = nVisits.Value;

            Instances = 0;
            if (nInstances.HasValue)
                Instances = nInstances.Value;
        }

        public static naru.ui.SortableBindingList<MetricBatch> Load()
        {
            naru.ui.SortableBindingList<MetricBatch> result = new naru.ui.SortableBindingList<MetricBatch>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT B.BatchID, B.ScavengeTypeID, L.Title AS ScavengeType, S.ProgramID, P.Title AS Program, S.SchemaID, S.Title AS Schema, S.DatabaseTable, Count(I.BatchID) AS Visits" +
                    " FROM Metric_Batches B" +
                        " INNER JOIN Metric_Schemas S ON B.SchemaID = S.SchemaID" +
                        " INNER JOIN LookupListItems L ON B.ScavengeTypeID = L.ItemID" +
                        " INNER JOIN LookupPrograms P ON S.ProgramID = P.ProgramID" +
                        " LEFT JOIN Metric_Instances I ON B.BatchID = I.BatchID" +
                    " GROUP BY B.BatchID, B.ScavengeTypeID, ScavengeType, S.ProgramID, Program, S.SchemaID, Schema, S.DatabaseTable", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    result.Add(new MetricBatch(dbRead.GetInt64(dbRead.GetOrdinal("BatchID"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("ScavengeTypeID"))
                        , dbRead.GetString(dbRead.GetOrdinal("ScavengeType"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("SchemaID"))
                        , dbRead.GetString(dbRead.GetOrdinal("Schema"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"))
                        , dbRead.GetString(dbRead.GetOrdinal("Program"))
                        , dbRead.GetString(dbRead.GetOrdinal("DatabaseTable"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Visits")
                        , 0));
                }
            }

            return result;
        }

        public Dictionary<long, List<MetricInstance>> LoadMetricInstances()
        {
            Dictionary<long, List<MetricInstance>> dVisitsToInstances = null;
            switch (DatabaseTable.ToLower())
            {
                case "metric_visitmetrics":
                    dVisitsToInstances = LoadVisitMetrics();
                    break;

                case "metric_channelunitmetrics":
                    dVisitsToInstances = LoadChannelUnitMetricsMetrics();
                    break;

                case "metric_tiermetrics":
                    ushort tierLevel = 1;
                    if (Schema.Name.ToLower().Contains("tier 2"))
                        tierLevel = 2;

                    dVisitsToInstances = LoadTierMetrics(tierLevel);
                    break;

                default:
                    throw new Exception("Unhandled database table");
            }

            return dVisitsToInstances;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <returns>Dictionary keyed by VisitID to a list of metric instances within this batch</returns>
        private Dictionary<long, List<MetricInstance>> LoadVisitMetrics()
        {
            Dictionary<long, List<MetricInstance>> instances = new Dictionary<long, List<MetricInstance>>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_Instances WHERE BatchID = @BatchID", dbCon))
                {
                    dbCom.Parameters.AddWithValue("BatchID", ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        instances[dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))] = new List<MetricInstance>() {
                            new MetricVisitInstance(
                                dbRead.GetInt64(dbRead.GetOrdinal("InstanceID"))
                                , dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))
                                , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")
                                ,naru.db.sqlite.SQLiteHelpers.GetSafeValueNDT(ref dbRead, "MetricsCalculatedOn"))
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

        private Dictionary<long, List<MetricInstance>> LoadTierMetrics(ushort TierLevel)
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
                    dbCom.Parameters.AddWithValue("BatchID", ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        long visitID = dbRead.GetInt64(dbRead.GetOrdinal("VisitID"));
                        if (!instances.ContainsKey(visitID))
                            instances[visitID] = new List<MetricInstance>();

                        instances[visitID].Add(new MetricTierInstance(
                            TierLevel
                            , dbRead.GetInt64(dbRead.GetOrdinal("InstanceID"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")
                                                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDT(ref dbRead, "MetricsCalculatedOn")
                            , dbRead.GetInt64(dbRead.GetOrdinal("TierID"))
                            , dbRead.GetString(dbRead.GetOrdinal("TierName"))));
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

        private Dictionary<long, List<MetricInstance>> LoadChannelUnitMetricsMetrics()
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
                    dbCom.Parameters.AddWithValue("BatchID", ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        long visitID = dbRead.GetInt64(dbRead.GetOrdinal("VisitID"));
                        if (!instances.ContainsKey(visitID))
                            instances[visitID] = new List<MetricInstance>();

                        instances[visitID].Add(new MetricChannelUnitInstance(
                            dbRead.GetInt64(dbRead.GetOrdinal("InstanceID"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDT(ref dbRead, "MetricsCalculatedOn")
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
    }
}
