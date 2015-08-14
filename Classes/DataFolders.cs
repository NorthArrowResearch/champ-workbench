using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CHaMPWorkbench.Classes
{
    /// <summary>
    /// This class is intended to make it easy to retrieve the folder paths for 
    /// CHaMP data. It is intended to work with BOTH local copies of the FTP
    /// site as well as local copies of the AWS unzipped data.
    /// 
    /// The AWS data unzip survey GDBs and TINs into a folder structure that
    /// is nested one level deeper than the AWS data.
    /// 
    /// FTP examples
    /// 
    /// VISIT_XXXX\Topo\orthogsurvey.gdb
    /// VISIT_XXXX\Topo\survey.gdb
    /// VISIT_XXXX\Topo\cbw55830045632verylongnamesurvey.gdb
    /// 
    /// VISIT_XXXX\Topo\tin
    /// VISIT_XXXX\Topo\tin1
    /// VISIT_XXXX\Topo\tin2
    /// 
    /// VISIT_XXXX\Topo\wsetin
    /// VISIT_XXXX\Topo\wsetin1
    /// VISIT_XXXX\Topo\watersurfacetin
    /// 
    /// AWS Examples
    /// 
    /// VISIT_XXXX\Topo\SurveyGDB\orthogsurvey.gdb
    /// VISIT_XXXX\Topo\SurveyGDB\survey.gdb
    /// VISIT_XXXX\Topo\SurveyGDB\cbw55830045632verylongnamesurvey.gdb
    /// 
    /// VISIT_XXXX\Topo\TIN\tin1
    /// VISIT_XXXX\Topo\TIN\tin2
    /// VISIT_XXXX\Topo\TIN\tin3
    /// 
    ///  VISIT_XXXX\Topo\WettedSurfaceTIN\wsetin
    ///  VISIT_XXXX\Topo\WettedSurfaceTIN\wsetin1
    ///  VISIT_XXXX\Topo\WettedSurfaceTIN\watersurfacetin
    /// </summary>
    public class DataFolders
    {
        private const string m_sVisitFolder = "VISIT_{0}";
        private const string m_sTopoFolder = "Topo";
        private const string m_sHydroFolder = "Hydro";
        private const string m_sRBTOutputs = "RBTOutputs";

        private const string m_sSurveyGDBZipFile = "SurveyGDB";
        private const string m_sSurveyGDBSearch = "*orthog*.gdb;*.gdb";

        private const string m_sTopoTINZipFile = "TIN";
        private const string m_sTopoTINSearch = "tin*";

        private const string m_sWSTINZipFile = "WettedSurfaceTIN";
        private const string m_sWSTINSearch = "ws*";

        /// <summary>
        /// Retrieves an existing visit folder below a top level folder
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dVisitFolder">Output, full absolute folder path to the visit. Null if does not exist.</param>
        /// <returns>True if the visit folder is valid and exists, otherwise false.</returns>
        public static bool Visit(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dVisitFolder)
        {
            dVisitFolder = null;

            if (!dTopLevelFolder.Exists)
                throw new ArgumentException("The top level folder must already exist.");

            if (nVisitID < 1)
                throw new ArgumentOutOfRangeException("nVisitID", nVisitID, "The visit ID must be greater than zero.");

            string sVisitFolderPattern = string.Format(m_sVisitFolder, nVisitID);
            return RetrieveSingleFolder(dTopLevelFolder, sVisitFolderPattern, out dVisitFolder);
        }

        /// <summary>
        /// Retrieves an existing topo data folder inside a top level folder
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dTopoFolder">Output, full absolute folder path to the TopoFolder within the visit folder. Null if does not exist.</param>
        /// <returns>True if the topo folder is valid and exists, otherwise false.</returns>
        public static bool Topo(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dTopoFolder)
        {
            DirectoryInfo dVisitFolder = null;
            dTopoFolder = null;

            if (Visit(dTopLevelFolder, nVisitID, out dVisitFolder))
                RetrieveSingleFolder(dVisitFolder, m_sTopoFolder, out dTopoFolder);

            return dTopoFolder is DirectoryInfo && dTopoFolder.Exists;
        }

        /// <summary>
        /// Retrieves an existing path to a survey GDB inside the top level folder for a particular visit.
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dSurveyGDB">Output, full absolute folder path to the TopoFolder within the visit folder. Null if does not exist.</param>
        /// <returns>True if the survey GDB is valid and exists, otherwise false.</returns>
        public static bool SurveyGDB(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB)
        {
            dSurveyGDB = null;
            DirectoryInfo dTopoFolder = null;

            if (!Topo(dTopoFolder, nVisitID, out dTopoFolder))
                return false;

            // Unzipped AWS bucket uses one additional folder level than the FTP
            // AWS: VISIT_XXXX\Topo\SurveyGDB\MySurvey.gdb
            // FTP: VISIT_XXXX\Topo\MySurvey.gdb
            DirectoryInfo dSurveyZipFolder = null;
            if (RetrieveSingleFolder(dTopoFolder, m_sSurveyGDBZipFile, out  dSurveyZipFolder))
                dTopoFolder = dSurveyZipFolder;

            return RetrieveSingleFolder(dTopoFolder, m_sSurveyGDBSearch, out dSurveyGDB);
        }

        /// <summary>
        /// Retrieves an existing path to a Topo TIN inside the top level folder for a particular visit.
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dTopoTin">Output, full absolute folder path to the topo TIN. Null if does not exist.</param>
        /// <returns>True if the topo TIN is valid and exists, otherwise null</returns>
        public static bool TopoTIN(DirectoryInfo dTopoLevelFolder, int nVisitID, out DirectoryInfo dTopoTin)
        {
            return TinFolder(dTopoLevelFolder, nVisitID, m_sTopoTINZipFile, m_sTopoTINSearch, out dTopoTin);
        }

        /// <summary>
        /// Retrieves an existing path to a water surface TIN inside the top level folder for a particular visit
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dWaterSurfaceTIN">Output, full absolute path to the water surface TIN. Null if does not exist</param>
        /// <returns>True if the topo TIN is valid and exists, otherwise Null.</returns>
        public static bool WaterSurfaceTIN(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dWaterSurfaceTIN)
        {
            return TinFolder(dTopLevelFolder, nVisitID, m_sWSTINZipFile, m_sWSTINSearch, out dWaterSurfaceTIN);
        }

        /// <summary>
        /// Private generic method for finding a TIN. Used by the two public methods for topo and water surface TINs
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="sTinZipFile">Name of the FTP site Zip File that AWS unzips the TIN into. See class constants</param>
        /// <param name="sTinSearchPattern">Semi colon concatenated list of wildcarded paths for TIN name. See class constants</param>
        /// <param name="dTIN">Output, full absolute path to the TIN. Null if does not exist</param>
        /// <returns>True if the TIN is found, orthwise Null</returns>
        private static bool TinFolder(DirectoryInfo dTopLevelFolder, int nVisitID, string sTinZipFile, string sTinSearchPattern, out DirectoryInfo dTIN)
        {
            dTIN = null;
            DirectoryInfo dTopoFolder = null;

            if (!Topo(dTopoFolder, nVisitID, out dTopoFolder))
                return false;

            // Unzipped AWS bucket uses one additional folder level than the FTP
            // AWS: VISIT_XXXX\Topo\TIN\TIN\*.adf
            // FTP: VISIT_XXXX\Topo\TIN\*.adf
            // or
            // AWS: VISIT_XXXX\Topo\WettedSurfaceTIN\wsetin\*.adf
            // FTP: VISIT_XXXX\Topo\wsetin\*.adf

            DirectoryInfo dTin = null;
            if (RetrieveSingleFolder(dTopoFolder, sTinZipFile, out dTin))
            {
                // Folder matching the TIN search pattern found. Could be FTP data
                // with a tin called "tin", "tin1", "wsetin1" etc or could be AWS
                // data and the TIN folder is nested lower.

                FileInfo[] fTinFiles = dTin.GetFiles("*.adf", SearchOption.TopDirectoryOnly);
                if (fTinFiles.Count<FileInfo>() == 0)
                {
                    // There is a folder called TIN under TopoData but it does not contain any ADF files
                    // which suggests this is AWS data. Look inside for the actual tin folder.
                    RetrieveSingleFolder(dTin, sTinSearchPattern, out dTin);
                }
                else
                {
                    // ADF files were found in the folder called under TopoData.
                    // This suggests it is FTP data.
                }
            }
            else
            {
                // No folder matching the AWS zip file was found. This could still be FTP
                // data with a tin called "tin1" or "wsetin" or "wsetin1"
                RetrieveSingleFolder(dTopoFolder, sTinSearchPattern, out dTin);
            }

            return dTin is DirectoryInfo && dTin.Exists;

        }

        /// <summary>
        /// Overloaded method for finding the Survey GDB and Topo TIN paths in one call
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dSurveyGDB">Output, full absolute path to the survey GDB. Null if doesn't exist.</param>
        /// <param name="dTopoTIN">Output, full absolute path to the Topo TIN. Null if doesn't exist</param>
        /// <returns>True if both the survey GDB and topo tin exist. False if either is missing.</returns>
        /// <remarks>Note that this method will find either path regardless of whether one is missing.
        /// The return value will only be true if both paths exist though</remarks>
        public static bool SurveyGDBTopoTin(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB, out DirectoryInfo dTopoTIN)
        {
            DirectoryInfo dTopo = null;
            dSurveyGDB = null;
            dTopoTIN = null;

            if (Topo(dTopLevelFolder, nVisitID, out dTopo))
            {
                SurveyGDB(dTopLevelFolder, nVisitID, out dSurveyGDB);
                TopoTIN(dTopLevelFolder, nVisitID, out dTopoTIN);
            }

            return dSurveyGDB is DirectoryInfo && dTopoTIN is DirectoryInfo;
        }

        /// <summary>
        /// Overloaded method for finding the Survey GDB, Topo TIN and water surface TIN paths in one call
        /// </summary>
        /// <param name="dTopLevelFolder">Existing top level monitoring data folder inside which are either years or watersheds</param>
        /// <param name="nVisitID">Unique Visit ID</param>
        /// <param name="dSurveyGDB">Output, full absolute path to the survey GDB. Null if doesn't exist.</param>
        /// <param name="dTopoTIN">Output, full absolute path to the Topo TIN. Null if doesn't exist</param>
        /// <param name="dWSETIN">Output, full absolute path to the water surface TIN. Null if it doesn't exist</param>
        /// <returns>True if all three paths exist. False if any one is missing</returns>
        /// <remarks>Note that this method will find and return any paths that exist even if one or more are missing.
        /// But the return value will only be true if all three are found and exist</remarks>
        public static bool SurveyGDBTopoTinWSTin(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB, out DirectoryInfo dTopoTIN, out DirectoryInfo dWSETIN)
        {
            dSurveyGDB = null;
            dTopoTIN = null;
            dWSETIN = null;

            if (SurveyGDBTopoTin(dTopLevelFolder, nVisitID, out dSurveyGDB, out dTopoTIN))
                WaterSurfaceTIN(dTopLevelFolder, nVisitID, out dWSETIN);

            return dSurveyGDB is DirectoryInfo && dTopoTIN is DirectoryInfo && dWSETIN is DirectoryInfo;
        }

        /// <summary>
        /// Private method for finding a single folder under the top level that matches the search pattern (list)
        /// </summary>
        /// <param name="dContainingFolder">Folder in which to look for subfolders</param>
        /// <param name="sSearchPatternList">The search pattern to look foder. Can include wildcards (tin*) and can be a semi colon separated list (orthog*.gdb;*.gdb)</param>
        /// <param name="dFolder">Output, full absolute path to the first folder that matches the search pattern</param>
        /// <returns>True if exactly one folder is found matching the search pattern, otherwise false</returns>
        /// <remarks>If the search pattern is a list then the list is searched in order. As soon as one directory
        /// matches a pattern, the method exists with success.</remarks>
        private static bool RetrieveSingleFolder(DirectoryInfo dContainingFolder, string sSearchPatternList, out DirectoryInfo dFolder)
        {
            dFolder = null;

            // Loop over all the search patterns, or just the name to look for if this is not a list
            foreach (string aPattern in sSearchPatternList.Split(';'))
            {
                // **Recursively** find all folders below the top level that match the pattern
                DirectoryInfo[] dMatchingFolders = dContainingFolder.GetDirectories(aPattern, SearchOption.AllDirectories);
                switch (dMatchingFolders.Count<DirectoryInfo>())
                {
                    case 0:
                        // No directories match this pattern. Continue looping through patterns.
                        break;

                    case 1:
                        // The first pattern that matches exactly one directly is considered success
                        dFolder = dMatchingFolders[0];
                        return true;

                    default:
                        // No pattern is allowed to match multiple directories (because additional logic would be required to pick which is correct)
                        throw new Exception(string.Format("Multiple ({0}) visit folders found with pattern '{1}' under {2}", dMatchingFolders.Count<DirectoryInfo>(), aPattern, dContainingFolder.FullName));
                }
            }

            return dFolder is DirectoryInfo && dFolder.Exists;
        }

        /// <summary>
        /// Builds the folder where the RBT outputs results, logs and artifacts
        /// </summary>
        /// <param name="dTopLevelOutputFolder">The top level of the output structure. e.g. D:\CHaMP\InputOutputFiles</param>
        /// <param name="dVisitFolder">Full, absolute path of a visit. Doesn't need to exist. Must be three levels deep. Can be rooted in watershed or year.</param>
        /// <param name="bCreateIfMissing">True will force the creation of the output path</param>
        /// <returns>Note the output includes the Topo folder. e.g. D:\CHaMP\InputOutputFiles\2012\Tucannon\CBW5583-2345\VISIT_XXXX\Topo</returns>
        public static DirectoryInfo RBTOutputFolder(DirectoryInfo dTopLevelOutputFolder, DirectoryInfo dVisitFolder, bool bCreateIfMissing = false)
        {
            if (!(dVisitFolder.FullName.Split(System.IO.Path.DirectorySeparatorChar).Count<string>() < 3))
                throw new Exception("The visit folder must be at least 3 levels deep (watershed/year/site/visit_xxx or year/watershed/site/visit_xxx");

            DirectoryInfo dMonitoringDataFolder = dVisitFolder.Parent.Parent.Parent;

            string sVisitOutputFolder = dVisitFolder.FullName.Replace(dMonitoringDataFolder.FullName, dTopLevelOutputFolder.FullName);
            DirectoryInfo dVisitOutputFolder = new DirectoryInfo(System.IO.Path.Combine(sVisitOutputFolder,m_sTopoFolder));

            if (bCreateIfMissing)
                dVisitFolder.Create();

            return dVisitOutputFolder;
        }
    }
}
