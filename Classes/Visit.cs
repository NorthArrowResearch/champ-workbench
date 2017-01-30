using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    public class Visit : naru.db.NamedObject
    {
        public long SiteID { get; internal set; }
        public long ProgramSiteID { get; internal set; }
        public long VisitYear { get; internal set; }
        public long HitchID { get; internal set; }
        public string HitchName { get { return base.ToString(); } }
        public string Organization { get; internal set; }
        public string CrewName { get; internal set; }
        public DateTime SampleDate { get; internal set; }
        public long ProtocolID { get; internal set; }
        public bool IsPrimary { get; internal set; }
        public bool QCVisit { get; internal set; }
        public string PanelName { get; internal set; }
        public string CategoryName { get; internal set; }
        public string VisitPhase { get; internal set; }
        public string VisitStatus { get; internal set; }
        public bool AEM { get; internal set; }
        public bool HasStreamTempLogger { get; internal set; }
        public bool HasFishData { get; internal set; }
        public double Discharge { get; internal set; }
        public double D84 { get; internal set; }
        public string Remarks { get; internal set; }

        public Visit(long nID, long nSiteID, long nProgramSiteID, long nVisitYear, long nHitchID, string sHitchName,
            string sOrganization, string sCrewName, DateTime dtSampleDate, long nProtocolID, bool bIsPrimary, bool bQCVisit,
            string sPanelName, string sCategoryName, string sVisitPhase, string sVisitStatus, bool bAEM, bool bHSTL, bool bHasFishData, 
            double fDischarge, double fD84, string sRemarks) : base(nID, sHitchName)
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
        }

        public static bindinglist<Visit> Load(string sDBCon)
        {

                   }


    }
}
