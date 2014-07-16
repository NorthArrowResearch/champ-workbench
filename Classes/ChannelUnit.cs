using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class ChannelUnit : NamedDBObject
    {
        private String m_sTier1;
        private String m_sTier2;

        public ChannelUnit(int nID, String sName, String sTier1, String sTier2)
            : base(nID, sName)
        {
            m_sTier1 = sTier1;
            m_sTier2 = sTier2;
        }

         public void WriteToXML(ref XmlTextWriter xFile)
        {
            xFile.WriteStartElement("unit");
            xFile.WriteElementString("id", ID.ToString());
            xFile.WriteElementString("unit_number", ID.ToString());
            xFile.WriteElementString("tier1", m_sTier1);
            xFile.WriteElementString("tier2", m_sTier2);
            xFile.WriteEndElement(); // unit
        }

    }
}
