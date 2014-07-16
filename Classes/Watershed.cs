using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    class Watershed : NamedDBObject
    {
        private String m_sFolder;

        public String Folder
        {
            get
            {
                return m_sFolder;
            }
        }

        public Watershed(int nID, String sName, String sFolder) : base((int)nID, sName)
        {
            m_sFolder = sFolder;
        }
    }
}
