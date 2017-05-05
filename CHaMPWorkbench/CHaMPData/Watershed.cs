using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class Watershed : naru.db.EditableNamedObject
    {
        public Watershed(long nWatershedID, String sWatershedName, naru.db.DBState eState)
            : base(nWatershedID, sWatershedName, eState)
        {
        }

        public Watershed(Watershed aWatershed, naru.db.DBState eState)
            : base(aWatershed.ID, aWatershed.Name, eState)
        {
        }

        public static Dictionary<long, Watershed> Load(string sDBCon)
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
                    dResult[nID] = new Watershed(nID, dbRead.GetString(dbRead.GetOrdinal("WatershedName")), naru.db.DBState.Unchanged);
                }
            }
            return dResult;
        }

        public static void Save(ref SQLiteTransaction dbTrans, List<Watershed> lWatersheds, List<long> lDeletedIDs = null)
        {
            string[] sFields = { "WatershedName" };
            SQLiteCommand comInsert = new SQLiteCommand(string.Format("INSERT INTO CHaMP_Watersheds (WatershedID, {1}) VALUES (@ID, @{1})", string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);
            comInsert.Parameters.Add("ID", System.Data.DbType.Int64);

            SQLiteCommand comUpdate = new SQLiteCommand(string.Format("UPDATE CHaMP_Watersheds SET {0} WHERE WatershedID = @ID", string.Join(",", sFields.Select(x => string.Format("{0} = @{0}", x)))), dbTrans.Connection, dbTrans);
            comUpdate.Parameters.Add("ID", System.Data.DbType.Int64);

            foreach (Watershed aWatershed in lWatersheds.Where<Watershed>(x => x.State != naru.db.DBState.Unchanged))
            {
                SQLiteCommand dbCom = null;
                if (aWatershed.State == naru.db.DBState.New)
                {
                    dbCom = comInsert;
                    if (aWatershed.ID > 0)
                        dbCom.Parameters["ID"].Value = aWatershed.ID;
                }
                else
                {
                    dbCom = comUpdate;
                    dbCom.Parameters["ID"].Value = aWatershed.ID;
                }

                AddParameter(ref dbCom, "WatershedName", System.Data.DbType.String, aWatershed.Name);

                dbCom.ExecuteNonQuery();

                if (aWatershed.State == naru.db.DBState.New && aWatershed.ID < 1)
                {
                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    aWatershed.ID = (long)dbCom.ExecuteScalar();
                }
            }

            if (lDeletedIDs is List<long>)
            {
                SQLiteCommand comDelete = new SQLiteCommand("DELETE FROM CHaMP_Watersheds WHERE WatershedID = @ID", dbTrans.Connection, dbTrans);
                SQLiteParameter pDelete = comDelete.Parameters.Add("ID", System.Data.DbType.Int64);
                foreach (long nID in lDeletedIDs)
                {
                    pDelete.Value = nID;
                    comDelete.ExecuteNonQuery();
                }
            }
        }
    }
}
