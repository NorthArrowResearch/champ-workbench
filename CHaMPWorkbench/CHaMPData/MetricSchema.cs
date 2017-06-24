using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    class MetricSchema : naru.db.NamedObject
    {
        public string RootXPath { get; internal set; }
        public string DatabaseTable { get; internal set; }

        public MetricSchema(long nID, string sName, string sRootXPath, string sDatabaseTable)
            : base(nID, sName)
        {
            RootXPath = sRootXPath;
            DatabaseTable = sDatabaseTable;
        }

        public static Dictionary<long, MetricSchema> Load(string sDBCon)
        {
            Dictionary<long, MetricSchema> dResult = new Dictionary<long, MetricSchema>();

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM Metric_Schemas ORDER BY Title", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nID = dbRead.GetInt64(dbRead.GetOrdinal("SchemaID"));
                    dResult[nID] = new MetricSchema(nID, dbRead.GetString(dbRead.GetOrdinal("Title"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "RootXPath")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "DatabaseTable"));
                }
            }

            return dResult;
        }
    }
}
