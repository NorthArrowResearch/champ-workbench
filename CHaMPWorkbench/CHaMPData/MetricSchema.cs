using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class MetricSchema : naru.db.NamedObject
    {
        public string RootXPath { get; internal set; }
        public string DatabaseTable { get; internal set; }
        public long ProgramID { get; internal set; }
        public string ProgramName { get; internal set; }
        public string MetricResultXMLFile { get; internal set; }
        public string MetricSchemaXMLFile { get; internal set; }

        public bool HasRootXPath { get { return !string.IsNullOrEmpty(RootXPath); } }
        public bool HasDatabaseTable { get { return !string.IsNullOrEmpty(DatabaseTable); } }
        public bool HasMetricResultXMLFile { get { return !string.IsNullOrEmpty(MetricResultXMLFile); } }
        public bool HasMetricSchemaXMLFile { get { return !string.IsNullOrEmpty(MetricSchemaXMLFile); } }

        public string NameWithProgram
        {
            get
            {
                return string.Format("{0} ({1})", Name, ProgramName);
            }
        }

        public MetricSchema(long nID, string sName, string sMetricResultXMLFile, string sMetricSchemaXMLFile, long nProgramID, string sProgramTitle, string sRootXPath, string sDatabaseTable)
            : base(nID, sName)
        {
            RootXPath = sRootXPath;
            DatabaseTable = sDatabaseTable;
            ProgramID = nProgramID;
            ProgramName = sProgramTitle;
            MetricResultXMLFile = sMetricResultXMLFile;
            MetricSchemaXMLFile = sMetricSchemaXMLFile;
        }

        public static Dictionary<long, MetricSchema> Load(string sDBCon)
        {
            Dictionary<long, MetricSchema> dResult = new Dictionary<long, MetricSchema>();

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT S.SchemaID AS SchemaID, S.Title AS Title, MetricResultXMLFile, MetricSchemaXMLFile, P.ProgramID AS ProgramID, P.Title AS ProgramTitle, RootXPath, DatabaseTAble" +
                    " FROM Metric_Schemas S INNER JOIN LookupPrograms P ON S.ProgramID = P.ProgramID ORDER BY S.Title", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nID = dbRead.GetInt64(dbRead.GetOrdinal("SchemaID"));
                    dResult[nID] = new MetricSchema(nID, dbRead.GetString(dbRead.GetOrdinal("Title"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "MetricResultXMLFile")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "MetricSchemaXMLFile")
                        , dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"))
                        , dbRead.GetString(dbRead.GetOrdinal("ProgramTitle"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "RootXPath")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "DatabaseTable"));
                }
            }

            return dResult;
        }
    }
}
