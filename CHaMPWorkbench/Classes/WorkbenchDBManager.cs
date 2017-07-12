using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes
{
    public class WorkbenchDBManager : naru.db.sqlite.DBManager
    {

        public WorkbenchDBManager(string sFilePath)
            : base(sFilePath, "SELECT ValueInfo FROM VersionInfo WHERE Key = 'DatabaseVersion'", 45, @"Database\database_structure.sql")
        {

        }

        protected override void UpgradeMaster(ref SQLiteTransaction dbTrans, int nRequiredVersion)
        {
            if (!RequiresUpdrade(nRequiredVersion))
                return;



            throw new NotImplementedException();
        }

        private void Update45()
        {

        }

        protected override void BaseInstall(ref SQLiteTransaction dbTrans)
        {
            throw new NotImplementedException();
        }
    }
}
