using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    class Watershed : NamedDBObject
    {       
        public Watershed(int nID, String sName, String sFolder)
            : base((int)nID, sName)
        {
        }

        public Watershed(RBTWorkbenchDataSet.CHAMP_WatershedsRow rWatershed)
            : base(rWatershed.WatershedID, rWatershed.WatershedName)
        {
        }
    }
}
