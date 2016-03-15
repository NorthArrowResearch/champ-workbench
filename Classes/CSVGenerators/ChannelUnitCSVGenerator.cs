using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes.CSVGenerators
{
    class ChannelUnitCSVGenerator : CSVGeneratorBase
    {
        public ChannelUnitCSVGenerator(string sDBCon) : base(sDBCon)
        {
        }

        public override System.IO.FileInfo Run(int nVisitID, string sFilePath)
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Sites.SiteName, C.ChannelUnitNumber, C.Tier1, C.Tier2, " +
                    " C.BouldersGT256, C.Cobbles65255, C.CoarseGravel1764, C.FineGravel316, C.Sand0062, C.FinesLT006, C.SumSubstrateCover," +
                    " W.WatershedName, V.SampleDate, V.CrewName, V.PanelName,C.ID As ChannelUnitID, S.SegmentNumber, S.SegmentName" +
                    " FROM CHAMP_Watersheds AS W INNER JOIN ((CHAMP_Sites INNER JOIN CHAMP_Visits AS V ON CHAMP_Sites.SiteID = V.SiteID) INNER JOIN (CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS C ON S.SegmentID = C.SegmentID) ON V.VisitID = S.VisitID) ON W.WatershedID = CHAMP_Sites.WatershedID" +
                    " WHERE V.VisitID=[@VisitID] ORDER BY C.ChannelUnitNumber", dbCon);

                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                try
                {
                    OleDbDataReader dbRead = dbCom.ExecuteReader();

                    string sUnit;
                    List<string> lUnits = new List<string>();
                    lUnits.Add("Watershed" +
                        ",SiteID" +
                        ",SampleDate" +
                        ",VisitID" +
                        ",MeasureNbr" +
                        ",Crew" +
                        ",VisitPhase" +
                        ",VisitStatus" +
                        ",StreamName" +
                        ",Panel" +
                        ",ChannelUnitID" +
                        ",UnitNumber" +
                        ",ChannelSegment" +
                        ",SegmentNumber" +
                        ",Tier1" +
                        ",Tier2" +
                        ",FieldNotes" +
                        ",CountOfPebbles" +
                        ",DataUpdateNotes" +
                        ",PercentFlow" +
                        ",SideChannelPresent" +
                        ",InQualifyingSideChannel" +
                        ",BouldersGT256" +
                        ",Cobbles65_255" +
                        ",CoarseGravel17_64" +
                        ",FineGravel3_16" +
                        ",Sand006_2" +
                        ",FinesLT006" +
                        ",SumSubstrateCover"
                     );

                    while (dbRead.Read())
                    {
                        sUnit = AddStringField(ref dbRead, "WatershedName", false);
                        sUnit += AddStringField(ref dbRead, "SiteName");
                        sUnit += AddStringField(ref dbRead, "SampleDate");
                        sUnit += string.Format(",{0}", nVisitID);
                        sUnit += ",1"; // Measure
                        sUnit += AddStringField(ref dbRead, "CrewName");
                        sUnit += ",1"; // Visit Phase
                        sUnit += ",1"; // Visit Status
                        sUnit += ","; // Stream Name
                        sUnit += AddStringField(ref dbRead, "PanelName");
                        sUnit += AddNumericField(ref dbRead, "ChannelUnitID");
                        sUnit += AddNumericField(ref dbRead, "ChannelUnitNumber");
                        sUnit += AddStringField(ref dbRead, "SegmentName");
                        sUnit += AddNumericField(ref dbRead, "SegmentNumber");
                        sUnit += AddStringField(ref dbRead, "Tier1");
                        sUnit += AddStringField(ref dbRead, "Tier2");
                        sUnit += ",";
                        sUnit += ",0"; // Pebbles
                        sUnit += ",";
                        sUnit += ",0"; // Percent Flow
                        sUnit += ",0"; // Side channel Present
                        sUnit += ",0"; // In qualifying side channel

                        sUnit += AddNumericField(ref dbRead, "BouldersGT256");
                        sUnit += AddNumericField(ref dbRead, "Cobbles65255");
                        sUnit += AddNumericField(ref dbRead, "CoarseGravel1764");
                        sUnit += AddNumericField(ref dbRead, "FineGravel316");
                        sUnit += AddNumericField(ref dbRead, "Sand0062");
                        sUnit += AddNumericField(ref dbRead, "FinesLT006");
                        sUnit += AddNumericField(ref dbRead, "SumSubstrateCover");
                        lUnits.Add(sUnit);
                    }
                    dbRead.Close();
                    System.IO.File.WriteAllLines(sFilePath, lUnits.ToArray<string>());

                }
                catch (Exception ex)
                {
                    ex.Data["Visit ID"] = nVisitID.ToString();
                    ex.Data["File Path"] = sFilePath;
                    throw;
                }
            }

            System.IO.FileInfo fiCSV = null;
            if (System.IO.File.Exists(sFilePath))
               fiCSV =  new System.IO.FileInfo(sFilePath);

            return fiCSV;
        }
    }
}
