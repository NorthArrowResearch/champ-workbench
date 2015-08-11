using System;
using System.IO;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Xml;
using System.Diagnostics;
using System.Collections.Specialized;

namespace CHaMPWorkbench.Classes
{

public class RBTBatchEngine
{
    private OleDbConnection m_dbCon;
	private string m_sTempRBTWorkspace = "C:\\CHaMP\\TempRBTWorkspace";
	private string m_sRBTPath;
    private System.Diagnostics.ProcessWindowStyle m_eWindowStyle;
    
    public RBTBatchEngine(OleDbConnection dbCon, String sRBTConsolePath, System.Diagnostics.ProcessWindowStyle eWindowStyle)
    {
        m_dbCon =dbCon;
        m_eWindowStyle = eWindowStyle;

        if (!System.IO.File.Exists(sRBTConsolePath))
        {
            Exception ex = new Exception("The RBT Console Path does not exist.");
            ex.Data.Add("Console path", sRBTConsolePath);
            throw ex;
        }

        m_sRBTPath = sRBTConsolePath;
    }


	public void Run(bool bScavengeResults, bool bScavengeLog)
	{
		string sInputFile = null;

		Dictionary<int, RBTRun> dFiles = new Dictionary<int, RBTRun>();
		List<string> sSearchList = new List<string>();
		sSearchList.Add("rbt/sites/site/visit/filegdb");
		sSearchList.Add("rbt/sites/site/visit/topo_tin");
		sSearchList.Add("rbt/sites/site/visit/ws_tin");
		//
		// On Philip's development computer, the OLEDb version 12 connection string works when compiled with X86 architecture.
		//
		// Do not change the connection string to work on different machines. The critical factor is compiling the software as
		// either x86 or x64 to match the version of Access on the end users computer. Leaving the compiler as "AnyCPU" will cause
		// the software to load in the same architecture as the OS, but this might not match the architecture of Access (e.g. 
		// Windows 64 bit machine with 32 bit Access installed needs the software to be compiled in x86 architecture.)
		//
		if (m_dbCon.State == System.Data.ConnectionState.Closed)
			m_dbCon.Open();
		//
		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		// Get the path of the RBT and the default RBT Temp Workspace
		/*
		OleDbCommand dbCom = new OleDbCommand("SELECT * FROM RBTConfig WHERE IsActive <> 0 ORDER BY ID", m_dbCon);
		OleDbDataReader dbRdr = dbCom.ExecuteReader();

        if (dbRdr.Read())
        {
            if (System.Convert.IsDBNull(dbRdr["RBTPath"]))
            {
#if DEBUG
                m_sRBTPath = m_sDebugRBTPath;
#else
				m_sRBTPath = m_sReleaseRBTPath;
#endif
            }
            else
                m_sRBTPath = (string)dbRdr["RBTPath"];
            
            if (!System.Convert.IsDBNull(dbRdr["RBTTimeout"]))
                nRBTTimeout = (int)dbRdr["RBTTimeout"];
            
            if (!System.Convert.IsDBNull(dbRdr["RBTTempWorkspace"]))
                m_sTempRBTWorkspace = (string)dbRdr["RBTTempWorkspace"];

        }
		dbRdr.Close();
        */
		//
		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		// Now get all of the active runs
		//
		OleDbCommand dbCom = new OleDbCommand("SELECT R.*, B.BatchName FROM RBT_Batches B RIGHT JOIN RBT_BatchRuns R ON B.ID = R.BatchID WHERE (B.Run <> 0) OR (R.Run <> 0) ORDER BY B.Priority, R.Priority;", m_dbCon);
		OleDbDataReader dbRdr = dbCom.ExecuteReader();
		while (dbRdr.Read())
        {
			if (System.Convert.IsDBNull(dbRdr["InputFile"]))
				Console.WriteLine("Warning: empty input file record in database");
			else
            {
                Debug.WriteLine("RBT input file: " + (string)dbRdr["InputFile"]);
                int nVisitID = 0;
                if (!Convert.IsDBNull((int) dbRdr["PrimaryVisitID"]))
                    nVisitID = (int) dbRdr["PrimaryVisitID"];

				dFiles.Add((int) dbRdr["ID"], new RBTRun((int) dbRdr["ID"], (string) dbRdr["InputFile"], (bool) dbRdr["ClearTempPrior"], (bool) dbRdr["ClearTempAfter"],nVisitID ));
            }
		}
		dbRdr.Close();

		foreach (RBTRun aRun in dFiles.Values) {
			sInputFile = aRun.InputFile;

			dbCom = new OleDbCommand("UPDATE RBT_BatchRuns SET Run = 0, DateTimeStarted = Now(), DateTimeCompleted = NULL WHERE ID = @ID", m_dbCon);
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
                    proc.Start();
                    proc.WaitForExit();
                }
                catch (Exception ex)
                {
                }

                dbCom = new OleDbCommand("UPDATE RBT_BatchRuns SET DateTimeCompleted = Now() WHERE ID = @ID", m_dbCon);
                dbCom.Parameters.AddWithValue("ID", aRun.ID);
                dbCom.ExecuteNonQuery();

                if (aRun.ClearTempWorkspaceAfter && dFiles.Count > 0)
                    ClearTempWorkspace();


                if (bScavengeResults || bScavengeLog)
                {
                    XmlDocument xmlR = new XmlDocument();
                    xmlR.Load(sInputFile);

                    ResultScavengerSingle scavenger = new ResultScavengerSingle(ref m_dbCon);
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
                            scavenger.ScavengeLogFile(nResultID, aNode.InnerText, sResultFile);
                    }
                }
            }
		}
	}

    //private void ScavengeLogFile(String sInputFile, int nVisitID)
    //{
    //    if (System.IO.File.Exists(sInputFile))
    //    {
    //        XmlDocument xmlR = new XmlDocument();
    //        xmlR.Load(sInputFile);
    //        XmlNode logNode = xmlR.SelectSingleNode("rbt/outputs/log");

    //        if (!String.IsNullOrWhiteSpace(logNode.InnerText))
    //        {
    //            String sLogFile = logNode.InnerText;
    //            if (System.IO.File.Exists(sLogFile))
    //                ResultScavengerSingle.ScavengeLogFile(ref m_dbCon, nVisitID, sLogFile, "");
    //        }
    //    }
    //}
    
	private void ClearTempWorkspace()
	{
		if (!string.IsNullOrEmpty(m_sTempRBTWorkspace)) {
			if (Directory.Exists(m_sTempRBTWorkspace)) {
				string[] sFiles = Directory.GetFiles(m_sTempRBTWorkspace);
				foreach (string aFile in sFiles) {
				System.IO.File.Delete(aFile); //, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently);
				}

				string[] sDirs = Directory.GetDirectories(m_sTempRBTWorkspace);
				foreach (string aDir in sDirs) {
					System.IO.Directory.Delete(aDir) ; //, FileIO.DeleteDirectoryOption.DeleteAllContents);
				}
			}
		}
	}
}

class RBTRun
{
    private int m_nID;
    private string m_sInputFile;
    private bool m_bClearTempWorkspacePrior;
    private bool m_bClearTempWorkspaceAfter;
    private int m_nVisitID;

    public RBTRun(int nID, string sInputFile, bool bClearTempWorkspacePrior, bool bClearTempWorkspaceAfter, int nVisitID)
	{
		if (nID < 1) {
			throw new ArgumentOutOfRangeException("nID", "The ID is invalid");
		}

		if (string.IsNullOrEmpty(sInputFile)) {
			throw new ArgumentNullException("sInputFile", "The input file is null or empty");
		}

		m_nID = nID;
        m_sInputFile = sInputFile.Replace("\"", "");
		m_bClearTempWorkspacePrior = bClearTempWorkspacePrior;
		m_bClearTempWorkspaceAfter = bClearTempWorkspaceAfter;
        m_nVisitID = nVisitID;
	}

    public int ID
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

    public int PrimaryVisitID
    {
        get { return m_nVisitID; }
    }
}

}