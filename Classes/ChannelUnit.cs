using System;
using System.Collections.Generic;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes
{
    class ChannelUnit : naru.db.NamedObject
    {
        public String Tier1 { get; internal set; }
        public String Tier2 { get; internal set; }
        public long ChannelUnitNumber { get; internal set; }

        public Nullable<long> Bedrock { get; internal set; }
        public Nullable<long> BouldersGT256 { get; internal set; }
        public Nullable<long> Cobbles65255 { get; internal set; }
        public Nullable<long> CoarseGravel1764 { get; internal set; }
        public Nullable<long> FineGravel316 { get; internal set; }
        public Nullable<long> Sand0062 { get; internal set; }
        public Nullable<long> FinesLT006 { get; internal set; }
        public Nullable<long> SumSubstrateCover { get; internal set; }
        public Nullable<long> LargeWoodCount { get; internal set; }

        public ChannelUnit(int nID, int nChannelUnitNumber, String sName, String sTier1, String sTier2)
            : base(nID, sName)
        {
            Tier1 = sTier1;
            Tier2 = sTier2;
            ChannelUnitNumber = nChannelUnitNumber;
        }

        public ChannelUnit(long nID, long nChannelUnitNumber, String sName, String sTier1, String sTier2,
             Nullable<long> nBedrock, Nullable<long> nBouldersGT256, Nullable<long> nCobbles65255
            , Nullable<long> nCoarseGravel1764, Nullable<long> nFineGravel316, Nullable<long> nSand0062
            , Nullable<long> nFinesLT006, Nullable<long> nSumSubstrateCover, Nullable<long> nLargeWoodCount)
            : base(nID, sName)
        {
            Tier1 = sTier1;
            Tier2 = sTier2;
            ChannelUnitNumber = nChannelUnitNumber;

            Bedrock = nBedrock;
            BouldersGT256 = nBouldersGT256;
            Cobbles65255 = nCobbles65255;
            CoarseGravel1764 = nCoarseGravel1764;
            FineGravel316 = nFineGravel316;
            Sand0062 = nSand0062;
            FinesLT006 = nFinesLT006;
            SumSubstrateCover = nSumSubstrateCover;
            LargeWoodCount = nLargeWoodCount;
        }

        public static Dictionary<long, ChannelUnit> Load(string sDBCon, long nSegmentID)
        {
            Dictionary<long, ChannelUnit> Segments = new Dictionary<long, ChannelUnit>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM CHaMP_ChannelUnits WHERE SegmentID = @SegmentID ORDER BY ChannelUnitNumber", dbCon);
                dbCom.Parameters.AddWithValue("SegmentID", nSegmentID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nID = dbRead.GetInt64(dbRead.GetOrdinal("ID"));
                    long nCU = dbRead.GetInt64(dbRead.GetOrdinal("ChannelUnitNumber"));
                    Segments[nID] = new ChannelUnit(nID
                        , nCU
                        , nCU.ToString()
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Tier1")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Tier2")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Bedrock")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "BouldersGT256")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Cobbles65255")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "CoarseGravel1764")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "FineGravel316")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Sand0062")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "FinesLT006")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "SumSubstrateCover")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "LargeWoodCount"));
                }
            }
            return Segments;
        }
        
        public void WriteToXML(ref XmlTextWriter xFile)
        {
            xFile.WriteStartElement("unit");
            xFile.WriteElementString("id", ID.ToString());
            xFile.WriteElementString("unit_number", ChannelUnitNumber.ToString());
            xFile.WriteElementString("tier1", Tier1);
            xFile.WriteElementString("tier2", Tier2);

            if (SumSubstrateCover > 0)
            {
                xFile.WriteElementString("bedrock", Bedrock.ToString());
                xFile.WriteElementString("bouldersgt256", BouldersGT256.ToString());
                xFile.WriteElementString("cobbles65255", Cobbles65255.ToString());
                xFile.WriteElementString("coarsegravel1764", CoarseGravel1764.ToString());
                xFile.WriteElementString("finegravel316", FineGravel316.ToString());
                xFile.WriteElementString("sand0062", Sand0062.ToString());
                xFile.WriteElementString("fineslt006", FinesLT006.ToString());
                xFile.WriteElementString("sumsubstratecolver", SumSubstrateCover.ToString());
            }
            else
            {
                xFile.WriteElementString("bedrock", "");
                xFile.WriteElementString("bouldersgt256", "");
                xFile.WriteElementString("cobbles65255", "");
                xFile.WriteElementString("coarsegravel1764", "");
                xFile.WriteElementString("finegravel316", "");
                xFile.WriteElementString("sand0062", "");
                xFile.WriteElementString("fineslt006", "");
                xFile.WriteElementString("sumsubstratecolver", "");
            }

            xFile.WriteEndElement(); // unit
        }
    }
}
