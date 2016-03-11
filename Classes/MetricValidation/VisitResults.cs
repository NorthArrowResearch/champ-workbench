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

            XmlNode nodManualResult = xmlDoc.CreateElement("manual_result");
            if (ManualResult is MetricValueBase)
                nodManualResult.InnerText = ManualResult.ToString();
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
                    float fDiff = (float)Math.Abs((decimal)((ManualResult.MetricValue - aResult.MetricValue) / ManualResult.MetricValue));
                    if (fDiff <= fTolerance)
                        nodStatus.InnerText = "Pass";
                    else
                        nodStatus.InnerText = "Fail";
                }
                nodResult.AppendChild(nodStatus);
            }
        }
    }
}
