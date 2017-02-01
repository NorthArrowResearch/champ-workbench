using System;
using System.Collections.Generic;
using System.Xml;
using System.Data.SQLite;
using naru.xml;

namespace CHaMPWorkbench.CHaMPData
{
    public class ChannelUnit : naru.db.NamedObject
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

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc)
        {
            XmlNode nodUnit = xmlDoc.CreateElement("unit");

            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "id", ID.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "unit_number", ChannelUnitNumber.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "tier1", Tier1);
            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "tier2", Tier2);

            AppendSedimentBin(ref xmlDoc, ref nodUnit, "bedrock", Bedrock);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "bouldersgt256", BouldersGT256);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "cobbles65255", Cobbles65255);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "coarsegravel1764", CoarseGravel1764);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "finegravel316", FineGravel316);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "sand0062", Sand0062);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "fineslt006", FinesLT006);
            AppendSedimentBin(ref xmlDoc, ref nodUnit, "sumsubstratecolver", SumSubstrateCover);

            return nodUnit;
        }

        private void AppendSedimentBin(ref XmlDocument xmlDoc, ref XmlNode nodUnit, string sNodeTag, Nullable<long> nValue)
        {
            XmlNode nodBin = xmlDoc.CreateElement(sNodeTag);
            if (nValue.HasValue)
                nodBin.InnerText = nValue.Value.ToString();
            nodUnit.AppendChild(nodBin);
        }
    }
}
