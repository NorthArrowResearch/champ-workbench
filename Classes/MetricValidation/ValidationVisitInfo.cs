using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class ValidationVisitInfo
    {
        public int VisitID { get; internal set; }
        public int VisitYear { get; internal set; }
        public string Site { get; internal set; }
        public string Watershed { get; internal set; }
        public int WatershedID { get; internal set; }
        public string Organization { get; internal set; }
        public string CrewName { get; internal set; }

        public ValidationVisitInfo(int nVisitID, int nVisitYear, string sSite, string sWatershed, int nWatershedID, string sOrganization, string sCrewName)
        {
            VisitID = nVisitID;
            VisitYear = nVisitYear;
            Site = sSite;
            Watershed = sWatershed;
            WatershedID = nWatershedID;
            Organization = sOrganization;
            CrewName = sCrewName;
        }
    }
}
