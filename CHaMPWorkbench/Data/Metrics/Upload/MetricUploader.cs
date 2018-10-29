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
    public class MetricUploader : Classes.Secrets
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
            //throw new Exception("Are you working in production?");

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
                    ReportProgress(string.Format("The {0} schema already exists on the API with {1} defined metrics.", batch.Schema, schemaDef.Metrics.Count));
                else
                {
                    if (!MetricSchemaExists(ref apiHelper, batch.Schema.Name))
                    {
                        CreateMetricSchema(ref apiHelper, ref schemaDef);
                    }
                    CheckedSchemas.Add(schemaDef.Name);
                }

                // Loop over each visit within the batch. The visit may have 1 or more metric instances
                foreach (KeyValuePair<long, List<MetricInstance>> kvp in dVisitsToInstances)
                {
                    apiVisit visit = new apiVisit(kvp.Key, Program.API);

                    try
                    {
                        // Get all existing metric instances for this visit
                        GeoOptix.API.Model.MetricInstanceModel[] apiInstances = GetExistingInstancesFromAPI(visit, schemaDef.Name);

                        // Loop over each metric instance
                        foreach (MetricInstance inst in kvp.Value)
                        {
                            if (bgWorker.CancellationPending)
                            {
                                ReportProgress("User cancelled process. Aborting metric upload.");
                                return;
                            }

                            CreateMetricInstance(visit, inst, ref schemaDef);
                        }

                        // Now delete the existing metric instances that were on the API before this process.
                        foreach (GeoOptix.API.Model.MetricInstanceModel oldInstance in apiInstances)
                        {
                            DeleteExistingMetricInstance(ref apiHelper, oldInstance, visit.VisitID);
                        }

                        ReportProgress(string.Format("{0} metric schema complete for visit {1}", schemaDef.Name, visit.VisitID));
                    }
                    catch (Exception ex)
                    {
                        ReportProgress(string.Format("Error processing visit {0}: {1}", visit.VisitID, ex.Message), StatusTypes.Error);
                        LogMessage(ex.Message, StatusTypes.Error);
                    }

                    InstancesProcessed += 1;
                }
            }
        }

        private void DeleteExistingMetricInstance(ref GeoOptix.API.ApiHelper api, GeoOptix.API.Model.MetricInstanceModel instance, long visitID)
        {
            int nAttempt = 1;
            bool bSuccess = false;

            do
            {
                GeoOptix.API.ApiResponse<string> apiResponse = api.DeleteInstance(instance);
                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        bSuccess = true;
                        LogMessage(string.Format("Deleted existing instance for visit {0}", visitID));
                        break;

                    case System.Net.HttpStatusCode.InternalServerError:
                        LogMessage(string.Format("Internal server error deleting existing metric instance on attempt {0}", nAttempt), StatusTypes.Warning);
                        System.Threading.Thread.Sleep(2000);
                        break;

                    default:
                        throw new Exception(string.Format("{0} error attempting to delete existing metric instance", apiResponse.StatusCode.ToString()));
                }

                nAttempt += 1;

            } while (!bSuccess && nAttempt < 11);

            if (!bSuccess)
                throw new Exception(string.Format("Failed to delete existing metric instance after {0} attempts. URL: {1}", nAttempt, instance.Url));
        }

        private void CreateMetricSchema(ref GeoOptix.API.ApiHelper api, ref SchemaDefinitionWorkbench schemaDef)
        {
            int nAttempt = 1;
            bool bSuccess = false;

            do
            {
                GeoOptix.API.ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiResponse = apiHelper.CreateSchema(schemaDef.Name, GeoOptix.API.Model.ObjectType.Visit, schemaDef.Metrics.ToList<KeyValuePair<string, string>>());

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        bSuccess = true;
                        LogMessage(string.Format("Created visit level metric schema {0}", schemaDef.Name));
                        break;

                    case System.Net.HttpStatusCode.InternalServerError:
                        LogMessage(string.Format("Internal server error while creating {0} visit level metric schema on attempt {1]", schemaDef.Name, nAttempt), StatusTypes.Warning);
                        System.Threading.Thread.Sleep(2000);
                        break;

                    default:
                        throw new Exception(string.Format("{0} error attempting to create {0} visit level metric schema", apiResponse.StatusCode.ToString(), schemaDef.Name));
                }

                nAttempt += 1;

            } while (!bSuccess && nAttempt < 11);

            if (!bSuccess)
                throw new Exception(string.Format("Failed to create new visit level metric schema {0} after {0} attempts", schemaDef.Name, nAttempt));
        }

        private GeoOptix.API.Model.MetricInstanceModel[] GetExistingInstancesFromAPI(apiVisit visit, string schemaName)
        {
            // Get all existing metric instances for this visit
            ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> apiInstances = apiHelper.GetMetricInstances(visit, schemaName);

            if (apiInstances.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(string.Format("Error while attempting to retrieve existing metric instances for visit {0}: {1}", visit.VisitID, apiInstances.StatusCode));

            if (apiInstances.Payload != null)
                LogMessage(string.Format("{0} {1} existing metric instance(s) retrieved for visit {2}", apiInstances.Payload.Count<GeoOptix.API.Model.MetricInstanceModel>(), schemaName, visit.VisitID));

            return apiInstances.Payload;
        }

        private void CreateMetricInstance(apiVisit visit, MetricInstance inst, ref SchemaDefinitionWorkbench schemaDef)
        {
            LogMessage(string.Format("Creating metric instance for visit {0} with {1} metric values", inst.VisitID, schemaDef.Metrics.Count));

            List<GeoOptix.API.Model.MetricValueModel> metricValues = inst.GetAPIMetricInstance(ref schemaDef);
            ApiResponse<GeoOptix.API.Model.MetricInstanceModel> apiResult = apiHelper.CreateMetricInstance(visit, schemaDef.Name, metricValues);
            if (apiResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(string.Format("Error creating {0} metric instance for visit {1}: {2}", schemaDef.Name, visit.VisitID, apiResult.StatusCode));
        }

        private bool MetricSchemaExists(ref GeoOptix.API.ApiHelper apiHelper, string schemaName)
        {
            // Check if the visit level metric schema for this batch is defined on the API. Create it if doesn't
            ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema = apiHelper.GetMetricSchema(GeoOptix.API.Model.ObjectType.Visit, schemaName);
            if (apiSchema.StatusCode == System.Net.HttpStatusCode.OK || apiSchema.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return (apiSchema.Payload != null);
            }
            else
                throw new Exception(string.Format("Error attempting to retrieve existing visit level metric schema {0}", schemaName));
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
            apiHelper = new ApiHelper(Program.API, Program.Keystone, GeoOptixClientID, GeoOptixClientSecret.ToUpper(), UserName, Password);

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
