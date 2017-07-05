using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHaMPWorkbench.Classes.Metrics
{
    public class MetricValueBase
    {
        public long MetricID { get; set; }

        public MetricValueBase(long nMetricID)
        {
            MetricID = nMetricID;
        }
    }
}
