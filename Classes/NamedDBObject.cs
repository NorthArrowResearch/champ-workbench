using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    class NamedDBObject
    {
        private int m_nID;
        private String m_sName;

        public NamedDBObject(int nID, String sName)
        {
            m_nID = nID;
            m_sName = sName;
        }

        public override string ToString()
        {
            return m_sName;
        }

        public int ID
        {
            get
            {
                return m_nID;
            }
        }

    }
}
