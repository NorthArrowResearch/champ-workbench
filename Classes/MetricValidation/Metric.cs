using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class Metric
    {
        private const int m_nValidationScavengeTypeID = 2;

        public string Title { get; internal set; }
        public int MetricID { get; internal set; }
        public Nullable<int> CMMetricID { get; internal set; }
        public int GroupTypeID { get; internal set; }
        public float Threshold { get; internal set; }
        public bool IsActive { get; internal set; }
        public string Units { get; internal set; }

        public Dictionary<int, VisitResults> Visits;

        public Metric(string sTitle, int nMetricID, Nullable<int> nCMMetricID, int nGroupTypeID, float fThreshold, bool bIsActive)
        {
            Title = sTitle;
            MetricID = nMetricID;
            CMMetricID = nCMMetricID;
            GroupTypeID = nGroupTypeID;
            Threshold = fThreshold;
            IsActive = bIsActive;
            Units = string.Empty;

            Visits = new Dictionary<int, VisitResults>();
        }

        public void LoadResults(string sDBCon, ref List<ListItem> lvisits, bool bManualMetricValues)
        {
            using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand(MetricResultSQLStatement(bManualMetricValues), dbCon);
                OleDbParameter pVisitID = dbCom.Parameters.Add("@VisitID", OleDbType.Integer);
                OleDbDataReader dbRead = null;

                foreach (ListItem aVisit in lvisits)
                {
                    if (!Visits.ContainsKey(aVisit.Value))
                    {
                        VisitResults aResult = new VisitResults(aVisit.Value, aVisit.Value, "test", "test");
                        Visits.Add(aVisit.Value, aResult);
                    }

                    pVisitID.Value = aVisit.Value;
                    System.Diagnostics.Debug.Print(dbCom.CommandText);
                    dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        if (bManualMetricValues)
                            Visits[aVisit.Value].ManualResult = new MetricValueBase((float)dbRead[0]);
                        else
                        {
                            string sModelVersion = dbRead.GetString(dbRead.GetOrdinal("RBTVersion"));
                            Visits[aVisit.Value].ModelResults[sModelVersion] = new MetricValueModel(sModelVersion, (float)dbRead[0]);
                        }
                    }
                }
            }
        }

        private string MetricResultSQLStatement(bool bManualMetricValues)
        {
            string sSQL = "SELECT V.MetricValue, R.RBTVersion";

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

            XmlNode nodTolerance = xmlDoc.CreateElement("tolerance");
            nodTolerance.InnerText = Threshold.ToString("#0.00");
            nodMetric.AppendChild(nodTolerance);

            XmlNode nodVisits = xmlDoc.CreateElement("visits");
            nodMetric.AppendChild(nodVisits);

            foreach (VisitResults aVisit in Visits.Values)
                aVisit.Serialize(ref xmlDoc, ref nodVisits, Threshold);
        }
    }
}
