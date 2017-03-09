using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class VisitBasic : naru.db.EditableNamedObject
    {
        public SiteBasic Site { get; internal set; }
        public long VisitYear { get; internal set; }
        public long ProgramID { get; internal set; }

        public VisitBasic(long nVisitID, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitYear, string sUTMZone, long nProgramID, naru.db.DBState eState)
            : base(nVisitID, string.Format("VisitID {0}, {1}, {2}, {3}", nVisitID, sWatershedName, sSiteName, nVisitYear), eState)
        {
            Site = new SiteBasic(nSiteID, sSiteName, nWatershedID, sWatershedName, sUTMZone, naru.db.DBState.Unchanged);
            VisitYear = nVisitYear;
            ProgramID = nProgramID;
        }

        public VisitBasic(VisitBasic aVisit, naru.db.DBState eState)
            : base(aVisit.ID, string.Format("VisitID {0}, {1}, {2}, {3}", aVisit.ID, aVisit.Site.Watershed.Name, aVisit.Site.Name, aVisit.VisitYear), eState)
        {
            Site = new SiteBasic(aVisit.Site, naru.db.DBState.Unchanged);
            VisitYear = aVisit.VisitYear;
            ProgramID = aVisit.ProgramID;
        }

        public string VisitFolderAbsolute(string sParentFolder)
        {
            return System.IO.Path.Combine(sParentFolder, VisitFolderRelative);
        }

        public string VisitFolderRelative
        {
            get
            {
                string sPath =  System.IO.Path.Combine(VisitYear.ToString(), Site.Watershed.Name, Site.Name, string.Format("VISIT_{0}", ID));
                sPath = sPath.Replace(" ", "");
                return sPath;
            }
        }

        public static VisitBasic Load(long nVisitID)
        {
            VisitBasic aVisit = null;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("select VisitID, SiteID, SiteName, WatershedID, WatershedName, VisitYear, UTMZone, ProgramID from vwVisits where visitID = @VisitID", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                dbRead.Read();

                aVisit = new VisitBasic(nVisitID
                    , dbRead.GetInt64(dbRead.GetOrdinal("SiteID"))
                    , dbRead.GetString(dbRead.GetOrdinal("SiteName"))
                    , dbRead.GetInt64(dbRead.GetOrdinal("WatershedID"))
                    , dbRead.GetString(dbRead.GetOrdinal("WatershedName"))
                    , dbRead.GetInt64(dbRead.GetOrdinal("VisitYear"))
                    , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "UTMZone")
                    , dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"))
                    , naru.db.DBState.Unchanged);
            }

            return aVisit;
        }
    }
}

