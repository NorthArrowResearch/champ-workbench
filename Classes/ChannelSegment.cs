using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class ChannelSegment : NamedDBObject
    {
        private int m_nNumber;
        private Dictionary<int, ChannelUnit> m_dChannelUnits;

        public int Number
        {
            get
            {
                return m_nNumber;
            }
        }

        public ChannelSegment(int nID, String sName, int nNumber)
            : base(nID, sName)
        {
            m_nNumber = nNumber;
            m_dChannelUnits = new Dictionary<int,ChannelUnit>();
        }

        public ChannelSegment(int nID, String sName , int nNumber, System.Data.OleDb.OleDbConnection dbCon) : this(nID, sName, nNumber)
        {
            using (System.Data.OleDb.OleDbCommand dbCom = new System.Data.OleDb.OleDbCommand("SELECT ID, ChannelUnitNumber, Tier1, Tier2 FROM CHAMP_ChannelUnits WHERE SegmentID = " + ID.ToString(), dbCon))
            {
                System.Data.OleDb.OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    ChannelUnit ch = new ChannelUnit((int) dbRead["ID"], (String) dbRead["Tier1"], (String) dbRead["Tier1"], (String) dbRead["Tier2"]);
                    m_dChannelUnits.Add(ch.ID,ch);
                }
            }
        }

        public void WriteToXML(ref XmlTextWriter xFile)
        {
            xFile.WriteStartElement("segment");
            xFile.WriteElementString("id", ID.ToString());
            xFile.WriteElementString("segment_number", Number.ToString());
            xFile.WriteElementString("segment_type", this.ToString());

            xFile.WriteStartElement("channel_units");
            foreach (ChannelUnit ch in m_dChannelUnits.Values)
                ch.WriteToXML(ref xFile);

            xFile.WriteEndElement(); // channel units

            xFile.WriteEndElement(); // Segment
        }
    }
}
