using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes
{
    class ClearDatabase
    {
        private OleDbConnection m_dbCon;
        private List<string> m_sTables;

        public ClearDatabase(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
            m_sTables = new List<string>();
        }

        public void AddTableToClear(string sTable)
        {
            if (string.IsNullOrWhiteSpace(sTable))
                throw new Exception("The table name cannot be null or empty.");

            m_sTables.Add(sTable);
        }

        public int TableCount
        {
            get { return m_sTables.Count; }
        }

        public void DoClear(ref string sSuccess, ref string sError)
        {
            if (m_dbCon.State == System.Data.ConnectionState.Closed)
                m_dbCon.Open();

            OleDbCommand dbCom;
            foreach (string sTable in m_sTables)
            {
                dbCom = new OleDbCommand("DELETE FROM " + sTable, m_dbCon);
                try
                {
                    dbCom.ExecuteNonQuery();
                    sSuccess += sTable +", ";
                }
                catch (Exception ex)
                {
                    sError += sTable + ", ";
                }
            }

            if (!string.IsNullOrWhiteSpace(sSuccess))
                sSuccess= sSuccess.Substring(0,sSuccess.Length-2);

            if (!string.IsNullOrWhiteSpace(sError))
                sError = sError.Substring(0, sError.Length-2);

        }
        
    }
}
