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
        
        public Visit(int nID, String sFolder, String sHitch, String sCrew, int nFieldSeason, String sFileGDB, String sTopoTIN, String sWSTIN, bool bPrimary)
            : base(nID, nFieldSeason.ToString() + " - " + sHitch + (bPrimary ? " - Primary" : ""))
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
        }

        public Visit(int nID, String sFolder, String sHitch, String sCrew, int nFieldSeason, String sFileGDB, String sTopoTIN, String sWSTIN, bool bPrimary, System.Data.OleDb.OleDbConnection dbCon)
            : this(nID, sFolder, sHitch, sCrew, nFieldSeason, sFileGDB, sTopoTIN, sWSTIN, bPrimary)
        {
            using (System.Data.OleDb.OleDbCommand dbCom = new System.Data.OleDb.OleDbCommand("SELECT SegmentID, SegmentNumber, SegmentName FROM CHAMP_Segments WHERE VisitID = " + ID.ToString(), dbCon))
            {
                System.Data.OleDb.OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    ChannelSegment sg = new ChannelSegment((int)dbRead["SegmentID"], (String)dbRead["SegmentName"], (int)dbRead["SegmentNumber"], dbCon);
                    m_dChannelSegments.Add(sg.ID, sg);
                }
            }
        }

        public void WriteToXML(ref XmlTextWriter xmlFile)
        {
            xmlFile.WriteStartElement("visit");
            xmlFile.WriteAttributeString("calculatemetrics", m_bCalculateMetrics.ToString());
            xmlFile.WriteAttributeString("changedetection", m_bChangeDetection.ToString());
            xmlFile.WriteAttributeString("makedemorthogonal", m_bMakeDEMsOrthogonal.ToString());
            xmlFile.WriteAttributeString("primary", m_bPrimary.ToString());

            xmlFile.WriteElementString("name", ToString());
            xmlFile.WriteElementString("fieldseason", FieldSeason.ToString());
            xmlFile.WriteElementString("filegdb", m_sFileGDB);

            xmlFile.WriteElementString("dem", "DEM");

            xmlFile.WriteElementString("topo_tin", ".//" + m_sTopoTIN);
            xmlFile.WriteElementString("ws_tin", ".//" + m_sWSTIN);
            
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
