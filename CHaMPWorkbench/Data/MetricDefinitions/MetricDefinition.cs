using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Data.MetricDefinitions
{
    public class MetricDefinition : naru.db.NamedObject
    {
        public string DisplayNameShort { get; internal set; }
        public long ModelID { get; internal set; }
        public string ModelName { get; internal set; }
        public string XPath { get; internal set; }
        public bool IsActive { get; internal set; }
        public long DataTypeID { get; internal set; }
        public string DataTypeName { get; internal set; }
        public long? Precision { get; internal set; }
        public double? Threshold { get; internal set; }
        public double? MinValue { get; internal set; }
        public double? MaxValue { get; internal set; }
        public string MMLink { get; internal set; }
        public string AltLink { get; internal set; }
        public DateTime UpdatedOn { get; internal set; }
        public DateTime AddedOn { get; internal set; }

        public List<long> MetricSchemas { get; internal set; }

        public MetricDefinition(string sTitle) : base(0, sTitle)
        {
            init();
        }

        public MetricDefinition(long nID, string sTitle, string sDisplayNameShort
            , long nModelID, string sModelName
            , string sXPath, bool bIsActive
            , long nDataTypeID, string sDataTypeName
            , long? nPrecision, double? fThreshold, double? fMinValue, double? fMaxValue
            , string sMMLink, string sAltLink
            , DateTime dtUpdatedOn, DateTime dtAddedOn)
            : base(nID, sTitle)
        {
            DisplayNameShort = sDisplayNameShort;
            ModelID = nModelID;
            ModelName = sModelName;
            XPath = sXPath;
            IsActive = bIsActive;
            DataTypeID = nDataTypeID;
            DataTypeName = sDataTypeName;
            Precision = nPrecision;
            Threshold = fThreshold;
            MinValue = fMinValue;
            MaxValue = fMaxValue;
            MMLink = sMMLink;
            AltLink = sAltLink;
            UpdatedOn = dtUpdatedOn;
            AddedOn = dtAddedOn;

            init();
        }

        private void init()
        {
            MetricSchemas = new List<long>();
        }

        public static naru.ui.SortableBindingList<MetricDefinition> Load(string sDBCon)
        {
            naru.ui.SortableBindingList<MetricDefinition> result = new naru.ui.SortableBindingList<MetricDefinition>();

            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM vwMetricDefinitions ORDER BY Title", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();

                using (SQLiteConnection conPrograms = new SQLiteConnection(sDBCon))
                {
                    conPrograms.Open();

                    SQLiteCommand comPrograms = new SQLiteCommand("SELECT SchemaID FROM Metric_Schema_Definitions WHERE MetricID = @MetricID", conPrograms);
                    SQLiteParameter pMetricID = comPrograms.Parameters.Add("MetricID", System.Data.DbType.Int64);

                    while (dbRead.Read())
                    {
                        MetricDefinition metricDef = new MetricDefinition(
                            dbRead.GetInt64(dbRead.GetOrdinal("MetricID"))
                            , dbRead.GetString(dbRead.GetOrdinal("Title"))
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "DisplayNameShort")
                            , dbRead.GetInt64(dbRead.GetOrdinal("ModelID"))
                            , dbRead.GetString(dbRead.GetOrdinal("ModelName"))
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "XPath")
                            , dbRead.GetBoolean(dbRead.GetOrdinal("IsActive"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("DataTypeID"))
                            , dbRead.GetString(dbRead.GetOrdinal("DataTypeName"))
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Precision")
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "Threahold")
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MinValue")
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MaxValue")
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "MMLink")
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "AltLink")
                            , TimeZoneInfo.ConvertTimeFromUtc(dbRead.GetDateTime(dbRead.GetOrdinal("UpdatedOn")), pstZone)
                            , TimeZoneInfo.ConvertTimeFromUtc(dbRead.GetDateTime(dbRead.GetOrdinal("AddedOn")), pstZone));

                        pMetricID.Value = metricDef.ID;
                        SQLiteDataReader readPrograms = comPrograms.ExecuteReader();
                        while (readPrograms.Read())
                            metricDef.MetricSchemas.Add(readPrograms.GetInt64(0));
                        readPrograms.Close();

                        result.Add(metricDef);
                    }
                }
            }

            return result;
        }

        public long Save()
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    string[] sFields = { "Title", "SchemaID", "ModelID", "XPath", "Threshold", "MinValue", "MaxValue", "IsActive", "DisplayNameShort", "Precision", "DataTypeID", "MMLink", "AltLink", "YearIntroduced" };
                    SQLiteCommand dbCom = new SQLiteCommand(string.Empty, dbTrans.Connection, dbTrans);

                    if (ID == 0)
                        dbCom.CommandText = string.Format("INSERT INTO Metric_Definitions ({0}) VALUES (@{1})", string.Join(", ", sFields), string.Join(", @", sFields));
                    else
                    {
                        dbCom.CommandText = string.Format("UPDATE Metric_Definitions SET {0} WHERE MetricID = @MetricID", string.Join(", ", sFields.Select(x => string.Format("{0} = @{0}", x))));
                        dbCom.Parameters.AddWithValue("MetricID", ID);
                    }

                    dbCom.Parameters.AddWithValue("Title", Name);
                    dbCom.Parameters.AddWithValue("ModelID", ModelID);
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, XPath, "XPath");
                    naru.db.sqlite.SQLiteHelpers.AddDoubleParameterN(ref dbCom, Threshold, "Threshold");
                    naru.db.sqlite.SQLiteHelpers.AddDoubleParameterN(ref dbCom, MinValue, "MinValue");
                    naru.db.sqlite.SQLiteHelpers.AddDoubleParameterN(ref dbCom, MaxValue, "MaxValue");
                    dbCom.Parameters.AddWithValue("IsActive", IsActive);
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, DisplayNameShort, "DisplayNameShort");
                    naru.db.sqlite.SQLiteHelpers.AddLongParameterN(ref dbCom, Precision, "Precision");
                    dbCom.Parameters.AddWithValue("DataTypeID", DataTypeID);
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, MMLink, "MMLink");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, AltLink, "AltLink");
                    dbCom.Parameters.AddWithValue("YearIntroduced", DateTime.Now.Year);

                    dbCom.ExecuteNonQuery();

                    if (ID == 0)
                    {
                        dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                        ID = (long)dbCom.ExecuteScalar();
                    }

                    // Now Save the programs with which this metric is associated.
                    SaveMetricPrograms(ref dbTrans);

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }

            return ID;
        }

        private void SaveMetricPrograms(ref SQLiteTransaction dbTrans)
        {
            List<long> existingSchemaIDs = new List<long>();
            // Get list of existing programs
            SQLiteCommand dbCom = new SQLiteCommand("SELECT SchemaID FROM Metric_Schema_Definitions WHERE MetricID = @MetricID", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("MetricID", ID);
            SQLiteDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
                existingSchemaIDs.Add(dbRead.GetInt64(0));
            dbRead.Close();

            // Insert the currently in use programs that are not already in the DB
            dbCom = new SQLiteCommand("INSERT INTO Metric_Schema_Definitions (MetricID, SchemaID) VALUES (@MetricID, @SchemaID)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("MetricID", ID);
            SQLiteParameter pProgramID = dbCom.Parameters.Add("SchemaID", System.Data.DbType.Int64);
            foreach (long currentProgramID in MetricSchemas)
            {
                if (!existingSchemaIDs.Contains(currentProgramID))
                {
                    pProgramID.Value = currentProgramID;
                    dbCom.ExecuteNonQuery();
                }
            }

            // Delete programs that are no longer in use but exist in the DB
            dbCom = new SQLiteCommand("DELETE FROM Metric_Schema_Definitions WHERE MetricID = @MetricID AND SchemaID = @SchemaID", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("MetricID", ID);
            pProgramID = dbCom.Parameters.Add("SchemaID", System.Data.DbType.Int64);
            foreach (long existingProgramID in existingSchemaIDs)
            {
                if (!MetricSchemas.Contains(existingProgramID))
                {
                    pProgramID.Value = existingProgramID;
                    dbCom.ExecuteNonQuery();
                }
            }
        }

        public static int ExportMetricSchemaToXML(string sFullPath, long nSchemaID)
        {
            int nExportedMetrics = 0;

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode nodSchema = xmlDoc.CreateElement("MetricSchema");
            xmlDoc.AppendChild(nodSchema);

            XmlAttribute attNameSpace = xmlDoc.CreateAttribute("xmlns:xsi");
            attNameSpace.InnerText = "http://www.w3.org/2001/XMLSchema-instance";
            nodSchema.Attributes.Append(attNameSpace);

            XmlAttribute attSchema = xmlDoc.CreateAttribute("xsi:noNamespaceSchemaLocation");
            attSchema.InnerText = "XSD/schema.xsd";
            nodSchema.Attributes.Append(attSchema);

            //Create an XML declaration. 
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8".ToLower(), null);
            xmlDoc.InsertBefore(xmldecl, nodSchema);

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT Title, RootXPath FROM Metric_Schemas WHERE (SchemaID = @SchemaID) AND (Title IS NOT NULL) AND (RootXPath IS NOT NULL)", dbCon);
                dbCom.Parameters.AddWithValue("SchemaID", nSchemaID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                if (!dbRead.Read())
                    throw new Exception(string.Format("Metric schema with ID {0} does not possess a title and XPathRoot.", nSchemaID));

                XmlNode nodName = xmlDoc.CreateElement("Name");
                nodName.InnerText = dbRead.GetString(dbRead.GetOrdinal("Title"));
                nodSchema.AppendChild(nodName);

                string sXPathRoot = dbRead.GetString(dbRead.GetOrdinal("RootXPath"));
                XmlNode nodRoot = xmlDoc.CreateElement("RootXPath");
                nodRoot.InnerText = sXPathRoot;
                nodSchema.AppendChild(nodRoot);

                XmlNode nodMetrics = xmlDoc.CreateElement("Metrics");
                nodSchema.AppendChild(nodMetrics);

                dbRead.Close();

                dbCom = new SQLiteCommand("SELECT DisplayNameShort, Title, XPath, DataTypeName, Precision FROM vwMetricDefinitions AS D INNER JOIN Metric_Schema_Definitions S ON D.MetricID = S.MetricID" +
                    " WHERE (SchemaID = @SchemaID) AND (DisplayNameShort IS NOT NULL) AND (IsActive <> 0) AND (XPath IS NOT NULL) ORDER BY Title", dbCon);
                dbCom.Parameters.AddWithValue("SchemaID", nSchemaID);
                dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    XmlNode nodMetric = xmlDoc.CreateElement("Metric");
                    nodMetrics.AppendChild(nodMetric);

                    XmlAttribute attName = xmlDoc.CreateAttribute("name");
                    attName.InnerText = dbRead.GetString(dbRead.GetOrdinal("DisplayNameShort"));
                    nodMetric.Attributes.Append(attName);

                    XmlAttribute attXPath = xmlDoc.CreateAttribute("xpath");
                    string sFullXPath = dbRead.GetString(dbRead.GetOrdinal("XPath"));
                    attXPath.InnerText = sFullXPath.Replace(sXPathRoot, "").TrimStart('/');
                    nodMetric.Attributes.Append(attXPath);

                    XmlAttribute attPrecision = xmlDoc.CreateAttribute("precision");
                    nodMetric.Attributes.Append(attPrecision);
                    long? nPrecision = naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Precision");
                    if (nPrecision.HasValue)
                        attPrecision.InnerText = nPrecision.ToString();

                    XmlAttribute attType = xmlDoc.CreateAttribute("type");
                    attType.InnerText = dbRead.GetString(dbRead.GetOrdinal("DataTypeName")).ToLower();
                    nodMetric.Attributes.Append(attType);
                    nExportedMetrics++;
                }
            }

            xmlDoc.Save(sFullPath);
            return nExportedMetrics;
        }
    }
}
