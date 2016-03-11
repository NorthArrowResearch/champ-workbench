using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    public class MetricValueModel : MetricValueBase
    {
        public string Version { get; internal set; }

        public MetricValueModel(string sVersion, float fMetricValue) : base(fMetricValue)
        {
            Version = sVersion;
        }
    }
}
