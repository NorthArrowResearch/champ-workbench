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

        public static bool VisitData(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dVisitFolder)
        {
            dVisitFolder = null;

            if (!dTopLevelFolder.Exists)
                throw new ArgumentException("The top level folder must already exist.");

            if (nVisitID < 1)
                throw new ArgumentOutOfRangeException("nVisitID", nVisitID, "The visit ID must be greater than zero.");

            string sVisitFolderPattern = string.Format(m_sVisitFolder, nVisitID);

            return RetrieveSingleFolder(dTopLevelFolder, sVisitFolderPattern, out dVisitFolder);
        }

        public static bool TopoData(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dTopoFolder)
        {
            DirectoryInfo dVisitFolder = null;
            dTopoFolder = null;

            if (VisitData(dTopLevelFolder, nVisitID, out dVisitFolder))
                RetrieveSingleFolder(dVisitFolder, m_sTopoFolder, out dTopoFolder);

            return dTopoFolder is DirectoryInfo && dTopoFolder.Exists;
        }

        public static bool SurveyGDB(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB)
        {
            dSurveyGDB = null;
            DirectoryInfo dTopoFolder = null;

            if (!TopoData(dTopoFolder, nVisitID, out dTopoFolder))
                return false;

            // Unzipped AWS bucket uses one additional folder level than the FTP
            // AWS: VISIT_XXXX\Topo\SurveyGDB\MySurvey.gdb
            // FTP: VISIT_XXXX\Topo\MySurvey.gdb
            DirectoryInfo dSurveyZipFolder = null;
            if (RetrieveSingleFolder(dTopoFolder, m_sSurveyGDBZipFile, out  dSurveyZipFolder))
                dTopoFolder = dSurveyZipFolder;

            return RetrieveSingleFolder(dTopoFolder, m_sSurveyGDBSearch, out dSurveyGDB);
        }

        public static bool TopoTin(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dTopoTIN)
        {
            dTopoTIN = null;
            DirectoryInfo dTopoFolder = null;

            if (!TopoData(dTopoFolder, nVisitID, out dTopoFolder))
                return false;

            // Unzipped AWS bucket uses one additional folder level than the FTP
            // AWS: VISIT_XXXX\Topo\TIN\TIN\*.adf
            // FTP: VISIT_XXXX\Topo\TIN\*.adf

            DirectoryInfo dTin = null;
            if (RetrieveSingleFolder(dTopoFolder, m_sTopoTINZipFile, out dTin))
            {
                // There is a folder called TIN in the TopoData folder, but the 
                // actual TIN could still be nested inside another folder.
                FileInfo[] fTinFiles = dTin.GetFiles("*.adf", SearchOption.TopDirectoryOnly);
                if (fTinFiles.Count<FileInfo>() == 0)
                {
                    // There is a folder called TIN under TopoData but it does not contain
                    // any ADF files which suggests this is AWS data. Look for the actual
                    // tin folder.
                    RetrieveSingleFolder(dTin, m_sTopoTINSearch, out dTin);
                }
                else
                {
                    // ADF files were found in the folder called TIN under TopoData.
                    // This suggests it is FTP data.
                }
            }
            else
            {
                // No folder called TIN was found. This could still be FTP data
                // but with a TIN called tin1 etc
                RetrieveSingleFolder(dTopoFolder, m_sTopoTINSearch, out dTin);
            }

            return dTin is DirectoryInfo && dTin.Exists;

        }
                
        public static bool SurveyGDBTinWSTinData(DirectoryInfo dTopLevelFolder, int nVisitID, out DirectoryInfo dSurveyGDB, out string dTopoTIN, out DirectoryInfo dWSETIN)
        {
            DirectoryInfo dVisitFolder = null;
            dSurveyGDB = null;
            dTopoTIN = null;
            dWSETIN = null;

            if (!VisitData(dTopLevelFolder, nVisitID, out dVisitFolder))
                return false;





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
