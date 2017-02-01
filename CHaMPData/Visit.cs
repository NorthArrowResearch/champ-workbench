using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.CHaMPData
{
    public class Visit : VisitBasic
    {
        public Nullable<long> ProgramSiteID { get; internal set; }
        public Nullable<long> HitchID { get; internal set; }
        public string HitchName { get { return base.ToString(); } }
        public string Organization { get; internal set; }
        public string CrewName { get; internal set; }
        public DateTime SampleDate { get; internal set; }
        public Nullable<long> ProtocolID { get; internal set; }
        public bool IsPrimary { get; internal set; }
        public bool QCVisit { get; internal set; }
        public string PanelName { get; internal set; }
        public string CategoryName { get; internal set; }
        public string VisitPhase { get; internal set; }
        public string VisitStatus { get; internal set; }
        public bool AEM { get; internal set; }
        public bool HasStreamTempLogger { get; internal set; }
        public bool HasFishData { get; internal set; }
        public Nullable<double> Discharge { get; internal set; }
        public Nullable<double> D84 { get; internal set; }
        public string Remarks { get; internal set; }

        public Dictionary<long, ChannelSegment> Segments { get; internal set; }

        public Visit(long nID, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitYear, Nullable<long> nProgramSiteID, Nullable<long> nHitchID, string sHitchName,
            string sOrganization, string sCrewName, DateTime dtSampleDate, Nullable<long> nProtocolID, bool bIsPrimary, bool bQCVisit,
            string sPanelName, string sCategoryName, string sVisitPhase, string sVisitStatus, bool bAEM, bool bHSTL, bool bHasFishData,
           Nullable<double> fDischarge, Nullable<double> fD84, string sRemarks, string sUTMZone) : base(nID, nWatershedID, sWatershedName, nSiteID, sSiteName, nVisitYear, sUTMZone)
        {
            ProgramSiteID = nProgramSiteID;
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

            Segments = ChannelSegment.Load(DBCon.ConnectionString, nID);
        }

        public static Visit Load(string sDBCon, long nVisitID)
        {
            Visit aVisit = null;
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM vwVisits WHERE VisitID = @VisitID", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    aVisit = BuildVisitFromReader(ref dbRead);
            }

            return aVisit;
        }

        public static Dictionary<long, Visit> Load(string sDBCon)
        {
            Dictionary<long, Visit> dVisits = new Dictionary<long, Visit>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM vwVisits", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nVisitID = dbRead.GetInt64(dbRead.GetOrdinal("VisitID"));
                    dVisits[nVisitID] = BuildVisitFromReader(ref dbRead);
                }
            }

            return dVisits;
        }

        private static Visit BuildVisitFromReader(ref SQLiteDataReader dbRead)
        {
            Visit aVisit = new Visit(dbRead.GetInt64(dbRead.GetOrdinal("VisitID"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("WatershedID"))
                        , dbRead.GetString(dbRead.GetOrdinal("WateshedName"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("SiteID"))
                        , dbRead.GetString(dbRead.GetOrdinal("SiteName"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("VisitYear"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "ProgramSiteID")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "HitchID")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "HitchName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Organization")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "CrewName")
                        , dbRead.GetDateTime(dbRead.GetOrdinal("SampleDate"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "ProtocolID")
                        , dbRead.GetBoolean(dbRead.GetOrdinal("IsPrimary"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("QCVisit"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "PanelName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "CategoryName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "VisitPhase")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "VisitStatus")
                        , dbRead.GetBoolean(dbRead.GetOrdinal("AEM"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("HasStreamTempLogger"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("HasFishData"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "Discharge")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "D84")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Remarks")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "UTMZone"));

            return aVisit;
        }

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc, System.IO.DirectoryInfo diTopoLevelTopDir, bool bRequireWSTIN, bool bCalculateMetrics, bool bChangeDetection, bool bPrimary)
        {
            System.IO.DirectoryInfo diSurveyGDB = null;
            if (!Classes.DataFolders.SurveyGDB(diTopoLevelTopDir, ID, out diSurveyGDB))
                return null;

            System.IO.DirectoryInfo diTopoTIN = null;
            if (!Classes.DataFolders.TopoTIN(diTopoLevelTopDir, ID, out diTopoTIN))
                return null;

            System.IO.DirectoryInfo diWSTIN = null;
            if (!Classes.DataFolders.WaterSurfaceTIN(diTopoLevelTopDir, ID, out diWSTIN) && bRequireWSTIN)
                return null;

            XmlNode nodVisit = xmlDoc.CreateElement("visit");

            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodVisit, "calculatemetrics", bCalculateMetrics.ToString());
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodVisit, "changedetection", bChangeDetection.ToString());
            //naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodVisit, "makedemorthogonal", false.ToString());
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodVisit, "primary", bPrimary.ToString());
            //naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodVisit, "generatecsv", false.ToString());

            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "visitid", ID.ToString());
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "name", this.ToString());
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "fieldseason", VisitYear.ToString());
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "filegdb", diSurveyGDB.FullName);
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "dem", "DEM");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "topo_tin", diTopoTIN.FullName);

            if (diWSTIN.Exists)
                naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "ws_tin", string.Empty);
            else
                naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "ws_tin", diWSTIN.FullName);

            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "topo_points", "Topo_Points");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "control_points", "Control_Points");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "thalweg", "Thalweg");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "orthoginfo", "OrthogInfo");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "wetted_extent_points", "EdgeOfWater_Points");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "wetted_extent", "WaterExtent");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "wetted_centerline", "CenterLine");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "wetted_cross_sections", "WettedXS");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "bankfull_extent", "Bankfull");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "bankfull_centerline", "BankfullCL");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "bankfull_cross_sections", "BankfullXS");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "detrended", "Detrended");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "survey_extent", "Survey_Extent");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "water_depth", "Water_Depth");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "geomorphic_units", "GeomorphicUnits");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "wsdem", "WSEDEM");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "wetted_islands", "WIslands");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "bankfull_islands", "BIslands");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "qaqc_points", "QaQc_RawPoints");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "channel_units", "Channel_Units");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "emap_wetted_cross_sections", "WetCross_EMap");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "emap_bankfull_cross_sections", "BankCross_EMap");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "error_surface", "ErrSurface");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "slope_raster", "AssocSlope");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "pdensity_raster", "AssocPDensity");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "pointquality_raster", "Assoc3DPQ");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "roughness_raster", "AssocD50");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "interperror_raster", "AssocIErr");

            XmlNode nodSegments = xmlDoc.CreateElement("channel_segments");
            foreach (ChannelSegment sg in Segments.Values)
                nodSegments.AppendChild(sg.CreateXMLNode(ref xmlDoc));

            return nodVisit;
        }
    }
}
