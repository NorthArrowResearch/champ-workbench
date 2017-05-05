using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes.CSVGenerators
{
    public abstract class CSVGeneratorBase
    {
        public string DBCon { get; internal set; }

        public CSVGeneratorBase(string sDBCon)
        {
            DBCon = sDBCon;
        }

        // All CSV generators must implement the run method that actually performs the CSV generation
        public abstract System.IO.FileInfo Run(long nVisitID, string sFilePath);

        protected string AddNumericField(ref SQLiteDataReader dbRead, string sFieldName)
        {
            string sResult = ",0";
            if (DBNull.Value != dbRead[sFieldName])
                sResult = "," + dbRead[sFieldName].ToString();
            return sResult;
        }

        protected string AddStringField(ref SQLiteDataReader dbRead, string sFieldName, bool bPreprendComma = true)
        {
            string sResult = string.Empty;

            if (bPreprendComma)
                sResult = ",";

            if (DBNull.Value != dbRead[sFieldName])
                sResult += dbRead[sFieldName].ToString().Replace(" ", "").Replace(",", "").Trim();
            return sResult;
        }

    }
}
