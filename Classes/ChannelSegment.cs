using System;
using System.Collections.Generic;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes
{
    public class ChannelSegment : naru.db.NamedObject
    {
        private long m_nNumber;
        private Dictionary<long, ChannelUnit> m_dChannelUnits;

        public long Number
        {
            get
            {
                return m_nNumber;
            }
        }

        public ChannelSegment(long nID, String sName, long nNumber)
            : base(nID, sName)
        {
            m_nNumber = nNumber;
            m_dChannelUnits = ChannelUnit.Load(DBCon.ConnectionString, nID);
        }

        public static Dictionary<long, ChannelSegment> Load(string sDBCon, long nVisitID)
        {
            Dictionary<long, ChannelSegment> Segments = new Dictionary<long, ChannelSegment>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM WHERE VisitID = @VisitID ORDER BY SegmentNumber", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nID = dbRead.GetInt64(dbRead.GetOrdinal("SegmentID"));
                    Segments[nID] = new ChannelSegment(nID, dbRead.GetString(dbRead.GetOrdinal("SegmentName")), dbRead.GetInt64(dbRead.GetOrdinal("SegmentNumber")));
                }
            }
            return Segments;
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
