using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Xml;

namespace CHaMPWorkbench.Experimental.Philip
{
    public class TopoMetricScavenger
    {
        public TopoMetricScavenger()
        {

        }

        public void Run()
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Top Level Result Folder";
            if (frm.ShowDialog() != DialogResult.OK)
                return;

            Dictionary<long, string> metricXPaths = GetMetrics();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    SQLiteCommand comResult = new SQLiteCommand("INSERT INTO Metric_Results (ResultFile, ModelVersion, VisitID, RunDateTime, ScavengeTypeID) VALUES (@ResultFile, @ModelVersion, @VisitID, @RunDateTime, 1)", dbTrans.Connection, dbTrans);
                    SQLiteParameter pResultFile = comResult.Parameters.Add("ResultFile", System.Data.DbType.String);
                    SQLiteParameter pModelVersion = comResult.Parameters.Add("ModelVersion", System.Data.DbType.String);
                    SQLiteParameter pVisitID = comResult.Parameters.Add("VisitID", System.Data.DbType.Int64);
                    SQLiteParameter pRunDateTime = comResult.Parameters.Add("RunDateTime", System.Data.DbType.DateTime);

                    SQLiteCommand comResultID = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    
                    SQLiteCommand comVisitMetrics = new SQLiteCommand("INSERT INTO Metric_VisitMetrics (ResultID, MetricID, MetricValue) VALUES (@ResultID, @MetricID, @MetricValue)", dbTrans.Connection, dbTrans);
                    SQLiteParameter pResultID = comVisitMetrics.Parameters.Add("ResultID", System.Data.DbType.Int64);
                    SQLiteParameter pMetricID = comVisitMetrics.Parameters.Add("MetricID", System.Data.DbType.Int64);
                    SQLiteParameter pMetricValue = comVisitMetrics.Parameters.Add("MetricValue", System.Data.DbType.Double);

                    XmlDocument xmlDoc = new XmlDocument();
                    foreach (string xmlFile in System.IO.Directory.GetFiles(frm.SelectedPath, "metrics.xml", System.IO.SearchOption.AllDirectories))
                    {
                        xmlDoc.Load(xmlFile);

                        XmlNode nodVisitID = xmlDoc.SelectSingleNode("/TopoMetrics/Meta/VisitID");
                        XmlNode nodVersion = xmlDoc.SelectSingleNode("/TopoMetrics/Meta/Version");
                        XmlNode nodRunDate = xmlDoc.SelectSingleNode("/TopoMetrics/Meta/DateCreated");

                        if (nodVisitID is XmlNode && nodVersion is XmlNode && nodRunDate is XmlNode)
                        {
                            pResultFile.Value = xmlFile;
                            pModelVersion.Value = nodVersion.InnerText;
                            pVisitID.Value = long.Parse(nodVisitID.InnerText);
                            pRunDateTime.Value = DateTime.Parse(nodRunDate.InnerText);
                            comResult.ExecuteNonQuery();

                            pResultID.Value = (long)comResultID.ExecuteScalar();
                            foreach (long nMetricID in metricXPaths.Keys)
                            {
                                XmlNode nodMetric = xmlDoc.SelectSingleNode(metricXPaths[nMetricID]);
                                if (nodMetric is XmlNode)
                                {
                                    pMetricID.Value = nMetricID;
                                    double fMetricValue;
                                    if (!string.IsNullOrEmpty(nodMetric.InnerText) && double.TryParse(nodMetric.InnerText, out fMetricValue))
                                        pMetricValue.Value = fMetricValue;
                                    else
                                        pMetricValue.Value = DBNull.Value;
                                    comVisitMetrics.ExecuteNonQuery();
                                }
                                else
                                    Console.Write("Failed to find metric with XPath {0}", metricXPaths[nMetricID]);
                            }
                        }
                    }

                    dbTrans.Commit();
                }
                catch(Exception ex)
                {
                    dbTrans.Rollback();
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private Dictionary<long, string> GetMetrics()
        {
            Dictionary<long, string> result = new Dictionary<long, string>();
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT MetricID, XPath FROM Metric_Definitions WHERE (XPath IS NOT NULL) ORDER BY MetricID", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    result[dbRead.GetInt64(0)] = dbRead.GetString(1);
            }
            return result;
        }
    }
}
