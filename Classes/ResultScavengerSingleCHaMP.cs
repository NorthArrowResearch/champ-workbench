using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    /// <summary>
    /// This class scavenges results from a single RBT result file but puts then 
    /// in the Workbench database using the new normalized table structure
    /// (See the class ResultScavengerSingle for the old code that puts the metrics
    /// in the old structure).
    /// </summary>
    public class ResultScavengerSingleCHaMP
    {
        private string m_sDBCon;

        /// <summary>
        /// These constants correspond to the LookupListItem IDs in the 
        /// Workbench database for "Scavenge Types" and "Metric Types"
        /// </summary>
        private const int m_nRBTScavengeTypeID = 1;

        private const int m_nVisitMetricTypeID = 3;
        private const int m_nTier1MetricTypeID = 4;
        private const int m_nTier2MetricTypeID = 5;
        private const int m_nChannelUnitTypeID = 6;

        public ResultScavengerSingleCHaMP(string sDBCon)
        {
            m_sDBCon = sDBCon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sResultFilePath">Full path to existing RBT result XML file containing metric values</param>
        /// <returns>Metric_Results.ResultID for this result file</returns>
        public int ScavengeResultFile(string sResultFilePath)
        {
            if (string.IsNullOrEmpty(sResultFilePath) || !System.IO.File.Exists(sResultFilePath))
            {
                Exception ex = new Exception("The RBT result file does not exist.");
                ex.Data["Result File"] = sResultFilePath;
                throw ex;
            }

            //List<ScavengeMetric> lTier1Metrics = GetMetrics(m_nTier1MetricTypeID);

            XmlDocument xmlResults = new XmlDocument();
            xmlResults.Load(sResultFilePath);

            int nResultID = 0;
            using (OleDbConnection DBCon = new OleDbConnection(m_sDBCon))
            {
                DBCon.Open();
                OleDbTransaction dbTrans = DBCon.BeginTransaction();

                nResultID = InsertResultRecord(ref dbTrans, sResultFilePath, ref xmlResults);
                if (nResultID > 0)
                {
                    ScavengeVisitMetrics(ref dbTrans, ref xmlResults, nResultID);
                }
            }

            return nResultID;
        }

        private List<ScavengeMetric> GetMetrics(int nMetricTypeID)
        {
            List<ScavengeMetric> lMetrics = new List<ScavengeMetric>();

            using (OleDbConnection DBCon = new OleDbConnection(m_sDBCon))
            {
                DBCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT MetricID, Title, CMMetricID, RBTResultXMLTag FROM Metric_Definitions WHERE (TypeID = @TypeID) AND (CMMetricID Is Not Null) AND (RBTResultXMLTag Is Not Null)", DBCon);
                dbCom.Parameters.AddWithValue("@TypeID", nMetricTypeID);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    lMetrics.Add(new ScavengeMetric(dbRead.GetInt32(dbRead.GetOrdinal("MetricID")),
                        dbRead.GetInt32(dbRead.GetOrdinal("CMMetricID")),
                        dbRead.GetString(dbRead.GetOrdinal("Title")),
                        dbRead.GetString(dbRead.GetOrdinal("RBTResultXMLTag"))));
                }
            }

            return lMetrics;
        }
        
        private int InsertResultRecord(ref OleDbTransaction dbTrans, string sResultFile, ref XmlDocument xmlResults)
        {
            int nResultID = 0;

            XmlNode nodVersion = xmlResults.SelectSingleNode("//rbt_results//meta_data//rbt_version");
            if (nodVersion is XmlNode && !string.IsNullOrEmpty(nodVersion.InnerText))
            {
                XmlNode nodCreated = xmlResults.SelectSingleNode("//rbt_results/meta_data//date_time_created");
                DateTime dtCreated;
                if (nodCreated is XmlNode && !string.IsNullOrEmpty(nodCreated.InnerText) && DateTime.TryParse(nodCreated.InnerText, out dtCreated))
                {
                    XmlNode nodVisitID = xmlResults.SelectSingleNode("//rbt_results//metric_results//visitid");
                    int nVisitID;
                    if (nodVisitID is XmlNode && !string.IsNullOrEmpty(nodVisitID.InnerText) && int.TryParse(nodVisitID.InnerText, out nVisitID))
                    {
                        OleDbCommand dbCom = new OleDbCommand("INSERT INTO Metric_Results (ResultFile, RBTVersion, VisitID, RBTRunDateTime, ScavengeTypeID)" +
                            " VALUES (@ResultFile, @RBTVersion, @VisitID, @RBTRunDateTime, @ScavengeTypeID)", dbTrans.Connection, dbTrans);

                        dbCom.Parameters.AddWithValue("@ResultFile", sResultFile);
                        dbCom.Parameters.AddWithValue("@RBTVersion", nodVersion.InnerText);
                        dbCom.Parameters.AddWithValue("@VisitID", nVisitID);
                        dbCom.Parameters.AddWithValue("@RBTRunDateTime", dtCreated);
                        dbCom.Parameters.AddWithValue("@ScavengeTypeID", m_nRBTScavengeTypeID);

                        dbCom.ExecuteNonQuery();

                        dbCom = new OleDbCommand("SELECT @@Identity FROM Metric_Results", dbTrans.Connection, dbTrans);
                        object objResultID = dbCom.ExecuteScalar();
                        if (objResultID != null && objResultID != DBNull.Value && objResultID is int)
                        {
                            nResultID = (int)objResultID;
                        }
                    }
                }
            }

            return nResultID;
        }

        private void ScavengeVisitMetrics(ref OleDbTransaction dbTrans, ref XmlDocument xmlResults, int nResultID)
        {
            List<ScavengeMetric> lVisitMetrics = GetMetrics(m_nVisitMetricTypeID);
            if (lVisitMetrics.Count < 1)
                return;

            OleDbCommand dbCom = new OleDbCommand("INSERT INTO Metric_VisitMetrics (ResultID, MetricID, MetricValue) VALUES (@ResultID, @MetricID, @MetricValue)", dbTrans.Connection, dbTrans);
            OleDbParameter pResultID = dbCom.Parameters.AddWithValue("@ResultID", nResultID);
            OleDbParameter pMetricID = dbCom.Parameters.Add("@MetricID", OleDbType.Integer);
            OleDbParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", OleDbType.Double);

            foreach (ScavengeMetric aMetric in lVisitMetrics)
            {
                XmlNode metricNode = xmlResults.SelectSingleNode(aMetric.XPath);
                if (metricNode is XmlNode)
                {
                    pMetricID.Value = aMetric.MetricID;
                    double fMetricValue;
                    if (!string.IsNullOrEmpty(metricNode.InnerText) && double.TryParse(metricNode.InnerText, out fMetricValue))
                        pMetricValue.Value = fMetricValue;
                    else
                        pMetricValue.Value = DBNull.Value;

                    dbCom.ExecuteNonQuery();
                }
            }
        }

        private class ScavengeMetric
        {
            public int MetricID { public get; internal set; }
            public int CMMetricID { public get; internal set; }
            public string Name { public get; internal set; }
            public string XPath { public get; internal set; }

            public ScavengeMetric(int nMetricID, int nCMMetricID, string sName, string sXPath)
            {
                MetricID = nMetricID;
                CMMetricID = nCMMetricID;
                Name = sName;
                XPath = sXPath;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }

}
