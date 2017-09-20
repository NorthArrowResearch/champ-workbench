using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class MetricBatch : naru.db.NamedObject
    {
        public naru.db.NamedObject ScavengeType { get; internal set; }
        public naru.db.NamedObject Schema { get; internal set; }
        public naru.db.NamedObject Program { get; internal set; }
        public string DatabaseTable { get; internal set; }
        public long Visits { get; internal set; }
        public long Instances { get; internal set; }

        public bool Copy { get; set; }

        public MetricBatch(long nbatchID, long nScavengetTypeID, string sScavengetType, long nSchemaID, string sSchema,
            long nProgramID, string sProgram, string sDatabaseTable, long? nVisits, long? nInstances)
            : base(nbatchID, string.Format("{0} - {1} - {2}", sProgram, sScavengetType, sSchema))
        {
            Schema = new naru.db.NamedObject(nSchemaID, sSchema);
            ScavengeType = new naru.db.NamedObject(nScavengetTypeID, sScavengetType);
            Program = new naru.db.NamedObject(nProgramID, sProgram);
            DatabaseTable = sDatabaseTable;
            Copy = false;

            Visits = 0;
            if (nVisits.HasValue)
                Visits = nVisits.Value;

            Instances = 0;
            if (nInstances.HasValue)
                Instances = nInstances.Value;
        }

        public static naru.ui.SortableBindingList<MetricBatch> Load()
        {
            naru.ui.SortableBindingList<MetricBatch> result = new naru.ui.SortableBindingList<MetricBatch>();

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT B.BatchID, B.ScavengeTypeID, L.Title AS ScavengeType, S.ProgramID, P.Title AS Program, S.SchemaID, S.Title AS Schema, S.DatabaseTable, Count(I.BatchID) AS Visits" +
                    " FROM Metric_Batches B" +
                        " INNER JOIN Metric_Schemas S ON B.SchemaID = S.SchemaID" +
                        " INNER JOIN LookupListItems L ON B.ScavengeTypeID = L.ItemID" +
                        " INNER JOIN LookupPrograms P ON S.ProgramID = P.ProgramID" +
                        " LEFT JOIN Metric_Instances I ON B.BatchID = I.BatchID" +
                    " GROUP BY B.BatchID, B.ScavengeTypeID, ScavengeType, S.ProgramID, Program, S.SchemaID, Schema, S.DatabaseTable", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    result.Add(new MetricBatch(dbRead.GetInt64(dbRead.GetOrdinal("BatchID"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("ScavengeTypeID"))
                        , dbRead.GetString(dbRead.GetOrdinal("ScavengeType"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("SchemaID"))
                        , dbRead.GetString(dbRead.GetOrdinal("Schema"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"))
                        , dbRead.GetString(dbRead.GetOrdinal("Program"))
                        , dbRead.GetString(dbRead.GetOrdinal("DatabaseTable"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Visits")
                        , 0));
                }
            }

            return result;
        }
    }
}
