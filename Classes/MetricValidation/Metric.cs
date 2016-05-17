using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    /// <summary>
    /// Class representing a CHaMP metric.
    /// </summary>
    /// <remarks>Each metric is a record in the Metric_MetricDefinitions table in the 
    /// Workbench database</remarks>
    public class Metric
    {
        // This is the LookupListID for validation runs of models (e.g. Carol's manual RBT values)
        private const int m_nValidationScavengeTypeID = 2;

        public string Title { get; internal set; }
        public int MetricID { get; internal set; }
        public Nullable<int> CMMetricID { get; internal set; }
        public int GroupTypeID { get; internal set; }
        public float Threshold { get; internal set; }
        public Nullable<double> MinValue { get; internal set; }
        public Nullable<double> MaxValue { get; internal set; }
        public bool IsActive { get; internal set; }
        public string Units { get; internal set; }
        public string ParentGroup { get; set; } // This is the parent grouping in the watershed report
        public string ChildGroup { get; set; } // this is the child grouping in the watershed report

        public Dictionary<int, VisitResults> Visits;

        public Metric(string sTitle, int nMetricID, Nullable<int> nCMMetricID, int nGroupTypeID, float fThreshold, Nullable<double> fMinValue, Nullable<double> fMaxValue, bool bIsActive,
           string sGroupType, string sChannelGroup)
        {
            Title = sTitle;
            MetricID = nMetricID;
            CMMetricID = nCMMetricID;
            GroupTypeID = nGroupTypeID;
            Threshold = fThreshold;
            MinValue = fMinValue;
            MaxValue = fMaxValue;
            IsActive = bIsActive;
            Units = string.Empty;
            ParentGroup = sGroupType;
            ChildGroup = sChannelGroup;

            Visits = new Dictionary<int, VisitResults>();
        }

        public void LoadResults(string sDBCon, ref Dictionary<int, ValidationVisitInfo> dVisits, bool bManualMetricValues)
        {
            using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand(MetricResultSQLStatement(bManualMetricValues), dbCon);
                OleDbParameter pVisitID = dbCom.Parameters.Add("@VisitID", OleDbType.Integer);
                OleDbDataReader dbRead = null;

                foreach (ValidationVisitInfo aVisit in dVisits.Values)
                {
                    pVisitID.Value = aVisit.VisitID;
                    System.Diagnostics.Debug.Print(dbCom.CommandText);
                    dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        if (!Visits.ContainsKey(aVisit.VisitID))
                        {
                            VisitResults aResult = new VisitResults(aVisit);
                            Visits.Add(aVisit.VisitID, aResult);
                        }

                        if (bManualMetricValues)
                        {
                            Visits[aVisit.VisitID].ManualResult = new MetricValueBase((float)(double)dbRead[0]);
                        }
                        else
                        {
                            string sModelVersion = GetFormattedRBTVersion(dbRead.GetString(dbRead.GetOrdinal("ModelVersion")));
                            float fMetricValue = GetMetricValue(ref dbRead, dbRead.GetOrdinal("MetricValue"));
                            Visits[aVisit.VisitID].ModelResults[sModelVersion] = new MetricValueModel(sModelVersion, fMetricValue);
                        }
                    }
                    dbRead.Close();
                }
            }
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

        private string MetricResultSQLStatement(bool bManualMetricValues)
        {
            string sSQL = "SELECT V.MetricValue, R.ModelVersion";

            switch (GroupTypeID)
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

            sSQL += string.Format(" WHERE (R.VisitID = @VisitID) AND (V.MetricID = {0}) AND (R.ScavengeTypeID {1} {2})", MetricID, (bManualMetricValues) ? "=" : "<>", m_nValidationScavengeTypeID);
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
            nodGroupTypeID.InnerText = GroupTypeID.ToString();
            nodMetric.AppendChild(nodGroupTypeID);

            XmlNode nodTolerance = xmlDoc.CreateElement("tolerance");
            nodTolerance.InnerText = Threshold.ToString("#0.00");
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
