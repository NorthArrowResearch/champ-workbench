using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHaMPWorkbench.Data.MetricDefinitions
{
    public class MetricDefinitionBase : naru.db.NamedObject
    {
        public string DisplayNameShort { get; internal set; }
        public long DataTypeID { get; internal set; }
        public string DataTypeName { get; internal set; }
        public long? Precision { get; internal set; }

        public MetricDefinitionBase(long nID, string sName)
            : base(0, sName)
        {

        }

        public MetricDefinitionBase(long nID, string sName, string sDisplayNameShort, long nDataTypeID, string sDataTypeName, long? nPrecision)
            : base(nID, sName)
        {
            DisplayNameShort = sDisplayNameShort;
            DataTypeID = nDataTypeID;
            DataTypeName = sDataTypeName;
            Precision = nPrecision;
        }
    }
}
