using System;
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

        protected void Init(string sName)
        {
            Name = sName;
            Metrics = new Dictionary<string, string>();

            Metrics.Add(CHaMPData.MetricInstance.MODEL_VERSION_METRIC_NAME, "string");
            Metrics.Add(CHaMPData.MetricInstance.GENERATION_DATE_METRIC_NAME, "string");
        }

        protected SchemaDefinition(long nSchemaID, string sSchemaName)
        {
            Name = sSchemaName;
            Init(sSchemaName);
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

            // Remember that Generation Date and ModelVersion will already be in the dictionary by virtue of Init()
            foreach (GeoOptix.API.Model.MetricAttributeModel apiMetric in apiSchema.Payload.Attributes)
                if (!Metrics.ContainsKey(apiMetric.Name))
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
    }
}
