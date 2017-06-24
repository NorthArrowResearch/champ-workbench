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
            
            GeoOptix.API.ApiHelper keystoneAPIHelper = Authenticate();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    long nBatchID = InsertMetricBatch(ref dbTrans);

                    foreach (CHaMPData.MetricSchema schema in schemas)
                    {
                        CurrentProcess = string.Format("Downloading {0} metrics...", schema.Name);

                        foreach (CHaMPData.VisitBasic visit in visits)
                        {
                            try
                            {
                                DownloadVisitMetrics(ref dbTrans, nBatchID, visit, schema);
                            }
                            catch (Exception ex)
                            {

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

        private GeoOptix.API.ApiHelper Authenticate()
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




  
            // https://www.champmonitoring.org/api/v1/visits/1/metricschemas/QA - Topo Tier 2 Metrics/metrics



        //    "metricSchemaUrl": "https://www.CHaMPMonitoring.org/api/v1/Visit/metricschemas/QA - Topo Tier 2 Metrics",
        //"itemUrl": "https://www.CHaMPMonitoring.org/api/v1/visits/1",
        //"values": [
        //    {
        //        "name": "GenerationDate",
        //        "value": "2017-06-01T19:31:03.306Z",
        //        "type": "String"
        //    },
        //    {
        //        "name": "Area",
        //        "value": "0.0",
        //        "type": "Numeric"
        //    },
        //    {
        //        "name": "Ct",
        //        "value": "0",
        //        "type": "Numeric"
        //    },

        }

    }
}
