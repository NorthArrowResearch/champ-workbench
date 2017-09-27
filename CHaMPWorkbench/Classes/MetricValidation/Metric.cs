using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    /// <summary>
    /// Class representing a CHaMP metric.
    /// </summary>
    /// <remarks>Each metric is a record in the Metric_MetricDefinitions table in the 
    /// Workbench database</remarks>
    public class Metric
    {
        public string Title { get; internal set; }
        public long MetricID { get; internal set; }
        public Nullable<long> CMMetricID { get; internal set; }
        public long SchemaID { get; internal set; }
        public Nullable<double> Threshold { get; internal set; }
        public Nullable<double> MinValue { get; internal set; }
        public Nullable<double> MaxValue { get; internal set; }
        public bool IsActive { get; internal set; }
        public string Units { get; internal set; }
        public string ParentGroup { get; set; } // This is the parent grouping in the watershed report
        public string ChildGroup { get; set; } // this is the child grouping in the watershed report

        public Dictionary<long, VisitResults> Visits;

        public Metric(string sTitle, long nMetricID, Nullable<long> nCMMetricID, long nSchemaID, double? fThreshold, Nullable<double> fMinValue, Nullable<double> fMaxValue, bool bIsActive,
           string sGroupType, string sChannelGroup)
        {
            Title = sTitle;
            MetricID = nMetricID;
            CMMetricID = nCMMetricID;
            SchemaID = nSchemaID;
            Threshold = fThreshold;
            MinValue = fMinValue;
            MaxValue = fMaxValue;
            IsActive = bIsActive;
            Units = string.Empty;
            ParentGroup = sGroupType;
            ChildGroup = sChannelGroup;

            Visits = new Dictionary<long, VisitResults>();
        }

        public void LoadResults(string sDBCon, ref Dictionary<long, ValidationVisitInfo> dVisits, ref List<naru.db.NamedObject> lRBTVersions, bool bManualMetricValues)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                // PGB 17 Oct 2016. Only visit level metrics are currently implemented. Other MetricGroupIDs return
                // an empty string.
                string sSQL = MetricResultSQLStatement(bManualMetricValues);
                if (string.IsNullOrEmpty(sSQL))
                    return;

                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);
                System.Diagnostics.Debug.Print("Broken query: {0}", dbCom.CommandText);
                SQLiteParameter pVisitID = dbCom.Parameters.Add("@VisitID", System.Data.DbType.Int64);
                SQLiteDataReader dbRead = null;

                foreach (ValidationVisitInfo aVisit in dVisits.Values)
                {
                    pVisitID.Value = aVisit.VisitID;
                    System.Diagnostics.Debug.Print(dbCom.CommandText);
                    dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        string sRBTVersion = dbRead.GetString(dbRead.GetOrdinal("ModelVersion"));

                        if (!Visits.ContainsKey(aVisit.VisitID))
                        {
                            VisitResults aResult = new VisitResults(aVisit);
                            Visits.Add(aVisit.VisitID, aResult);
                        }
                        
                        if (bManualMetricValues)
                        {
                            Visits[aVisit.VisitID].ManualResult = new MetricValueBase( naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue"));
                        }
                        else
                        {
                            // If This version isn't in the list then skip it
                            if (lRBTVersions.Find(x => x.Name.Equals(sRBTVersion)) != null)
                            {

                                string sModelVersion = GetFormattedRBTVersion(sRBTVersion);
                                double? fMetricValue = naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue");
                                Visits[aVisit.VisitID].ModelResults[sModelVersion] = new MetricValueModel(sModelVersion, fMetricValue);
                            }
                        }
                    }
                    dbRead.Close();
                }
            }
        }

        /// <summary>
        /// Make the RBT version pretty
        /// </summary>
        /// <param name="sRawRBTVersion"></param>
        /// <returns></returns>
        public static string GetFormattedRBTVersion(string sRawRBTVersion)
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

        private string MetricResultSQLStatement(bool bManualMetricValues)
        {
            string sSQL = "SELECT V.MetricValue, R.ModelVersion";

            switch (SchemaID)
            {
                case 1: // visit metrics
                    sSQL += " FROM Metric_Instances R INNER JOIN Metric_VisitMetrics V ON R.InstanceID = V.InstanceID INNER JOIN Metric_Batches B ON R.BatchID = B.BatchID";
                    break;

                case 3: // tier 1 metrics
                    System.Diagnostics.Debug.Print("WARNING: Tier 1 metrics not yet implemented for MetricGroupID {0}." , SchemaID);
                    return string.Empty;
                    break;

                case 4: // tier 2 metrics
                    System.Diagnostics.Debug.Print("WARNING: Tier 2 metrics not yet implemented for MetricGroupID {0}.", SchemaID);
                    return string.Empty;
                    break;

                case 2: // Channel unit metrics
                    System.Diagnostics.Debug.Print("WARNING: channel unit metrics not yet implemented for MetricGroupID {0}.", SchemaID);
                    return string.Empty;
                    break;
            }

            sSQL += string.Format(" WHERE (R.VisitID = @VisitID) AND (V.MetricID = {0}) AND (B.ScavengeTypeID {1} {2})", MetricID, (bManualMetricValues) ? "=" : "<>", CHaMPWorkbench.Properties.Settings.Default.ModelScavengeTypeID_Manual);
            return sSQL;
        }

        public void Serialize(ref XmlDocument xmlDoc, ref XmlNode nodMetrics)
        {
            if (Visits.Count < 1)
                return;

            XmlNode nodMetric = xmlDoc.CreateElement("metric");
            nodMetrics.AppendChild(nodMetric);

            XmlNode nodMetricName = xmlDoc.CreateElement("name");
            nodMetricName.InnerText = Title;
            nodMetric.AppendChild(nodMetricName);

            XmlNode nodMetricUnits = xmlDoc.CreateElement("unit");
            nodMetricUnits.InnerText = Units;
            nodMetric.AppendChild(nodMetricUnits);

            // TODO: PUT A REAL VALUE IN ME
            XmlNode nodDisplayParentGroup = xmlDoc.CreateElement("display_parent_group");
            nodDisplayParentGroup.InnerText = ParentGroup;
            nodMetric.AppendChild(nodDisplayParentGroup);

            // TODO: PUT A REAL VALUE IN ME
            XmlNode nodDisplayChildGroup = xmlDoc.CreateElement("display_child_group");
            nodDisplayChildGroup.InnerText = ChildGroup;
            nodMetric.AppendChild(nodDisplayChildGroup);

            // TODO: PUT A REAL VALUE IN ME
            XmlNode nodMetricCalcTypeID = xmlDoc.CreateElement("metric_calc_type_id");
            nodMetricCalcTypeID.InnerText = CMMetricID.ToString();
            nodMetric.AppendChild(nodMetricCalcTypeID);

            // TODO: PUT A REAL VALUE IN ME
            XmlNode nodGroupTypeID = xmlDoc.CreateElement("group_type_id");
            nodGroupTypeID.InnerText = SchemaID.ToString();
            nodMetric.AppendChild(nodGroupTypeID);

            XmlNode nodTolerance = xmlDoc.CreateElement("tolerance");
            if (Threshold.HasValue)
                nodTolerance.InnerText = Threshold.Value.ToString("#0.00");
            nodMetric.AppendChild(nodTolerance);

            XmlNode nodMinValue = xmlDoc.CreateElement("minimum");
            if (MinValue.HasValue)
                nodMinValue.InnerText = MinValue.Value.ToString("#0.00");
            nodMetric.AppendChild(nodMinValue);

            XmlNode nodMaxValue = xmlDoc.CreateElement("maximum");
            if (MaxValue.HasValue)
                nodMaxValue.InnerText = MaxValue.Value.ToString("#0.00");
            nodMetric.AppendChild(nodMaxValue);

            XmlNode nodVisits = xmlDoc.CreateElement("visits");
            nodMetric.AppendChild(nodVisits);

            foreach (VisitResults aVisit in Visits.Values)
            {
                aVisit.Serialize(ref xmlDoc, ref nodVisits, this);
            }
        }
    }
}
