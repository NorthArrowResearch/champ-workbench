using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class RBTValidationReport
    {
        private const int m_nValidation = 2;

        private System.IO.FileInfo m_fiOutputPath;
        private string DBCon { get; set; }

        public RBTValidationReport(string sDBCon, System.IO.FileInfo fiOutputPath)
        {
            m_fiOutputPath = fiOutputPath;
            DBCon = sDBCon;
        }

        public void Run()
        {
            // Loop through all scavenged results and produce a metric XML file for each.

            // Key = Metric Name, Value = [DatabaseTable].[DatabaseField]
            Dictionary<string, Metric> dValidationMetrics = RetrieveValidationMetrics();

            // Get Unique list of tables being validated and the fields in use.
            Dictionary<string, List<string>> dUniqueTables = GetListOfUniqueTables(dValidationMetrics);

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode nodReport = xmlDoc.CreateElement("report");
            xmlDoc.AppendChild(nodReport);

            XmlNode nodDate = xmlDoc.CreateElement("date");
            nodDate.InnerText = DateTime.Now.ToString("O");
            nodReport.AppendChild(nodDate);

            XmlNode nodMetrics = xmlDoc.CreateElement("metrics");
            nodReport.AppendChild(nodMetrics);

            // Loop over each metric
            foreach (Metric theMetric in dValidationMetrics.Values)
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();

                    XmlNode nodMetric = xmlDoc.CreateElement("metric");
                    nodMetrics.AppendChild(nodMetric);

                    XmlNode nodMetricName = xmlDoc.CreateElement("name");
                    nodMetricName.InnerText = theMetric.Title;
                    nodMetric.AppendChild(nodMetricName);

                    XmlNode nodMetricUnits = xmlDoc.CreateElement("unit");
                    nodMetricUnits.InnerText = theMetric.Units;
                    nodMetric.AppendChild(nodMetricUnits);

                    XmlNode nodTolerance = xmlDoc.CreateElement("tolerance");
                    nodTolerance.InnerText = theMetric.Threshold.ToString("#0.00");
                    nodMetric.AppendChild(nodTolerance);

                    // First go and get the manually calculate "truth" variable"
                    string sSQL = string.Format("SELECT {0} FROM {1} WHERE ScavengeTypeID = {0}", theMetric.DBField, theMetric.DBTable, m_nValidation);
                    System.Diagnostics.Debug.Print(sSQL);
                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();

                    XmlNode nodManualResult = xmlDoc.CreateElement("manual_result");
                    Nullable<float> fManualValue = new Nullable<float>();
                    if (dbRead.Read() && !dbRead.IsDBNull(0))
                    {
                        fManualValue = dbRead.GetFloat(0);
                        nodManualResult.InnerText = fManualValue.ToString();
                    }
                    dbRead.Close();
                    nodMetric.AppendChild(nodManualResult);

                    XmlNode nodResults = xmlDoc.CreateElement("results");
                    nodMetric.AppendChild(nodResults);

                    // Now go and get all the other RBT generated versions (and compare them to the truth)
                    sSQL = string.Format("SELECT {0}, RBTVersion FROM {1} WHERE ScavengeTypeID <> {0}", theMetric.DBField, theMetric.DBTable, m_nValidation);
                    System.Diagnostics.Debug.Print(sSQL);
                    dbCom = new OleDbCommand(sSQL, dbCon);
                    dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        // Skip results that don't have an RBT version
                        if (dbRead.IsDBNull(1))
                            continue;

                        XmlNode nodResult = xmlDoc.CreateElement("result");
                        nodResults.AppendChild(nodResult);

                        XmlNode nodVersion = xmlDoc.CreateElement("version");
                        nodVersion.InnerText = dbRead.GetString(dbRead.GetOrdinal("RBTVersion"));
                        nodResult.AppendChild(nodVersion);

                        XmlNode nodValue = xmlDoc.CreateElement("result");
                        Nullable<float> fValue = new Nullable<float>();
                        if (!dbRead.IsDBNull(0))
                        {
                            fValue = dbRead.GetFloat(0);
                            nodValue.InnerText = fValue.ToString();
                        }

                        XmlNode nodStatus = xmlDoc.CreateElement("status");
                        if (fManualValue.HasValue && fValue.HasValue)
                        {
                            float fDiff = (float)Math.Abs((decimal) ((fManualValue - fValue) / fManualValue));
                            if (fDiff <= theMetric.Threshold)
                                nodStatus.InnerText = "Pass";
                            else
                                nodStatus.InnerText = "Fail";
                        }
                        nodResult.AppendChild(nodStatus);
                    }
                }
            }

            try
            {
                xmlDoc.Save(m_fiOutputPath.FullName);
            }
            catch (Exception ex)
            {
                ex.Data["File Path"] = m_fiOutputPath.FullName;
            }
        }

        private Dictionary<string, Metric> RetrieveValidationMetrics()
        {
            Dictionary<string, Metric> theResult = new Dictionary<string, Metric>();
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT Title, CMMetricID, RBTResultXMLTag, CHaMPWorkBenchField, Threshold, IsActive" +
                    " FROM Metric_Definitions" +
                    " WHERE (Title Is Not Null) AND (CMMetricID Is Not Null) AND (CHaMPWorkBenchField Is Not Null) AND (IsActive = True)" +
                    " ORDER BY Title", dbCon);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    theResult.Add((string)dbRead["Title"], new Metric(
                        (string)dbRead["Title"]
                        , (int)dbRead["CMMetricID"]
                        , (string)dbRead["RBTResultXMLTag"]
                        , (string)dbRead["CHaMPWorkBenchField"]
                        , (float)dbRead["Threshold"]
                        , (bool)dbRead["IsActive"]));
            }

            return theResult;
        }

        private Dictionary<string, List<string>> GetListOfUniqueTables(Dictionary<string, Metric> dValidationMetrics)
        {
            Dictionary<string, List<string>> dResult = new Dictionary<string, List<string>>();

            foreach (Metric m in dValidationMetrics.Values)
            {
                // Ensure that the db table is added to the dictionary
                if (!dResult.Keys.Contains<string>(m.DBTable))
                    dResult.Add(m.DBTable, new List<string>());

                // ensure that the db field is added to the list of fields for the table
                if (!dResult[m.DBTable].Contains<string>(m.DBField))
                    dResult[m.DBTable].Add(m.DBField);
            }

            return dResult;
        }

        private class Metric
        {
            public string Title { get; internal set; }
            public int MetricID { get; internal set; }
            public string XPath { get; internal set; }
            public string DBTable { get; internal set; }
            public string DBField { get; internal set; }
            public float Threshold { get; internal set; }
            public bool IsActive { get; internal set; }
            public string Units { get; internal set; }

            public string DBTableAndField
            {
                get
                {
                    return string.Format("{0}.{1}", DBTable, DBField);
                }
            }

            public Metric(string sTitle, int nMetricID, string sXPath, string sCHaMPWorkBenchField, float fThreshold, bool bIsActive)
            {
                Title = sTitle;
                MetricID = nMetricID;
                XPath = sXPath;
                Threshold = fThreshold;
                IsActive = bIsActive;
                Units = string.Empty;

                Regex rTable = new Regex("^\\[([A-Z_a-z]*)\\]");
                Regex rField = new Regex("\\.\\[([A-Z_a-z]*)\\]");

                DBTable = rTable.Match(sCHaMPWorkBenchField).Value;
                DBField = rField.Match(sCHaMPWorkBenchField).Value.TrimStart('.');

            }
        }
    }
}
