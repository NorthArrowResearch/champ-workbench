using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMUI.Classes
{
    static public class Paths
    {
        private static string _sInputsFolder = "Inputs";
        private static string _sSimulationsFolder = "Simulations";
        private static string m_sSimulationXMLFolder = "XML";
        private static string m_sPreparedInputsFolder = "PreparedInputs";
        private static string m_sOutputsFolder = "Outputs";
        private static string m_sHSCOutputsFolder = "HSOutputs";
        private static string _sHSCFolder = "HSC";

        private const string m_sRunFilePrefix = "run_yyyyMMdd_HHmmss_";

        public enum ProjectInputType
        {
            Text,
            Raster,
            Shapefile,
        }

        public static string GetRelativePath(string sFullPath)
        {
            if (string.IsNullOrEmpty(HMUI.Classes.HSProjectManager.ProjectFolder))
            {
                throw new Exception("The database path must be provided");
            }

            int iIndex = sFullPath.ToLower().IndexOf(HMUI.Classes.HSProjectManager.ProjectFolder.ToLower());
            if (iIndex >= 0)
            {
                sFullPath = sFullPath.Substring(HMUI.Classes.HSProjectManager.ProjectFolder.Length, sFullPath.Length - HMUI.Classes.HSProjectManager.ProjectFolder.Length);
                sFullPath = sFullPath.TrimStart(System.IO.Path.DirectorySeparatorChar);
            }

            return sFullPath;
        }

        public static string GetAbsolutePath(string sRelativePath)
        {
            return System.IO.Path.Combine(HMUI.Classes.HSProjectManager.ProjectFolder, sRelativePath);
        }

        //public static bool IsDatabaseInDirectory(string sDbPath)
        //{
        //    bool bDatabaseInDirectory = false;
        //    string[] lDBFiles = System.IO.Directory.GetFiles(System.IO.get, "*.accdb");
        //    if (lDBFiles.Length >= 0)
        //    {
        //        bDatabaseInDirectory = true;
        //    }
        //    return bDatabaseInDirectory;
        //}

        #region INPUTS
        public static string GetProjectInputsFolder()
        {
            return System.IO.Path.Combine(HMUI.Classes.HSProjectManager.ProjectFolder, _sInputsFolder);
        }

        public static string GetSpecificInputFolder(string sInputName)
        {
            string sCleanName = RemoveDangerousCharacters(System.IO.Path.GetFileNameWithoutExtension(sInputName));
            string sSpecificInputFolder = System.IO.Path.Combine(GetProjectInputsFolder(), System.IO.Path.GetFileNameWithoutExtension(sCleanName));
            return sSpecificInputFolder;
        }

        public static string GetSpecificInputFullPath(string sInputName, string sExtension)
        {
            string sSpecificInputPath = GetSpecificInputFolder(sInputName);
            //string sInputExtension = System.IO.Path.GetExtension(sInputName);
            string sCleanInputName = HMUI.Classes.Paths.RemoveDangerousCharacters(System.IO.Path.GetFileNameWithoutExtension(sInputName));
            sSpecificInputPath = System.IO.Path.Combine(sSpecificInputPath, sCleanInputName);
            sSpecificInputPath = System.IO.Path.ChangeExtension(sSpecificInputPath, sExtension);
            return sSpecificInputPath;
        }

        public static bool CopyInputToProject(string sInputName, string sOriginalInputPath)
        {
            bool bSuccess = true;
            try
            {
                //Create folder and copy the input to the new folder (in later work we will check for orthogonality, etc.
                sInputName = RemoveDangerousCharacters(sInputName);
                string sInputPath = HMUI.Classes.Paths.GetProjectInputsFolder();
                if (!System.IO.Directory.Exists(sInputPath))
                {
                    System.IO.Directory.CreateDirectory(sInputPath);
                }

                if (System.IO.Directory.Exists(sInputPath))
                {
                    string sSpecificInputFolderPath = HMUI.Classes.Paths.GetSpecificInputFolder(sInputName);
                    if (!System.IO.Directory.Exists(sSpecificInputFolderPath))
                    {
                        System.IO.Directory.CreateDirectory(sSpecificInputFolderPath);
                    }
                    if (System.IO.Directory.Exists(sSpecificInputFolderPath))
                    {
                        string sOriginalExtension = System.IO.Path.GetExtension(sOriginalInputPath);
                        string sSpecificInputFullPath = HMUI.Classes.Paths.GetSpecificInputFullPath(sInputName, sOriginalExtension);


                        if (string.Compare(sOriginalExtension, ".txt") != 0)
                        {
                            string[] lAssociatedFiles = GetBaseFileMatches(sOriginalInputPath);
                            foreach (string sFilePath in lAssociatedFiles)
                            {
                                string sFileName = System.IO.Path.GetFileName(sFilePath);
                                string sExtension = System.IO.Path.GetExtension(sFilePath);
                                string sCopyFilePath = GetSpecificInputFullPath(sInputName, sExtension);
                                sOriginalInputPath = System.IO.Path.ChangeExtension(sOriginalInputPath, sExtension);
                                if (System.IO.File.Exists(sOriginalInputPath))
                                {
                                    System.IO.File.Copy(sOriginalInputPath, sCopyFilePath);
                                }
                            }
                        }
                        else
                        {
                            System.IO.File.Copy(sOriginalInputPath, sSpecificInputFullPath);
                        }
                    }
                }
            }

            catch
            {
                throw new Exception("Unable to copy files to project directory. Project input not added to project database.");
                //return bSuccess = false;
            }

            return bSuccess;
        }

        #endregion


        #region SIMULATIONS

        public static string GetSimulationsFolder()
        {
            string sSimulationsFolder = System.IO.Path.Combine(HSProjectManager.ProjectFolder, _sSimulationsFolder);
            return sSimulationsFolder;
        }

        public static string GetSimulationXMLFolder()
        {
            string sSimulationXMLFolder = System.IO.Path.Combine(HSProjectManager.ProjectFolder, m_sSimulationXMLFolder);
            return sSimulationXMLFolder;
        }

        public static string GetSpecificSimulationFolder(string sSimulationName)
        {
            sSimulationName = RemoveDangerousCharacters(sSimulationName);
            string sSpecificSimulationFolder = System.IO.Path.Combine(GetSimulationsFolder(), sSimulationName);
            return sSpecificSimulationFolder;
        }

        public static string GetSpecificSimulationInputsFolder(string sSimulationName)
        {
            string sFolder = GetSpecificSimulationFolder(sSimulationName);
            sFolder = System.IO.Path.Combine(sFolder, m_sPreparedInputsFolder);
            return sFolder;
        }

        public static string GetSpecificSimulationOutputsFolder(string sSimulationName)
        {
            string sFolder = GetSpecificSimulationFolder(sSimulationName);
            sFolder = System.IO.Path.Combine(sFolder, m_sOutputsFolder);
            return sFolder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSimulationName"></param>
        /// <returns>C:\ProjectDir\Simulations\MySimulation\Outputs\HSOutputs</returns>
        public static string GetSpecificSimulationHSOutputsFolder(string sSimulationName)
        {
            string sFolder = GetSpecificSimulationFolder(sSimulationName);
            sFolder = System.IO.Path.Combine(sFolder, m_sOutputsFolder);
            sFolder = System.IO.Path.Combine(sFolder, m_sHSCOutputsFolder);
            return sFolder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSimulationName"></param>
        /// <returns>C:\ProjectDir\Simulations\MySimulation\PreparedInputs</returns>
        public static string GetSpecificSimulationHSPreparedFolder(string sSimulationName)
        {
            string sFolder = GetSpecificSimulationFolder(sSimulationName);
            sFolder = System.IO.Path.Combine(sFolder, m_sPreparedInputsFolder);
            return sFolder;
        }

        public static string GetUniqueSimulationInputFile()
        {
            return GetModelRunXMLFile("input");
        }

        public static string GetUniqueSimulationOutputFile()
        {
            return GetModelRunXMLFile("output");
        }

        private static string GetModelRunXMLFile(string sSuffix)
        {
            string sXMLFolder = Classes.Paths.GetSimulationXMLFolder();
            string sXMLFile = System.IO.Path.Combine(sXMLFolder, DateTime.Now.ToString(m_sRunFilePrefix) + sSuffix);
            sXMLFile = System.IO.Path.ChangeExtension(sXMLFile, "xml");
            return sXMLFile;
        }

        //public static string GetSpecificSimulationRunFolder(string dbPath, string sSimulationName, string sTimeStamp)
        //{
        //    string sCleanSimulationName = RemoveDangerousCharacters(sSimulationName);
        //    string sSpecificSimulationRunFolder = GetSpecificSimulationFolder(dbPath, sCleanSimulationName);
        //    sSpecificSimulationRunFolder = System.IO.Path.Combine(sSpecificSimulationRunFolder, sCleanSimulationName + "_" + sTimeStamp);
        //    return sSpecificSimulationRunFolder;
        //}

        /// <summary>
        /// Gets the file path to the main HSI or FIS habitat output
        /// </summary>
        /// <param name="sSimulationName"></param>
        /// <param name="sOutputName"></param>
        /// <returns>Simulations\MySim\Outputs\MySim.tif</returns>
        public static string GetSpecificOutputFullPath(string sSimulationName)
        {
            string sFolder = GetSpecificSimulationOutputsFolder(sSimulationName);
            string sOutput = RemoveDangerousCharacters(sSimulationName);
            sOutput = System.IO.Path.Combine(sFolder, sOutput);
            sOutput = System.IO.Path.ChangeExtension(sOutput, "tif");
            return sOutput;
        }

        /// <summary>
        /// Gets the file path to the HSC output for a particular HSC input
        /// </summary>
        /// <param name="sSimulationName"></param>
        /// <param name="sOutputName"></param>
        /// <returns>Simulations\MySim\Outputs\HSOutputs\Velocity.tif</returns>
        public static string GetSpecificOutputHSFullPath(string sSimulationName, string sOutputName)
        {
            string sFolder = GetSpecificSimulationHSOutputsFolder(sSimulationName);
            string sHSOutput = RemoveDangerousCharacters(sOutputName);
            sHSOutput = System.IO.Path.Combine(sFolder, sOutputName);
            sHSOutput = System.IO.Path.ChangeExtension(sHSOutput, "tif");
            return sHSOutput;
        }

        public static string GetSpecificPreparedHSFullPath(string sSimulationName, string sOutputName)
        {
            string sFolder = GetSpecificSimulationHSPreparedFolder(sSimulationName);
            string sHSOutput = RemoveDangerousCharacters(sOutputName);
            sHSOutput = System.IO.Path.Combine(sFolder, sOutputName);
            sHSOutput = System.IO.Path.ChangeExtension(sHSOutput, "tif");
            return sHSOutput;
        }




        #endregion

        public static string RemoveDangerousCharacters(string sInput)
        {
            string sResult = sInput;
            if (!string.IsNullOrEmpty(sInput))
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    sResult = sResult.Replace(c, '\0');
                }

            sResult = sResult.Replace("-", "");
            sResult = sResult.Replace(" ", "");
            sResult = sResult.Replace(".", "");
            sResult = sResult.Replace("!", "");
            sResult = sResult.Replace("@", "");
            sResult = sResult.Replace("#", "");
            sResult = sResult.Replace("$", "");
            sResult = sResult.Replace("%", "");
            sResult = sResult.Replace("^", "");
            sResult = sResult.Replace("&", "");
            sResult = sResult.Replace("*", "");
            sResult = sResult.Replace("(", "");
            sResult = sResult.Replace(")", "");
            sResult = sResult.Replace("+", "");
            sResult = sResult.Replace("=", "");
            sResult = sResult.Replace("'", "");
            sResult = sResult.Replace("~", "");
            sResult = sResult.Replace("`", "");
            sResult = sResult.Replace("{", "");
            sResult = sResult.Replace("}", "");
            sResult = sResult.Replace("[", "");
            sResult = sResult.Replace("]", "");
            sResult = sResult.Replace(";", "");
            sResult = sResult.Replace(",", "");
            sResult = sResult.Replace("<", "");
            sResult = sResult.Replace(">", "");
            sResult = sResult.Replace("\0", "");

            return sResult;
        }

        public static string[] GetBaseFileMatches(string sFullPath)
        {
            string sBaseName = System.IO.Path.GetFileNameWithoutExtension(sFullPath) + ".*";
            string sDirectory = System.IO.Path.GetDirectoryName(sFullPath);
            string[] lAssociatedFiles = System.IO.Directory.GetFiles(sDirectory, sBaseName);
            return lAssociatedFiles;
        }

        public static bool ConfirmValidDirectory(string sPath)
        {
            bool bValid = false;
            try
            {
                if (!String.IsNullOrEmpty(sPath))
                {
                    string sDirectory = System.IO.Path.GetDirectoryName(sPath);
                    if (System.IO.Path.IsPathRooted(sDirectory))
                    {
                        if (System.IO.Directory.Exists(sDirectory))
                        {
                            bValid = true;
                            return bValid;
                        }
                    }
                }
                return bValid;
            }
            catch
            {
                return bValid;
            }
        }

        public static bool ConfirmValidFile(string sPath)
        {
            bool bValid = false;
            try
            {
                if (!String.IsNullOrEmpty(sPath))
                {
                    string sDirectory = System.IO.Path.GetDirectoryName(sPath);
                    if (System.IO.Path.IsPathRooted(sDirectory))
                    {
                        if (!System.IO.Directory.Exists(sDirectory))
                        {
                            bValid = true;
                            return bValid;
                        }
                    }
                }
                return bValid;
            }
            catch
            {
                return bValid;
            }
        }
        public static string GetTempFilePath(string sRoot, string sExtension, string sFolder = "")
        {
            if (string.IsNullOrEmpty(sExtension))
                throw new Exception("Must provide a file extension");

            if (string.IsNullOrEmpty(sRoot))
                sRoot = "HaibtatTempFile";
            else
                sRoot = RemoveDangerousCharacters(sRoot);
            
            if (string.IsNullOrEmpty(sFolder) || !System.IO.Directory.Exists(sFolder))
                sFolder = Environment.GetEnvironmentVariable("TEMP");

            int i = 0;
            string sPath = System.IO.Path.ChangeExtension(System.IO.Path.Combine(sFolder, sRoot), sExtension);
            while (System.IO.File.Exists(sPath))
            {
                sPath = System.IO.Path.Combine(sFolder, sRoot);
                if (i > 0)
                    sPath += i.ToString();

                sPath = System.IO.Path.ChangeExtension(sPath, sExtension);
                i++;
            }
            
            return sPath;
        }
    }


}
