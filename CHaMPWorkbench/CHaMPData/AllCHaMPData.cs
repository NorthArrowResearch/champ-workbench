using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.CHaMPData
{
    class AllCHaMPData
    {
        public Dictionary<long, Watershed> Watersheds { get; internal set; }
        public Dictionary<long, Site> Sites { get; internal set; }
        public Dictionary<long, Visit> Visits { get; internal set; }

        public AllCHaMPData(string sDBCon)
        {
            Watersheds = Watershed.Load(sDBCon);
            Sites = Site.Load(sDBCon);
            Visits = Visit.Load(sDBCon);
        }
    }
}
