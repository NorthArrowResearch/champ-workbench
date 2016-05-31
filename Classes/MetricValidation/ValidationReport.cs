using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using Newtonsoft.Json;
using System.Xml;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    class ValidationReport
    {
        // Full file path to the report XSL file
        private System.IO.FileInfo m_fiReportXSL;

        // Full file path to the output HTML report file.
        private System.IO.FileInfo m_fiOutputPath;

        // Database connection string
        private string DBCon { get; set; }

        /// <summary>
        /// Create a new validation report generator
        /// </summary>
        /// <param name="sDBCon">Connection string to the database</param>
        /// <param name="fiReportXSL">Full file path to the report XSL file</param>
        /// <param name="fiOutputPath">Full file path to the output HTML report file.</param>
        public ValidationReport(string sDBCon, System.IO.FileInfo fiReportXSL, System.IO.FileInfo fiOutputPath)
        {
            DBCon = sDBCon;
            m_fiReportXSL = fiReportXSL;
            m_fiOutputPath = fiOutputPath;
        }

        /// <summary>
        /// Generate a new report.
        /// </summary>
        /// <param name="lVisits">List of visits to include in the report</param>
        /// <returns>Creates an XML file containing all the metric data for the specified
        /// visits and then uses an XSL transform to convert this file to a HTML report.</returns>
        public ValidationReportResults Run(List<ListItem> lVisits, List<ListItem> lRBTVersions)
        {
            // This return variable really just counts metrics and visits included in the report.
            ValidationReportResults theResult = new ValidationReportResults();

            // Load a dictionary of all metrics that are flagged as "Active" for validation.
            // Then load the metric values for any visits in the argument list
            Dictionary<string, Metric> dValidationMetrics = RetrieveValidationMetrics();
            theResult.Metrics = dValidationMetrics.Count;

            // Start a new XML document that will contain all the metric result data
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodReport = xmlDoc.CreateElement("report");
            xmlDoc.AppendChild(nodReport);

            // Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.InsertBefore(xmldecl, nodReport);

            // Metadata about the report
            XmlNode nodDate = xmlDoc.CreateElement("date");
            nodDate.InnerText = DateTime.Now.ToString("d MMM yyyy");
            nodReport.AppendChild(nodDate);

            // Start the node under which all metric results will be included.
            XmlNode nodMetrics = xmlDoc.CreateElement("metrics");
            nodReport.AppendChild(nodMetrics);

            // If there are no
            Dictionary<int, ValidationVisitInfo> dVisits = GetVisitInfo(ref lVisits);
            if (dVisits.Count < 1)
                return theResult;

            // Loop over all the validation metrics loaded.
            // First load a single manual result value (if it exists)
            // Second load all model run metric values
            // Serialize the metrics to the XML document.
            foreach (Metric aMetric in dValidationMetrics.Values)
            {
                System.Diagnostics.Debug.Print(string.Format("Metric {0} {1}", aMetric.MetricID, aMetric.Title));

                aMetric.LoadResults(DBCon, ref dVisits, ref lRBTVersions, true);
                aMetric.LoadResults(DBCon, ref dVisits, ref lRBTVersions, false);

                aMetric.Serialize(ref xmlDoc, ref nodMetrics);

                theResult.Visits += aMetric.Visits.Count;
            }

            //
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Now create and write the XML document to file and then transform it.
            try
            {
                if (string.Compare(m_fiOutputPath.Extension, ".xml", true) == 0)
                {
                    // When in XML mode, simply export the data to XML file.
                    xmlDoc.Save(m_fiOutputPath.FullName);
                }
                else
                {
                    // Since we need to do some Javascript magiv on top of our XSL magic we includ the entire report in JSON format
                    // next to the xml
                    XmlNode nodJSON = xmlDoc.CreateElement("json");
                    nodJSON.InnerText = JsonConvert.SerializeXmlNode(nodReport);
                    nodReport.AppendChild(nodJSON);

                    // When in HTML mode we need and XSL transformer to convert the XML to HTML.
                    System.Xml.Xsl.XslCompiledTransform theTransformer = new System.Xml.Xsl.XslCompiledTransform();

                    if (!m_fiReportXSL.Exists)
                        throw new Exception("The XSL template does is missing at " + m_fiReportXSL.FullName);

                    System.Xml.XmlReaderSettings xmlSettings = new System.Xml.XmlReaderSettings();
                    xmlSettings.DtdProcessing = DtdProcessing.Ignore;

                    System.Xml.Xsl.XsltArgumentList xslArgs = new System.Xml.Xsl.XsltArgumentList();

                    System.Xml.Xsl.XsltSettings xslSettings = new System.Xml.Xsl.XsltSettings();
                    xslSettings.EnableDocumentFunction = true;
                    xslSettings.EnableScript = true;

                    using (System.Xml.XmlReader xReader = System.Xml.XmlReader.Create(m_fiReportXSL.FullName, xmlSettings))
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

            return theResult;
        }

        private Dictionary<int, ValidationVisitInfo> GetVisitInfo(ref List<ListItem> lVisits)
        {
            Dictionary<int, ValidationVisitInfo> dResult = new Dictionary<int, ValidationVisitInfo>();

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear" +
                    " , CHaMP_Visits.Organization, CHaMP_Visits.CrewName" +
                    " FROM CHAMP_Watersheds INNER JOIN (CHAMP_Sites INNER JOIN (Metric_Results INNER JOIN CHAMP_Visits ON Metric_Results.VisitID = CHAMP_Visits.VisitID) ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID) ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID" +
                    " GROUP BY CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear, CHaMP_Visits.Organization, CHaMP_Visits.CrewName", dbCon);


                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    string sOrganization = string.Empty;
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("Organization")))
                        sOrganization = dbRead.GetString(dbRead.GetOrdinal("Organization"));

                    string sCrewName = string.Empty;
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("CrewName")))
                        sCrewName = dbRead.GetString(dbRead.GetOrdinal("CrewName"));

                    int nVisitIDFromDb = dbRead.GetInt32(dbRead.GetOrdinal("VisitID"));

                    if (lVisits.Find(x => x.Value == nVisitIDFromDb) != null)
                    {
                        dResult.Add(dbRead.GetInt32(dbRead.GetOrdinal("VisitID")), new ValidationVisitInfo(
                            dbRead.GetInt32(dbRead.GetOrdinal("VisitID")),
                            dbRead.GetInt16(dbRead.GetOrdinal("VisitYear")),
                            dbRead.GetString(dbRead.GetOrdinal("SiteName")),
                            dbRead.GetString(dbRead.GetOrdinal("WatershedName")),
                            dbRead.GetInt32(dbRead.GetOrdinal("WatershedID")),
                            sOrganization,
                            sCrewName));
                    }

                }
            }

            return dResult;
        }

        private Dictionary<string, Metric> RetrieveValidationMetrics()
        {
            Dictionary<string, Metric> theResult = new Dictionary<string, Metric>();
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT D.MetricID, D.Title, D.CMMetricID, D.TypeID, D.Threshold, D.MinValue, D.MaxValue, D.IsActive, D.CMMetricID, MGs.Title AS MetricParentGroup, MCGs.Title AS MetricChildGroup" +
                    " FROM (Metric_Definitions AS D LEFT JOIN LookupListItems AS MGs ON D.MetricGroupID = MGs.ItemID) LEFT JOIN LookupListItems AS MCGs ON D.MetricChannelGroupID = MCGs.ItemID" +
                    " WHERE (D.Title Is Not Null) AND (D.TypeID Is Not Null) AND (D.IsActive = True) ORDER BY D.Title", dbCon);

                System.Diagnostics.Debug.Print(dbCom.CommandText);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    Nullable<int> nCMMetricID = new Nullable<int>();
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("CMMetricID")))
                        nCMMetricID = dbRead.GetInt32(dbRead.GetOrdinal("CMMetricID"));

                    Nullable<double> fMinValue = new Nullable<float>();
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("MinValue")))
                        fMinValue = dbRead.GetDouble(dbRead.GetOrdinal("MinValue"));

                    Nullable<double> fMaxValue = new Nullable<float>();
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("MaxValue")))
                        fMaxValue = dbRead.GetDouble(dbRead.GetOrdinal("MaxValue"));

                    string sParentGroup = string.Empty; // Watershed report parent grouping
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("MetricParentGroup")))
                        sParentGroup = dbRead.GetString(dbRead.GetOrdinal("MetricParentGroup"));

                    string sChildGroup = string.Empty; // Watershed report child grouping
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("MetricChildGroup")))
                        sChildGroup = dbRead.GetString(dbRead.GetOrdinal("MetricChildGroup"));

                    theResult.Add((string)dbRead["Title"], new Metric(
                        (string)dbRead["Title"]
                        , (int)dbRead["MetricID"]
                        , nCMMetricID
                        , (int)dbRead["TypeID"]
                        , (float)dbRead["Threshold"]
                        , fMinValue
                        , fMaxValue
                        , (bool)dbRead["IsActive"]
                        , sParentGroup
                        , sChildGroup));
                }
            }

            return theResult;
        }

        public class ValidationReportResults
        {
            public int Visits { get; set; }
            public int Metrics { get; set; }

            public ValidationReportResults()
            {
                Visits = 0;
                Metrics = 0;
            }
        }
    }
}
