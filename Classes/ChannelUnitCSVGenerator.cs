using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes
{
    class ChannelUnitCSVGenerator
    {
        private OleDbConnection m_dbCon;

        public ChannelUnitCSVGenerator(ref OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
        }

        public void Run(int nVisitID, string sFilePath)
        {
            OleDbCommand dbCom = new OleDbCommand("SELECT CHAMP_Sites.SiteName, C.ChannelUnitNumber, C.Tier1, C.Tier2, C.BouldersGT256, C.Cobbles65255, C.CoarseGravel1764, C.FineGravel316, C.Sand0062, C.FinesLT006, C.SumSubstrateCover" +
                " FROM CHAMP_Sites INNER JOIN ((CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS C ON S.SegmentID = C.SegmentID) INNER JOIN CHAMP_Visits AS V ON S.VisitID = V.VisitID) ON CHAMP_Sites.SiteID = V.SiteID" +
                " WHERE V.VisitID = @VisitID ORDER BY C.ChannelUnitNumber", m_dbCon);

            dbCom.Parameters.AddWithValue("VisitID", nVisitID);
            try
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();

                string sUnit;
                List<string> lUnits = new List<string>();
                lUnits.Add("VisitID,SiteName,UnitNumber,Tier1,Tier2,PercentFlow,SideChannelPresent,InQualifyingSideChannel,BouldersGT256,Cobbles65255,CoarseGravel1764,FineGravel316,Sand0062,FinesLT006,SumSubstrateCover");
                while (dbRead.Read())
                {
                    sUnit = nVisitID.ToString();
                    sUnit += AddStringField(ref dbRead, "SiteName");
                    sUnit += AddNumericField(ref dbRead, "ChannelUnitNumber");
                    sUnit += AddStringField(ref dbRead, "Tier1");
                    sUnit += AddStringField(ref dbRead, "Tier2");

                    sUnit += ",0";
                    sUnit += ",0";
                    sUnit += ",0";
                 
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

        private string AddNumericField(ref OleDbDataReader dbRead, string sFieldName)
        {
            string sResult = ",0";
            if (DBNull.Value != dbRead[sFieldName])
                sResult = "," + dbRead[sFieldName].ToString();
            return sResult;
        }

        private string AddStringField(ref OleDbDataReader dbRead, string sFieldName)
        {
            string sResult = ",EmptyString";
            if (DBNull.Value != dbRead[sFieldName])
                sResult = "," + dbRead[sFieldName].ToString().Replace(" ", "").Trim();
            return sResult;
        }
    }
}
