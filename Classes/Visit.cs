using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes
{
    public class Visit : naru.db.NamedObject
    {
        public long SiteID { get; internal set; }
        public Nullable<long> ProgramSiteID { get; internal set; }
        public long VisitYear { get; internal set; }
        public Nullable<long> HitchID { get; internal set; }
        public string HitchName { get { return base.ToString(); } }
        public string Organization { get; internal set; }
        public string CrewName { get; internal set; }
        public DateTime SampleDate { get; internal set; }
        public Nullable<long> ProtocolID { get; internal set; }
        public bool IsPrimary { get; internal set; }
        public bool QCVisit { get; internal set; }
        public string PanelName { get; internal set; }
        public string CategoryName { get; internal set; }
        public string VisitPhase { get; internal set; }
        public string VisitStatus { get; internal set; }
        public bool AEM { get; internal set; }
        public bool HasStreamTempLogger { get; internal set; }
        public bool HasFishData { get; internal set; }
        public Nullable<double> Discharge { get; internal set; }
        public Nullable<double> D84 { get; internal set; }
        public string Remarks { get; internal set; }

        public Dictionary<long, ChannelSegment> Segments { get; internal set; }

        public Visit(long nID, long nSiteID, Nullable<long> nProgramSiteID, long nVisitYear, Nullable<long> nHitchID, string sHitchName,
            string sOrganization, string sCrewName, DateTime dtSampleDate, Nullable<long> nProtocolID, bool bIsPrimary, bool bQCVisit,
            string sPanelName, string sCategoryName, string sVisitPhase, string sVisitStatus, bool bAEM, bool bHSTL, bool bHasFishData,
           Nullable<double> fDischarge, Nullable<double> fD84, string sRemarks) : base(nID, sHitchName)
        {
            SiteID = nSiteID;
            ProgramSiteID = nProgramSiteID;
            VisitYear = nVisitYear;
            HitchID = nHitchID;
            Organization = sOrganization;
            CrewName = sCrewName;
            SampleDate = dtSampleDate;
            ProtocolID = nProtocolID;
            IsPrimary = bIsPrimary;
            QCVisit = bQCVisit;
            PanelName = sPanelName;
            CategoryName = sCategoryName;
            VisitPhase = sVisitPhase;
            VisitStatus = sVisitStatus;
            AEM = bAEM;
            HasStreamTempLogger = bHSTL;
            HasFishData = bHasFishData;
            Discharge = fDischarge;
            D84 = fD84;
            Remarks = sRemarks;

            Segments = ChannelSegment.Load(DBCon.ConnectionString, nID);
        }

        public static naru.ui.SortableBindingList<Visit> Load(string sDBCon)
        {
            naru.ui.SortableBindingList<Visit> lVisits = new naru.ui.SortableBindingList<Visit>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM CHaMP_Visits ORDER BY VisitID", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    lVisits.Add(new Visit(
                        dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("SiteID"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "ProgramSiteID")
                        , dbRead.GetInt64(dbRead.GetOrdinal("VisitYear"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "HitchID")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "HitchName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Organization")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "CrewName")
                        , dbRead.GetDateTime(dbRead.GetOrdinal("SampleDate"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "ProtocolID")
                        , dbRead.GetBoolean(dbRead.GetOrdinal("IsPrimary"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("QCVisit"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "PanelName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "CategoryName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "VisitPhase")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "VisitStatus")
                        , dbRead.GetBoolean(dbRead.GetOrdinal("AEM"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("HasStreamTempLogger"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("HasFishData"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "Discharge")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "D84")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Remarks")));

                    dbRead.GetInt64(dbRead.GetOrdinal(""));
                }
            }

            return lVisits;
        }
    }
}
