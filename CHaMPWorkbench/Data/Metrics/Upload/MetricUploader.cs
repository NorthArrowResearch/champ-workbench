using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHaMPWorkbench.CHaMPData;
using System.Data.SQLite;
using GeoOptix.API;
using System.IO;

namespace CHaMPWorkbench.Data.Metrics.Upload
{
    public class MetricUploader
    {
        private enum StatusTypes
        {
            Info
            , Warning
            , Error
        }

        public CHaMPData.Program Program { get; internal set; }
        Dictionary<long, MetricSchema> MetricSchemas;
        Dictionary<long, MetricDefinitions.MetricDefinition> MetricDefs;
        public System.IO.FileInfo fiLog { get; internal set; }

        private int InstanceTotal { get; set; }
        private int InstancesProcessed { get; set; }

        GeoOptix.API.ApiHelper apiHelper;

        #region ProgressTracking

        private System.ComponentModel.BackgroundWorker bgWorker;
        public StringBuilder Messages { get; internal set; }

        private void ReportProgress(string sMessage, StatusTypes eStatus = StatusTypes.Info)
        {
            System.Diagnostics.Debug.Print(sMessage);
            Messages.AppendLine(sMessage);
            LogMessage(sMessage, eStatus);

            int progressPercent = 0;
            if (InstancesProcessed != 0 && InstanceTotal != 0)
                progressPercent = Convert.ToInt32(100.0 * ((double)InstancesProcessed / (double)InstanceTotal));

            bgWorker.ReportProgress(progressPercent);
        }

        private void LogMessage(string sMessage, StatusTypes eStatus = StatusTypes.Info)
        {
            if (fiLog != null)
            {
                if (!File.Exists(fiLog.FullName))
                {
                    using (StreamWriter sw = File.CreateText(fiLog.FullName))
                    {
                        sw.WriteLine("status,message");
                    }
                }

                using (StreamWriter sw = File.AppendText(fiLog.FullName))
                {
                    sw.WriteLine(string.Format("{0},{1}", eStatus.ToString().PadRight("Warning".Length, ' '), sMessage.Replace(",", "")));
                }
            }
        }

        #endregion

        public MetricUploader(System.ComponentModel.BackgroundWorker bgw, CHaMPData.Program theProgram, string sLogFile)
        {
            bgWorker = bgw;
            Program = theProgram;
            if (!string.IsNullOrEmpty(sLogFile))
                fiLog = new System.IO.FileInfo(sLogFile);

            MetricSchemas = MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString);
            MetricDefs = MetricDefinitions.MetricDefinition.Load(naru.db.sqlite.DBCon.ConnectionString);
        }

        public void Run(Dictionary<long, MetricBatch> selectedBatches, string UserName, string Password)
        {
            // Reset the messages for each run.
            Messages = new StringBuilder();

            try
            {
                ReportProgress(string.Format("Metric uploader initialized with {0} metric batches", selectedBatches.Count));

                if (AuthenticateAPI(UserName, Password))
                    if (VerifyMetricSchemasMatch(selectedBatches))
                        UploadMetrics(selectedBatches);

                ReportProgress("Metric upload complete");
            }
            catch (Exception ex)
            {
                ReportProgress(ex.Message, StatusTypes.Error);
                ReportProgress("Process aborted due to unhandled error", StatusTypes.Error);
            }
        }

