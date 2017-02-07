using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml;
using System.Diagnostics;
using System.Collections.Specialized;
using naru.db.sqlite;

namespace CHaMPWorkbench.Classes
{

    public class RBTBatchEngine
    {
        private string m_sTempRBTWorkspace = "C:\\CHaMP\\TempRBTWorkspace";
        private string m_sRBTPath;
        private System.Diagnostics.ProcessWindowStyle m_eWindowStyle;

        public RBTBatchEngine(String sRBTConsolePath, System.Diagnostics.ProcessWindowStyle eWindowStyle)
        {
            m_eWindowStyle = eWindowStyle;

            if (!System.IO.File.Exists(sRBTConsolePath))
            {
                Exception ex = new Exception("The RBT Console Path does not exist.");
                ex.Data.Add("Console path", sRBTConsolePath);
                throw ex;
            }

            m_sRBTPath = sRBTConsolePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bScavengeResults"></param>
        /// <param name="bScavengeLog"></param>
        /// <remarks>Do not change the connection string to work on different machines. The critical factor is compiling the software as
        /// either x86 or x64 to match the version of Access on the end users computer. Leaving the compiler as "AnyCPU" will cause
        /// the software to load in the same architecture as the OS, but this might not match the architecture of Access (e.g. 
        /// Windows 64 bit machine with 32 bit Access installed needs the software to be compiled in x86 architecture.)
        ///</remarks>
        public void Run(bool bScavengeResults, bool bScavengeLog)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                dbCon.Open();

                //
                ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                // Get all of the active runs
                //
                SQLiteCommand dbCom = new SQLiteCommand("SELECT R.*, B.BatchName FROM Model_BatchRuns R LEFT JOIN Model_Batches B ON B.ID = R.BatchID WHERE (R.Run <> 0) ORDER BY R.Priority;", dbCon);
                SQLiteDataReader dbRdr = dbCom.ExecuteReader();
                Dictionary<long, RBTRun> dFiles = new Dictionary<long, RBTRun>();
                while (dbRdr.Read())
                {
                    if (System.Convert.IsDBNull(dbRdr["InputFile"]))
                        Console.WriteLine("Warning: empty input file record in database");
                    else
                    {
                        Debug.WriteLine("RBT input file: " + (string)dbRdr["InputFile"]);
                        long nVisitID = 0;
                        if (!Convert.IsDBNull((long)dbRdr["PrimaryVisitID"]))
                            nVisitID = (long)dbRdr["PrimaryVisitID"];

                        dFiles.Add((long)dbRdr["ID"], new RBTRun((long)dbRdr["ID"], (string)dbRdr["InputFile"], false, false, nVisitID));
                    }
                }
                dbRdr.Close();

                string sInputFile = null;
                foreach (RBTRun aRun in dFiles.Values)
                {
                    sInputFile = aRun.InputFile;

                    dbCom = new SQLiteCommand("UPDATE Model_BatchRuns SET Run = 0, DateTimeStarted = CURRENT_TIMESTAMP, DateTimeCompleted = NULL WHERE ID = @ID", dbCon);
                    dbCom.Parameters.AddWithValue("ID", aRun.ID);
                    dbCom.ExecuteNonQuery();

                    if (File.Exists(sInputFile))
                    {
                        try
                        {
                            if (aRun.ClearTempWorkspacePrior)
                                ClearTempWorkspace();

                            //System.IO.Directory.SetCurrentDirectory(Path.GetDirectoryName(sOutputPath));

                            //Console.WriteLine("Running RBT with input file: " & sOutputPath)
                            Console.WriteLine("\n******************************************************************************");

                            // http://gis.stackexchange.com/questions/108230/arcgis-geoprocessing-and-32-64-bit-architecture-issue/108788#108788
                            ProcessStartInfo psi = new ProcessStartInfo(m_sRBTPath);
                            if (CHaMPWorkbench.Properties.Settings.Default.RBTPathVariableActive)
                            {
                                if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.RBTPathVariable))
                                {
                                    StringDictionary dictionary = psi.EnvironmentVariables;
                                    psi.EnvironmentVariables["PATH"] = "C:\\Python27\\ArcGISx6410.1;C:\\Python27\\ArcGISx6410.1\\DLLs";
                                    psi.UseShellExecute = false;
                                    psi.Arguments = sInputFile;
                                    psi.WindowStyle = m_eWindowStyle;
                                }
                            }
                            System.Diagnostics.Process proc = new Process();

                            proc.StartInfo = psi;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.WindowStyle = m_eWindowStyle;

                            if (m_eWindowStyle == System.Diagnostics.ProcessWindowStyle.Hidden)
                                proc.StartInfo.CreateNoWindow = true;

                            proc.Start();
                            proc.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                        }

                        dbCom = new SQLiteCommand("UPDATE Model_BatchRuns SET DateTimeCompleted = CURRENT_TIMESTAMP WHERE ID = @ID", dbCon);
                        dbCom.Parameters.AddWithValue("ID", aRun.ID);
                        dbCom.ExecuteNonQuery();

                        if (aRun.ClearTempWorkspaceAfter && dFiles.Count > 0)
                            ClearTempWorkspace();

                        if (bScavengeResults || bScavengeLog)
                        {
                            XmlDocument xmlR = new XmlDocument();
                            xmlR.Load(sInputFile);

                            //ResultScavengerSingle scavenger = new ResultScavengerSingle(ref m_dbCon);
                            ResultScavengerSingleCHaMP scavenger = new ResultScavengerSingleCHaMP(dbCon.ConnectionString);
                            int nResultID = 0;
                            string sResultFile = "";

                            if (bScavengeResults)
                            {
                                XmlNode aNode = xmlR.SelectSingleNode("rbt/outputs/results");
                                if (aNode is XmlNode)
                                {
                                    sResultFile = aNode.InnerText;
                                    nResultID = scavenger.ScavengeResultFile(sResultFile);
                                }
                            }

                            if (bScavengeLog)
                            {
                                XmlNode aNode = xmlR.SelectSingleNode("rbt/outputs/log");
                                if (aNode is XmlNode)
                                    scavenger.ScavengeLogFile(dbCon.ConnectionString, nResultID, aNode.InnerText, sResultFile, aRun.ID);
                            }
                        }
                    }
                }
            }
        }

        private void ClearTempWorkspace()
        {
            if (!string.IsNullOrEmpty(m_sTempRBTWorkspace))
            {
                if (Directory.Exists(m_sTempRBTWorkspace))
                {
                    string[] sFiles = Directory.GetFiles(m_sTempRBTWorkspace);
                    foreach (string aFile in sFiles)
                    {
                        System.IO.File.Delete(aFile); //, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently);
                    }

                    string[] sDirs = Directory.GetDirectories(m_sTempRBTWorkspace);
                    foreach (string aDir in sDirs)
                    {
                        System.IO.Directory.Delete(aDir); //, FileIO.DeleteDirectoryOption.DeleteAllContents);
                    }
                }
            }
        }
    }

    class RBTRun
    {
        private long m_nID;
        private string m_sInputFile;
        private bool m_bClearTempWorkspacePrior;
        private bool m_bClearTempWorkspaceAfter;
        private long m_nVisitID;

        public RBTRun(long nID, string sInputFile, bool bClearTempWorkspacePrior, bool bClearTempWorkspaceAfter, long nVisitID)
        {
            if (nID < 1)
            {
                throw new ArgumentOutOfRangeException("nID", "The ID is invalid");
            }

            if (string.IsNullOrEmpty(sInputFile))
            {
                throw new ArgumentNullException("sInputFile", "The input file is null or empty");
            }

            m_nID = nID;
            m_sInputFile = sInputFile.Replace("\"", "");
            m_bClearTempWorkspacePrior = bClearTempWorkspacePrior;
            m_bClearTempWorkspaceAfter = bClearTempWorkspaceAfter;
            m_nVisitID = nVisitID;
        }

        public long ID
        {
            get { return m_nID; }
        }

        public string InputFile
        {
            get { return m_sInputFile; }
        }

        public bool ClearTempWorkspacePrior
        {
            get { return m_bClearTempWorkspacePrior; }
        }

        public bool ClearTempWorkspaceAfter
        {
            get { return m_bClearTempWorkspaceAfter; }
        }

        public long PrimaryVisitID
        {
            get { return m_nVisitID; }
        }
    }

}