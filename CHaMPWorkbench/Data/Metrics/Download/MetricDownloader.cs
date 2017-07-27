using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data.Metrics
{
    public class MetricDownloader
    {
        private const long ScavengeTypeID = 1;

        private readonly string DBCon;
        private readonly Dictionary<long, CHaMPData.Program> Programs;

        public string CurrentProcess { get; internal set; }
        public StringBuilder ErrorMessages { get; internal set; }

        public string UserName { get; internal set; }
        public string Password { get; internal set; }

        private Dictionary<long, Dictionary<string, long>> schemaMetrics;
        private Dictionary<long, Dictionary<string, long>> tierTypes;

        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate OnProgressUpdate;

        public MetricDownloader(string sDBCon)
        {
            DBCon = sDBCon;

            // Load the list of programs (CHaMP, AEM etc) to which visits belong. Programs store the API URL.
            Programs = CHaMPData.Program.Load(DBCon);

            // Append each error message as a new line
            ErrorMessages = new StringBuilder();

            // Load the master lookup of tier types from the database
            LoadTierTypes();
        }

        public void Run(List<CHaMPData.VisitBasic> visits, List<CHaMPData.MetricSchema> schemas, System.ComponentModel.BackgroundWorker bgw, string sUserName, string sPassword)
        {
            if (!naru.web.CheckForInternetConnection())
            {
                ErrorMessages.AppendLine("Failed to detect valid internet connection.");
                ReportProgress(100, "Aborted");
                return;
            }

            UserName = sUserName;
            Password = sPassword;
            int nTotalCalculations = visits.Count * schemas.Count;

            // Organize the metric schemas by their program ID so that authenication is only performed once per program
            Dictionary<long, List<CHaMPData.MetricSchema>> programSchemaIDs = OrganizeSchemasByProgram(schemas);

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    int nVisitCounter = 0;
                    foreach (long programID in programSchemaIDs.Keys)
                    {
                        ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), string.Format("Authenticating against {0} API", Programs[programID]));
                        GeoOptix.API.ApiHelper apiHelper = new GeoOptix.API.ApiHelper(Programs[programID].API, Programs[programID].Keystone,
                            CHaMPWorkbench.Properties.Settings.Default.GeoOptixClientID,
                           Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(), UserName, Password);

                        bool bAuthorizedToViewMetrics = true;

                        foreach (CHaMPData.MetricSchema schema in programSchemaIDs[programID])
                        {
                            ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), string.Format("Downloading {0} metrics...", schema.Name));

                            long nBatchID = GetBatchID(ref dbTrans, schema);

                            // Load the metric definitions for this schema. This will populate member dictionary keyed by schema ID
                            LoadMetricDefinitionsForSchema(ref dbTrans, schema.ID);

                            foreach (CHaMPData.VisitBasic visit in visits.Where<CHaMPData.VisitBasic>(x => x.ProgramID == programID))
                            {
                                if (bgw.CancellationPending)
                                {
                                    dbTrans.Rollback();
                                    return;
                                }

                                // Report progress at top of the loop because some steps below skip code
                                nVisitCounter++;
                                ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), string.Format("{0} visit {1}", schema.Name, visit.ID));

                                // Delete existing API downloaded metrics for this visit and schema
                                DeleteExistingMetrics(ref dbTrans, nBatchID, visit.ID);

                                string visitURL = string.Format(@"{0}/visits/{1}", Programs[schema.ProgramID].API, visit.ID);
                                GeoOptix.API.Model.VisitSummaryModel aVisit = new GeoOptix.API.Model.VisitSummaryModel((int)visit.ID, visit.ID.ToString(), visitURL, string.Empty, string.Empty, null, null, null, null);
                                GeoOptix.API.ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> theMetrics = apiHelper.GetMetricInstances(aVisit, schema.Name);

                                if (theMetrics.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    // User's account does not allow for viewing metrics
                                    string sAuthorizationError = string.Format("Not authorized to download metrics for schema {0}.", schema.Name);
                                    ErrorMessages.AppendLine(sAuthorizationError);
                                    ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), sAuthorizationError);
                                    bAuthorizedToViewMetrics = false;
                                    break;
                                }

                                if (theMetrics.StatusCode != System.Net.HttpStatusCode.OK)
                                {
                                    if (theMetrics.StatusCode == System.Net.HttpStatusCode.BadRequest && theMetrics.PayloadAsText.ToLower().Contains("unknown schema"))
                                    {
                                        ErrorMessages.AppendLine(string.Format("Visit {0} does not possess any metrics for the {1} schema", aVisit.Id, schema.Name));
                                    }
                                    else
                                    {
                                        ErrorMessages.AppendLine(string.Format("Status {0} for visit {1} and schema {2}: {3}", theMetrics.StatusCode.ToString(), aVisit.Id, schema.Name, theMetrics.PayloadAsText));
                                    }
                                }

                                if (theMetrics.Payload == null || theMetrics.Payload.Length < 1)
                                    continue;

                                GeoOptix.API.Model.MetricInstanceModel[] metricInstances = theMetrics.Payload;
                                if (metricInstances.Length == 0)
                                {
                                    ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), string.Format("No {0} metrics for visit {1}", schema.Name, visit.ID));
                                    continue;
                                }

                                try
                                {
                                    switch (schema.DatabaseTable.ToLower())
                                    {
                                        case ("metric_visitmetrics"):
                                            DownloadVisitMetrics(ref dbTrans, nBatchID, visit.ID, schema, ref metricInstances);
                                            break;

                                        case ("metric_channelunitmetrics"):
                                            DownloadChannelUnitMetrics(ref dbTrans, nBatchID, visit.ID, schema, ref metricInstances);
                                            break;

                                        case ("metric_tiermetrics"):
                                            DownloadTierMetrics(ref dbTrans, nBatchID, visit.ID, schema, ref metricInstances);
                                            break;

                                        default:
                                            throw new Exception(string.Format("Unhandled metric schema database table '{0}'", schema.DatabaseTable));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), string.Format("Error on visit {0}: {1}", visit.ID, ex.Message));
                                }
                            }

                            // Do not attempt to process other schemas within this API if the user can't authenticate
                            if (!bAuthorizedToViewMetrics)
                                break;
                        }
                    }

                    dbTrans.Commit();

                    if (ErrorMessages.Length < 1)
                        ReportProgress(100, "Process completed without errors.");
                    else
                        ReportProgress(100, "Process completed with errors.");
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }
        }

        private void ReportProgress(int value, string sMessage)
        {
            CurrentProcess = sMessage;
            OnProgressUpdate(value);
        }

        private int ProgressPercent(int nVisitCounter, int TotalNumberVisits)
        {
            if (TotalNumberVisits == 0)
                return 0;
            else
            {
                if (nVisitCounter == 0)
                    return 0;
                else
                    if (nVisitCounter == TotalNumberVisits)
                    return 100;
                else
                    return (int)(100.0 * (double)nVisitCounter / (double)TotalNumberVisits);
            }
        }

        private void LoadTierTypes()
        {
            // Create a new lookup dictionary for the tier types. 
            // Key is tier level (1, 2) and the value is a dictionary of tier types to their DB IDs
            tierTypes = new Dictionary<long, Dictionary<string, long>>();

            Dictionary<long, long> tierListIDs = new Dictionary<long, long>();
            tierListIDs[1] = 5;
            tierListIDs[2] = 11;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                foreach (long typeIndex in tierListIDs.Keys)
                {
                    tierTypes[typeIndex] = new Dictionary<string, long>();

                    SQLiteCommand dbCom = new SQLiteCommand("SELECT ItemID, Title FROM LookupListItems WHERE (ListID = @ListID) ORDER BY Title", dbCon);
                    dbCom.Parameters.AddWithValue("ListID", tierListIDs[typeIndex]);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                        tierTypes[typeIndex][dbRead.GetString(1)] = dbRead.GetInt64(0);
                }
            }
        }

        private void LoadMetricDefinitionsForSchema(ref SQLiteTransaction dbTrans, long schemaID)
        {
            if (schemaMetrics == null)
                schemaMetrics = new Dictionary<long, Dictionary<string, long>>();

            if (schemaMetrics.ContainsKey(schemaID))
                return;

            schemaMetrics[schemaID] = new Dictionary<string, long>();

            SQLiteCommand dbCom = new SQLiteCommand("SELECT D.MetricID, D.DisplayNameShort FROM Metric_Schema_Definitions S INNER JOIN Metric_Definitions D ON S.MetricID = D.MetricID" +
                " WHERE (S.SchemaID = @SchemaID) AND (DisplayNameShort IS NOT NULL) AND (DataTypeID = @DataTypeID)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("SchemaID", schemaID);
            dbCom.Parameters.AddWithValue("DataTypeID", 10023); // Numeric only metrics
            SQLiteDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
                schemaMetrics[schemaID][dbRead.GetString(1)] = dbRead.GetInt64(0);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemas">List of Metric SchemaIDs</param>
        /// <returns>Dictionary of program IDs keyed to a list of metric schemas that
        /// belong to this schema</returns>
        /// <remarks>We only want to authenticate againt each API once. Therefore
        /// reorganize the schemas by unique program</remarks>
        private Dictionary<long, List<CHaMPData.MetricSchema>> OrganizeSchemasByProgram(List<CHaMPData.MetricSchema> schemas)
        {
            Dictionary<long, List<CHaMPData.MetricSchema>> programsToSchemas = new Dictionary<long, List<CHaMPData.MetricSchema>>();

            foreach (CHaMPData.MetricSchema schema in schemas)
            {
                if (!programsToSchemas.ContainsKey(schema.ProgramID))
                    programsToSchemas[schema.ProgramID] = new List<CHaMPData.MetricSchema>();
                programsToSchemas[schema.ProgramID].Add(schema);
            }

            return programsToSchemas;
        }

        private long GetBatchID(ref SQLiteTransaction dbTrans, CHaMPData.MetricSchema schema)
        {
            SQLiteCommand dbCom = new SQLiteCommand("SELECT BatchID FROM Metric_Batches WHERE (SchemaID = @SchemaID) AND (ScavengeTypeID = @ScavengeTypeID)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("SchemaID", schema.ID);
            dbCom.Parameters.AddWithValue("ScavengeTypeID", ScavengeTypeID);
            long nBatchID = naru.db.sqlite.SQLiteHelpers.GetScalarID(ref dbCom);

            if (nBatchID < 1)
            {
                try
                {
                    dbCom = new SQLiteCommand("INSERT INTO Metric_Batches (SchemaID, ScavengeTypeID, Title) VALUES (@SchemaID, @ScavengeTypeID, 'API Download')", dbTrans.Connection, dbTrans);
                    dbCom.Parameters.AddWithValue("SchemaID", schema.ID);
                    dbCom.Parameters.AddWithValue("ScavengeTypeID", ScavengeTypeID);
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

        private long InsertMetricInstance(ref SQLiteTransaction dbTrans, long batchID, long visitID, DateTime dtAPIInsertionOn, string sModelVersion)
        {
            string sMetricsCalculatedOn = string.Empty;

            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Instances (BatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn)" +
                " VALUES (@BatchID, @VisitID, @ModelVersion, @MetricsCalculatedOn, @APIInsertionOn)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("BatchID", batchID);
            dbCom.Parameters.AddWithValue("VisitID", visitID);
            dbCom.Parameters.AddWithValue("MetricsCalculatedOn", DBNull.Value); // placeholder until this value is available in the API
            dbCom.Parameters.AddWithValue("APIInsertionOn", dtAPIInsertionOn);
            naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, sModelVersion, "ModelVersion");
            dbCom.ExecuteNonQuery();

            dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
            long instanceID = (long)dbCom.ExecuteScalar();
            return instanceID;
        }

        private Dictionary<string, Tuple<int, DateTime, string>> GetLatestMetricInstances(ref GeoOptix.API.Model.MetricInstanceModel[] metricInstances, string sDistinguishingMetricName)
        {
            Dictionary<string, Tuple<int, DateTime, string>> newestMetricInstances = new Dictionary<string, Tuple<int, DateTime, string>>();

            for (int i = 0; i < metricInstances.Length; i++)
            {
                string sDistinguishingValue = "none";
                DateTime dtAPIInsertionDate = new DateTime();
                string sModelVersion = string.Empty;
                foreach (GeoOptix.API.Model.MetricValueModel aValue in metricInstances[i].Values)
                {
                    if (string.Compare(aValue.Name, sDistinguishingMetricName, true) == 0)
                        sDistinguishingValue = aValue.Value;

                    if (string.Compare(aValue.Name, "GenerationDate", true) == 0)
                        dtAPIInsertionDate = DateTime.Parse(aValue.Value);

                    if (string.Compare(aValue.Name, "ModelVersion", true) == 0)
                        sModelVersion = aValue.Value;

                }

                if (string.Compare(sDistinguishingValue, "none", true) == 0)
                    System.Diagnostics.Debug.Assert(string.IsNullOrEmpty(sDistinguishingMetricName), "Only visit level metrics - with no distinguishing metric - should have no distinguishing metric value.");

                if (!newestMetricInstances.ContainsKey(sDistinguishingValue) || dtAPIInsertionDate > newestMetricInstances[sDistinguishingValue].Item2)
                    newestMetricInstances[sDistinguishingValue] = new Tuple<int, DateTime, string>(i, dtAPIInsertionDate, sModelVersion);
            }

            return newestMetricInstances;
        }

        private void DownloadVisitMetrics(ref SQLiteTransaction dbTrans, long batchID, long visitID, CHaMPData.MetricSchema schema, ref GeoOptix.API.Model.MetricInstanceModel[] metricInstances)
        {
            // The API might erroneously contain duplicates. Find the newest instance for each channel unit
            Dictionary<string, Tuple<int, DateTime, string>> newestMetricInstances = GetLatestMetricInstances(ref metricInstances, string.Empty);

            if (newestMetricInstances.Count < 1)
                return;

            long nInstanceID = InsertMetricInstance(ref dbTrans, batchID, visitID, newestMetricInstances.Values.First<Tuple<int, DateTime, string>>().Item2, newestMetricInstances.Values.First<Tuple<int, DateTime, string>>().Item3);

            SQLiteCommand dbCom = new SQLiteCommand(string.Format("INSERT INTO {0} (InstanceID, MetricID, MetricValue) VALUES (@InstanceID, @MetricID, @MetricValue)", schema.DatabaseTable), dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("InstanceID", nInstanceID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("MetricValue", System.Data.DbType.Double);

            System.Diagnostics.Debug.Assert(newestMetricInstances.Count == 1, "There should only be one instance of visit level metrics that is newest, even if there are erroneously multiple in the API");
            foreach (Tuple<int, DateTime, string> metricInstanceInfo in newestMetricInstances.Values)
            {
                foreach (GeoOptix.API.Model.MetricValueModel aValue in metricInstances[metricInstanceInfo.Item1].Values)
                {
                    System.Diagnostics.Debug.Assert(schemaMetrics.ContainsKey(schema.ID));

                    if (schemaMetrics[schema.ID].ContainsKey(aValue.Name))
                    {
                        pMetricID.Value = schemaMetrics[schema.ID][aValue.Name];
                        pMetricValue.Value = aValue.Value;
                        dbCom.ExecuteNonQuery();
                    }
                }
            }
        }

        private void DownloadChannelUnitMetrics(ref SQLiteTransaction dbTrans, long batchID, long visitID, CHaMPData.MetricSchema schema, ref GeoOptix.API.Model.MetricInstanceModel[] metricInstances)
        {
            // The API might erroneously contain duplicates. Find the newest instance for each channel unit
            Dictionary<string, Tuple<int, DateTime, string>> newestMetricInstances = GetLatestMetricInstances(ref metricInstances, "ChUnitNumber");

            if (newestMetricInstances.Count < 1)
                return;

            long nInstanceID = InsertMetricInstance(ref dbTrans, batchID, visitID, newestMetricInstances.Values.First<Tuple<int, DateTime, string>>().Item2, newestMetricInstances.Values.First<Tuple<int, DateTime, string>>().Item3);

            SQLiteCommand dbCom = new SQLiteCommand(string.Format("INSERT INTO {0} (InstanceID, ChannelUnitNumber, MetricID, MetricValue) VALUES (@InstanceID, @ChannelUnitNumber, @MetricID, @MetricValue)", schema.DatabaseTable), dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("InstanceID", nInstanceID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pChannelUnitNumber = dbCom.Parameters.Add("ChannelUnitNumber", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("MetricValue", System.Data.DbType.Double);

            foreach (string sChannelUnitNumber in newestMetricInstances.Keys)
            {
                pChannelUnitNumber.Value = long.Parse(sChannelUnitNumber);

                int metricInstanceIndex = newestMetricInstances[sChannelUnitNumber].Item1;
                foreach (GeoOptix.API.Model.MetricValueModel aValue in metricInstances[metricInstanceIndex].Values)
                {
                    if (schemaMetrics[schema.ID].ContainsKey(aValue.Name))
                    {
                        pMetricID.Value = schemaMetrics[schema.ID][aValue.Name];
                        pMetricValue.Value = aValue.Value;
                        dbCom.ExecuteNonQuery();
                    }
                }
            }
        }

        private void DownloadTierMetrics(ref SQLiteTransaction dbTrans, long batchID, long visitID, CHaMPData.MetricSchema schema, ref GeoOptix.API.Model.MetricInstanceModel[] metricInstances)
        {
            long tierIndex = 1;
            if (schema.Name.ToLower().Contains("tier 2"))
                tierIndex = 2;
            string sTierMetricName = string.Format("Tier{0}", tierIndex);

            // The API might erroneously contain duplicates. Find the newest instance for each channel unit
            Dictionary<string, Tuple<int, DateTime, string>> newestMetricInstances = GetLatestMetricInstances(ref metricInstances, sTierMetricName);

            if (newestMetricInstances.Count < 1)
                return;

            long nInstanceID = InsertMetricInstance(ref dbTrans, batchID, visitID, newestMetricInstances.Values.First<Tuple<int, DateTime, string>>().Item2, newestMetricInstances.Values.First<Tuple<int, DateTime, string>>().Item3);

            SQLiteCommand dbCom = new SQLiteCommand(string.Format("INSERT INTO {0} (InstanceID, MetricID, TierID, MetricValue) VALUES (@InstanceID, @MetricID, @TierID, @MetricValue)", schema.DatabaseTable), dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("InstanceID", nInstanceID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pTierID = dbCom.Parameters.Add("TierID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("MetricValue", System.Data.DbType.Double);

            foreach (string sTierID in newestMetricInstances.Keys)
            {
                pTierID.Value = tierTypes[tierIndex][sTierID];
                int nMetricInstanceIndex = newestMetricInstances[sTierID].Item1;

                foreach (GeoOptix.API.Model.MetricValueModel aValue in metricInstances[nMetricInstanceIndex].Values)
                {
                    System.Diagnostics.Debug.Assert(schemaMetrics.ContainsKey(schema.ID));

                    if (schemaMetrics[schema.ID].ContainsKey(aValue.Name))
                    {
                        pMetricID.Value = schemaMetrics[schema.ID][aValue.Name];
                        pMetricValue.Value = aValue.Value;
                        dbCom.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
