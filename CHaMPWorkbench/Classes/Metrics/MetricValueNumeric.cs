using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHaMPWorkbench.Classes.Metrics
{
    public class MetricValueNumeric : MetricValueBase
    {
        public double? MetricValue { get; internal set; }

        public MetricValueNumeric(long nMetricID, double? fMetricValue) : base(nMetricID)
        {
            MetricValue = fMetricValue;
        }
    }
}
