using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHaMPWorkbench.CHaMPData;
using System.Data.SQLite;
using GeoOptix.API;

namespace CHaMPWorkbench.Data.Metrics.Upload
{
    public class MetricUploader
    {
        public CHaMPData.Program Program { get; internal set; }
        Dictionary<long, MetricSchema> MetricSchemas;
        Dictionary<long, MetricDefinitions.MetricDefinition> MetricDefs;

        GeoOptix.API.ApiHelper apiHelper;

        #region ProgressTracking

        private System.ComponentModel.BackgroundWorker bgWorker;
        public StringBuilder Messages { get; internal set; }

        private void ReportProgress(int value, string sMessage)
        {
            Messages.AppendLine(sMessage);
            bgWorker.ReportProgress(value);
        }

        #endregion

        public MetricUploader(System.ComponentModel.BackgroundWorker bgw, CHaMPData.Program theProgram)
        {
            bgWorker = bgw;
            Program = theProgram;

            MetricSchemas = MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString);
            MetricDefs = MetricDefinitions.MetricDefinition.Load(naru.db.sqlite.DBCon.ConnectionString);
        }

        public void Run(Dictionary<long, MetricBatch> selectedBatches, string UserName, string Password)
        {
            Messages = new StringBuilder();


            try
            {
                AuthenticateAPI(UserName, Password);

                if (VerifyMetricSchemasMatch(selectedBatches))
                {
                    UploadMetrics(selectedBatches);
                }
                else
                    ReportProgress(0, "Aborting due to mismatching metric schemas. No metrics uploaded.");
            }
            catch (Exception ex)
            {
                ReportProgress(100, string.Format("ERROR: {0}", ex.Message));
                ReportProgress(100, "Process aborted.");
            }
        }

        private void UploadMetrics(Dictionary<long, MetricBatch> selectedBatches)
        {



            foreach (CHaMPData.MetricBatch batch in selectedBatches.Values)
            {
                List<MetricInstance> instances = MetricInstance.Load(batch);
                ReportProgress(0, string.Format("Processing the {0} schema with {1} visits", batch.Schema, instances.Count));

                SchemaDefinition schemaDef = new SchemaDefinition(batch.Schema.ID, batch.Schema.Name);
                ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(GeoOptix.API.Model.ObjectType.Visit, batch.Schema.Name);
                if (apiSchema.Payload == null)
                {
                    // Visit metric schema does not exist... create it
                    apiHelper.CreateSchema(schemaDef.Name, GeoOptix.API.Model.ObjectType.Visit, schemaDef.Metrics.ToList<KeyValuePair<string, string>>());
                }

                foreach (MetricInstance inst in instances)
                {
                    if (bgWorker.CancellationPending)
                    {
                        ReportProgress(0, "User cancelled process. Aborting metric upload.");
                        return;
                    }

                    apiVisit visit = new apiVisit(inst.VisitID, Program.API);
                    ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> apiInstances = apiHelper.GetMetricInstances(visit, batch.Schema.Name);
                    if (apiInstances.Payload != null)
                    {
                        ReportProgress(0, string.Format("{0} {1} existing metric instances retrieved for visit {2}", apiInstances.Payload.Count< GeoOptix.API.Model.MetricInstanceModel>(), batch.Schema.Name, inst.VisitID));
                    }

                    GeoOptix.API.Model.MetricInstanceModel newInstance = new GeoOptix.API.Model.MetricInstanceModel();

                    List<GeoOptix.API.Model.MetricValueModel> vals;
                    switch (MetricSchemas[batch.Schema.ID].DatabaseTable.ToLower())
                    {
                        case "metric_visitMetrics":
                            vals = schemaDef.GetVisitMetricValues(inst.InstanceID);
                            break;
                            
                        //case "metric_channelunitmetrics":

                        //    break;


                        //case "metric_tiermetrics":

                        //    break;


                        default:
                            throw new Exception("Unhandled database table");
                    }

                    apiHelper.CreateMetricInstance(visit, batch.Schema.Name, vals);

                    // Now delete the existing metric instances that were on the visit before this process.
                    foreach (GeoOptix.API.Model.MetricInstanceModel oldInstance in apiInstances.Payload)
                        apiHelper.DeleteInstance(oldInstance);

                    ReportProgress(0, string.Format("\tVisit {0} with {1} metric values", inst.VisitID, inst.Metrics.Count));
                }
            }

            ReportProgress(100, "Metric upload complete.");
        }

