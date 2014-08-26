using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;
using System.Xml;
using System.ComponentModel;
using System.IO;

namespace CHaMPWorkbench.Classes
{
    class ResultScavengerBatch
    {
        private string m_sTopLevelFolder;
        private string m_sFileSearch;
        private bool m_bEmptyDatabaseBefore;
        private SearchOption m_SearchOption;
        private OleDbConnection m_dbCon;

        private string m_sLogFilePattern;

        public ResultScavengerBatch(OleDbConnection dbCon, string sTopLevelFolder, string sFileSearch, bool bRecursive, bool bEmptyDatabaseBefore, string sLogFilePattern)
        {
            m_dbCon = dbCon;

            if (string.IsNullOrEmpty(sTopLevelFolder))
            {
                throw new ArgumentNullException("sTopLevelFolder", "The top level folder cannot be null or empty");
            }
            else
            {
                if (!Directory.Exists(sTopLevelFolder))
                {
                    ArgumentException ex = new ArgumentException("The folder selected does not exist", "sTopLevelFolder");
                    ex.Data.Add("Folder", sTopLevelFolder);
                    throw ex;
                }
            }
            m_sTopLevelFolder = sTopLevelFolder;

            if (bRecursive)
            {
                m_SearchOption = SearchOption.AllDirectories;
            }
            else
            {
                m_SearchOption = SearchOption.TopDirectoryOnly;
            }

            m_bEmptyDatabaseBefore = bEmptyDatabaseBefore;

            if (string.IsNullOrEmpty(sFileSearch))
            {
                m_sFileSearch = "*.*";
            }
            else
            {
                m_sFileSearch = sFileSearch;
            }

            m_sLogFilePattern = sLogFilePattern;
        }

        public long Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            int nProcessed = 0;
            List<Exception> eErrors = new List<Exception>();
            string[] sFiles = Directory.GetFiles(m_sTopLevelFolder, m_sFileSearch, m_SearchOption);
  
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            if (m_bEmptyDatabaseBefore)
            {
                ClearDatabase(ref m_dbCon);
            }

            ResultScavengerSingle scavenger = new ResultScavengerSingle(ref m_dbCon);

            for (int i = 0; i <= sFiles.Count() - 1; i++)
            {
                int ResultID = 0;
                try
                {
                    ResultID = scavenger.ScavengeResultFile(sFiles[i]);
                }
                catch (Exception ex)
                {
                    //
                    // these are legimitate RBT XML result files that have errors. Add them
                    // to the running list of problems and continue with next file.
                    //
                    eErrors.Add(ex);
                }

                if (!string.IsNullOrEmpty(m_sLogFilePattern))
                {
                    //string sLogFile as strin
                    //scavenger.ScavengeLogFile(0, nResultID, sFiles[i]);
                    //PopulateTable_Log(ref m_dbCon, ResultID, Path.GetDirectoryName(sFiles[i]), m_sLogFilePattern, sFiles[i]);
                }

                worker.ReportProgress((i + 1) * 100 / sFiles.Count());
            }

            if (eErrors.Count > 0)
            {
                Exception ex = new Exception(nProcessed.ToString() + " RBT result file(s) processed. " + eErrors.Count.ToString() + " files encountered errors.", eErrors[0]);
                throw ex;
            }

            return (sFiles.Count() - eErrors.Count());

        }

        private bool ClearDatabase(ref OleDbConnection dbCon)
        {

            bool bResult = false;
            try
            {
                OleDbCommand dbCom = new OleDbCommand("DELETE * FROM Visits", dbCon);
                dbCom.ExecuteNonQuery();

                dbCom = new OleDbCommand("DELETE * FROM LogFiles", dbCon);
                dbCom.ExecuteNonQuery();

                bResult = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error clearing database", ex);
            }

            return bResult;

        }

        //public void PopulateTable_Log(ref OleDbConnection dbcon, int nVisitID, string sDirectory, string sLogFile, string sResultFilePath)
        //{

        //    foreach (string sLog in Directory.GetFiles(sDirectory, sLogFile, SearchOption.TopDirectoryOnly))
        //    {
        //       ScavengeLogFile(ref dbcon, nVisitID, sLog, sResultFilePath);
        //    }
        //}      
    }

}
