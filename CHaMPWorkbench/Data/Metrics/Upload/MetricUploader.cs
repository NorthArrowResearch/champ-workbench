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
            System.Diagnostics.Debug.Print(sMessage);
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
            // Reset the messages for each run.
            Messages = new StringBuilder();

            try
            {
                if (AuthenticateAPI(UserName, Password))
                    if (VerifyMetricSchemasMatch(selectedBatches))
                        UploadMetrics(selectedBatches);
            }
            catch (Exception ex)
            {
                ReportProgress(100, string.Format("ERROR: {0}", ex.Message));
                ReportProgress(100, "Process aborted due to unhandled error.");
            }
        }

        private void UploadMetrics(Dictionary<long, MetricBatch> selectedBatches)
        {
            long nTotalInstances = GetTotalVisitCount(ref selectedBatches);
            long nInstancesProcessed = 0;

            foreach (CHaMPData.MetricBatch batch in selectedBatches.Values)
            {
                Dictionary<long, List<MetricInstance>> dVisitsToInstances = null;
                switch (MetricSchemas[batch.Schema.ID].DatabaseTable.ToLower())
                {
                    case "metric_visitmetrics":
                        dVisitsToInstances = CHaMPData.MetricVisitInstance.LoadVisitMetrics(batch);
                        break;

                    case "metric_channelunitmetrics":
                        dVisitsToInstances = CHaMPData.MetricChannelUnitInstance.LoadChannelUnitMetricsMetrics(batch);
                        break;

                    case "metric_tiermetrics":
                        ushort tierLevel = 1;
                        if (batch.Schema.Name.Contains("ier2"))
                            tierLevel = 2;

                        dVisitsToInstances = CHaMPData.MetricTierInstance.LoadTierMetrics(tierLevel, batch);
                        break;

                    default:
                        throw new Exception("Unhandled database table");
                }


                ReportProgress(GetProgressPercent(nInstancesProcessed, nTotalInstances), string.Format("Processing the {0} schema with metrics for {1} visits", batch.Schema, dVisitsToInstances.Count));

                SchemaDefinitionWorkbench schemaDef = new SchemaDefinitionWorkbench(batch.Schema.ID, batch.Schema.Name);
                ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(GeoOptix.API.Model.ObjectType.Visit, batch.Schema.Name);
                if (apiSchema.Payload == null)
                {
                    // Visit metric schema does not exist... create it
                    ReportProgress(0, string.Format("The {0} schema does not exist on the API and is being created.", batch.Schema, dVisitsToInstances.Count));
                    apiHelper.CreateSchema(schemaDef.Name, GeoOptix.API.Model.ObjectType.Visit, schemaDef.Metrics.ToList<KeyValuePair<string, string>>());
                }

                foreach (KeyValuePair<long, List<MetricInstance>> kvp in dVisitsToInstances)
                {
                    // Build a visit object
                    apiVisit visit = new apiVisit(kvp.Key, Program.API);
                    ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> apiInstances = apiHelper.GetMetricInstances(visit, batch.Schema.Name);
                    if (apiInstances.Payload != null)
                        ReportProgress(GetProgressPercent(nInstancesProcessed, nTotalInstances), string.Format("\t{0} {1} existing metric instance(s) retrieved for visit {2}", apiInstances.Payload.Count<GeoOptix.API.Model.MetricInstanceModel>(), batch.Schema.Name, kvp.Key));

                    foreach (MetricInstance inst in kvp.Value)
                    {
                        if (bgWorker.CancellationPending)
                        {
                            ReportProgress(GetProgressPercent(nInstancesProcessed, nTotalInstances), "User cancelled process. Aborting metric upload.");
                            return;
                        }

                        // Create each new metric instance                    
                        ReportProgress(GetProgressPercent(nInstancesProcessed, nTotalInstances), string.Format("\tCreating metric instance for visit {0} with {1} metric values", inst.VisitID, inst.Metrics.Count));
                        List<GeoOptix.API.Model.MetricValueModel> metricValues = inst.GetAPIMetricInstance(ref schemaDef);
                        ApiResponse<GeoOptix.API.Model.MetricInstanceModel> apiResult = apiHelper.CreateMetricInstance(visit, batch.Schema.Name, metricValues);
                        
                        // Now delete the existing metric instances that were on the API before this process.
                        foreach (GeoOptix.API.Model.MetricInstanceModel oldInstance in apiInstances.Payload)
                            apiHelper.DeleteInstance(oldInstance);
                    }

                    nInstancesProcessed += 1;
                }

            }

            ReportProgress(100, "Metric upload complete.");
        }

        private long GetTotalVisitCount(ref Dictionary<long, MetricBatch> selectedBatches)
        {
            long nTotalVisitCount = 0;
            string batchIDs = string.Join(",", selectedBatches.Keys.Select<long, string>(x => x.ToString()));

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                nTotalVisitCount = naru.db.sqlite.SQLiteHelpers.GetScalarID(dbCon, string.Format("SELECT Count(*) FROM Metric_Instances WHERE BatchID IN ({0})", batchIDs));
            }
            return nTotalVisitCount;
        }

        private int GetProgressPercent(long nProcessed, long nTotal)
        {
            if (nProcessed == 0 || nTotal == 0)
                return 0;
            else
                return Convert.ToInt32(100.0 * ((double)nProcessed / (double)nTotal));
        }

        private bool AuthenticateAPI(string UserName, string Password)
        {
            apiHelper = new GeoOptix.API.ApiHelper(Program.API, Program.Keystone,
                        CHaMPWorkbench.Properties.Settings.Default.GeoOptixClientID,
                        Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(), UserName, Password);

            if (apiHelper.AuthToken.IsError)
            {
                ReportProgress(0, "ERROR: Unable to authenticate on API.");
                ReportProgress(0, string.Format("\t{0}", apiHelper.AuthToken.Error));
            }
            else
                ReportProgress(0, "API authentication successful.");

            return !apiHelper.AuthToken.IsError;
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
                SchemaDefinition dbDef = new SchemaDefinitionWorkbench(schemaID, uniqueSchemas[schemaID]);
                SchemaDefinition xmlDef = new SchemaDefinition(MetricSchemas[schemaID].MetricSchemaXMLFile);
                List<string> Messages = null;
                if (!dbDef.Equals(ref xmlDef, out Messages))
                {
                    ReportProgress(0, string.Format("The {0} schema differs between the Workbench database and the online XML definition.", uniqueSchemas[schemaID]));
                    bStatus = false;
                }

                // If the metric schema has been defined online already then verify that it matches
                ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(GeoOptix.API.Model.ObjectType.Visit, uniqueSchemas[schemaID]);
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
            else
                ReportProgress(0, "Aborting due to mismatching metric schemas. No metrics uploaded.");

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
    }
}
