using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;

namespace CHaMPWorkbench.Classes
{
    public class WorkbenchDBManager : naru.db.sqlite.DBManager
    {

        public WorkbenchDBManager(string sFilePath)
            : base(sFilePath, "SELECT ValueInfo FROM VersionInfo WHERE Key = 'DatabaseVersion'", 45, @"Database\database_structure.sql")
        {

        }

        protected override void Upgrade(ref SQLiteTransaction dbTrans, int nNewVersion)
        {
            string sUpdateMethod = string.Format("Update{0}", nNewVersion);
            
            try
            {
                MethodInfo theMethod = this.GetType().GetMethod(sUpdateMethod, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { dbTrans.GetType(), System.Type.GetType("System.Int32") }, null);
                if (theMethod == null)
                    throw new Exception("Error attempting database upgrade. Failed to fine upgrade override function.");

                theMethod.Invoke(this, new[] { (object)dbTrans, (object)nNewVersion });
            }
            catch (Exception ex)
            {
                ex.Data["Database Path"] = this.FilePath.FullName;
                ex.Data["Update Method"] = sUpdateMethod;
                ex.Data["New Version"] = nNewVersion.ToString();
                throw;
            }
        }

        private void Update45(SQLiteTransaction dbTrans, int nNewVersion)
        {
            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO VersionChangeLog (Version, Description) VALUES (@Version, @Description)", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("Version", nNewVersion);
            dbCom.Parameters.AddWithValue("Description", "Test upgrade query");
            dbCom.ExecuteNonQuery();
        }

        protected override void BaseInstall(ref SQLiteTransaction dbTrans)
        {
            throw new NotImplementedException();
        }
    }
}
