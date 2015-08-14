using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CHaMPWorkbench.Classes
{
    public class VisitFolder
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

        public static bool Topo(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dTopoFolder)
        {
            DirectoryInfo dVisitFolder = null;
            dTopoFolder = null;

            if (Visit(dTopLevelFolder, nVisitID, out dVisitFolder))
                RetrieveSingleFolder(dVisitFolder, m_sTopoFolder, out dTopoFolder);

            return dTopoFolder is DirectoryInfo && dTopoFolder.Exists;
        }

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

        public static bool TopoTIN(DirectoryInfo dTopoLevelFolder, int nVisitID, out DirectoryInfo dTopoTin)
        {
            return TinFolder(dTopoLevelFolder, nVisitID, m_sTopoTINZipFile, m_sTopoTINSearch, out dTopoTin);
        }

        public static bool WaterSurfaceTIN(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dWaterSurfaceTIN)
        {
            return TinFolder(dTopLevelFolder, nVisitID, m_sWSTINZipFile, m_sWSTINSearch, out dWaterSurfaceTIN);
        }

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

        public static bool SurveyGDBTopoTin(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB, out DirectoryInfo dTopoTIN)
        {
            DirectoryInfo dTopo = null;
            dSurveyGDB = null;
            dTopoTIN = null;

            if (Topo(dTopLevelFolder, nVisitID, out dTopo))
                if (SurveyGDB(dTopLevelFolder, nVisitID, out dSurveyGDB))
                    TopoTIN(dTopLevelFolder, nVisitID, out dTopoTIN);

            return dSurveyGDB is DirectoryInfo && dTopoTIN is DirectoryInfo;
        }

        public static bool SurveyGDBTopoTinWSTin(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB, out DirectoryInfo dTopoTIN, out DirectoryInfo dWSETIN)
        {
            dSurveyGDB = null;
            dTopoTIN = null;
            dWSETIN = null;

            if (SurveyGDBTopoTin(dTopLevelFolder, nVisitID, out dSurveyGDB, out dTopoTIN))
                WaterSurfaceTIN(dTopLevelFolder, nVisitID, out dWSETIN);

            return dSurveyGDB is DirectoryInfo && dTopoTIN is DirectoryInfo && dWSETIN is DirectoryInfo;
        }

        private static bool RetrieveSingleFolder(DirectoryInfo dContainingFolder, string sSearchPatternList, out DirectoryInfo dFolder)
        {
            dFolder = null;

            foreach (string aPattern in sSearchPatternList.Split(';'))
            {
                DirectoryInfo[] dMatchingFolders = dContainingFolder.GetDirectories(aPattern, SearchOption.AllDirectories);

                switch (dMatchingFolders.Count<DirectoryInfo>())
                {
                    case 0:
                        return false;

                    case 1:
                        dFolder = dMatchingFolders[0];
                        break;

                    default:
                        throw new Exception(string.Format("Multiple ({0}) visit folders found with pattern '{1}' under {2}", dMatchingFolders.Count<DirectoryInfo>(), aPattern, dContainingFolder.FullName));

                }
            }

            return dFolder is DirectoryInfo && dFolder.Exists;
        }
    }
}
