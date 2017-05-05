using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    public class BudgetSegregation
    {
        private string m_sName;
        private string m_sMaskName;

        public BudgetSegregation(string sName, string sMaskName)
        {
            m_sName = sName;
            m_sMaskName = sMaskName;
        }

        public override string ToString()
        {
            return m_sName;
        }

        public string Name
        {
            get { return m_sName; }
        }

        public string MaskName
        {
            get { return m_sMaskName; }
        }
    }
}
