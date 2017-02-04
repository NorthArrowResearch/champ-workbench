using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SQLite;
using naru.db.sqlite;

namespace CHaMPWorkbench.CHaMPData
{
    class Site : SiteBasic
    {
        public string StreamName { get; internal set; }
        public bool UC_Chin { get; internal set; }
        public bool LC_Steel { get; internal set; }
        public bool MC_Steel { get; internal set; }
        public bool UC_Steel { get; internal set; }
        public bool SN_Steel { get; internal set; }
        public Nullable<double> Latitude { get; internal set; }
        public Nullable<double> Longitude { get; internal set; }
        public Nullable<long> GageID { get; internal set; }
        public Dictionary<long, Visit> Visits { get; internal set; }

        public Site(long nSiteID, String sSiteName, long nWatershedID, string sWatershedName, string sStreamName, String sUTMZone,
            bool bUC_Chin, bool bSN_Chin, bool bLC_Steel, bool bMC_Steel, bool bUC_Steel, bool bSN_Steel,
            Nullable<double> fLatitude, Nullable<double> fLongitude, Nullable<long> nGageID)
                : base(nSiteID, sSiteName, nWatershedID, sWatershedName, sUTMZone)
        {
            StreamName = sStreamName;
            UC_Chin = bUC_Chin;
            LC_Steel = bLC_Steel;
            MC_Steel = bMC_Steel;
            UC_Steel = bUC_Steel;
            SN_Steel = bSN_Steel;
            Latitude = fLatitude;
            Longitude = fLongitude;

            Visits = Visit.Load(DBCon.ConnectionString);
        }

        public static Dictionary<long , Site> Load(string sDBCon)
        {
            Dictionary<long, Site> dResult = new Dictionary<long, Site>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM CHaMP_Sites ORDER BY WatershedID, SiteName", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nSiteID = dbRead.GetInt64(dbRead.GetOrdinal("SiteID"));
                    dResult[nSiteID] = new Site(nSiteID
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "SiteName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueInt(ref dbRead, "WatershedID")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "WatershedName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "StreamName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "UTMZone")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "UC_Chin")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "SN_Chin")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "LC_Steel")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "MC_Steel")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "UC_Steel")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "SN_Steel")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "Latitude")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "Longitude")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "GageID"));
                }
            }

            return dResult;
        }

        public string NameForDatabaseBatch(ref Visit targetVisit)
        {
            string sName = string.Format("{0}, {1}, VisitID {2}", targetVisit.VisitYear, Watershed, ID);

            if (!string.IsNullOrWhiteSpace(targetVisit.HitchName))
                sName += targetVisit.HitchName + ", ";

            if (!string.IsNullOrWhiteSpace(targetVisit.CrewName))
                sName += targetVisit.CrewName + ", ";

            if (targetVisit.IsPrimary)
            {
                sName += " Primary";
                sName += ", ";
            }
            sName += Visits.Count.ToString() + " visits";
            return sName;
        }
    }
}
