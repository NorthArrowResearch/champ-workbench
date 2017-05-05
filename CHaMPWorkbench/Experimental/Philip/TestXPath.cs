using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Experimental.Philip
{
    class TestXPath
    {
        private XmlDocument m_xml;

        public TestXPath(string sRBTXMLPath)
        {
            if (!System.IO.File.Exists(sRBTXMLPath))
                throw new Exception("The RBT XML File Path does not exist.");

            m_xml= new XmlDocument();
            m_xml.Load(sRBTXMLPath);
        }

        public int RunTest(ref List<string> lInvalidXPaths, string sWhereClause)
        {
            lInvalidXPaths = new List<string>();
            int nProcessed = 0;

            string sSQL = "SELECT MetricID, Title, ResultXMLTag FROM Metric_Definitions";
            if (!string.IsNullOrWhiteSpace(sWhereClause))
                sSQL += " WHERE " + sWhereClause;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand (sSQL, dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    if (DBNull.Value != dbRead["ResultXMLTag"])
                    {
                        string sXPath = (string)dbRead["ResultXMLTag"];
                        //sXPath = "rbt_results/metric_results/" + sXPath;

                        sXPath = sXPath.Replace("%%CHANNEL_UNIT_NUMBER%%", "1")
                                       .Replace("%%TIER1_NAME%%", "'Fast-Turbulent'")
                                       .Replace("%%TIER2_NAME%%", "'Riffle'");

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
