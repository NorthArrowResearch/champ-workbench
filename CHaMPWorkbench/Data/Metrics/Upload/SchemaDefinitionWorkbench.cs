using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data.Metrics.Upload
{
    public class SchemaDefinitionWorkbench : SchemaDefinition
    {
        public long SchemaID { get; internal set; }
        public Dictionary<long, MetricDefinitions.MetricDefinitionBase> MetricsByID { get; internal set; }

        /// <summary>
        /// Constructor for loading schema from database
        /// </summary>
        /// <param name="SchemaID"></param>
        /// <param name="sName"></param>
        public SchemaDefinitionWorkbench(long nSchemaID, string sSchemaName)
                 : base(nSchemaID, sSchemaName)
        {
            SchemaID = nSchemaID;
            MetricsByID = new Dictionary<long, MetricDefinitions.MetricDefinitionBase>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                using (SQLiteCommand dbCom = new SQLiteCommand("SELECT D.MetricID AS MetricID, D.DisplayNameShort as Title, L.ItemID AS DataTypeID, L.Title AS DataType, Precision" +
                    " FROM Metric_Definitions D" +
                    " INNER JOIN Metric_Schema_Definitions S ON D.MetricID = S.MetricID" +
                    " INNER JOIN LookupListItems L ON D.DataTypeID = L.ItemID" +
                    " WHERE (SchemaID = @SchemaID) AND (IsActive <> 0)", dbCon))
                {
                    dbCom.Parameters.AddWithValue("SchemaID", SchemaID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        long nMetricID = dbRead.GetInt64(dbRead.GetOrdinal("MetricID"));
                        string sMetricTitle = dbRead.GetString(dbRead.GetOrdinal("Title"));
                        string sDataType = dbRead.GetString(dbRead.GetOrdinal("DataType"));

                        Metrics[sMetricTitle] =sDataType;
                        MetricsByID[nMetricID] = new MetricDefinitions.MetricDefinitionBase(nMetricID, sMetricTitle, sMetricTitle, dbRead.GetInt64(dbRead.GetOrdinal("DataTypeID")), sDataType,
                            naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Precision"));
                    }
                }
            }
        }
    }
}
