using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class Program : naru.db.NamedObject
    {
        public string WebSiteURL { get; internal set; }
        public string FTPURL { get; internal set; }
        public string AWSBucket { get; internal set; }
        public string Remarks { get; internal set; }

        public Program(long nProgramID, string sTitle, string sWebSiteURL, string sFTPURL, string sAWSBucket, string sRemarks)
            : base(nProgramID, sTitle)
        {
            WebSiteURL = sWebSiteURL;
            FTPURL = sFTPURL;
            AWSBucket = sAWSBucket;
            Remarks = sRemarks;
        }

        public static Dictionary<long, Program> Load(string sDBCon)
        {
            Dictionary<long, Program> dResult = new Dictionary<long, Program>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Programs ORDER BY Title", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nProgramID = dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"));
                    dResult[nProgramID] = new Program(nProgramID,
                        dbRead.GetString(dbRead.GetOrdinal("Title"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "WebSiteURL")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "FTPURL")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "AWSBucket")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Remarks"));
                }
                return dResult;
            }
        }
    }
}
