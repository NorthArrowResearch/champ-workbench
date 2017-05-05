using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class MetricValueBase
    {
        public float MetricValue { get; internal set; }

        public MetricValueBase(float fMetricValue)
        {
            MetricValue = fMetricValue;
        }
    }
}
