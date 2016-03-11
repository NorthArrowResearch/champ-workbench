using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Xml;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    class ValidationReport
    {
        private const int m_nValidationScavengeTypeID = 2;

        private System.IO.FileInfo m_fiReportXSL;
        private System.IO.FileInfo m_fiOutputPath;
        private string DBCon { get; set; }

        public ValidationReport(string sDBCon, System.IO.FileInfo fiReportXSL, System.IO.FileInfo fiOutputPath)
        {
            DBCon = sDBCon;
            m_fiReportXSL = fiReportXSL;
            m_fiOutputPath = fiOutputPath;
        }

        public ValidationReportResults Run(List<ListItem> lVisits)
        {
            // Loop through all scavenged results and produce a metric XML file for each.
            ValidationReportResults theResult = new ValidationReportResults();

            // Key = Metric Name, Value = [DatabaseTable].[DatabaseField]
            Dictionary<string, Metric> dValidationMetrics = RetrieveValidationMetrics();


            // Get Unique list of tables being validated and the fields in use.
            //Dictionary<string, List<string>> dUniqueTables = GetListOfUniqueTables(dValidationMetrics);

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
                return theResult;

            foreach (Metric aMetric in dValidationMetrics.Values)
            {
                System.Diagnostics.Debug.Print(string.Format("Metric {0} {1}", aMetric.MetricID, aMetric.Title));

                aMetric.LoadResults(DBCon, ref lVisits, true);
                aMetric.LoadResults(DBCon, ref lVisits, false);

                aMetric.Serialize(ref xmlDoc, ref nodMetrics);
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

        private string GetMetricSQLStatement(Metric theMetric, bool bManualMetricValues)
        {
            string sSQL = "SELECT V.MetricValue, R.RBTVersion";

            switch (theMetric.GroupTypeID)
            {
                case 3: // visit metrics
                    sSQL += " FROM Metric_Results R INNER JOIN Metric_VisitMetrics V ON R.ResultID = V.ResultID";
                    break;

                case 4: // tier 1 metrics

                    break;

                case 5: // tier 2 metrics

                    break;

                case 6: // Channel unit metrics
                    break;
            }

            sSQL += string.Format(" WHERE (R.VisitID = @VisitID) AND (V.MetricID = {0}) AND (R.ScavengeTypeID {1} {2})", theMetric.MetricID, (bManualMetricValues) ? "=" : "<>", m_nValidationScavengeTypeID);
            return sSQL;
        }

        private string GetFormattedRBTVersion(string sRawRBTVersion)
        {
            string[] sVersionParts = sRawRBTVersion.Split('.');
            List<string> lVersionParts = new List<string>();

            for (int i = 0; i < sVersionParts.Count<string>(); i++)
            {
                if (i == 0)
                    lVersionParts.Add(sVersionParts[i]);
                else
                {
                    int nVersionPart = 0;
                    int.TryParse(sVersionParts[i], out nVersionPart);
                    lVersionParts.Add(nVersionPart.ToString("00"));
                }
            }

            return string.Join(".", lVersionParts.ToArray<string>());
        }

        private List<ListItem> GetVisitIDs()
        {
            List<ListItem> lResult = new List<ListItem>();

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear" +
                    " FROM CHAMP_Watersheds INNER JOIN (CHAMP_Sites INNER JOIN (Metric_SiteMetrics INNER JOIN CHAMP_Visits ON Metric_SiteMetrics.VisitID = CHAMP_Visits.VisitID) ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID) ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID" +
                    " GROUP BY CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Visits.VisitID, CHAMP_Visits.VisitYear", dbCon);

                //" WHERE (((Metric_SiteMetrics.ScavengeTypeID)=2))" +

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

                OleDbCommand dbCom = new OleDbCommand("SELECT MetricID, Title, CMMetricID, TypeID, Threshold, IsActive" +
                    " FROM Metric_Definitions" +
                    " WHERE (Title Is Not Null) AND (TypeID Is Not Null) AND (IsActive = True)" +
                    " ORDER BY Title", dbCon);

                System.Diagnostics.Debug.Print(dbCom.CommandText);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    Nullable<int> nCMMetricID = new Nullable<int>();
                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("CMMetricID")))
                        nCMMetricID = dbRead.GetInt32(dbRead.GetOrdinal("CMMetricID"));

                    theResult.Add((string)dbRead["Title"], new Metric(
                        (string)dbRead["Title"]
                        , (int) dbRead["MetricID"]
                        , nCMMetricID
                        , (int)dbRead["TypeID"]
                        , (float)dbRead["Threshold"]
                        , (bool)dbRead["IsActive"]));
                }
            }

            return theResult;
        }

        //private Dictionary<string, List<string>> GetListOfUniqueTables(Dictionary<string, Metric> dValidationMetrics)
        //{
        //    Dictionary<string, List<string>> dResult = new Dictionary<string, List<string>>();

        //    foreach (Metric m in dValidationMetrics.Values)
        //    {
        //        // Ensure that the db table is added to the dictionary
        //        if (!dResult.Keys.Contains<string>(m.DBTable))
        //            dResult.Add(m.DBTable, new List<string>());

        //        // ensure that the db field is added to the list of fields for the table
        //        if (!dResult[m.DBTable].Contains<string>(m.DBField))
        //            dResult[m.DBTable].Add(m.DBField);
        //    }

        //    return dResult;
        //}

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

        //private class Metric
        //{
        //    public string Title { get; internal set; }
        //    public int MetricID { get; internal set; }
        //    public int GroupTypeID { get; internal set; }
        //    public float Threshold { get; internal set; }
        //    public bool IsActive { get; internal set; }
        //    public string Units { get; internal set; }

        //    public Metric(string sTitle, int nMetricID, int nGroupTypeID, float fThreshold, bool bIsActive)
        //    {
        //        Title = sTitle;
        //        MetricID = nMetricID;
        //        GroupTypeID = nGroupTypeID;
        //        Threshold = fThreshold;
        //        IsActive = bIsActive;
        //        Units = string.Empty;
        //    }
        //}
    }
}
