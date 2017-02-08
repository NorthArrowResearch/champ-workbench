using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Data.SQLite;
using System.Xml;
using System.ComponentModel;
using System.IO;
using naru.db.sqlite;

namespace CHaMPWorkbench.Classes
{
    class ResultScavengerBatch
    {
        private string m_sTopLevelFolder;
        private string m_sFileSearch;
        private bool m_bEmptyDatabaseBefore;
        private SearchOption m_SearchOption;

        private string m_sLogFilePattern;

        public List<Exception> Errors { get; internal set; }

        public ResultScavengerBatch(string sTopLevelFolder, string sFileSearch, bool bRecursive, bool bEmptyDatabaseBefore, string sLogFilePattern)
        {
            Errors = new System.Collections.Generic.List<System.Exception>();

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

            m_SearchOption = SearchOption.TopDirectoryOnly;
            if (bRecursive)
                m_SearchOption = SearchOption.AllDirectories;

            m_bEmptyDatabaseBefore = bEmptyDatabaseBefore;

            m_sFileSearch = "*.*";
            if (!string.IsNullOrEmpty(sFileSearch))
                m_sFileSearch = sFileSearch;

            m_sLogFilePattern = sLogFilePattern;
        }

        public long Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            int nProcessed = 0;
            string[] sResultFiles = Directory.GetFiles(m_sTopLevelFolder, m_sFileSearch, m_SearchOption);
            List<string> lLogFiles = null;

            if (!string.IsNullOrEmpty(m_sLogFilePattern))
                lLogFiles = new System.Collections.Generic.List<string>(Directory.GetFiles(m_sTopLevelFolder, m_sLogFilePattern, m_SearchOption));

            if (m_bEmptyDatabaseBefore)
            {
                ClearDatabase();
            }

            //ResultScavengerSingle scavenger = new ResultScavengerSingle(ref m_dbCon);
            ResultScavengerSingleCHaMP scavengerCHaMP = new ResultScavengerSingleCHaMP(DBCon.ConnectionString);

            for (int i = 0; i < sResultFiles.Count(); i++)
            {
                long nResultID = 0;
                try
                {
                    nResultID = scavengerCHaMP.ScavengeResultFile(sResultFiles[i]);

                    // Try to find a corresponding log file in this folder.
                    if (nResultID > 0 && lLogFiles != null)
                    {
                        string sRelatedLogFile = System.IO.Path.GetDirectoryName(sResultFiles[i]);
                        //sRelatedLogFile = System.IO.Path.Combine(sRelatedLogFile, m_sLogFilePattern);
                        string[] sLogs = Directory.GetFiles(sRelatedLogFile, m_sLogFilePattern, SearchOption.TopDirectoryOnly);
                        for (int j = 0; i < sLogs.Count(); i++)
                        {
                            // Log exists. Process it and relate to the result.
                            // Then remove it from later indepdendant processing
                            scavengerCHaMP.ScavengeLogFile(DBCon.ConnectionString, nResultID, sLogs[j], sResultFiles[i]);

                            if (lLogFiles.Contains(sLogs[i]))
                                lLogFiles.Remove(sLogs[i]);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //
                    // these are legimitate RBT XML result files that have errors. Add them
                    // to the running list of problems and continue with next file.
                    //
                    Errors.Add(ex);
                }

                if (i < 1 || sResultFiles.Length < 1)
                    worker.ReportProgress(0);
                else
                    worker.ReportProgress((i) * 100 / sResultFiles.Length);
                
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    // User clicked cancel on the user interace.
                    return (sResultFiles.Count() - Errors.Count());

                }
            }

            if (!string.IsNullOrEmpty(m_sLogFilePattern))
            {
                // Process all the remaining log files that might not be related to result files.
                foreach (string sLog in lLogFiles)
                {
                    try
                    {
                        scavengerCHaMP.ScavengeLogFile(DBCon.ConnectionString,  0, sLog, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(ex);
                    }
                }
            }

            return (sResultFiles.Count() - Errors.Count());
        }

        private bool ClearDatabase()
        {

            bool bResult = false;
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand dbCom = new SQLiteCommand("DELETE * FROM Metric_SiteMetrics WHERE (VisitID Is Not NULL) AND (ScavengeTypeID <> 2)", dbCon);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("DELETE * FROM LogFiles", dbCon);
                    dbCom.ExecuteNonQuery();

                    bResult = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error clearing database", ex);
                }
            }

            return bResult;
        }
    }
}
