using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class BatchSite : NamedDBObject
    {
        private String m_sUTMZone;
        private Watershed m_Watershed;
        private Dictionary<int, BatchVisit> m_dVisits;

        public BatchSite(int nID, String sName, String sUTMZone, ref Watershed aWatershed)
            : base(nID, sName)
        {
            m_dVisits = new Dictionary<int, BatchVisit>();
            m_Watershed = aWatershed;
            m_sUTMZone = sUTMZone;
        }

        public void AddVisit(BatchVisit aVisit)
        {
            System.Diagnostics.Debug.Assert(!m_dVisits.ContainsKey(aVisit.VisitID), "The visit already exists in the site.");
            m_dVisits.Add(aVisit.ID, aVisit);
        }

        public void WriteToXML(XmlTextWriter xmlFile, String sSourceFolder, Boolean bRequireWSTIN)
        {
            xmlFile.WriteStartElement("site");
            xmlFile.WriteElementString("name", this.ToString());
            xmlFile.WriteElementString("utm_zone", m_sUTMZone);
            xmlFile.WriteElementString("watershed", m_Watershed.ToString());
            xmlFile.WriteElementString("stream_name", "");
            xmlFile.WriteElementString("sitegdb", "");

            foreach (BatchVisit aVisit in m_dVisits.Values)
                aVisit.WriteToXML(ref xmlFile, bRequireWSTIN);

            xmlFile.WriteEndElement(); // site
        }

        public string NameForDatabaseBatch
        {
            get
            {
                BatchVisit targetVisit = null;
                foreach (BatchVisit v in m_dVisits.Values)
                    if (v.CalculateMetrics || v.ChangeDetection)
                        targetVisit = v;

                string sName = "";
                if (targetVisit is BatchVisit)
                    sName = targetVisit.FieldSeason.ToString() + ", ";

                if (m_Watershed is Watershed)
                    sName += m_Watershed.ToString() + ", ";

                sName += this.ToString() + ", ";

                if (targetVisit is BatchVisit)
                {
                    if (!string.IsNullOrWhiteSpace(targetVisit.Hitch))
                        sName += targetVisit.Hitch + ", ";

                    if (!string.IsNullOrWhiteSpace(targetVisit.Crew))
                        sName += targetVisit.Crew + ", ";

                    if (targetVisit.Primary)
                    {
                        sName += " Primary";
                        sName += ", ";
                    }
                }

                sName += m_dVisits.Count.ToString() + " visits";
                return sName;
            }
        }
    }
}
