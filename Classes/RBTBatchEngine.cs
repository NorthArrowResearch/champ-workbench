using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{


public class BatchEngine
{
    private OleDbConnection m_dbCon;

    public BatchEngine(OleDbConnection dbCon)
    {
        m_dbCon =dbCon;
    }

	private string m_sTempRBTWorkspace = "D:\\Temp\\TempRBTWorkspace";
	private string m_sRBTPath;
	private const string m_sDebugRBTPath = "D:\\Projects\\RBT\\RBTarc10\\RBTConsole\\bin\\Debug\\rbt.exe";

	private const string m_sReleaseRBTPath = "D:\\Projects\\RBT\\RBTarc10\\RBTConsole\\bin\\Release\\rbt.exe";

	public void Run(string sDatabasePath)
	{
		sDatabasePath = sDatabasePath.Trim("\"");

		if (string.IsNullOrEmpty(sDatabasePath)) {
			throw new ArgumentNullException("Null or empty database path");
		} else {

			if (IO.File.Exists(sDatabasePath)) {
				//Try
				//    ClearTempWorkspace()
				//Catch ex As Exception
				//    Throw New Exception("Error attempting to clear the temp workspace")
				//End Try

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
				using (OleDbConnection dbCon = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sDatabasePath + ";Persist Security Info=True")) {
					dbCon.Open();
					//
					///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
					// Get the path of the RBT and the default RBT Temp Workspace
					//
					OleDbCommand dbCom = new OleDbCommand("SELECT * FROM RBTConfig WHERE IsActive <> 0 ORDER BY ID", dbCon);
					OleDbDataReader dbRdr = dbCom.ExecuteReader;

					if (dbRdr.Read) {
						#if DEBUG
						if (Information.IsDBNull(dbRdr["RBTPath"])) {
							m_sRBTPath = m_sDebugRBTPath;
							#elif
							m_sRBTPath = m_sReleaseRBTPath;
							#endif
						} else {
							m_sRBTPath = dbRdr["RBTPath"];
						}

						try {
							if (!Information.IsDBNull(dbRdr["RBTTimeout"])) {
								nRBTTimeout = dbRdr["RBTTimeout"];
							}

						} catch (Exception ex) {
						}

						if (!Information.IsDBNull(dbRdr["RBTTempWorkspace"])) {
							m_sTempRBTWorkspace = dbRdr["RBTTempWorkspace"];
						}
					}
					dbRdr.Close();
					//
					///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
					// Now get all of the active runs
					//
					dbCom = new OleDbCommand("SELECT BatchRuns.*, Batches.BatchName FROM Batches RIGHT JOIN BatchRuns ON Batches.ID = BatchRuns.BatchID WHERE (((Batches.Run)<>0)) OR (((BatchRuns.Run)<>0)) ORDER BY Batches.Priority, BatchRuns.Priority;", dbCon);
					dbRdr = dbCom.ExecuteReader;
					while (dbRdr.Read) {
						if (Information.IsDBNull(dbRdr["InputFile"])) {
							Console.WriteLine("Warning: empty input file record in database");
						} else {
							dFiles.Add(dbRdr["ID"], new RBTRun(dbRdr["ID"], dbRdr["InputFile"], dbRdr["ModifyInputFile"], dbRdr["ClearTempPrior"], dbRdr["ClearTempAfter"]));
						}
					}
					dbRdr.Close();

					foreach (RBTRun aRun in dFiles.Values) {
						sInputFile = aRun.InputFile;

						dbCom = new OleDbCommand("UPDATE BatchRuns SET Run = 0, DateTimeStarted = Now(), DateTimeCompleted = NULL, Comments = NULL, LogFile=NULL, NewInputFile=NULL WHERE ID = @ID", dbCon);
						dbCom.Parameters.AddWithValue("ID", aRun.ID);
						dbCom.ExecuteNonQuery();

						if (IO.File.Exists(sInputFile)) {
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
									int i = 0;
									i = theNodes.InnerText.IndexOf("\\input\\");
									if (i > 0) {
										theNodes.InnerText = IO.Path.Combine(IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(sInputFile)), theNodes.InnerText.Substring(i + 1));
									}
								}
								//
								// Replace any relative file paths that start with a period with the absolute file path
								//
							}

							string sLogPath = inputXML.SelectSingleNode("rbt/outputs/log").InnerText;
							string sResultPath = inputXML.SelectSingleNode("rbt/outputs/results").InnerText;
							if (aRun.ModifyInputFile) {
								sLogPath = IO.Path.Combine(IO.Path.GetDirectoryName(sInputFile), "log.xml");
								// GetNewSafeFileName(IO.Path.Combine(IO.Path.GetDirectoryName(sInputFile), "log.xml"))
								sResultPath = IO.Path.Combine(IO.Path.GetDirectoryName(sInputFile), "results.xml");
								// GetNewSafeFileName(IO.Path.Combine(IO.Path.GetDirectoryName(sInputFile), "Result.xml"))

								inputXML.SelectSingleNode("rbt/outputs/log").InnerText = sLogPath;

								inputXML.SelectSingleNode("rbt/outputs/temp_workspace").InnerText = m_sTempRBTWorkspace;
								inputXML.SelectSingleNode("rbt/outputs/artifacts_path").InnerText = IO.Path.GetDirectoryName(sInputFile);

								sOutputPath = GetNewSafeFileName(sInputFile);
								inputXML.Save(sOutputPath);
								Console.WriteLine("New input file at " + sOutputPath);
							} else {
								sOutputPath = sInputFile;
							}

							int nResult = 0;
							bool bSuccess = false;
							try {
								if (aRun.ClearTempWorkspacePrior) {
									ClearTempWorkspace();
								}

								System.IO.Directory.SetCurrentDirectory(IO.Path.GetDirectoryName(sOutputPath));

								//Console.WriteLine("Running RBT with input file: " & sOutputPath)
								Console.WriteLine(Constants.vbNewLine + "******************************************************************************");
								nResult = Interaction.Shell(m_sRBTPath + " " + sOutputPath, AppWinStyle.NormalFocus, true, nRBTTimeout);
								bSuccess = true;
							} catch (Exception ex) {
								nResult = -1;
							}

							dbCom = new OleDbCommand("UPDATE BatchRuns SET DateTimeCompleted = Now(), NewInputFile = @NewInputFile, Comments = @Comments, LogFile = @LogFile, ResultFile = @ResultFile WHERE ID = @ID", dbCon);
							OleDbParameter p = new OleDbParameter("NewInputFile", OleDbType.Char, sOutputPath.Length);
							p.Value = sOutputPath.ToString;
							dbCom.Parameters.Add(p);

							p = new OleDbParameter("Comments", OleDbType.Char, nResult.ToString.Length);
							p.Value = nResult.ToString;
							dbCom.Parameters.Add(p);
							//
							///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
							// Stream the RBT log XML file into the query parameter
							//
							p = new OleDbParameter("LogFile", OleDbType.Char);
							if (bSuccess && !string.IsNullOrEmpty(sLogPath) && IO.File.Exists(sLogPath)) {
								try {
									IO.StreamReader xmlLog = new IO.StreamReader(sLogPath);
									p.Value = xmlLog.ReadToEnd;
									IO.FileInfo logFile = new IO.FileInfo(sLogPath);
									p.Size = logFile.Length;
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
							p = new OleDb.OleDbParameter("ResultFile", OleDbType.Char);
							if (bSuccess && !string.IsNullOrEmpty(sResultPath) && IO.File.Exists(sResultPath)) {
								try {
									IO.StreamReader xmlResult = new IO.StreamReader(sResultPath);
									p.Value = xmlResult.ReadToEnd;
									IO.FileInfo ResultFile = new IO.FileInfo(sResultPath);
									p.Size = ResultFile.Length;
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
							Console.WriteLine("Warning: input file path does not exist (" + aRun.ID.ToString + ") " + sInputFile);
						}

						if (aRun.ClearTempWorkspaceAfter && dFiles.Count > 0) {
							ClearTempWorkspace();
						}
					}
				}
			} else {
				throw new ArgumentException("The database file does not exist", sDatabasePath);
			}
		}
	}


	private void ClearTempWorkspace()
	{
		if (!string.IsNullOrEmpty(m_sTempRBTWorkspace)) {
			if (Directory.Exists(m_sTempRBTWorkspace)) {
				string[] sFiles = Directory.GetFiles(m_sTempRBTWorkspace);
				foreach (string aFile in sFiles) {
					Computer.FileSystem.DeleteFile(aFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently);
				}

				string[] sDirs = IO.Directory.GetDirectories(m_sTempRBTWorkspace);
				foreach (string aDir in sDirs) {
					My.Computer.FileSystem.DeleteDirectory(aDir, FileIO.DeleteDirectoryOption.DeleteAllContents);
				}
			}
		}

	}

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
				sSafeFilePath = Path.ChangeExtension(sSafeFilePath, IO.Path.GetExtension(sSuggestedPath));
			}
			//
			// Build the final path
			sSafeFilePath = Path.Combine(Path.GetDirectoryName(sSuggestedPath), sSafeFilePath);
			i += 1;
		} while (File.Exists(sSafeFilePath) && i < 1000);

		return sSafeFilePath;
	}
}

