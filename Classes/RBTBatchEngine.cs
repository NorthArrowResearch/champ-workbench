using System;
using System.IO;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{

public class RBTBatchEngine
{
    private OleDbConnection m_dbCon;

    public RBTBatchEngine(OleDbConnection dbCon)
    {
        m_dbCon =dbCon;
    }

	private string m_sTempRBTWorkspace = "D:\\Temp\\TempRBTWorkspace";
	private string m_sRBTPath;
	private const string m_sDebugRBTPath = "D:\\Projects\\RBT\\RBTarc10\\RBTConsole\\bin\\Debug\\rbt.exe";

	private const string m_sReleaseRBTPath = "D:\\Projects\\RBT\\RBTarc10\\RBTConsole\\bin\\Release\\rbt.exe";

	public void Run()
	{
		string sInputFile = null;
		XmlDocument inputXML = default(XmlDocument);
		string sOutputPath = null;
		int nRBTTimeout = -1;

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
		//
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
		//
		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		// Now get all of the active runs
		//
		dbCom = new OleDbCommand("SELECT B.*, B.BatchName FROM RBT_Batches B RIGHT JOIN RBT_BatchRuns R ON B.ID = R.BatchID WHERE (B.Run <> 0) OR (R.Run <> 0) ORDER BY B.Priority, R.Priority;", m_dbCon);
		dbRdr = dbCom.ExecuteReader();
		while (dbRdr.Read())
        {
			if (System.Convert.IsDBNull(dbRdr["InputFile"]))
				Console.WriteLine("Warning: empty input file record in database");
			else
				dFiles.Add((int) dbRdr["ID"], new RBTRun((int) dbRdr["ID"], (string) dbRdr["InputFile"], (bool) dbRdr["ClearTempPrior"], (bool) dbRdr["ClearTempAfter"]));
		}
		dbRdr.Close();

		foreach (RBTRun aRun in dFiles.Values) {
			sInputFile = aRun.InputFile;

			dbCom = new OleDbCommand("UPDATE BatchRuns SET Run = 0, DateTimeStarted = Now(), DateTimeCompleted = NULL, Comments = NULL, LogFile=NULL, NewInputFile=NULL WHERE ID = @ID", m_dbCon);
			dbCom.Parameters.AddWithValue("ID", aRun.ID);
			dbCom.ExecuteNonQuery();

			if (File.Exists(sInputFile)) {
				inputXML = new XmlDocument();
				inputXML.Load(sInputFile);
				//
				// Loop through the File GDB, TIN and WS TIN (the nodes with file paths)
				//
				foreach (string sSearchItem in sSearchList) {
					//
					// Replace any absolute file paths that include the path of the input XML file
					//
					foreach (XmlNode theNodes in inputXML.SelectNodes(sSearchItem)) {
						int i = theNodes.InnerText.IndexOf("\\input\\");
						if (i > 0) {
							theNodes.InnerText = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(sInputFile)), theNodes.InnerText.Substring(i + 1));
						}
					}
					//
					// Replace any relative file paths that start with a period with the absolute file path
					//
				}

				string sLogPath = inputXML.SelectSingleNode("rbt/outputs/log").InnerText;
				string sResultPath = inputXML.SelectSingleNode("rbt/outputs/results").InnerText;
				sOutputPath = sInputFile;
				
				int nResult = 0;
				bool bSuccess = false;
				try {
					if (aRun.ClearTempWorkspacePrior) {
						ClearTempWorkspace();
					}

					System.IO.Directory.SetCurrentDirectory(Path.GetDirectoryName(sOutputPath));

					//Console.WriteLine("Running RBT with input file: " & sOutputPath)
					Console.WriteLine("\n******************************************************************************");
                    System.Diagnostics.Process.Start(m_sRBTPath ,sOutputPath); //, AppWinStyle.NormalFocus, true, nRBTTimeout);
					bSuccess = true;
				} catch (Exception ex) {
					nResult = -1;
				}

				dbCom = new OleDbCommand("UPDATE BatchRuns SET DateTimeCompleted = Now(), NewInputFile = @NewInputFile, Comments = @Comments, LogFile = @LogFile, ResultFile = @ResultFile WHERE ID = @ID", m_dbCon);
				OleDbParameter p = new OleDbParameter("NewInputFile", OleDbType.Char, sOutputPath.Length);
				p.Value = sOutputPath.ToString();
				dbCom.Parameters.Add(p);

				p = new OleDbParameter("Comments", OleDbType.Char, nResult.ToString().Length);
				p.Value = nResult.ToString();
				dbCom.Parameters.Add(p);
				//
				///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
				// Stream the RBT log XML file into the query parameter
				//
				p = new OleDbParameter("LogFile", OleDbType.Char);
				if (bSuccess && !string.IsNullOrEmpty(sLogPath) && File.Exists(sLogPath)) {
					try {
						StreamReader xmlLog = new StreamReader(sLogPath);
						p.Value = xmlLog.ReadToEnd();
						FileInfo logFile = new FileInfo(sLogPath);
						p.Size = (int) logFile.Length;
						xmlLog.Close();
					} catch (Exception ex) {
						p.Size = 0;
						p.Value = DBNull.Value;
					}
				} else {
					p.Size = 0;
					p.Value = DBNull.Value;
				}
				dbCom.Parameters.Add(p);
				//
				///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
				// Stream the RBT result XML file into the query parameter
				//
				p = new OleDbParameter("ResultFile", OleDbType.Char);
				if (bSuccess && !string.IsNullOrEmpty(sResultPath) && File.Exists(sResultPath)) {
					try {
						StreamReader xmlResult = new System.IO.StreamReader(sResultPath);
						p.Value = xmlResult.ReadToEnd();
						FileInfo ResultFile = new FileInfo(sResultPath);
						p.Size = (int)ResultFile.Length;
						xmlResult.Close();
					} catch (Exception ex) {
						p.Size = 0;
						p.Value = DBNull.Value;
					}
				} else {
					p.Size = 0;
					p.Value = DBNull.Value;
				}
				dbCom.Parameters.Add(p);

				dbCom.Parameters.AddWithValue("ID", aRun.ID);
				dbCom.ExecuteNonQuery();
			} else {
				Console.WriteLine("Warning: input file path does not exist (" + aRun.ID.ToString() + ") " + sInputFile);
			}

			if (aRun.ClearTempWorkspaceAfter && dFiles.Count > 0) {
				ClearTempWorkspace();
			}
		}
	}


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

    /*
	/// <summary>
	/// Gets a file name that doesn't already exist, based on a suggested file path.
	/// </summary>
	/// <param name="sSuggestedPath"></param>
	/// <returns></returns>
	/// <remarks>Use this method to get a file path that is guaranteed to be unique based on a suggested path and file name
	/// that might already exist</remarks>
	private string GetNewSafeFileName(string sSuggestedPath)
	{
		if (string.IsNullOrEmpty(sSuggestedPath))
			return string.Empty;

		string sSafeFilePath = "";
		int i = 0;
		do {
			// 
			// Get the initially suggested file name without an extension
			//
			sSafeFilePath = Path.GetFileNameWithoutExtension(sSuggestedPath);
			if (i > 0) {
				sSafeFilePath += i.ToString();
			}
			//
			// Add back any extension
			//
			if (Path.HasExtension(sSuggestedPath)) {
				sSafeFilePath = Path.ChangeExtension(sSafeFilePath, Path.GetExtension(sSuggestedPath));
			}
			//
			// Build the final path
			sSafeFilePath = Path.Combine(Path.GetDirectoryName(sSuggestedPath), sSafeFilePath);
			i += 1;
		} while (File.Exists(sSafeFilePath) && i < 1000);

		return sSafeFilePath;
	}
     */
}

class RBTRun
{
    private int m_nID;
    private string m_sInputFile;
    private bool m_bClearTempWorkspacePrior;
    private bool m_bClearTempWorkspaceAfter;

    public RBTRun(int nID, string sInputFile, bool bClearTempWorkspacePrior, bool bClearTempWorkspaceAfter)
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
}

}