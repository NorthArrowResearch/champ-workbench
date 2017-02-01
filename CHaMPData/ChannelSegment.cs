using System;
using System.Collections.Generic;
using System.Xml;
using System.Data.SQLite;
using naru.xml;

namespace CHaMPWorkbench.CHaMPData
{
    public class ChannelSegment : naru.db.NamedObject
    {
        public long ChannelSegmentNumber { get; internal set; }
        public Dictionary<long, ChannelUnit> ChannelUnits { get; internal set; }

        public ChannelSegment(long nID, String sName, long nNumber)
            : base(nID, sName)
        {
            ChannelSegmentNumber = nNumber;
            ChannelUnits = ChannelUnit.Load(DBCon.ConnectionString, nID);
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

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc)
        {
            XmlNode nodSegment = xmlDoc.CreateElement("segment");
            XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "id", ID.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "segment_number", ChannelSegmentNumber.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "segment_type", this.ToString());
            XmlNode nodChannelUnits = XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "channel_units");
            foreach (ChannelUnit ch in ChannelUnits.Values)
                nodChannelUnits.AppendChild(ch.CreateXMLNode(ref xmlDoc));

            return nodSegment;
        }
    }
}
