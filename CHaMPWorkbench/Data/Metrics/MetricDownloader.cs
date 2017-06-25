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

        public string UserName { get; internal set;  }
        public string Password { get; internal set; }

        private Dictionary<long, Dictionary<string, long>> schemaMetrics;

        public MetricDownloader(string sDBCon)
        {
            DBCon = sDBCon;

            // Load the list of programs (CHaMP, AEM etc) to which visits belong. Programs store the API URL.
            Programs = CHaMPData.Program.Load(DBCon);
        }

        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate OnProgressUpdate;

        public void Run(List<CHaMPData.VisitBasic> visits, List<CHaMPData.MetricSchema> schemas, string sUserName, string sPassword)
        {
            UserName = sUserName;
            Password = sPassword;
            int nTotalCalculations = visits.Count * schemas.Count;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    int nVisitCounter = 0;
                    foreach (long programID in programSchemaIDs.Keys)
                    {
                        CurrentProcess = string.Format("Downloading {0} metrics...", schema.Name);

                        foreach (CHaMPData.VisitBasic visit in visits)
                        {
                            LoadMetricsForSchema(schema.ID);

                            CurrentProcess = string.Format("Downloading {0} metrics...", schema.Name);
                            long nBatchID = GetBatchID(ref dbTrans, schema);

                            foreach (CHaMPData.VisitBasic visit in visits.Where<CHaMPData.VisitBasic>(x => x.ProgramID == programID))
                            {
                                try
                                {
                                    DownloadVisitMetrics(ref dbTrans, nBatchID, visit, schema, ref helper);
                                    ReportProgress(ProgressPercent(nVisitCounter, nTotalCalculations), visit.ID.ToString());
                                    nVisitCounter ++;
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                        }
                    }

                    dbTrans.Commit();

                }
                catch(Exception ex)
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
                    return (int)(100 * nVisitCounter / TotalNumberVisits);
            }
        }

        private void LoadMetricsForSchema(long schemaID)
        {
            if (schemaMetrics == null)
                schemaMetrics = new Dictionary<long, Dictionary<string, long>>();

            if (schemaMetrics.ContainsKey(schemaID))
                return;

            schemaMetrics[schemaID] = new Dictionary<string, long>();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT D.MetricID, D.DisplayNameShort FROM Metric_Schema_Definitions S INNER JOIN Metric_Definitions D ON S.MetricID = D.MetricID WHERE (S.SchemaID = 1) AND (DisplayNameShort IS NOT NULL)", dbCon);
                dbCom.Parameters.AddWithValue("SchemaID", schemaID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    schemaMetrics[schemaID][dbRead.GetString(1)] = dbRead.GetInt64(0);
            }
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
            CurrentProcess = "Authenticating user with Keystone API";

            // Determine if the program is pointing at QA or Production and use the corresponding keystone
            string keystoneURL = "https://keystone.sitkatech.com/core/connect/token";
            if (aProgram.API.Contains("https://qa."))
                keystoneURL = keystoneURL.Replace("https://", "https://qa.");

            //System.Windows.Forms.MessageBox.Show(string.Format("{0}\n{1}\n{2}\n{3}", aProgram.API, keystoneURL, Properties.Settings.Default.GeoOptixClientID, Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper()));
            //System.Windows.Forms.MessageBox.Show(string.Format("{0}\n{1}", UserName, Password));

            GeoOptix.API.ApiHelper keystoneApiHelper = new GeoOptix.API.ApiHelper(aProgram.API, keystoneURL, Properties.Settings.Default.GeoOptixClientID,
                Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(), UserName, Password);

            if (keystoneApiHelper.AuthToken.IsError)
            {
                Exception ex = new Exception(keystoneApiHelper.AuthToken.ErrorDescription, keystoneApiHelper.AuthToken.Exception);
                ex.Data["JSON"] = keystoneApiHelper.AuthToken.Json.ToString();
                throw ex;
            }

            return keystoneApiHelper;
        }

        private long InsertMetricBatch(ref SQLiteTransaction dbTrans)
        {
            long nBatchID = 0;

            try
            {
                SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Batches (ScavengeTypeID) VALUES (@ScavengeTypeID)", dbTrans.Connection, dbTrans);
                dbCom.Parameters.AddWithValue("ScavengeTypeID", ScavengeTypeID);

                dbCom.ExecuteNonQuery();

                dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                nBatchID = (long)dbCom.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting new metric batch.", ex);
            }

            return nBatchID;
        }

        private void DownloadVisitMetrics(ref SQLiteTransaction dbTrans, long nBatchID, CHaMPData.VisitBasic visit, CHaMPData.MetricSchema schema)
        {
            // Delete existing visit metrics for this schema
            SQLiteCommand dbCom = new SQLiteCommand(String.Format("DELETE FROM {0} WHERE VisitID = @VisitID", schemas.), dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("VisitID", visit.ID);

            string visitURL = string.Format(@"{0}/visits/{1}", Programs[schema.ProgramID].API, visit.ID);
            GeoOptix.API.Model.VisitSummaryModel aVisit = new GeoOptix.API.Model.VisitSummaryModel((int)visit.ID, visit.ID.ToString(), visitURL, string.Empty, string.Empty, null, null, null, null);
            GeoOptix.API.ApiResponse<GeoOptix.API.Model.MetricInstanceModel[]> theMetrics = apiHelper.GetMetricInstances(aVisit, schema.Name);

            dbCom = new SQLiteCommand(string.Format("INSERT INTO {0} (BatchID, VisitID, MetricID, MetricValue) VALUES (@BatchID, @VisitID, @MetricID, @MetricValue)", schema.DatabaseTable), dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("VisitID", visit.ID);
            dbCom.Parameters.AddWithValue("BatchID", nBatchID);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("MetricID", System.Data.DbType.Int64);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("MetricValue", System.Data.DbType.Double);

            GeoOptix.API.Model.MetricInstanceModel[] theInstances = theMetrics.Payload;
            if (theMetrics.Payload != null && theInstances.Length > 0)
            {
                foreach (GeoOptix.API.Model.MetricValueModel aValue in theInstances[0].Values)
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

            // https://www.champmonitoring.org/api/v1/visits/1/metricschemas/QA - Topo Tier 2 Metrics/metrics        
        }

    }
}
