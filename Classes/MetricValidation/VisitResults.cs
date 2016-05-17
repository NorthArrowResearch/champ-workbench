using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class VisitResults
    {
        public int VisitID { get; internal set; }
        public int VisitYear { get; internal set; }
        public string Site { get; internal set; }
        public string Watershed { get; internal set; }

        public MetricValueBase ManualResult;
        public Dictionary<string, MetricValueModel> ModelResults;

        public VisitResults(int nVisitID, int nVisitYear, string sSite, string sWatershed)
        {
            VisitID = nVisitID;
            VisitYear = nVisitYear;
            Site = sSite;
            Watershed = sWatershed;

            ModelResults = new Dictionary<string, MetricValueModel>();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - VisitID {3}", VisitYear, Watershed, Site, VisitID);
        }

        public void Serialize(ref XmlDocument xmlDoc, ref XmlNode nodVisits, float fTolerance)
        {
            XmlNode nodVisit = xmlDoc.CreateElement("visit");
            nodVisits.AppendChild(nodVisit);

            XmlNode nodVisitID = xmlDoc.CreateElement("visit_id");
            nodVisitID.InnerText = VisitID.ToString();
            nodVisit.AppendChild(nodVisitID);

            XmlNode nodVisitName = xmlDoc.CreateElement("visit_name");
            nodVisitName.InnerText = this.ToString();
            nodVisit.AppendChild(nodVisitName);

            XmlNode nodFieldSeason = xmlDoc.CreateElement("field_season");
            nodFieldSeason.InnerText = VisitYear.ToString();
            nodVisit.AppendChild(nodFieldSeason);

            // TODO: PUT A REAL VALUE IN ME
            XmlNode nodnodWaterShedID = xmlDoc.CreateElement("watershed_id");
            nodnodWaterShedID.InnerText = "99999999999";
            nodVisit.AppendChild(nodnodWaterShedID);

            XmlNode nodnodWaterShedName = xmlDoc.CreateElement("watershed_name");
            nodnodWaterShedName.InnerText = Watershed;
            nodVisit.AppendChild(nodnodWaterShedName);

            XmlNode nodSiteName = xmlDoc.CreateElement("site");
            nodSiteName.InnerText = Site;
            nodVisit.AppendChild(nodSiteName);

            XmlNode nodOrganization = xmlDoc.CreateElement("organization");
            nodOrganization.InnerText = "ORGANIZATION_NAME";
            nodVisit.AppendChild(nodOrganization);

            XmlNode nodCrewName = xmlDoc.CreateElement("crew_name");
            nodCrewName.InnerText = "CREW_NAME";
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
                        float fDelta = (float)Math.Abs(ManualResult.MetricValue - aResult.MetricValue);
                        float fDiff = fDelta / ManualResult.MetricValue;
                        if (fDiff <= fTolerance)
                            nodStatus.InnerText = "Pass";
                        else
                            nodStatus.InnerText = "Fail";

                    }
                }
                nodResult.AppendChild(nodStatus);
            }
        }
    }
}