        private void UploadMetrics(Dictionary<long, MetricBatch> selectedBatches)
        {
            InstanceTotal = GetTotalVisitCount(ref selectedBatches);

            // Speed up processing by only checking that each metric schema exists on the API once and then put it in this dict.
            List<string> CheckedSchemas = new List<string>();

            foreach (CHaMPData.MetricBatch batch in selectedBatches.Values)
            {
                SchemaDefinitionWorkbench schemaDef = new SchemaDefinitionWorkbench(batch.Schema.ID, batch.Schema.Name);

                // Load metric instance(s) from Workbench database for this batch. Dictionary of VisitID to instance list
                Dictionary<long, List<MetricInstance>> dVisitsToInstances = batch.LoadMetricInstances();
                ReportProgress(string.Format("Processing the {0} schema with {1} defined metrics and {2} visits", batch.Schema, schemaDef.Metrics.Count, dVisitsToInstances.Count));

                // Only bother checking that the schema exists on API if we haven't already processed this schema
                if (CheckedSchemas.Contains(schemaDef.Name))
                {
                    // Check if the visit level metric schema for this batch is defined on the API. Create it if doesn't
                    ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(GeoOptix.API.Model.ObjectType.Visit, batch.Schema.Name);
                    if (apiSchema.Payload == null)
                    {
                        // Visit metric schema does not exist... create it
                        ReportProgress(string.Format("The {0} schema does not exist on the API and is being created.", batch.Schema, dVisitsToInstances.Count));
                        apiHelper.CreateSchema(schemaDef.Name, GeoOptix.API.Model.ObjectType.Visit, schemaDef.Metrics.ToList<KeyValuePair<string, string>>());
                    }
                    CheckedSchemas.Add(schemaDef.Name);
                }
                else
                    ReportProgress(string.Format("The {0} schema does already exists on the API with {1} defined metrics.", batch.Schema, schemaDef.Metrics.Count));

                // Loop over each visit within the batch. The visit may have 1 or more metric instances
                foreach (KeyValuePair<long, List<MetricInstance>> kvp in dVisitsToInstances)
                {
                    apiVisit visit = new apiVisit(kvp.Key, Program.API);

                    try
                    {
                        // Get all existing metric instances for this visit
                        ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> apiInstances = GetExistingInstancesFromAPI(visit, schemaDef.Name);

                        // Loop over each metric instance
                        foreach (MetricInstance inst in kvp.Value)
                        {
                            if (bgWorker.CancellationPending)
                            {
                                ReportProgress("User cancelled process. Aborting metric upload.");
                                return;
                            }

                            // Create each new metric instance                    
                            ReportProgress(string.Format("Creating metric instance for visit {0} with {1} metric values", inst.VisitID, schemaDef.Metrics.Count));
                            List<GeoOptix.API.Model.MetricValueModel> metricValues = inst.GetAPIMetricInstance(ref schemaDef);
                            ApiResponse<GeoOptix.API.Model.MetricInstanceModel> apiResult = apiHelper.CreateMetricInstance(visit, batch.Schema.Name, metricValues);
                        }

                        // Now delete the existing metric instances that were on the API before this process.
                        foreach (GeoOptix.API.Model.MetricInstanceModel oldInstance in apiInstances.Payload)
                        {
                            apiHelper.DeleteInstance(oldInstance);
                            LogMessage(string.Format("Deleted existing instance for visit {0}", visit.VisitID));
                        }
                    }
                    catch (Exception ex)
                    {
                        ReportProgress(string.Format("Error processing visit {0}", visit.VisitID), StatusTypes.Error);
                        LogMessage(ex.Message, StatusTypes.Error);
                    }

                    InstancesProcessed += 1;
                }
            }
        }

        private ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> GetExistingInstancesFromAPI(apiVisit visit, string schemaName)
        {
            // Get all existing metric instances for this visit
            ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> apiInstances = apiHelper.GetMetricInstances(visit, schemaName);
            if (apiInstances.Payload != null)
                ReportProgress(string.Format("{0} {1} existing metric instance(s) retrieved for visit {2}", apiInstances.Payload.Count<GeoOptix.API.Model.MetricInstanceModel>(), schemaName, visit.VisitID));

            return apiInstances;
        }

        private int GetTotalVisitCount(ref Dictionary<long, MetricBatch> selectedBatches)
        {
            int nTotalVisitCount = 0;
            string batchIDs = string.Join(",", selectedBatches.Keys.Select<long, string>(x => x.ToString()));

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                nTotalVisitCount = Convert.ToInt32(naru.db.sqlite.SQLiteHelpers.GetScalarID(dbCon, string.Format("SELECT Count(*) FROM Metric_Instances WHERE BatchID IN ({0})", batchIDs)));
            }
            return nTotalVisitCount;
        }

        private bool AuthenticateAPI(string UserName, string Password)
        {
            apiHelper = new GeoOptix.API.ApiHelper(Program.API, Program.Keystone,
                        CHaMPWorkbench.Properties.Settings.Default.GeoOptixClientID,
                        Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(), UserName, Password);

            if (apiHelper.AuthToken.IsError)
            {
                ReportProgress(string.Format("Unable to authenticate on API at {0} as user {1}", Program.Keystone, UserName), StatusTypes.Error);
                ReportProgress(string.Format("Auth Error: {0}", apiHelper.AuthToken.Error), StatusTypes.Error);
            }
            else
                ReportProgress(string.Format("API authentication successful for {0}", Program.API));

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
                    ReportProgress(string.Format("The {0} schema differs between the Workbench database and the online XML definition", uniqueSchemas[schemaID]));
                    bStatus = false;
                }

                // If the metric schema has been defined online already then verify that it matches
                ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(GeoOptix.API.Model.ObjectType.Visit, uniqueSchemas[schemaID]);
                if (apiSchema.Payload != null)
                {
                    SchemaDefinition apiDef = new SchemaDefinition(ref apiSchema);
                    if (!dbDef.Equals(ref xmlDef, out Messages))
                    {
                        ReportProgress(string.Format("The {0} schema differs between the definition in the API and the online XML definition", uniqueSchemas[schemaID]));
                        bStatus = false;
                    }
                }
            }

            if (bStatus)
                ReportProgress("Metric schemas match between Workbench database and online XML definitions");
            else
                ReportProgress("Aborting due to mismatching metric schemas. No metrics uploaded", StatusTypes.Warning);

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
