using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Experimental.Philip
{
    class TestXPath
    {
        private OleDbConnection m_dbCon;
        private XmlDocument m_xml;

        public TestXPath(OleDbConnection dbCon, string sRBTXMLPath)
        {
            if (!System.IO.File.Exists(sRBTXMLPath))
                throw new Exception("The RBT XML File Path does not exist.");

            m_xml= new XmlDocument();
            m_xml.Load(sRBTXMLPath);
            
            if (dbCon.State != System.Data.ConnectionState.Open)
                dbCon.Open();
            m_dbCon = dbCon;
        }

        public int RunTest(ref List<string> lInvalidXPaths, string sWhereClause)
        {
            lInvalidXPaths = new List<string>();
            int nProcessed = 0;

            string sSQL = "SELECT MetricID, Title, RBTResultXMLTag FROM Metric_Definitions";
            if (!string.IsNullOrWhiteSpace(sWhereClause))
                sSQL += " WHERE " + sWhereClause;

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                if (DBNull.Value != dbRead["RBTResultXMLTag"])
                {
                    string sXPath = (string)dbRead["RBTResultXMLTag"];
                    //sXPath = "rbt_results/metric_results/" + sXPath;

                    sXPath = sXPath.Replace("%%CHANNEL_UNIT_NUMBER%%", "1")
                                   .Replace("%%TIER1_NAME%%", "'Fast-Turbulent'")
                                   .Replace("%%TIER2_NAME%%", "'Rapid'");

                    try
                    {
                        XmlNode aNode = m_xml.SelectSingleNode(sXPath);
                        if (aNode == null)
                            lInvalidXPaths.Add(((int)dbRead["MetricID"]).ToString() + "," + (string)dbRead["Title"] + "\n");
                        else
                           System.Diagnostics.Debug.WriteLine(aNode.Name);
                    }
                    catch (Exception ex)
                    {
                        lInvalidXPaths.Add(((int)dbRead["MetricID"]).ToString() + "," + (string)dbRead["Title"]+"\n");
                    }
                }
                nProcessed++;
            }
            dbRead.Close();

            return nProcessed;
        }
    }
}
