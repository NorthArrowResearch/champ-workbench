using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes.Metrics
{
    public class MetricInstance
    {
        public long ID { get; internal set; }
        public long BatchID { get; internal set; }
        public string ModelVersion { get; internal set; }
        public DateTime? MetricsCalculatedOn { get; internal set; }
        public DateTime? APIInsertionOn { get; internal set; }
        public DateTime WorkbenchInsertionOn { get; internal set; }

        public CHaMPData.VisitBasic Visit { get; internal set; }

        public MetricInstance(long nID, long nBatchID, string sModelVersion, DateTime? dtMetricsCalculatedOn, DateTime? dtAPIInsertionOn, DateTime dtWorkbenchInsertionOn)
        {
            ID = nID;
            BatchID = nBatchID;
            ModelVersion = sModelVersion;
            MetricsCalculatedOn = dtMetricsCalculatedOn;
            APIInsertionOn = dtAPIInsertionOn;
            WorkbenchInsertionOn = dtWorkbenchInsertionOn;
        }

        public static Dictionary<long, MetricInstance> Load(long nSchemaID, long nScavengeTypeID)
        {
            Dictionary<long, MetricInstance> dResult = new Dictionary<long, MetricInstance>();

            Dictionary<long, long> dTempVisitIDs = new Dictionary<long, long>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT I.InstanceID, I.BatchID, I.VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn, V.SiteID, V.VisitYear, S.WatershedID" +
                    " FROM Metric_Instances I INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                    " INNER JOIN CHaMP_Visits V ON I.VisitID = V.VisitID" +
                    " INNER JOIN CHaMP_Sites S ON V.SiteID = S.SiteID" +
                    " WHERE (SchemaID = @SchemaID) AND (ScavengeTypeID = @ScavengeTypeID)", dbCon);
                dbCom.Parameters.AddWithValue("SchemaID", nSchemaID);
                dbCom.Parameters.AddWithValue("ScavengeTypeID", nScavengeTypeID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nInstanceID = dbRead.GetInt64(dbRead.GetOrdinal("InstanceID"));
                    dResult[nInstanceID] = new MetricInstance(nInstanceID,
                        dbRead.GetInt64(dbRead.GetOrdinal("BatchID"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDT(ref dbRead, "MetricsCalculatedOn")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDT(ref dbRead, "APIInsertionOn")
                        , dbRead.GetDateTime(dbRead.GetOrdinal("WorkbenchInsertionOn")));

                    dTempVisitIDs[nInstanceID] = dbRead.GetInt64(dbRead.GetOrdinal("VisitID"));
                }

                // Load the visit basic information
                foreach (long nInstanceID in dTempVisitIDs.Keys)
                    dResult[nInstanceID].Visit = CHaMPData.VisitBasic.Load(dTempVisitIDs[nInstanceID]);
            }

            return dResult;
        }
    }
}
