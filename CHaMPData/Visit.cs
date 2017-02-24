using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;
using naru.db.sqlite;
using naru.xml;

namespace CHaMPWorkbench.CHaMPData
{
    public class Visit : VisitBasic
    {
        private string m_sHitch;
        private string m_sOrganization;
        private string m_sCrew;
        private DateTime? m_dtSampleDate;
        private long? m_nProtocolID;
        private string m_sPanel;
        private string m_sVisitStatus;
        public double? m_fDischarge;
        public double? m_fD84;

        // Unavailable in the API
        public bool IsPrimary { get; internal set; }
        public bool QCVisit { get; internal set; }
        public string CategoryName { get; internal set; }
        public string VisitPhase { get; internal set; }

        // Available but not implemented
        private bool m_bHasStreamTempLogger;
        private bool m_bHasFishData;

        public string Remarks { get; internal set; }

        public Dictionary<long, ChannelUnit> ChannelUnits { get; internal set; }

        #region Properties

        public string Hitch
        {
            get { return m_sHitch; }
            set
            {
                if (string.Compare(m_sHitch, value, false) != 0)
                {
                    m_sHitch = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public string Organization
        {
            get { return m_sOrganization; }
            set
            {
                if (string.Compare(m_sOrganization, value, false) != 0)
                {
                    m_sOrganization = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public string Crew
        {
            get { return m_sCrew; }
            set
            {
                if (string.Compare(m_sCrew, value, false) != 0)
                {
                    m_sCrew = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public DateTime? SampleDate
        {
            get { return m_dtSampleDate; }
            set
            {
                if (m_dtSampleDate != value)
                {
                    m_dtSampleDate = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public long? ProtocolID
        {
            get { return m_nProtocolID; }
            set
            {
                if (m_nProtocolID != value)
                {
                    m_nProtocolID = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public string Panel
        {
            get { return m_sPanel; }
            set
            {
                if (string.Compare(m_sPanel, value, false) != 0)
                {
                    m_sPanel = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public string VisitStatus
        {
            get { return m_sVisitStatus; }
            set
            {
                if (string.Compare(m_sVisitStatus, value, false) != 0)
                {
                    m_sVisitStatus = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public double? Discharge
        {
            get { return m_fDischarge; }
            set
            {
                if (m_fDischarge != value)
                {
                    m_fDischarge = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public double? D84
        {
            get { return m_fD84; }
            set
            {
                if (m_fD84 != value)
                {
                    m_fD84 = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public bool HasStreamTempLogger
        {
            get { return m_bHasStreamTempLogger; }
            set
            {
                if (m_bHasStreamTempLogger != value)
                {
                    m_bHasStreamTempLogger = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        public bool HasFishData
        {
            get { return m_bHasFishData; }
            set
            {
                if (m_bHasFishData != value)
                {
                    m_bHasFishData = value;
                    State = naru.db.DBState.Edited;
                }
            }
        }

        #endregion

        public Visit(long nID, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitYear, string sHitch,
            string sOrganization, string sCrew, DateTime dtSampleDate, Nullable<long> nProtocolID, long nProgramID, bool bIsPrimary, bool bQCVisit,
            string sPanel, string sCategoryName, string sVisitPhase, string sVisitStatus, bool bHSTL, bool bHasFishData,
           Nullable<double> fDischarge, Nullable<double> fD84, string sRemarks, string sUTMZone, naru.db.DBState eState)
            : base(nID, nWatershedID, sWatershedName, nSiteID, sSiteName, nVisitYear, sUTMZone, nProgramID, eState)
        {
            m_sHitch = sHitch;
            m_sOrganization = sOrganization;
            m_sCrew = sCrew;
            m_dtSampleDate = dtSampleDate;
            m_nProtocolID = nProtocolID;
            IsPrimary = bIsPrimary;
            QCVisit = bQCVisit;
            m_sPanel = sPanel;
            CategoryName = sCategoryName;
            VisitPhase = sVisitPhase;
            m_sVisitStatus = sVisitStatus;
            m_bHasStreamTempLogger = bHSTL;
            m_bHasFishData = bHasFishData;
            m_fDischarge = fDischarge;
            m_fD84 = fD84;
            Remarks = sRemarks;

            Init(nID);
        }

        public Visit(long nID, long nWatershedID, string sWatershedName, long nSiteID, string sSiteName, long nVisitYear, long nProgramID, string sUTMZone, naru.db.DBState eState)
            : base(nID, nWatershedID, sWatershedName, nSiteID, sSiteName, nVisitYear, sUTMZone, nProgramID, eState)
        {
            Init(nID);
        }

        private void Init(long nID)
        {
            ChannelUnits = ChannelUnit.Load(DBCon.ConnectionString, nID);
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
                        , dbRead.GetString(dbRead.GetOrdinal("WatershedName"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("SiteID"))
                        , dbRead.GetString(dbRead.GetOrdinal("SiteName"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("VisitYear"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "HitchName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Organization")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "CrewName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueDT(ref dbRead, "SampleDate")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "ProtocolID")
                        , dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("IsPrimary"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("QCVisit"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "PanelName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "CategoryName")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "VisitPhase")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "VisitStatus")
                        , dbRead.GetBoolean(dbRead.GetOrdinal("HasStreamTempLogger"))
                        , dbRead.GetBoolean(dbRead.GetOrdinal("HasFishData"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "Discharge")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "D84")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Remarks")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "UTMZone")
                        , naru.db.DBState.Unchanged);

            return aVisit;
        }

        public static void Save(ref SQLiteTransaction dbTrans, List<Visit> lVisits, List<long> lDeletedIDs = null)
        {
            string[] sFields = { "SiteID", "VisitYear", "ProgramID", "HitchName", "Organization", "CrewName", "SampleDate", "ProtocolID", "PanelName", "VisitStatus", "Discharge", "D84", "HasFishData", "HasStreamTempLogger" };
            SQLiteCommand comInsert = new SQLiteCommand(string.Format("INSERT INTO CHaMP_Visits (VisitID, {0}) VALUES (@ID, @{1})", string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);
            comInsert.Parameters.Add("ID", System.Data.DbType.Int64);

            SQLiteCommand comUpdate = new SQLiteCommand(string.Format("UPDATE CHaMP_Visits SET {0} WHERE VisitID = @ID", string.Join(", ", sFields.Select(x => x + " = @" + x))), dbTrans.Connection, dbTrans);
            comUpdate.Parameters.Add("ID", System.Data.DbType.Int64);

            foreach (Visit aVisit in lVisits)
            {
                // Only save the visit if it is new or changed. (but don't include this in loop as Linq filter because channel units might need saving below)
                if (aVisit.State != naru.db.DBState.Unchanged)
                {
                    SQLiteCommand dbCom = null;
                    if (aVisit.State == naru.db.DBState.New)
                    {
                        dbCom = comInsert;
                        if (aVisit.ID > 0)
                            dbCom.Parameters["ID"].Value = aVisit.ID;
                    }
                    else
                    {
                        dbCom = comUpdate;
                        dbCom.Parameters["ID"].Value = aVisit.ID;
                    }

                    AddParameter(ref dbCom, "SiteID", System.Data.DbType.Int64, aVisit.Site.ID);
                    AddParameter(ref dbCom, "VisitYear", System.Data.DbType.Int64, aVisit.VisitYear);
                    AddParameter(ref dbCom, "ProgramID", System.Data.DbType.Int64, aVisit.ProgramID);
                    AddParameter(ref dbCom, "HitchName", System.Data.DbType.String, aVisit.Hitch);
                    AddParameter(ref dbCom, "Organization", System.Data.DbType.String, aVisit.Organization);
                    AddParameter(ref dbCom, "CrewName", System.Data.DbType.String, aVisit.Crew);
                    AddParameter(ref dbCom, "SampleDate", System.Data.DbType.DateTime, aVisit.SampleDate);
                    AddParameter(ref dbCom, "ProtocolID", System.Data.DbType.String, aVisit.ProtocolID);
                    AddParameter(ref dbCom, "PanelName", System.Data.DbType.String, aVisit.Panel);
                    AddParameter(ref dbCom, "VisitStatus", System.Data.DbType.String, aVisit.VisitStatus);
                    AddParameter(ref dbCom, "Discharge", System.Data.DbType.Double, aVisit.Discharge);
                    AddParameter(ref dbCom, "D84", System.Data.DbType.Double, aVisit.D84);
                    AddParameter(ref dbCom, "HasStreamTempLogger", System.Data.DbType.Double, aVisit.HasStreamTempLogger);
                    AddParameter(ref dbCom, "HasFishData", System.Data.DbType.Double, aVisit.HasFishData);

                    dbCom.ExecuteNonQuery();

                    if (aVisit.State == naru.db.DBState.New && aVisit.ID < 1)
                    {
                        dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                        aVisit.ID = (long)dbCom.ExecuteScalar();
                    }
                }

                // Now save any channel units that have changed
                ChannelUnit.Save(ref dbTrans, aVisit.ChannelUnits.Values.ToList<ChannelUnit>().Where<ChannelUnit>(x => x.State != naru.db.DBState.Unchanged));
            }
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
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "sample_date", SampleDate.Value.ToString("o"));
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "filegdb", diSurveyGDB.FullName);
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "dem", "DEM");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "topo_tin", diTopoTIN.FullName);

            if (diWSTIN.Exists)
                naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "ws_tin", diWSTIN.FullName);
            else
                naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "ws_tin", string.Empty);

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
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "roughness_raster", "AssocRough");
            naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodVisit, "interperror_raster", "AssocIErr");

            XmlNode nodSegments = xmlDoc.CreateElement("channel_segments");

            // Retrieve a list of distinct segment numbers from all the channel units
            IEnumerable<long> lSegmentNumbers = ChannelUnits.Values.ToList<ChannelUnit>().Select(x => x.SegmentNumber).ToList<long>().Distinct<long>();
            foreach (long nSegmentNumber in lSegmentNumbers)
            {
                XmlNode nodSegment = xmlDoc.CreateElement("segment");
                XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "id", ID.ToString());
                XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "segment_number", nSegmentNumber.ToString());
                XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "segment_type", string.Empty);
                XmlNode nodChannelUnits = XMLHelpers.AddNode(ref xmlDoc, ref nodSegment, "channel_units");

                foreach (ChannelUnit cu in ChannelUnits.Values.Where<ChannelUnit>(x => x.SegmentNumber == nSegmentNumber))
                    nodChannelUnits.AppendChild(cu.CreateXMLNode(ref xmlDoc));
                nodVisit.AppendChild(nodSegments);
            }

            return nodVisit;
        }
    }
}
