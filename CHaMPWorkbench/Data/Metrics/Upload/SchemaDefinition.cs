﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Data.Metrics.Upload
{
    /// <summary>
    /// The purpose of this class is to help determine if two definitions of a metric schema are identical.
    /// You can construct a copy of this class from the Workbench database and then another from an XML definition
    /// file on GitHub and determine if they match using the Equals() method
    /// </summary>
    public class SchemaDefinition
    {
        public string Name { get; internal set; }
        public Dictionary<string, string> Metrics { get; internal set; }

        private void Init(string sName)
        {
            Name = sName;
            Metrics = new Dictionary<string, string>();

            Metrics.Add("ModelVersion", "string");
            Metrics.Add("GenerationDate", "string");
        }

        /// <summary>
        /// Constructor for loading schema from database
        /// </summary>
        /// <param name="SchemaID"></param>
        /// <param name="sName"></param>
        public SchemaDefinition(long SchemaID, string sName)
        {
            Init(sName);

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT D.DisplayNameShort as Title, L.Title AS DataType" +
                    " FROM Metric_Definitions D" +
                    " INNER JOIN Metric_Schema_Definitions S ON D.MetricID = S.MetricID" +
                    " INNER JOIN LookupListItems L ON D.DataTypeID = L.ItemID" +
                    " WHERE (SchemaID = @SchemaID) AND (IsActive <> 0)", dbCon))
                {
                    dbCom.Parameters.AddWithValue("SchemaID", SchemaID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                        Metrics[dbRead.GetString(dbRead.GetOrdinal("Title"))] = dbRead.GetString(dbRead.GetOrdinal("DataType"));
                }
            }
        }

        public SchemaDefinition(string schemaDefinitionURL)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(schemaDefinitionURL);

            Init(xmlDoc.SelectSingleNode("/MetricSchema/Name").InnerText);

            foreach (XmlNode nodMetric in xmlDoc.SelectNodes("MetricSchema/Metrics/Metric"))
                Metrics[nodMetric.Attributes["name"].InnerText] = nodMetric.Attributes["type"].InnerText;
        }

        public SchemaDefinition(ref GeoOptix.API.ApiResponse<GeoOptix.API.Model.MetricSchemaModel> apiSchema)
        {
            Init(apiSchema.Payload.Name);

            foreach (GeoOptix.API.Model.MetricAttributeModel apiMetric in apiSchema.Payload.Attributes)
                Metrics.Add(apiMetric.Name, apiMetric.Type);
        }

        public bool Equals(ref SchemaDefinition otherSchema, out List<string> Messages)
        {
            bool bStatus = true;
            Messages = new List<string>();

            if (string.Compare(Name, otherSchema.Name, false) != 0)
            {
                Messages.Add(string.Format("The schema names {0} and {1} do not match.", Name, otherSchema.Name));
                bStatus = false;
            }

            foreach (string aMetric in Metrics.Keys)
            {
                if (otherSchema.Metrics.ContainsKey(aMetric))
                {
                    if (string.Compare(Metrics[aMetric], otherSchema.Metrics[aMetric], true) != 0)
                    {
                        Messages.Add(string.Format("The metric {0} data type does not appear in all schemas.", aMetric));
                        bStatus = false;
                    }
                }
                else
                {
                    Messages.Add(string.Format("The metric {0} does not appear in all schemas.", aMetric));
                    bStatus = false;
                }
            }

            foreach (string aMetric in otherSchema.Metrics.Keys)
            {
                if (!otherSchema.Metrics.ContainsKey(aMetric))
                {
                    Messages.Add(string.Format("The metric {0} does not appear in all schemas.", aMetric));
                    bStatus = false;
                }
            }
            return bStatus;
        }

        public List<GeoOptix.API.Model.MetricValueModel> GetVisitMetricValues(long InstanceID)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = new List<GeoOptix.API.Model.MetricValueModel>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT D.DisplayNameShort AS MetricName, DataTypeID, Precision, MetricValue FROM Metric_VisitMetrics M INNER JOIN Metric_Definitions D ON M.MetricID = D.MetricID WHERE InstanceID = @InstanceID", dbCon))
                {
                    dbCom.Parameters.AddWithValue("InstanceID", InstanceID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        string metricStringValue = string.Empty;
                        if (!dbRead.IsDBNull(dbRead.GetOrdinal("MetricValue")))
                        {
                            double metricRawValue = dbRead.GetDouble(dbRead.GetOrdinal("MetricValue"));
                            long? precision = naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Precision");
                            if (dbRead.GetInt64(dbRead.GetOrdinal("DataTypeID")) == 10023 && precision.HasValue)
                                metricStringValue = metricRawValue.ToString(string.Format("#.{0}", new string('0',Convert.ToInt32(precision.Value))));
                            else
                                metricStringValue.ToString();
                        }

                        metricValues.Add(new GeoOptix.API.Model.MetricValueModel(dbRead.GetString(dbRead.GetOrdinal("MetricName")), metricStringValue));
                    }
                }
            }

            return metricValues;
        }
    }
}
