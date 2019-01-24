using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public abstract class MetricInstance
    {
        public const string MODEL_VERSION_METRIC_NAME = "ModelVersion";
        public const string GENERATION_DATE_METRIC_NAME = "GenerationDate";

        public long InstanceID { get; internal set; }
        public long VisitID { get; internal set; }
        public string ModelVersion { get; internal set; }
        public Dictionary<long, double?> Metrics { get; internal set; }
        public DateTime? GenerationDate { get; internal set; }

        public MetricInstance(long nInstanceID, long nVisitID, string sModelVersion, DateTime? dtGenerationDate)
        {
            InstanceID = nInstanceID;
            VisitID = nVisitID;
            ModelVersion = sModelVersion;
            GenerationDate = dtGenerationDate;
            Metrics = new Dictionary<long, double?>();
        }

        public virtual List<GeoOptix.API.Model.MetricValueModel> GetAPIMetricInstance(ref Data.Metrics.Upload.SchemaDefinitionWorkbench schemaDef)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = new List<GeoOptix.API.Model.MetricValueModel>();

            if (string.IsNullOrEmpty(ModelVersion))
                metricValues.Add(new GeoOptix.API.Model.MetricValueModel(MODEL_VERSION_METRIC_NAME, null));
            else
                metricValues.Add(new GeoOptix.API.Model.MetricValueModel(MODEL_VERSION_METRIC_NAME, ModelVersion));

            if (GenerationDate.HasValue)
                metricValues.Add(new GeoOptix.API.Model.MetricValueModel(GENERATION_DATE_METRIC_NAME, GenerationDate.Value.ToString("o")));
            else
                metricValues.Add(new GeoOptix.API.Model.MetricValueModel(GENERATION_DATE_METRIC_NAME, null));

            // Note that this loop is only over numeric metrics from the database. String metrics are handled in the code above.
            foreach (Data.MetricDefinitions.MetricDefinitionBase metricDef in schemaDef.MetricsByID.Values.Where<Data.MetricDefinitions.MetricDefinitionBase>(x => x.DataTypeID == 10023))
            {
                if (Metrics.ContainsKey(metricDef.ID) && Metrics[metricDef.ID].HasValue)
                {
                    string sFormat = "0";
                    if (metricDef.Precision.HasValue)
                        sFormat = string.Format("0.{0}", new string('0', Convert.ToInt32(metricDef.Precision.Value)));

                    string sMetricValue = Metrics[metricDef.ID].Value.ToString(sFormat);
                    metricValues.Add(new GeoOptix.API.Model.MetricValueModel(metricDef.Name, sMetricValue));
                }
                else
                    metricValues.Add(new GeoOptix.API.Model.MetricValueModel(metricDef.Name, null));
            }

            return metricValues;
        }

        public class MetricInstanceValue : naru.db.NamedObject
        {
            public double? MetricValue { get; internal set; }

            public MetricInstanceValue(long nID, string sName, double? fMetricValue)
                : base(nID, sName)
            {
                MetricValue = fMetricValue;
            }
        }
    }

    public class MetricVisitInstance : MetricInstance
    {
        public MetricVisitInstance(long nInstanceID, long nVisitID, string sModelVersion, DateTime? dtGenerationDate)
            : base(nInstanceID, nVisitID, sModelVersion, dtGenerationDate)
        {

        }
    }

    public class MetricTierInstance : MetricInstance
    {
        public long TierID { get; internal set; }
        public string TierName { get; internal set; }
        public ushort TierLevel { get; internal set; }

        public MetricTierInstance(ushort nTierLevel, long nInstanceID, long nVisitID, string sModelVersion, DateTime? dtGenerationDate, long nTierID, string sTierName)
        : base(nInstanceID, nVisitID, sModelVersion, dtGenerationDate)
        {
            TierID = nTierID;
            TierName = sTierName;
            TierLevel = nTierLevel;
        }

        public override List<GeoOptix.API.Model.MetricValueModel> GetAPIMetricInstance(ref Data.Metrics.Upload.SchemaDefinitionWorkbench schemaDef)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = base.GetAPIMetricInstance(ref schemaDef);
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel(string.Format("Tier{0}", TierLevel), TierName));
            return metricValues;
        }
    }

    public class MetricChannelUnitInstance : MetricInstance
    {
        public long ChannelUnitNumber { get; internal set; }

        public string Tier1Name { get; internal set; }
        public string Tier2Name { get; internal set; }

        public MetricChannelUnitInstance(long nInstanceID, long nVisitID, string sModelVersion, DateTime? dtGenerationDate, long nChannelUnitNumber, string sTier1Name, string sTier2Name)
        : base(nInstanceID, nVisitID, sModelVersion, dtGenerationDate)
        {
            ChannelUnitNumber = nChannelUnitNumber;
            Tier1Name = sTier1Name;
            Tier2Name = sTier2Name;
        }

        public override List<GeoOptix.API.Model.MetricValueModel> GetAPIMetricInstance(ref Data.Metrics.Upload.SchemaDefinitionWorkbench schemaDef)
        {
            List<GeoOptix.API.Model.MetricValueModel> metricValues = base.GetAPIMetricInstance(ref schemaDef);
            metricValues.First(x => string.Compare(x.Name, "ChUnitNumber", true) == 0).Value = ChannelUnitNumber.ToString();
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel("Tier1", Tier1Name));
            metricValues.Add(new GeoOptix.API.Model.MetricValueModel("Tier2", Tier2Name));
            return metricValues;
        }
    }
}
