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
        private List<string> m_sSQLStatements;

        public ClearDatabase(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
            m_sSQLStatements = new List<string>();
        }

        public void AddSQLStatementToClear(string sTable)
        {
            if (string.IsNullOrWhiteSpace(sTable))
                throw new Exception("The SQL statement cannot be null or empty.");

            m_sSQLStatements.Add(sTable);
        }

        public int TableCount
        {
            get { return m_sSQLStatements.Count; }
        }

        public void DoClear(ref string sSuccess, ref string sError)
        {
            if (m_dbCon.State == System.Data.ConnectionState.Closed)
                m_dbCon.Open();

            OleDbCommand dbCom;
            foreach (string sSQL in m_sSQLStatements)
            {
                dbCom = new OleDbCommand(sSQL, m_dbCon);
                try
                {
                    dbCom.ExecuteNonQuery();
                    sSuccess += sSQL +", ";
                }
                catch (Exception ex)
                {
                    sError += sSQL + ", ";
                }
            }

            if (!string.IsNullOrWhiteSpace(sSuccess))
                sSuccess= sSuccess.Substring(0,sSuccess.Length-2);

            if (!string.IsNullOrWhiteSpace(sError))
                sError = sError.Substring(0, sError.Length-2);
        }        
    }
}
