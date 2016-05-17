using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class VisitResults
    {
        public  ValidationVisitInfo  VisitInfo { get; internal set;}
        public MetricValueBase ManualResult;
        public Dictionary<string, MetricValueModel> ModelResults;

        public VisitResults(ValidationVisitInfo theVisitInfo)
        {
            VisitInfo = theVisitInfo;
            ModelResults = new Dictionary<string, MetricValueModel>();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - VisitID {3}", VisitInfo.VisitYear, VisitInfo.Watershed, VisitInfo.Site, VisitInfo.VisitID);
        }

        public void Serialize(ref XmlDocument xmlDoc, ref XmlNode nodVisits, Metric theMetric)
        {
            XmlNode nodVisit = xmlDoc.CreateElement("visit");
            nodVisits.AppendChild(nodVisit);

            XmlNode nodVisitID = xmlDoc.CreateElement("visit_id");
            nodVisitID.InnerText = VisitInfo.VisitID.ToString();
            nodVisit.AppendChild(nodVisitID);

            XmlNode nodVisitName = xmlDoc.CreateElement("visit_name");
            nodVisitName.InnerText = this.ToString();
            nodVisit.AppendChild(nodVisitName);

            XmlNode nodFieldSeason = xmlDoc.CreateElement("field_season");
            nodFieldSeason.InnerText = VisitInfo.VisitYear.ToString();
            nodVisit.AppendChild(nodFieldSeason);

            XmlNode nodnodWaterShedID = xmlDoc.CreateElement("watershed_id");
            nodnodWaterShedID.InnerText = VisitInfo.WatershedID.ToString();
            nodVisit.AppendChild(nodnodWaterShedID);

            XmlNode nodnodWaterShedName = xmlDoc.CreateElement("watershed_name");
            nodnodWaterShedName.InnerText = VisitInfo.Watershed;
            nodVisit.AppendChild(nodnodWaterShedName);

            XmlNode nodSiteName = xmlDoc.CreateElement("site");
            nodSiteName.InnerText = VisitInfo.Site;
            nodVisit.AppendChild(nodSiteName);

            XmlNode nodOrganization = xmlDoc.CreateElement("organization");
            nodOrganization.InnerText = VisitInfo.Organization;
            nodVisit.AppendChild(nodOrganization);

            XmlNode nodCrewName = xmlDoc.CreateElement("crew_name");
            nodCrewName.InnerText = VisitInfo.CrewName;
            nodVisit.AppendChild(nodCrewName);

            XmlNode nodManualResult = xmlDoc.CreateElement("manual_result");
            if (ManualResult is MetricValueBase)
                nodManualResult.InnerText = ManualResult.MetricValue.ToString();
            nodVisit.AppendChild(nodManualResult);

            XmlNode nodResults = xmlDoc.CreateElement("results");
            nodVisit.AppendChild(nodResults);

            // Now go and get all the other RBT generated versions (and compare them to the truth)

            foreach (MetricValueModel aResult in ModelResults.Values)
            {
                XmlNode nodResult = xmlDoc.CreateElement("result");
                nodResults.AppendChild(nodResult);

                XmlNode nodVersion = xmlDoc.CreateElement("version");
                nodVersion.InnerText = aResult.Version;
                nodResult.AppendChild(nodVersion);

                XmlNode nodValue = xmlDoc.CreateElement("value");
                nodValue.InnerText = aResult.MetricValue.ToString();
                nodResult.AppendChild(nodValue);

                XmlNode nodStatus = xmlDoc.CreateElement("status");
                if (ManualResult is MetricValueBase)
                {
                    if (ManualResult.MetricValue == 0)
                    {
                        nodStatus.InnerText = "N/A";
                    }
                    else
                    {
                        if (theMetric.MinValue.HasValue && aResult.MetricValue < theMetric.MinValue)
                            nodStatus.InnerText = "OUTOFRANGE_BELOW";
                        else if (theMetric.MaxValue.HasValue && aResult.MetricValue > theMetric.MaxValue)
                            nodStatus.InnerText = "OUTOFRANGE_ABOVE";
                        else
                        {
                            float fDelta = ManualResult.MetricValue - aResult.MetricValue;
                            float fDiff = fDelta / ManualResult.MetricValue;
                            if ((float)Math.Abs(fDiff) <= theMetric.Threshold)
                                nodStatus.InnerText = "PASS";
                            else
                            {
                                if (fDiff < 0)
                                    nodStatus.InnerText = "FAIL_BELOW";
                                else
                                    nodStatus.InnerText = "FAIL_ABOVE";
                            }
                        }
                    }
                }
                nodResult.AppendChild(nodStatus);
            }
        }
    }
}
