using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class Visit : NamedDBObject
    {
        private String m_sHitch;
        private String m_sCrew;
        private int m_nFieldSeason;
        private Dictionary<int, ChannelSegment> m_dChannelSegments;
        private bool m_bPrimary;
        private bool m_bCalculateMetrics;
        private bool m_bChangeDetection;
        private bool m_bMakeDEMsOrthogonal;
        private bool m_bGenerateCSVs;

        private String m_sFolder;
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

        public String Folder
        {
            get
            {
                return m_sFolder; // System.IO.Path.Combine(m_sHitch, "topo");
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

        public Visit(int nID, String sFolder, String sHitch, String sCrew, int nFieldSeason, String sFileGDB, String sTopoTIN, String sWSTIN, bool bPrimary)
            : base(nID, sHitch)
        {
            m_sHitch = sHitch;
            m_sCrew = sCrew;
            m_dChannelSegments = new Dictionary<int, ChannelSegment>();
            m_sFileGDB = sFileGDB;
            m_sTopoTIN = sTopoTIN;
            m_sWSTIN = sWSTIN;
            m_sFolder = sFolder;
            m_nFieldSeason = nFieldSeason;

            m_bPrimary = bPrimary;
            m_bCalculateMetrics = false;
            m_bMakeDEMsOrthogonal = false;
            m_bChangeDetection = false;
            m_bGenerateCSVs = false;
        }

        public Visit(RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit, bool bCalculateMetrics, bool bChangeDetection, bool bDEMOrthogonal, bool bGenerateCSVs)
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

            if (!rVisit.IsFolderNull())
                m_sFolder = rVisit.Folder;

            m_nFieldSeason = rVisit.VisitYear;
            m_bCalculateMetrics = bCalculateMetrics;
            m_bMakeDEMsOrthogonal = bDEMOrthogonal;
            m_bChangeDetection = bChangeDetection;
            m_dChannelSegments = new Dictionary<int, ChannelSegment>();
        
            foreach (RBTWorkbenchDataSet.CHaMP_SegmentsRow rSegment in rVisit.GetCHaMP_SegmentsRows())
            {
                m_dChannelSegments.Add(rSegment.SegmentID, new ChannelSegment(rSegment));
            }
        }

        public void WriteToXML(ref XmlTextWriter xmlFile, String sSourceFolder)
        {
            if (String.IsNullOrWhiteSpace(m_sFileGDB) || string.IsNullOrWhiteSpace(m_sTopoTIN) || string.IsNullOrWhiteSpace(m_sWSTIN) || string.IsNullOrWhiteSpace(m_sFolder))
                return;
            
            xmlFile.WriteStartElement("visit");
            xmlFile.WriteAttributeString("calculatemetrics", m_bCalculateMetrics.ToString());
            xmlFile.WriteAttributeString("changedetection", m_bChangeDetection.ToString());
            xmlFile.WriteAttributeString("makedemorthogonal", m_bMakeDEMsOrthogonal.ToString());
            xmlFile.WriteAttributeString("primary", m_bPrimary.ToString());
            xmlFile.WriteAttributeString("generatecsv", m_bGenerateCSVs.ToString());

            xmlFile.WriteElementString("name", base.ToString());
            xmlFile.WriteElementString("fieldseason", FieldSeason.ToString());
            xmlFile.WriteElementString("filegdb", System.IO.Path.Combine(sSourceFolder, m_sFileGDB));

            xmlFile.WriteElementString("dem", "DEM");
            xmlFile.WriteElementString("error_surface", "ElevationError");

            xmlFile.WriteElementString("topo_tin", System.IO.Path.Combine(sSourceFolder, m_sTopoTIN));
            xmlFile.WriteElementString("ws_tin", System.IO.Path.Combine(sSourceFolder, m_sWSTIN));

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

            xmlFile.WriteElementString("wetted_islands", "WIslands");
            xmlFile.WriteElementString("bankfull_islands", "BIslands");

            xmlFile.WriteElementString("qaqc_points", "QaQc_RawPoints");

            string sChannelUnitFeatureClassName = "Channel_Units";
            if (FieldSeason == 2011)
            {
                sChannelUnitFeatureClassName = "Habitat_Units";
            }
            xmlFile.WriteElementString("channel_units", sChannelUnitFeatureClassName);

            xmlFile.WriteElementString("emap_wetted_cross_sections", "WetCross_EMap");
            xmlFile.WriteElementString("emap_bankfull_cross_sections", "BankCross_EMap");

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
