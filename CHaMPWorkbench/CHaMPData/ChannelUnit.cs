using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Data.SQLite;
using naru.xml;
using naru.db;

namespace CHaMPWorkbench.CHaMPData
{
    public class ChannelUnit : naru.db.EditableNamedObject
    {
        public long VisitID { get; internal set; }
        private String m_sTier1;
        private String m_sTier2;
        private long m_nChannelUnitNumber;
        private long m_nSegmentNumber;

        private Nullable<long> m_nBedrock;
        private Nullable<long> m_nBouldersGT256;
        private Nullable<long> m_nCobbles65255;
        private Nullable<long> m_nCoarseGravel1764;
        private Nullable<long> m_nFineGravel316;
        private Nullable<long> m_nSand0062;
        private Nullable<long> m_nFinesLT006;
        private Nullable<long> m_nSumSubstrateCover;
        public long LargeWoodCount { get; internal set; }

        #region Properties

        public long ChannelUnitNumber
        {
            get { return m_nChannelUnitNumber; }
            set
            {
                if (m_nChannelUnitNumber != value)
                {
                    m_nChannelUnitNumber = value;
                    State = DBState.Edited;
                }
            }
        }

        public long SegmentNumber
        {
            get { return m_nSegmentNumber; }
            set
            {
                if (m_nSegmentNumber != value)
                {
                    m_nSegmentNumber = value;
                    State = DBState.Edited;
                }
            }
        }

        public string Tier1
        {
            get { return m_sTier1; }
            set
            {
                if (string.Compare(m_sTier1, value, false) != 0)
                {
                    m_sTier1 = value;
                    State = DBState.Edited;
                }
            }
        }


        public string Tier2
        {
            get { return m_sTier2; }
            set
            {
                if (string.Compare(m_sTier2, value, false) != 0)
                {
                    m_sTier2 = value;
                    State = DBState.Edited;
                }
            }
        }


