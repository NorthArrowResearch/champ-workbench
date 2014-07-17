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

        public ChannelSegment(RBTWorkbenchDataSet.CHaMP_SegmentsRow rSegment) : this(rSegment.SegmentID, rSegment.SegmentName, rSegment.SegmentNumber)
        {
            foreach (RBTWorkbenchDataSet.CHAMP_ChannelUnitsRow rUnit in rSegment.GetCHAMP_ChannelUnitsRows())
            {
                m_dChannelUnits.Add(rUnit.ID, new ChannelUnit(rUnit));
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