public class RBTRun
{
	private int m_nID;
	private string m_sInputFile;
	private bool m_bModifyInputFile;
	private bool m_bClearTempWorkspacePrior;
	private bool m_bClearTempWorkspaceAfter;

	public RBTRun(int nID, string sInputFile, bool bModifyInputFile, bool bClearTempWorkspacePrior, bool bClearTempWorkspaceAfter)
	{
		if (nID < 1) {
			throw new ArgumentOutOfRangeException("nID", "The ID is invalid");
		}

		if (string.IsNullOrEmpty(sInputFile)) {
			throw new ArgumentNullException("sInputFile", "The input file is null or empty");
		}

		m_nID = nID;
		m_sInputFile = sInputFile.Trim("\"");
		m_bModifyInputFile = bModifyInputFile;
		m_bClearTempWorkspacePrior = bClearTempWorkspacePrior;
		m_bClearTempWorkspaceAfter = bClearTempWorkspaceAfter;
	}

	public int ID {
		get { return m_nID; }
	}

	public string InputFile {
		get { return m_sInputFile; }
	}

	public bool ModifyInputFile {
		get { return m_bModifyInputFile; }
	}

	public bool ClearTempWorkspacePrior {
		get { return m_bClearTempWorkspacePrior; }
	}

	public bool ClearTempWorkspaceAfter {
		get { return m_bClearTempWorkspaceAfter; }
	}
}
}