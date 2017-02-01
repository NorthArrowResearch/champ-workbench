using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.CHaMPData
{
    public class Watershed : naru.db.NamedObject
    {       
        public Watershed(long nWatershedID, String sWatershedName)
            : base(nWatershedID, sWatershedName)
        {

        }
    }
}
