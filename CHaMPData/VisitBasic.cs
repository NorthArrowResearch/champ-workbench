
namespace CHaMPWorkbench.CHaMPData
{
    public class VisitBasic : naru.db.NamedObject
    {
        public SiteBasic Site { get; internal set; }
        public long VisitYear { get; internal set; }
        public long ProgramID { get; internal set; }

        public VisitBasic(long nVisitID, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitYear, string sUTMZone, long nProgramID)
            : base(nVisitID, string.Format("VisitID {0}, {1}, {2}, {3}", nVisitID, sWatershedName, sSiteName, nVisitYear))
        {
            Site = new SiteBasic(nSiteID, sSiteName, nWatershedID, sWatershedName, sUTMZone);
            VisitYear = nVisitYear;
            ProgramID = nProgramID;
        }
    }
}