        private void AuthenticateAPI(string UserName, string Password)
        {
            apiHelper = new GeoOptix.API.ApiHelper(Program.API, Program.Keystone,
                        CHaMPWorkbench.Properties.Settings.Default.GeoOptixClientID,
                        Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(), UserName, Password);
        }

        private bool VerifyMetricSchemasMatch(Dictionary<long, MetricBatch> selectedBatches)
        {
            // Build a list of distinct metric schemas
            Dictionary<long, string> uniqueSchemas = new Dictionary<long, string>();
            foreach (MetricBatch batch in selectedBatches.Values)
            {
                if (!uniqueSchemas.ContainsKey(batch.Schema.ID))
                    uniqueSchemas[batch.Schema.ID] = batch.Schema.Name;
            }

            bool bStatus = true;
            foreach (long schemaID in uniqueSchemas.Keys)
            {
                SchemaDefinition dbDef = new SchemaDefinition(schemaID, uniqueSchemas[schemaID]);
                SchemaDefinition xmlDef = new SchemaDefinition(MetricSchemas[schemaID].MetricSchemaXMLFile);
                List<string> Messages = null;
                if (!dbDef.Equals(ref xmlDef, out Messages))
                {
                    ReportProgress(0, string.Format("The {0} schema differs between the Workbench database and the online XML definition.", uniqueSchemas[schemaID]));
                    bStatus = false;
                }

                // If the metric schema has been defined online already then verify that it matches
                API.ApiResponse<API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(API.Model.ObjectType.Visit, uniqueSchemas[schemaID]);
                if (apiSchema.Payload != null)
                {
                    SchemaDefinition apiDef = new SchemaDefinition(ref apiSchema);
                    if (!dbDef.Equals(ref xmlDef, out Messages))
                    {
                        ReportProgress(0, string.Format("The {0} schema differs between the definition in the API and the online XML definition.", uniqueSchemas[schemaID]));
                        bStatus = false;
                    }
                }
            }

            if (bStatus)
                ReportProgress(0, "Metric schemas match between Workbench database and online XML definitions.");

            return bStatus;
        }

        private class apiVisit : GeoOptix.API.Interface.IHasUrl
        {
            public long VisitID { get; internal set; }
            public string RootAPIURL { get; internal set; }

            public string Url { get { return string.Format("{0}/visits/{1}", RootAPIURL, VisitID); } }
            public GeoOptix.API.Model.ObjectType ObjectType { get { return GeoOptix.API.Model.ObjectType.Visit; } }

            public apiVisit(long visitID, string rootAPIURL)
            {
                VisitID = visitID;
                RootAPIURL = rootAPIURL;
            }
        }

        private class BatchMetrics
        {
            MetricBatch Batch;

            List<MetricInstance> Instances;

            public BatchMetrics(MetricBatch batch)
            {
                Batch = batch;
                Instances = MetricInstance.Load(batch);
            }
        }

        private class MetricInstance
        {
            public long InstanceID { get; internal set; }
            public long VisitID { get; internal set; }
            public string ModelVersion { get; internal set; }
            public List<MetricValueBase> Metrics { get; internal set; }

            public MetricInstance(long nInstanceID, long nVisitID, string sModelVersion)
            {
                InstanceID = nInstanceID;
                VisitID = nVisitID;
                ModelVersion = sModelVersion;
                Metrics = new List<MetricValueBase>();
            }

