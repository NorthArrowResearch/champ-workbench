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
        private Dictionary<string, string> m_sSQLStatements;
        private List<string> m_lMessages;

        public ClearDatabase(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
            m_sSQLStatements = new Dictionary<string, string>();
        }

        public void AddSQLStatementToClear(string sTable, string sMessage)
        {
            if (string.IsNullOrWhiteSpace(sTable))
                throw new Exception("The SQL statement cannot be null or empty.");

            m_sSQLStatements.Add(sTable, sMessage);
        }

        public int TableCount
        {
            get { return m_sSQLStatements.Count; }
        }

        public void DoClear(ref List<string> lSuccesses, ref List<string> lErrors)
        {
            if (m_dbCon.State == System.Data.ConnectionState.Closed)
                m_dbCon.Open();

            OleDbCommand dbCom;
            foreach (string sSQL in m_sSQLStatements.Keys)
            {
                dbCom = new OleDbCommand(sSQL, m_dbCon);
                try
                {
                    dbCom.ExecuteNonQuery();
                    lSuccesses.Add(m_sSQLStatements[sSQL]);
                }
                catch (Exception ex)
                {
                    lErrors.Add(" ");
                    lErrors.Add(String.Format("QUERY: \"{0}\"",sSQL));
                    lErrors.Add(String.Format("Error: \"{0}\"", ex.Message));
                }
            }
        }
    }
}
