using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace CHaMPWorkbench.Classes
{
    /// <summary>
    /// 
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
        // Strings to use when naming things
        private const string m_sTopoFolder = "Topo";


        // Regex Patterns for things
        private const string m_sVisitFolder = "\\\\VISIT_{0}$";
        private const string m_sVisitFolderTest = "\\\\VISIT_[0-9]+$";
        private const string m_sTopoFolderTest = "\\\\Topo$";

        private const string m_sSurveyGDBFolder = "\\\\.*\\.gdb$";
        private const string m_sSurveyGDBOrthogFolder = "\\\\.*orthog.*\\.gdb$";

        private const string m_sTopoTINSearch = "\\\\tin.*";

        private const string m_sWSTINFolder = "\\\\(ws.*|WettedSurfaceTIN)$";

        private const string m_sHydroResultsFolder = "\\\\HydroModelResults";
        private const string m_sHydroResultsFile = "dem_grid_results.csv";

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

            if (nVisitID < 1)
                throw new ArgumentOutOfRangeException("nVisitID", nVisitID, "The visit ID must be greater than zero.");

            string sVisitFolderPattern = string.Format(m_sVisitFolder, nVisitID);
            return FolderFindRecursive(dTopLevelFolder, sVisitFolderPattern, out dVisitFolder, 4);
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
                FolderFindRecursive(dVisitFolder, m_sTopoFolderTest, out dTopoFolder, 1);

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

            if (!Topo(dTopLevelFolder, nVisitID, out dTopoFolder))
                return false;

            // Prioritize the Orthog Over the plain GDB result
            if (FolderFindRecursive(dTopoFolder, m_sSurveyGDBOrthogFolder, out dSurveyGDB, 2))
                return true;
            else
                return FolderFindRecursive(dTopoFolder, m_sSurveyGDBFolder, out dSurveyGDB, 2);
        }

        public static bool HydroResultCSV(DirectoryInfo dTopLevelFolder, int nVisitID, out FileInfo dHydroResultCSV)
        {
            dHydroResultCSV = null;
            DirectoryInfo dTopoFolder = null;

            if (!Topo(dTopLevelFolder, nVisitID, out dTopoFolder))
                return false;

            System.IO.DirectoryInfo dHydroResultsFolder = null;
            if (!FolderFindRecursive(dTopoFolder, m_sHydroResultsFolder, out dHydroResultsFolder, 3))
                return false;

            FileInfo[] fHydroResults = dHydroResultsFolder.GetFiles(m_sHydroResultsFile, SearchOption.TopDirectoryOnly);
            if (fHydroResults.Count() == 1)
            {
                dHydroResultCSV = fHydroResults[0];
                return true;
            }
            else
                return false;

        }

        public static bool D50Raster(DirectoryInfo dTopLevelFolder, string sD50RasterFile, int nVisitID, out FileInfo d50RasterFile)
        {
            d50RasterFile = null;
            DirectoryInfo dTopoFolder = null;

            if (!Topo(dTopLevelFolder, nVisitID, out dTopoFolder))
                return false;

            FileInfo[] fD50Rasters = dTopoFolder.GetFiles(sD50RasterFile, SearchOption.TopDirectoryOnly);
            if (fD50Rasters.Count() == 1)
            {
                d50RasterFile = fD50Rasters[0];
                return true;
            }
            else
                return false;
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
            return TinFolder(dTopoLevelFolder, nVisitID, m_sTopoTINSearch, out dTopoTin);
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
            return TinFolder(dTopLevelFolder, nVisitID, m_sWSTINFolder, out dWaterSurfaceTIN);
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
        private static bool TinFolder(DirectoryInfo dTopLevelFolder, int nVisitID, string sTinZipFile, out DirectoryInfo dTIN)
        {
            dTIN = null;
            DirectoryInfo dTopoFolder = null;

            if (!Topo(dTopLevelFolder, nVisitID, out dTopoFolder))
                return false;

            // Unzipped AWS bucket uses one additional folder level than the FTP
            // AWS: VISIT_XXXX\Topo\TIN\TIN\*.adf
            // FTP: VISIT_XXXX\Topo\TIN\*.adf
            // or
            // AWS: VISIT_XXXX\Topo\WettedSurfaceTIN\wsetin\*.adf
            // FTP: VISIT_XXXX\Topo\wsetin\*.adf

            DirectoryInfo dTempTin = dTopoFolder;
            while (FolderFindRecursive(dTempTin, sTinZipFile, out dTempTin, 1))
            {
                dTIN = dTempTin;
            }

            if (dTIN == null)
                return false;

            FileInfo[] fTinFiles = dTIN.GetFiles("*.adf", SearchOption.TopDirectoryOnly);
            return fTinFiles.Count<FileInfo>() > 0;

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
            dSurveyGDB = null;
            dTopoTIN = null;

            if (SurveyGDB(dTopLevelFolder, nVisitID, out dSurveyGDB))
                TopoTIN(dTopLevelFolder, nVisitID, out dTopoTIN);

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
        /// Recursively walk through a folder structure up to a specified depth and try to match a pattern
        /// </summary>
        /// <param name="root"></param>
        /// <param name="depthRemaining"></param>
        /// <param name="sSearchPattern"></param>
        /// <param name="dFolder"></param>
        /// <returns></returns>
        private static bool FolderFindRecursive(System.IO.DirectoryInfo root, string sSearchPattern, out DirectoryInfo dFolder, int depthRemaining)
        {
            dFolder = null;
            Regex r = new Regex(sSearchPattern, RegexOptions.IgnoreCase);
            System.IO.DirectoryInfo[] subDirs = null;

            if (depthRemaining <= 0)
                return false;

            // Now find all the subdirectories under this directory. 3
            subDirs = root.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                Match m = r.Match(dirInfo.FullName);
                if (m.Success)
                {
                    dFolder = dirInfo;
                    return true;
                }
                else
                {
                    bool bRecurse = FolderFindRecursive(dirInfo, sSearchPattern, out dFolder, (depthRemaining - 1));
                    if (bRecurse)
                        return true;
                }
            }
            return dFolder is DirectoryInfo && dFolder.Exists;
        }

        /// <summary>
        /// Builds the folder where the RBT outputs results, logs and artifacts
        /// </summary>
        /// <param name="sTopLevelOutputFolder">The top level of the output structure. e.g. D:\CHaMP\InputOutputFiles</param>
        /// <param name="dVisitFolder">Full, absolute path of a visit. Doesn't need to exist. Must be three levels deep. Can be rooted in watershed or year.</param>
        /// <param name="bCreateIfMissing">True will force the creation of the output path</param>
        /// <returns>Note the output includes the Topo folder. e.g. D:\CHaMP\InputOutputFiles\2012\Tucannon\CBW5583-2345\VISIT_XXXX\Topo</returns>
        /// <remarks>Note that the output folder is a string so that it can accommodate a simple period "." indicating the current directly
        /// as well as explicit paths</remarks>
        public static DirectoryInfo RBTOutputFolder(string sTopLevelOutputFolder, DirectoryInfo dVisitFolder)
        {
            Regex r = new Regex(m_sVisitFolderTest, RegexOptions.IgnoreCase);
            if (!r.Match(dVisitFolder.FullName).Success)
                throw new Exception("The visit folder must be at least 3 levels deep (watershed/year/site/visit_xxx or year/watershed/site/visit_xxx");

            DirectoryInfo dMonitoringDataFolder = dVisitFolder.Parent.Parent.Parent.Parent;

            string sVisitOutputFolder = dVisitFolder.FullName.Replace(dMonitoringDataFolder.FullName, sTopLevelOutputFolder);
            DirectoryInfo dVisitOutputFolder = new DirectoryInfo(System.IO.Path.Combine(sVisitOutputFolder, m_sTopoFolder));

            return dVisitOutputFolder;
        }

        public static FileInfo RBTInputFile(string sTopLevelOutputFolder, DirectoryInfo dVisitTopoFolder, string sInputFileName)
        {
            string sInputFile = Classes.DataFolders.RBTOutputFolder(sTopLevelOutputFolder, dVisitTopoFolder.Parent).FullName;
            sInputFile = System.IO.Path.Combine(sInputFile, sInputFileName);
            sInputFile = System.IO.Path.ChangeExtension(sInputFile, "xml");
            return new FileInfo(sInputFile);
        }
    }
}
