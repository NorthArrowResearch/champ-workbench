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
        private const int m_nRBTRun = 1;
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

            // Loop over the unique tables doing one SQL command for each
            foreach (string sTable in dUniqueTables.Keys)
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();

                    string sSQL = string.Format("SELECT {0} FROM {1}", string.Join(",", dUniqueTables[sTable].ToArray<string>()), sTable);
                    System.Diagnostics.Debug.Print(sSQL);
                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();

                    // Loop over all the metrics retrieved from this table using this command.
                    foreach (string sDBField in dUniqueTables[sTable])
                    {
                        Metric m = RetrieveMetric(sTable, sDBField, ref dValidationMetrics);
                        if (m == null)
                            continue;

                        XmlNode nodMetric = xmlDoc.CreateElement("metric");
                        nodMetrics.AppendChild(nodMetric);

                        XmlNode nodMetricName = xmlDoc.CreateElement("name");
                        nodMetricName.InnerText = m.Title;
                        nodMetric.AppendChild(nodMetricName);

                        XmlNode nodMetricUnits = xmlDoc.CreateElement("unit");
                        nodMetricUnits.InnerText = m.Units;
                        nodMetric.AppendChild(nodMetricUnits);

                        XmlNode nodTolerance = xmlDoc.CreateElement("tolerance");
                        nodTolerance.InnerText = m.Threshold.ToString("#0.00");
                        nodMetric.AppendChild(nodTolerance);

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

        private Metric RetrieveMetric(string sTable, string sDBField, ref  Dictionary<string, Metric> dValidationMetrics)
        {
            Metric theResult = null;

            foreach (Metric m in dValidationMetrics.Values)
            {
                if (string.Compare(m.DBTable, sTable, true) == 0 && string.Compare(m.DBField, sDBField, true) == 0)
                {
                    theResult = m;
                    break;
                }
            }
            return theResult;
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
