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

        private static bool RetrieveSingleFolder(DirectoryInfo dContainingFolder, string sSearchPattern, out DirectoryInfo dFolder)
        {
            dFolder = null;
            DirectoryInfo[] dMatchingFolders = dContainingFolder.GetDirectories(sSearchPattern, SearchOption.AllDirectories);

            switch (dMatchingFolders.Count<DirectoryInfo>())
            {
                case 0:
                    return false;

                case 1:
                    dFolder = dMatchingFolders[0];
                    return dFolder.Exists;

                default:
                    throw new Exception(string.Format("Multiple ({0}) visit folders found with pattern '{1}' under {2}", dMatchingFolders.Count<DirectoryInfo>(), sSearchPattern, dContainingFolder.FullName));

            }
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

    }
}
