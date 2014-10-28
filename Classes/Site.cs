using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class Site : NamedDBObject
    {
        private String m_sFolder;
        private String m_sUTMZone;
        private Watershed m_Watershed;
        private Dictionary<int, Visit> m_dVisits;

        public Site(int nID, String sName, String sFolder, String sUTMZone, ref Watershed aWatershed)
            : base(nID, sName)
        {
            m_sFolder = sFolder;
            m_dVisits = new Dictionary<int, Visit>();
            m_Watershed = aWatershed;
            m_sUTMZone = sUTMZone;
        }

        public Site(RBTWorkbenchDataSet.CHAMP_SitesRow rSite)
            : base(rSite.SiteID, rSite.SiteName)
        {
            if (!rSite.IsFolderNull())
                m_sFolder = rSite.Folder;

            m_dVisits = new Dictionary<int, Visit>();

            if (!rSite.IsWatershedIDNull())
                m_Watershed = new Watershed(rSite.CHAMP_WatershedsRow);

            if (!rSite.IsUTMZoneNull())
                m_sUTMZone = rSite.UTMZone;

        }


        public String Folder
        {
            get
            {
                return m_sFolder;
            }
        }

        public void AddVisit(Visit aVisit)
        {
            m_dVisits.Add(aVisit.ID, aVisit);
        }

        public void WriteToXML(XmlTextWriter xmlFile, String sSourceFolder)
        {
            xmlFile.WriteStartElement("site");
            xmlFile.WriteElementString("name", this.ToString());
            xmlFile.WriteElementString("utm_zone", m_sUTMZone);
            xmlFile.WriteElementString("watershed", m_Watershed.ToString());
            xmlFile.WriteElementString("stream_name", "");
            xmlFile.WriteElementString("sitegdb", "");

            foreach (Visit aVisit in m_dVisits.Values)
            {
                if (!String.IsNullOrWhiteSpace(aVisit.Folder) && !String.IsNullOrWhiteSpace(Folder))
                {
                    String sVisitTopoDatafolder = System.IO.Path.Combine(sSourceFolder, aVisit.FieldSeason.ToString());
                    sVisitTopoDatafolder = System.IO.Path.Combine(sVisitTopoDatafolder, m_Watershed.Folder);
                    sVisitTopoDatafolder = System.IO.Path.Combine(sVisitTopoDatafolder, Folder);
                    sVisitTopoDatafolder = System.IO.Path.Combine(sVisitTopoDatafolder, aVisit.Folder);
                    aVisit.WriteToXML(ref xmlFile, sVisitTopoDatafolder);
                }
            }

            xmlFile.WriteEndElement(); // site
        }

        public string NameForDatabaseBatch
                {
            get
            {
                Visit targetVisit = null;
                foreach (Visit v in m_dVisits.Values)
                    if (v.CalculateMetrics || v.ChangeDetection)
                        targetVisit = v;

                   string sName ="";
                if (targetVisit is Visit)
                    sName = targetVisit.FieldSeason.ToString() + ", ";

                if (m_Watershed is Watershed)
                    sName += m_Watershed.ToString() + ", ";

                sName += this.ToString() + ", ";

                if (targetVisit is Visit)
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
