using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes
{
    class Visit : NamedDBObject
    {
        private int m_nVisitID;

        private String m_sHitch;
        private String m_sCrew;
        private int m_nFieldSeason;
        private DateTime m_dSampleDate;
        private Dictionary<int, ChannelSegment> m_dChannelSegments;
        private bool m_bPrimary;
        private bool m_bCalculateMetrics;
        private bool m_bChangeDetection;
        private bool m_bMakeDEMsOrthogonal;
        private bool m_bGenerateCSVs;

        private String m_sFileGDB;
        private String m_sTopoTIN;
        private String m_sWSTIN;

        public int FieldSeason
        {
            get
            {
                return m_nFieldSeason;
            }
        }

        public Boolean Primary
        {
            get { return m_bPrimary; }
        }

        public Boolean CalculateMetrics
        {
            get { return m_bCalculateMetrics; }
            set { m_bCalculateMetrics = value; }
        }

        public Boolean ChangeDetection
        {
            get { return m_bChangeDetection; }
            set { m_bChangeDetection = value; }
        }

        public Boolean MakeDEMsOrthogonal
        {
            get { return m_bMakeDEMsOrthogonal; }
            set { m_bMakeDEMsOrthogonal = value; }
        }

        public Boolean GenerateCSVs
        {
            get { return m_bGenerateCSVs; }
            set { m_bGenerateCSVs = value; }
        }

        public override string ToString()
        {
            return FieldSeason.ToString() + " - " + m_sHitch + (m_bPrimary ? " - Primary" : "");
        }

        public string Hitch
        {
            get { return m_sHitch; }
        }

        public string Crew
        {
            get { return m_sCrew; }
        }

        public int VisitID
        {
            get { return m_nVisitID; }
            set { m_nVisitID = value; }
        }

        public string SurveyGDB { get { return m_sFileGDB; } }

        public Visit(int nID, String sHitch, String sCrew, int nFieldSeason, String sFileGDB, String sTopoTIN, String sWSTIN, DateTime dSampleDate, bool bTarget, bool bPrimary)
            : base(nID, sHitch)
        {
            m_nVisitID = nID;
            m_sHitch = sHitch;
            m_sCrew = sCrew;
            m_dChannelSegments = new Dictionary<int, ChannelSegment>();
            m_sFileGDB = sFileGDB;
            m_sTopoTIN = sTopoTIN;
            m_sWSTIN = sWSTIN;
            m_nFieldSeason = nFieldSeason;
            m_dSampleDate = dSampleDate;

            m_bPrimary = bPrimary;
            m_bCalculateMetrics = bTarget;
            m_bMakeDEMsOrthogonal = false;
            m_bChangeDetection = bTarget || bPrimary;
            m_bGenerateCSVs = bTarget;
        }

        public Visit(RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit, bool bCalculateMetrics, bool bChangeDetection, bool bDEMOrthogonal, bool bGenerateCSVs, bool bForcePrimary)
            : base(rVisit.VisitID, rVisit.HitchName)
        {
            if (!rVisit.IsHitchNameNull())
                m_sHitch = rVisit.HitchName;

            if (!rVisit.IsCrewNameNull())
                m_sCrew = rVisit.CrewName;

            if (!rVisit.IsSurveyGDBNull())
                m_sFileGDB = rVisit.SurveyGDB;

            if (!rVisit.IsTopoTINNull())
                m_sTopoTIN = rVisit.TopoTIN;

            if (!rVisit.IsWSTINNull())
                m_sWSTIN = rVisit.WSTIN;

            if (!rVisit.IsIsPrimaryNull())
                m_bPrimary = rVisit.IsPrimary || bForcePrimary;

            if (!rVisit.IsSampleDateNull())
                m_dSampleDate = rVisit.SampleDate;
            else
                m_dSampleDate = new DateTime(rVisit.VisitYear, 1, 1);

            m_nVisitID = rVisit.VisitID;
            m_nFieldSeason = rVisit.VisitYear;
            m_bCalculateMetrics = bCalculateMetrics;
            m_bMakeDEMsOrthogonal = bDEMOrthogonal;
            m_bChangeDetection = bChangeDetection;
            m_dChannelSegments = new Dictionary<int, ChannelSegment>();
            m_bGenerateCSVs = bGenerateCSVs;

            foreach (RBTWorkbenchDataSet.CHaMP_SegmentsRow rSegment in rVisit.GetCHaMP_SegmentsRows())
            {
                m_dChannelSegments.Add(rSegment.SegmentID, new ChannelSegment(rSegment));
            }
        }

        public void WriteToXML(ref XmlTextWriter xmlFile, Boolean bRequireWSTIN)
        {
            if (string.IsNullOrWhiteSpace(m_sWSTIN) && bRequireWSTIN)
                return;

            xmlFile.WriteStartElement("visit");
            xmlFile.WriteAttributeString("calculatemetrics", m_bCalculateMetrics.ToString());
            xmlFile.WriteAttributeString("changedetection", m_bChangeDetection.ToString());
            xmlFile.WriteAttributeString("makedemorthogonal", m_bMakeDEMsOrthogonal.ToString());
            xmlFile.WriteAttributeString("primary", m_bPrimary.ToString());
            xmlFile.WriteAttributeString("generatecsv", m_bGenerateCSVs.ToString());
            
            xmlFile.WriteElementString("visitid", m_nVisitID.ToString());
            xmlFile.WriteElementString("name", base.ToString());
            xmlFile.WriteElementString("fieldseason", FieldSeason.ToString());
            xmlFile.WriteElementString("sample_date", m_dSampleDate.ToString());
            xmlFile.WriteElementString("filegdb", m_sFileGDB);

            xmlFile.WriteElementString("dem", "DEM");
            xmlFile.WriteElementString("topo_tin",m_sTopoTIN);

            if (string.IsNullOrEmpty(m_sWSTIN))
                xmlFile.WriteElementString("ws_tin", "");
            else
                xmlFile.WriteElementString("ws_tin", m_sWSTIN);

            xmlFile.WriteElementString("topo_points", "Topo_Points");
            xmlFile.WriteElementString("control_points", "Control_Points");
            xmlFile.WriteElementString("thalweg", "Thalweg");
            xmlFile.WriteElementString("orthoginfo", "OrthogInfo");
            xmlFile.WriteElementString("wetted_extent_points", "EdgeOfWater_Points");
            xmlFile.WriteElementString("wetted_extent", "WaterExtent");
            xmlFile.WriteElementString("wetted_centerline", "CenterLine");
            xmlFile.WriteElementString("wetted_cross_sections", "WettedXS");
            xmlFile.WriteElementString("bankfull_extent", "Bankfull");
            xmlFile.WriteElementString("bankfull_centerline", "BankfullCL");
            xmlFile.WriteElementString("bankfull_cross_sections", "BankfullXS");
            xmlFile.WriteElementString("detrended", "Detrended");
            xmlFile.WriteElementString("survey_extent", "Survey_Extent");
            xmlFile.WriteElementString("water_depth", "Water_Depth");
            xmlFile.WriteElementString("geomorphic_units", "GeomorphicUnits");
            xmlFile.WriteElementString("wsdem", "WSEDEM");
            xmlFile.WriteElementString("wetted_islands", "WIslands");
            xmlFile.WriteElementString("bankfull_islands", "BIslands");
            xmlFile.WriteElementString("qaqc_points", "QaQc_RawPoints");
            xmlFile.WriteElementString("channel_units", "Channel_Units");

            xmlFile.WriteElementString("emap_wetted_cross_sections", "WetCross_EMap");
            xmlFile.WriteElementString("emap_bankfull_cross_sections", "BankCross_EMap");

            xmlFile.WriteElementString("error_surface", "ErrSurface");
            xmlFile.WriteElementString("slope_raster", "AssocSlope");
            xmlFile.WriteElementString("pdensity_raster", "AssocPDensity");
            xmlFile.WriteElementString("pointQuality_raster", "Assoc3DPQ");
            xmlFile.WriteElementString("roughness_raster", "AssocD50");
            xmlFile.WriteElementString("interperror_raster", "AssocIErr");

            xmlFile.WriteStartElement("channel_segments");
            foreach (ChannelSegment sg in m_dChannelSegments.Values)
                sg.WriteToXML(ref xmlFile);

            // channel segments (units)
            xmlFile.WriteEndElement();

            // visit
            xmlFile.WriteEndElement();
        }
    }
}
