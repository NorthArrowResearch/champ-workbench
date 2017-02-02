using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class Watershed : naru.db.NamedObject
    {       
        public Watershed(long nWatershedID, String sWatershedName)
            : base(nWatershedID, sWatershedName)
        {

        }

        public static Dictionary<long,Watershed> Load(string sDBCon)
        {
            Dictionary<long, Watershed> dResult = new Dictionary<long, Watershed>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                
                SQLiteCommand dbCom = new SQLiteCommand("SELECT WatershedID, WatershedName FROM CHaMP_Watersheds ORDER BY WatershedName", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nID = dbRead.GetInt64(dbRead.GetOrdinal("WatershedID"));
                    dResult[nID] = new Watershed(nID, dbRead.GetString(dbRead.GetOrdinal("WatershedName")));
                }             
            }
            return dResult;
        }
    }
}
