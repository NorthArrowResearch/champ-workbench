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
            Nullable<double> fLatitude, Nullable<double> fLongitude, Nullable<long> nGageID, naru.db.DBState eState)
                : base(nSiteID, sSiteName, nWatershedID, sWatershedName, sUTMZone, eState)
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

        public static Dictionary<long, Site> Load(string sDBCon)
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
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "GageID")
                        , naru.db.DBState.Unchanged);
                }
            }

            return dResult;
        }

        public string NameForDatabaseBatch(ref Visit targetVisit)
        {
            string sName = string.Format("{0}, {1}, VisitID {2}", targetVisit.VisitYear, Watershed, ID);

            if (!string.IsNullOrWhiteSpace(targetVisit.Hitch))
                sName += targetVisit.Hitch + ", ";

            if (!string.IsNullOrWhiteSpace(targetVisit.Crew))
                sName += targetVisit.Crew + ", ";

            if (targetVisit.IsPrimary)
            {
                sName += " Primary";
                sName += ", ";
            }
            sName += Visits.Count.ToString() + " visits";
            return sName;
        }

        public static void Save(ref SQLiteTransaction dbTrans, List<Site> lSites, List<long> lDeletedIDs = null)
        {
            string[] sFields = { "SiteName", "WatershedID", "StreamName", "UTMZone", "Latitude", "Longitude" };
            SQLiteCommand comInsert = new SQLiteCommand(string.Format("INSERT INTO CHaMP_Sites (SiteID, {0}) VALUES (@ID, @{1})", string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);
            comInsert.Parameters.Add("ID", System.Data.DbType.Int64);

            SQLiteCommand comUpdate = new SQLiteCommand(string.Format("UPDATE CHaMP_Sites SET {0} WHERE SiteID = @ID", string.Join(", ", sFields.Select(x => x + " = @" + x))), dbTrans.Connection, dbTrans);
            comUpdate.Parameters.Add("ID", System.Data.DbType.Int64);
            foreach (Site aSite in lSites.Where<Site>(x => x.State != naru.db.DBState.Unchanged))
            {
                SQLiteCommand dbCom = null;
                if (aSite.State == naru.db.DBState.New)
                {
                    dbCom = comInsert;
                    if (aSite.ID > 0)
                        dbCom.Parameters["ID"].Value = aSite.ID;
                }
                else
                {
                    dbCom = comUpdate;
                    dbCom.Parameters["ID"].Value = aSite.ID;
                }

                AddParameter(ref dbCom, "SiteName", System.Data.DbType.String, aSite.Name);
                AddParameter(ref dbCom, "WatershedID", System.Data.DbType.Int64, aSite.Watershed.ID);
                AddParameter(ref dbCom, "StreamName", System.Data.DbType.String, aSite.StreamName);
                AddParameter(ref dbCom, "UTMZone", System.Data.DbType.String, aSite.UTMZone);
                AddParameter(ref dbCom, "Latitude", System.Data.DbType.String, aSite.Latitude);
                AddParameter(ref dbCom, "Longitude", System.Data.DbType.String, aSite.Longitude);

                dbCom.ExecuteNonQuery();

                if (aSite.State == naru.db.DBState.New && aSite.ID < 1)
                {
                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    aSite.ID = (long)dbCom.ExecuteScalar();
                }
            }

            //if (lDeletedIDs is List<long>)
            //{
            //    SQLiteCommand comDelete = new SQLiteCommand("DELETE FROM CHaMP_Watersheds WHERE WatershedID = @ID", dbTrans.Connection, dbTrans);
            //    SQLiteParameter pDelete = comDelete.Parameters.Add("ID", System.Data.DbType.Int64);
            //    foreach (long nID in lDeletedIDs)
            //    {
            //        pDelete.Value = nID;
            //        comDelete.ExecuteNonQuery();
            //    }
            //}
        }
    }
}
