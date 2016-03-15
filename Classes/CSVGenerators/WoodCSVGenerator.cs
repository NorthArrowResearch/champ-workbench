using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes.CSVGenerators
{
    public class WoodCSVGenerator : CSVGeneratorBase
    {
        public WoodCSVGenerator(string sDBCon)
            : base(sDBCon)
        {

        }

        public override System.IO.FileInfo Run(int nVisitID, string sFilePath)
        {
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                OleDbCommand dbCom = new OleDbCommand("SELECT C.ChannelUnitNumber, C.LargeWoodCount FROM CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS C ON S.SegmentID = C.SegmentID WHERE (S.VisitID = @VisitID) ORDER BY C.ChannelUnitNumber", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);

                try
                {
                    OleDbDataReader dbRead = dbCom.ExecuteReader();

                    string sUnit;
                    List<string> lUnits = new List<string>();
                    lUnits.Add("ChannelUnitNumber,SumLWDCount");

                    while (dbRead.Read())
                    {
                        sUnit = AddNumericField(ref dbRead, "ChannelUnitNumber");
                        sUnit += AddNumericField(ref dbRead, "LargeWoodCount");
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
