using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Experimental.Philip
{
    /// <summary>
    /// This class tests that a single metric XML result file contains all the XPaths that are defined in
    /// the CHaMP Workbench Metric_Definitions table
    /// </summary>
    class TestXPath
    {
        private XmlDocument m_xml;

        public TestXPath(string sXMLPath)
        {
            if (!System.IO.File.Exists(sXMLPath))
                throw new Exception("The XML file path does not exist.");

            m_xml= new XmlDocument();
            m_xml.Load(sXMLPath);
        }

        public int RunTest(ref List<string> lInvalidXPaths, string sWhereClause)
        {
            lInvalidXPaths = new List<string>();
            int nProcessed = 0;

            string sSQL = "SELECT MetricID, Title, XPath FROM Metric_Definitions";
            if (!string.IsNullOrWhiteSpace(sWhereClause))
                sSQL += " WHERE " + sWhereClause;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand (sSQL, dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    if (DBNull.Value != dbRead["XPath"])
                    {
                        string sXPath = (string)dbRead["XPath"];
                        //sXPath = "rbt_results/metric_results/" + sXPath;

                        //sXPath = sXPath.Replace("%%CHANNEL_UNIT_NUMBER%%", "1")
                        //               .Replace("%%TIER1_NAME%%", "'Fast-Turbulent'")
                        //               .Replace("%%TIER2_NAME%%", "'Riffle'");

                        try
                        {
                            XmlNode aNode = m_xml.SelectSingleNode(sXPath);
                            if (aNode == null)
                                lInvalidXPaths.Add(string.Format("{0}, {1}, {2}\n", dbRead["MetricID"], dbRead["Title"], sXPath));
                            else
                                System.Diagnostics.Debug.WriteLine(aNode.Name);
                        }
                        catch (Exception ex)
                        {
                            lInvalidXPaths.Add(((int)dbRead["MetricID"]).ToString() + "," + (string)dbRead["Title"] + "\n");
                        }
                    }
                    nProcessed++;
                }
                dbRead.Close();
            }

            return nProcessed;
        }
    }
}
