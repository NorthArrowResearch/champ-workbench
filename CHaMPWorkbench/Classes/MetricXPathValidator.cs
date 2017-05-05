using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes
{
    public class MetricXPathValidator
    {
        public string DBCon { get; internal set; }

        public MetricXPathValidator(string sDBCon)
        {
            DBCon = sDBCon;
        }

        public List<string> Run(ref Dictionary<string, Experimental.Philip.frmMetricScraper.MetricSchema> MetricSchemas)
        {
            List<string> messages = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT MetricID FROM Metric_Definitions WHERE (XPath = @XPath) AND (Title = @Title)", dbCon);
                SQLiteParameter pXPath = dbCom.Parameters.Add("XPath", System.Data.DbType.String);
                SQLiteParameter pTitle = dbCom.Parameters.Add("Title", System.Data.DbType.String);
                SQLiteDataReader dbRead = null;

                foreach (string sMetricTypeName in MetricSchemas.Keys)
                {
                    xmlDoc.Load(MetricSchemas[sMetricTypeName].XMLDefinition);
                    string sMetricXMLFileName = System.IO.Path.GetFileName(MetricSchemas[sMetricTypeName].XMLDefinition);

                    XmlNode nodRoot = xmlDoc.SelectSingleNode("/MetricSchema/RootXPath");
                    if (nodRoot == null)
                    {
                        messages.Add(string.Format("Error: Failed to find RootXPath node in {0}", sMetricXMLFileName));
                        continue;
                    }

                    foreach (XmlNode nodMetric in xmlDoc.SelectNodes("MetricSchema/Metrics/Metric"))
                    {
                        string sMetricNameXML = GetMetricDefinitionAttribute(nodMetric, "name", sMetricXMLFileName, ref messages);
                        if (string.IsNullOrEmpty(sMetricNameXML))
                            continue;

                        string sMetricXPath = GetMetricDefinitionAttribute(nodMetric, "xpath", sMetricXMLFileName, ref messages);
                        if (string.IsNullOrEmpty(sMetricXPath))
                            continue;
                        sMetricXPath = string.Format("{0}/{1}", nodRoot.InnerText, sMetricXPath);

                        string sMetricType = GetMetricDefinitionAttribute(nodMetric, "type", sMetricXMLFileName, ref messages);
                        if (string.IsNullOrEmpty(sMetricType) || string.Compare(sMetricType, "string", true) == 0)
                            continue;

                        // This query should return one metric that uses the specified XPath.
                        pXPath.Value = sMetricXPath;
                        pTitle.Value = sMetricNameXML;
                        dbRead = dbCom.ExecuteReader();
                        if (dbRead.Read())
                        {
                            long nMetricID = dbRead.GetInt64(0);

                            // Verify that there is only one metric with this XPath
                            if (dbRead.Read())
                            {
                                messages.Add(string.Format("The {0} metric '{1}' possesses an XPath that occurs multiple times in the Workbench metric definitions: {2}", sMetricXMLFileName, sMetricNameXML, sMetricXPath));
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Found {1} metric '{0}' with 1 occurance in DB with XPath {2}", sMetricXMLFileName, sMetricNameXML, sMetricXPath));
                                MetricSchemas[sMetricTypeName].MetricDefs[sMetricNameXML] = new Experimental.Philip.frmMetricScraper.MetricDef(nMetricID, sMetricNameXML, sMetricXPath);
                            }
                        }
                        else
                        {
                            messages.Add(string.Format("The {0} metric '{1}' does not occur in the Workbench metric definitions: {2}", sMetricXMLFileName, sMetricNameXML, sMetricXPath));
                        }
                        dbRead.Close();
                    }

                    Console.WriteLine(string.Format("Metric schema complete for XML file {0}", sMetricXMLFileName));
                }
            }

            foreach (Experimental.Philip.frmMetricScraper.MetricSchema schema in MetricSchemas.Values)
            {
                List<string> uniqueMetricIDs = new List<string>();

                foreach (Experimental.Philip.frmMetricScraper.MetricDef metric in schema.MetricDefs.Values)
                {
                    if (uniqueMetricIDs.Contains(metric.XPath.ToLower()))
                    {
                        Console.Write("stop");
                    }
                    else
                        uniqueMetricIDs.Add(metric.XPath.ToLower());

                }
            }

            Console.WriteLine(messages.Count.ToString());
            return messages;
        }

        private string GetMetricDefinitionAttribute(XmlNode nodMetric, string sAttibuteName, string sMetricXMLFileName, ref List<string> messages)
        {
            if (nodMetric.Attributes[sAttibuteName] == null)
            {
                messages.Add(string.Format("Error: Failed to find {0} attribute on metric node in {1}", sAttibuteName, sMetricXMLFileName));
                return string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(nodMetric.Attributes[sAttibuteName].InnerText))
                {
                    messages.Add(string.Format("Error: Empty metric {0} attribute in {1}.", sAttibuteName, sMetricXMLFileName));
                    return string.Empty;
                }
            }

            return nodMetric.Attributes[sAttibuteName].InnerText;
        }
    }
}
