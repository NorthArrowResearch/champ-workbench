using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class MetricValueBase
    {
        public double? MetricValue { get; internal set; }

        public MetricValueBase(double? fMetricValue)
        {
            MetricValue = fMetricValue;
        }
    }
}
