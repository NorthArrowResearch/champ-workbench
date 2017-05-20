using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data.MetricDefinitions
{
    public class MetricDefinition : naru.db.NamedObject
    {
        public string DisplayNameShort { get; internal set; }
        public long SchemaID { get; internal set; }
        public string SchemaName { get; internal set; }
        public long ModelID { get; internal set; }
        public string ModelName { get; internal set; }
        public string XPath { get; internal set; }
        public bool IsActive { get; internal set; }
        public long DataTypeID { get; internal set; }
        public string DataTypeName { get; internal set; }
        public int Precision { get; internal set; }
        public double Threshold { get; internal set; }
        public double MinValue { get; internal set; }
        public double MaxValue { get; internal set; }

        public MetricDefinition(long nID, string sTitle, string sDisplayNameShort
            , long nSchemaID, string sSchemaName
            , long nModelID, string sModelName
            , string sXPath, bool bIsActive
            , long nDataTypeID, string sDataTypeName
            , int nPrecision, double fThreshold, double fMinValue, double fMaxValue)
            : base(nID, sTitle)
        {
            DisplayNameShort = sDisplayNameShort;
            SchemaID = nSchemaID;
            SchemaName = sSchemaName;
            ModelID = nModelID;
            ModelName = sModelName;
            XPath = sXPath;
            IsActive = bIsActive;
            DataTypeID = nDataTypeID;
            DataTypeName = sDataTypeName;
            Precision = nPrecision;
            Threshold = fThreshold;
            MinValue = fMinValue;
            MaxValue = fMaxValue;
        }

        public static naru.ui.SortableBindingList<MetricDefinition> Load(string sDBCon)
        {
            naru.ui.SortableBindingList<MetricDefinition> result = new naru.ui.SortableBindingList<MetricDefinition>();


            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM vwMetricDefinitions ORDER BY Title", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    dbRead.GetInt64(dbRead.GetOrdinal(""));
                }
            }
        }
    }
}
