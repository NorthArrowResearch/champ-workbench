using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HMDesktop.Classes
{
    class FISRuleFile
    {
        private string m_sRuleFilePath;

        private List<string> m_lFISInputs;

        /// <summary>
        /// Get the list of FIS inputs contained in the FIS rule file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> FISInputs
        {
            get { return m_lFISInputs; }
        }

        /// <summary>
        /// Get the full absolute file path of the *.fis FIS rule file
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RuleFilePath
        {
            get { return m_sRuleFilePath; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRuleFilePath">Full, absolute path to a *.fis rule file.</param>
        /// <remarks></remarks>
        public FISRuleFile(string sRuleFilePath)
        {
            if (string.IsNullOrEmpty(sRuleFilePath))
            {
                throw new Exception("The FIS rule file path cannot be null or empty.");
            }
            else
            {
                if (!System.IO.File.Exists(sRuleFilePath))
                {
                    Exception ex = new Exception("The FIS rule file cannot be found on the file system.");
                    ex.Data["FIS Rule File Path"] = sRuleFilePath;
                    throw ex;
                }
            }

            m_sRuleFilePath = sRuleFilePath;

            try
            {
                string sRuleFileText = System.IO.File.ReadAllText(sRuleFilePath);
                m_lFISInputs = new List<string>();

                Regex theRegEx = new Regex("dd");
                Match theMatch = theRegEx.Match(sRuleFileText);
                int nIndex = 0;

                // Match data between single quotes hesitantly.
                MatchCollection col = Regex.Matches(sRuleFileText, "\\[Input[0-9]\\]\\s*Name='([^']*)'");
                foreach (Match m in col)
                {
                    // Access first Group and its value.
                    Group g = m.Groups[1];
                    m_lFISInputs.Add(g.Value);
                }

            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error parsing FIS rule file", ex);
                ex2.Data["FIS Rule File Path"] = sRuleFilePath;
                throw ex2;
            }
        }

    }
}