            public static List<MetricInstance> Load(CHaMPData.MetricBatch batch)
            {
                List<MetricInstance> instances = new List<MetricInstance>();

                using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
                {
                    dbCon.Open();
                    SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_Instances WHERE BatchID = @BatchID", dbCon);
                    dbCom.Parameters.AddWithValue("BatchID", batch.ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        instances.Add(new MetricInstance(dbRead.GetInt64(dbRead.GetOrdinal("InstanceID")), dbRead.GetInt64(dbRead.GetOrdinal("VisitID")), naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "ModelVersion")));
                    }
                    dbRead.Close();

                    string sqlMetrics = string.Empty;
                    switch (batch.DatabaseTable.ToLower())
                    {
                        case "metric_visitmetrics":
                            sqlMetrics = "SELECT * FROM Metric_VisitMetrics WHERE InstanceID = @InstanceID";
                            break;

                        case "metric_channelunitmetrics":
                            sqlMetrics = "SELECT M.MetricID, MetricValue, C.ChannelUnitNumber AS ChannelUnitNumber, T1.Title AS Tier1, T2.Title AS Tier2" +
                                " FROM Metric_ChannelUnitMetrics M" +
                                " INNER JOIN Metric_Instances I ON M.InstanceID = I.InstanceID" +
                                " INNER JOIN CHaMP_ChannelUnits C ON M.ChannelUnitNumber = C.ChannelUnitNumber AND I.VisitID = C.VisitID" +
                                " INNER JOIN LookupListItems T1 ON C.Tier1 = T1.ItemID" +
                                " INNER JOIN LookupListItems T2 ON C.Tier2 = T2.ItemID" +
                                " WHERE I.InstanceID = @InstanceID";
                            break;

                        case "metric_tiermetrics":
                            sqlMetrics = "SELECT M.MetricID, MetricValue, T.Title AS Tier" +
                                " FROM Metric_TierMetrics M" +
                                " INNER JOIN Metric_Instances I ON M.InstanceID = I.InstanceID" +
                                " INNER JOIN LookupListItems T ON M.TierID = T.ItemID" +
                                " WHERE I.InstanceID = @InstanceID";
                            break;

                        default:
                            throw new Exception(string.Format("Unhandled metric table: {0}", batch.DatabaseTable));
                    }

                    dbCom = new SQLiteCommand(sqlMetrics, dbCon);
                    SQLiteParameter pInstanceID = dbCom.Parameters.Add("@InstanceID", System.Data.DbType.Int64);

                    // Now load all the metric values for this instance.
                    foreach (MetricInstance instance in instances)
                    {
                        pInstanceID.Value = instance.InstanceID;
                        dbRead = dbCom.ExecuteReader();
                        while (dbRead.Read())
                        {
                            long metricID = dbRead.GetInt64(dbRead.GetOrdinal("MetricID"));
                            double? metricValue = naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue");

                            switch (batch.DatabaseTable.ToLower())
                            {
                                case "metric_visitmetrics":
                                    instance.Metrics.Add(new MetricValueBase(metricID, metricValue));
                                    break;

                                case "metric_channelunitmetrics":
                                    instance.Metrics.Add(new ChannelUnitMetricValue(metricID, metricValue
                                        , dbRead.GetInt64(dbRead.GetOrdinal("ChannelUnityNumber"))
                                        , dbRead.GetString(dbRead.GetOrdinal("Tier1"))
                                        , dbRead.GetString(dbRead.GetOrdinal("Tier2"))));
                                    break;

                                case "metric_tiermetrics":
                                    instance.Metrics.Add(new TierMetricValue(metricID, metricValue, dbRead.GetString(dbRead.GetOrdinal("Tier"))));
                                    break;

                                default:
                                    throw new Exception(string.Format("Unhandled metric table: {0}", batch.DatabaseTable));
                            }
                        }
                        dbRead.Close();
                    }
                }

                return instances;
            }
        }

        private class MetricValueBase
        {
            public long MetricID { get; internal set; }
            public double? MetricValue { get; internal set; }

            public MetricValueBase(long nMetricID, double? fMetricValue)
            {
                MetricID = nMetricID;
                MetricValue = fMetricValue;
            }
        }

        private class TierMetricValue : MetricValueBase
        {
            public string Tier { get; internal set; }

            public TierMetricValue(long nMetricID, double? fMetricValue, string sTier)
                : base(nMetricID, fMetricValue)
            {
                Tier = sTier;
            }
        }

        private class ChannelUnitMetricValue : MetricValueBase
        {
            public long ChannelUnitNumber { get; internal set; }
            public string Tier1 { get; internal set; }
            public string Tier2 { get; internal set; }

            public ChannelUnitMetricValue(long nMetricID, double? fMetricValue, long nChannelUnitNumber, string sTier1, string sTier2)
                : base(nMetricID, fMetricValue)
            {
                ChannelUnitNumber = nChannelUnitNumber;
                Tier1 = sTier1;
                Tier2 = sTier2;
            }
        }
    }
}
