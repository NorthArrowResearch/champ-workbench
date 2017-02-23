
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
    }
}