        public Nullable<long> Bedrock
        {
            get { return m_nBedrock; }
            set
            {
                if (m_nBedrock != value)
                {
                    m_nBedrock = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> BouldersGT256
        {
            get { return m_nBouldersGT256; }
            set
            {
                if (m_nBouldersGT256 != value)
                {
                    m_nBouldersGT256 = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> Cobbles65255
        {
            get { return m_nCobbles65255; }
            set
            {
                if (m_nCobbles65255 != value)
                {
                    m_nCobbles65255 = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> CoarseGravel1764
        {
            get { return m_nCoarseGravel1764; }
            set
            {
                if (m_nCoarseGravel1764 != value)
                {
                    m_nCoarseGravel1764 = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> FineGravel316
        {
            get { return m_nFineGravel316; }
            set
            {
                if (m_nFineGravel316 != value)
                {
                    m_nFineGravel316 = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> Sand0062
        {
            get { return m_nSand0062; }
            set
            {
                if (m_nSand0062 != value)
                {
                    m_nSand0062 = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> FinesLT006
        {
            get { return m_nFinesLT006; }
            set
            {
                if (m_nFinesLT006 != value)
                {
                    m_nFinesLT006 = value;
                    State = DBState.Edited;
                }
            }
        }

        public Nullable<long> SumSubstrateCover
        {
            get { return m_nSumSubstrateCover; }
            set
            {
                if (m_nSumSubstrateCover != value)
                {
                    m_nSumSubstrateCover = value;
                    State = DBState.Edited;
                }
            }
        }

        #endregion

        public ChannelUnit(long nID, long nVisitID, long nChannelUnitNumber, long nSegmentNumber, String sTier1, String sTier2, naru.db.DBState eState)
            : base(nID, string.Format("{0} - {1}//{2}", nChannelUnitNumber, sTier1, sTier2), eState)
        {
            Init(nVisitID, nChannelUnitNumber, nSegmentNumber, sTier1, sTier2);
        }


        public ChannelUnit(long nID, long nVisitID, long nChannelUnitNumber, long nSegmentNumber, String sTier1, String sTier2,
             Nullable<long> nBedrock, Nullable<long> nBouldersGT256, Nullable<long> nCobbles65255
            , Nullable<long> nCoarseGravel1764, Nullable<long> nFineGravel316, Nullable<long> nSand0062
            , Nullable<long> nFinesLT006, Nullable<long> nSumSubstrateCover, long nLargeWoodCount, naru.db.DBState eState)
            : base(nID, string.Format("{0} - {1}//{2}", nChannelUnitNumber, sTier1, sTier2), eState)
        {
            Init(nVisitID, nChannelUnitNumber, nSegmentNumber, sTier1, sTier2);

            m_nBedrock = nBedrock;
            m_nBouldersGT256 = nBouldersGT256;
            m_nCobbles65255 = nCobbles65255;
            m_nCoarseGravel1764 = nCoarseGravel1764;
            m_nFineGravel316 = nFineGravel316;
            m_nSand0062 = nSand0062;
            m_nFinesLT006 = nFinesLT006;
            m_nSumSubstrateCover = nSumSubstrateCover;
            LargeWoodCount = nLargeWoodCount;
        }

        private void Init(long nVisitID, long nChannelUnitNumber, long nSegmentNumber, string sTier1, string sTier2)
        {
            VisitID = nVisitID;
            m_nChannelUnitNumber = nChannelUnitNumber;
            m_nSegmentNumber = nSegmentNumber;
            m_sTier1 = sTier1;
            m_sTier2 = sTier2;
        }

        public static Dictionary<long, ChannelUnit> Load(string sDBCon, long nVisitID)
        {
            Dictionary<long, ChannelUnit> ChannelUnits = new Dictionary<long, ChannelUnit>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM CHaMP_ChannelUnits WHERE VisitID = @VisitID ORDER BY ChannelUnitNumber", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nID = dbRead.GetInt64(dbRead.GetOrdinal("ID"));
                    long nCU = dbRead.GetInt64(dbRead.GetOrdinal("ChannelUnitNumber"));
                    ChannelUnits[nCU] = new ChannelUnit(nID
                        , nVisitID
                        , nCU
                        , dbRead.GetInt64(dbRead.GetOrdinal("SegmentNumber"))
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
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueInt(ref dbRead, "LargeWoodCount")
                        , DBState.Unchanged);
                }
            }
            return ChannelUnits;
        }

        public static void Save(ref SQLiteTransaction dbTrans, IEnumerable<ChannelUnit> lChannelUnits, List<long> lDeletedIDs = null)
        {
            string[] sFields = { "VisitID", "SegmentNumber", "ChannelUnitNumber", "Tier1", "Tier2", "BouldersGT256", "Cobbles65255", "CoarseGravel1764", "FineGravel316", "Sand0062", "FinesLT006", "SumSubstrateCover", "Bedrock", "LargeWoodCount" };

            // Note that this insert query is slightly unique and doesn't include the in-memory ID.
            SQLiteCommand comInsert = new SQLiteCommand(string.Format("INSERT INTO CHaMP_ChannelUnits ({0}) VALUES (@{1})", string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);
            comInsert.Parameters.Add("ID", System.Data.DbType.Int64);

            SQLiteCommand comUpdate = new SQLiteCommand(string.Format("UPDATE CHaMP_ChannelUnits SET {0} WHERE (VisitID = @VisitID) AND (ChannelUnitNumber = @ChannelUnitNumber)", string.Join(", ", sFields.Select(x => x + " = @" + x))), dbTrans.Connection, dbTrans);
            comUpdate.Parameters.Add("ID", System.Data.DbType.Int64);

            foreach (ChannelUnit aChannelUnit in lChannelUnits.Where<ChannelUnit>(x => x.State != naru.db.DBState.Unchanged))
            {
                SQLiteCommand dbCom = null;
                if (aChannelUnit.State == naru.db.DBState.New)
                {
                    dbCom = comInsert;
                    if (aChannelUnit.ID > 0)
                        dbCom.Parameters["ID"].Value = aChannelUnit.ID;
                }
                else
                {
                    dbCom = comUpdate;
                    //dbCom.Parameters["VisitID"].Value = aChannelUnit.VisitID;
                    //dbCom.Parameters["ChannelUnitNumber"].Value 
                }

                AddParameter(ref dbCom, "VisitID", System.Data.DbType.Int64, aChannelUnit.VisitID);
                AddParameter(ref dbCom, "SegmentNumber", System.Data.DbType.Int64, aChannelUnit.SegmentNumber);
                AddParameter(ref dbCom, "ChannelUnitNumber", System.Data.DbType.Int64, aChannelUnit.ChannelUnitNumber);
                AddParameter(ref dbCom, "Tier1", System.Data.DbType.String, aChannelUnit.Tier1);
                AddParameter(ref dbCom, "Tier2", System.Data.DbType.String, aChannelUnit.Tier2);
                AddParameter(ref dbCom, "BouldersGT256", System.Data.DbType.Int64, aChannelUnit.BouldersGT256);
                AddParameter(ref dbCom, "Cobbles65255", System.Data.DbType.Int64, aChannelUnit.Cobbles65255);
                AddParameter(ref dbCom, "CoarseGravel1764", System.Data.DbType.Int64, aChannelUnit.CoarseGravel1764);
                AddParameter(ref dbCom, "FineGravel316", System.Data.DbType.Int64, aChannelUnit.FineGravel316);
                AddParameter(ref dbCom, "Sand0062", System.Data.DbType.Int64, aChannelUnit.Sand0062);
                AddParameter(ref dbCom, "FinesLT006", System.Data.DbType.Int64, aChannelUnit.FinesLT006);
                AddParameter(ref dbCom, "SumSubstrateCover", System.Data.DbType.Int64, aChannelUnit.SumSubstrateCover);
                AddParameter(ref dbCom, "Bedrock", System.Data.DbType.Int64, aChannelUnit.Bedrock);
                AddParameter(ref dbCom, "LargeWoodCount", System.Data.DbType.Int64, aChannelUnit.LargeWoodCount);

                try
                {
                    dbCom.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write("stop");
                }

                if (aChannelUnit.State == naru.db.DBState.New && aChannelUnit.ID < 1)
                {
                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    aChannelUnit.ID = (long)dbCom.ExecuteScalar();
                }
            }
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
