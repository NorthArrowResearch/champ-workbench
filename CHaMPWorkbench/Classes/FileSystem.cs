using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    public class FileSystem
    {
        public enum SearchTypes
        {
            Directory,
            File
        }

        public static  bool LookForMatchingItems(string sContainingFolderPath, string sPatternList, SearchTypes eType, out string sResult)
        {
            sResult = "";
            foreach (string aPattern in sPatternList.Split(';'))
            {
                String[] sMatches;
                if (eType == SearchTypes.Directory)
                {
                    sMatches = System.IO.Directory.GetDirectories(sContainingFolderPath, aPattern,System.IO.SearchOption.AllDirectories);
                }
                else
                {
                    sMatches = System.IO.Directory.GetFiles(sContainingFolderPath, aPattern);
                }

                if (sMatches.Count<String>() == 1)
                {
                    sResult = sMatches[0];
                    break;
                }
            }

            return !string.IsNullOrEmpty(sResult);
        }
    }
}
