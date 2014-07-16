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

        public Site(int nID, String sName, String sFolder, String sUTMZone, ref Watershed aWatershed) : base(nID, sName)
        {
            m_sFolder = sFolder;
            m_dVisits = new Dictionary<int,Visit>();
            m_Watershed = aWatershed;
            m_sUTMZone = sUTMZone;
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
                aVisit.WriteToXML(ref xmlFile, sSourceFolder);

            xmlFile.WriteEndElement(); // site
        }

    }
}
