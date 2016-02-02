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

            //Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.InsertBefore(xmldecl, nodReport);

            XmlNode nodDate = xmlDoc.CreateElement("date");
            nodDate.InnerText = DateTime.Now.ToString("d MMM yyyy");
            nodReport.AppendChild(nodDate);

            XmlNode nodMetrics = xmlDoc.CreateElement("metrics");
            nodReport.AppendChild(nodMetrics);

            List<ListItem> lVisitIDs = GetVisitIDs();
            if (lVisitIDs.Count < 1)
                throw new Exception("There are no visits containing manual validation data.");

            // Loop over each metric
            foreach (Metric theMetric in dValidationMetrics.Values)
            {
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

                XmlNode nodVisits = xmlDoc.CreateElement("visits");
                nodMetric.AppendChild(nodVisits);

                // Query to get the manually calculate "truth" variable
                string sSQL = string.Format("SELECT {0} FROM {1} WHERE (ScavengeTypeID = {2}) AND (VisitID = @VisitID)", theMetric.DBField, theMetric.DBTable, m_nValidation);
                System.Diagnostics.Debug.Print(sSQL);
                using (OleDbConnection conManual = new OleDbConnection(DBCon))
                {
                    conManual.Open();
                    OleDbCommand comManual = new OleDbCommand(sSQL, conManual);
                    OleDbParameter pManualVisit = comManual.Parameters.Add("@VisitID", OleDbType.Integer);

                    // Query to get the RBT result values
                    using (OleDbConnection conRBT = new OleDbConnection(DBCon))
                    {
                        conRBT.Open();

                        sSQL = string.Format("SELECT {0}, RBTVersion FROM {1} AS M WHERE (ScavengeTypeID <> {2}) AND (VisitID = @VisitID)", theMetric.DBField, theMetric.DBTable, m_nValidation);
                        System.Diagnostics.Debug.Print(sSQL);
                        OleDbCommand comRBT = new OleDbCommand(sSQL, conRBT);
                        OleDbParameter pRBTVisit = comRBT.Parameters.Add("@VisitID", OleDbType.Integer);

                        foreach (ListItem aVisit in lVisitIDs)
                        {
                            // Retrieve the manual visit. Only proceed and include RBT results if there's a manual result.
                            pManualVisit.Value = aVisit.Value;
                            OleDbDataReader rdManual = comManual.ExecuteReader();
                            if (!rdManual.Read() || rdManual.IsDBNull(0))
                                continue;

                            XmlNode nodVisit = xmlDoc.CreateElement("visit");
                            nodVisits.AppendChild(nodVisit);

                            XmlNode nodVisitID = xmlDoc.CreateElement("visit_id");
                            nodVisitID.InnerText = aVisit.Value.ToString();
                            nodVisit.AppendChild(nodVisitID);

                            XmlNode nodVisitName = xmlDoc.CreateElement("visit_name");
                            nodVisitName.InnerText = aVisit.ToString();
                            nodVisit.AppendChild(nodVisitName);

                            XmlNode nodManualResult = xmlDoc.CreateElement("manual_result");
                            Nullable<float> fManualValue = new Nullable<float>();
                            fManualValue = (float)rdManual.GetDouble(0);
                            nodManualResult.InnerText = fManualValue.ToString();
                            nodVisit.AppendChild(nodManualResult);

                            XmlNode nodResults = xmlDoc.CreateElement("results");
                            nodVisit.AppendChild(nodResults);

                            // Now go and get all the other RBT generated versions (and compare them to the truth)
                            pRBTVisit.Value = aVisit.Value;
                            OleDbDataReader rdRBT = comRBT.ExecuteReader();
                            while (rdRBT.Read())
                            {
                                // Skip results that don't have an RBT version
                                if (rdRBT.IsDBNull(rdRBT.GetOrdinal("RBTVersion")))
                                    continue;

                                XmlNode nodResult = xmlDoc.CreateElement("result");
                                nodResults.AppendChild(nodResult);

                                XmlNode nodVersion = xmlDoc.CreateElement("version");
                                nodVersion.InnerText = rdRBT.GetString(rdRBT.GetOrdinal("RBTVersion"));
                                nodResult.AppendChild(nodVersion);

                                XmlNode nodValue = xmlDoc.CreateElement("value");
                                Nullable<float> fValue = new Nullable<float>();
                                if (!rdRBT.IsDBNull(0))
                                {
                                    fValue = (float)rdManual.GetDouble(0);
                                    nodValue.InnerText = fValue.ToString();
                                }
                                nodResult.AppendChild(nodValue);

                                XmlNode nodStatus = xmlDoc.CreateElement("status");
                                if (fManualValue.HasValue && fValue.HasValue)
                                {
                                    float fDiff = (float)Math.Abs((decimal)((fManualValue - fValue) / fManualValue));
                                    if (fDiff <= theMetric.Threshold)
                                        nodStatus.InnerText = "Pass";
                                    else
                                        nodStatus.InnerText = "Fail";
                                }
                                nodResult.AppendChild(nodStatus);
                            }
                        }
                    }
                }

                if (!nodVisits.HasChildNodes)
                    nodMetrics.RemoveChild(nodMetric);
            }

            try
            {
                if (string.Compare(m_fiOutputPath.Extension, ".xml", true) == 0)
                {
                    // When in XML mode, simply export the data to XML file.
                    xmlDoc.Save(m_fiOutputPath.FullName);
                }
                else
                {
                    // When in HTML mode we need and XSL transformer to convert the XML to HTML.
                    System.Xml.Xsl.XslCompiledTransform theTransformer = new System.Xml.Xsl.XslCompiledTransform();

                    string sXSLPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "RBT\\ValidationReport\\style.xsl");
                    if (!System.IO.File.Exists(sXSLPath))
                        throw new Exception("The XSL template does is missing at " + sXSLPath);

                    System.Xml.XmlReaderSettings xmlSettings = new System.Xml.XmlReaderSettings();
                    xmlSettings.DtdProcessing = DtdProcessing.Ignore;

                    System.Xml.Xsl.XsltArgumentList xslArgs = new System.Xml.Xsl.XsltArgumentList();

                    System.Xml.Xsl.XsltSettings xslSettings = new System.Xml.Xsl.XsltSettings();
                    xslSettings.EnableDocumentFunction = true;
                    xslSettings.EnableScript = true;

                    using (System.Xml.XmlReader xReader = System.Xml.XmlReader.Create(sXSLPath, xmlSettings))
                    {
                        // Load the XSL into the transformer.
                        theTransformer.Load(xReader, xslSettings, null);
                        using (System.IO.TextWriter fOutput = System.IO.File.CreateText(m_fiOutputPath.FullName))
                        {
                            // Perform the transformation from XML to XSL
                            theTransformer.Transform(nodReport.CreateNavigator(), xslArgs, fOutput);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data["File Path"] = m_fiOutputPath.FullName;
            }
        }

        private List<ListItem> GetVisitIDs()
        {
            List<ListItem> lResult = new List<ListItem>();

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear" +
                    " FROM CHAMP_Watersheds INNER JOIN (CHAMP_Sites INNER JOIN (Metric_SiteMetrics INNER JOIN CHAMP_Visits ON Metric_SiteMetrics.VisitID = CHAMP_Visits.VisitID) ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID) ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID" +
" WHERE (((Metric_SiteMetrics.ScavengeTypeID)=2))" +
" GROUP BY CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear", dbCon);

                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    lResult.Add(new ListItem(
                        string.Format("{0}, {1}, {2}",
                        dbRead.GetString(dbRead.GetOrdinal("WatershedName")),
                        dbRead.GetString(dbRead.GetOrdinal("SiteName")),
                        dbRead.GetInt16(dbRead.GetOrdinal("VisitYear"))),
                        dbRead.GetInt32(dbRead.GetOrdinal("VisitID"))
                        ));
            }

            return lResult;
        }

        private float GetMetricValue(ref OleDbDataReader dbRead, int nOrdinal)
        {
            float fResult = 0;
            object objValue = dbRead[0];
            float fTheValue;
            if (!float.TryParse(objValue.ToString(), out fTheValue))
            {
                double ffValue;
                if (double.TryParse(objValue.ToString(), out ffValue))
                {
                    fTheValue = (float)ffValue;
                }
            }
            fResult = fTheValue;
            return fResult;
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
