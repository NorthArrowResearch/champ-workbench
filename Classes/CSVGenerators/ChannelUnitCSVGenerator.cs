using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes.CSVGenerators
{
    class ChannelUnitCSVGenerator : CSVGeneratorBase
    {
        public ChannelUnitCSVGenerator(string sDBCon) : base(sDBCon)
        {
        }

        public override System.IO.FileInfo Run(int nVisitID, string sFilePath)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT CHAMP_Sites.SiteName, C.ChannelUnitNumber, C.Tier1, C.Tier2, " +
                    " C.BouldersGT256, C.Cobbles65255, C.CoarseGravel1764, C.FineGravel316, C.Sand0062, C.FinesLT006, C.SumSubstrateCover," +
                    " W.WatershedName, V.SampleDate, V.CrewName, V.PanelName,C.ID As ChannelUnitID, S.SegmentNumber, S.SegmentName" +
                    " FROM CHAMP_Watersheds AS W INNER JOIN ((CHAMP_Sites INNER JOIN CHAMP_Visits AS V ON CHAMP_Sites.SiteID = V.SiteID) INNER JOIN (CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS C ON S.SegmentID = C.SegmentID) ON V.VisitID = S.VisitID) ON W.WatershedID = CHAMP_Sites.WatershedID" +
                    " WHERE V.VisitID=[@VisitID] ORDER BY C.ChannelUnitNumber", dbCon);

                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                try
                {
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();

                    string sUnit;
                    List<string> lUnits = new List<string>();
                    lUnits.Add("VisitID" +
                        ",SiteName" +
                        ",UnitID" +
                        ",UnitNumber" +
                        ",Tier1" +
                        ",Tier2" +
                        ",PercentFlow" +
                        ",SideChannelPresent" +
                        ",InQualifyingSideChannel" +
                        ",Bedrock" +
                        ",BouldersGT256" +
                        ",Cobbles65_255" +
                        ",CoarseGravel17_64" +
                        ",FineGravel3_16" +
                        ",Sand006_2" +
                        ",FinesLT006" +
                        ",SumSubstrateCover" +
                        ",SegmentNumber" +
                        ",ChannelSegment"
                     );

                    while (dbRead.Read())
                    {
                        sUnit = string.Format("{0}", nVisitID);
                        sUnit += AddStringField(ref dbRead, "SiteName");
                        sUnit += AddNumericField(ref dbRead, "ChannelUnitID");
                        sUnit += AddNumericField(ref dbRead, "ChannelUnitNumber");
                        sUnit += AddStringField(ref dbRead, "Tier1");
                        sUnit += AddStringField(ref dbRead, "Tier2");
                        sUnit += ",0"; // Percent Flow
                        sUnit += ",0"; // Side channel Present
                        sUnit += ",0"; // In qualifying side channel
                        sUnit += ",0"; // Bedrock
                        sUnit += AddNumericField(ref dbRead, "BouldersGT256");
                        sUnit += AddNumericField(ref dbRead, "Cobbles65255");
                        sUnit += AddNumericField(ref dbRead, "CoarseGravel1764");
                        sUnit += AddNumericField(ref dbRead, "FineGravel316");
                        sUnit += AddNumericField(ref dbRead, "Sand0062");
                        sUnit += AddNumericField(ref dbRead, "FinesLT006");
                        sUnit += AddNumericField(ref dbRead, "SumSubstrateCover");
                        sUnit += AddNumericField(ref dbRead, "SegmentNumber");
                        sUnit += AddStringField(ref dbRead, "SegmentName");

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
                fiCSV = new System.IO.FileInfo(sFilePath);

            return fiCSV;
        }
    }
}
