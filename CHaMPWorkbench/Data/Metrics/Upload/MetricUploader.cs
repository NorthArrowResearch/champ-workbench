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
                List<MetricInstance> instances = null;
                switch (MetricSchemas[batch.Schema.ID].DatabaseTable.ToLower())
                {
                    case "metric_visitMetrics":
                        instances = CHaMPData.MetricVisitInstance.LoadVisitMetrics(batch);
                        break;

                    case "metric_channelunitmetrics":
                        instances = CHaMPData.MetricChannelUnitInstance.LoadChannelUnitMetricsMetrics(batch);
                        break;

                    case "metric_tiermetrics":
                        instances = CHaMPData.MetricTierInstance.LoadTierMetrics(batch);
                        break;

                    default:
                        throw new Exception("Unhandled database table");
                }




                = MetricInstance.Load(batch);
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
                        ReportProgress(0, string.Format("{0} {1} existing metric instances retrieved for visit {2}", apiInstances.Payload.Count<GeoOptix.API.Model.MetricInstanceModel>(), batch.Schema.Name, inst.VisitID));
                    }

                    GeoOptix.API.Model.MetricInstanceModel newInstance = new GeoOptix.API.Model.MetricInstanceModel();

                    List<GeoOptix.API.Model.MetricValueModel> vals;
                    switch (MetricSchemas[batch.Schema.ID].DatabaseTable.ToLower())
                    {
                        case "metric_visitMetrics":
                            vals = schemaDef.GetVisitMetricValues(inst);
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

    }
}
