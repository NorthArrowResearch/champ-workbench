using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class Program : naru.db.NamedObject
    {
        public string m_sWebSiteURL { get; internal set; }
        public string m_sFTPURL { get; internal set; }
        public string m_sAWSBucket { get; internal set; }
        public string m_sRemarks { get; internal set; }

        #region Properties

        public string WebSiteURL
        {
            get { return m_sWebSiteURL; }
            set
            {
                m_sWebSiteURL = value;
                State = naru.db.DBState.Edited;
            }
        }

        public string FTPURL
        {
            get { return m_sFTPURL; }
            set
            {
                m_sFTPURL = value;
                State = naru.db.DBState.Edited;
            }
        }

        public string AWSBucket
        {
            get { return m_sAWSBucket; }
            set
            {
                m_sAWSBucket = value;
                State = naru.db.DBState.Edited;
            }
        }

        public string Remarks
        {
            get { return m_sRemarks; }
            set
            {
                m_sRemarks = value;
                State = naru.db.DBState.Edited;
            }
        }

        #endregion

        /// <summary>
        /// Default constructor used by bound user interface controls
        /// </summary>
        public Program() : base(0, string.Empty)
        {
        }

        public Program(long nProgramID, string sTitle, string sWebSiteURL, string sFTPURL, string sAWSBucket, string sRemarks)
            : base(nProgramID, sTitle)
        {
            Init(sWebSiteURL, sFTPURL, sAWSBucket, sRemarks);
        }

        public Program(string sTitle, string sWebSiteURL, string sFTPURL, string sAWSBucket, string sRemarks)
            : base(0, sTitle)
        {
            Init(sWebSiteURL, sFTPURL, sAWSBucket, sRemarks);
        }

        private void Init(string sWebSiteURL, string sFTPURL, string sAWSBucket, string sRemarks)
        {
            m_sWebSiteURL = sWebSiteURL;
            m_sFTPURL = sFTPURL;
            m_sAWSBucket = sAWSBucket;
            m_sRemarks = sRemarks;
        }

        public static Dictionary<long, Program> Load(string sDBCon)
        {
            Dictionary<long, Program> dResult = new Dictionary<long, Program>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM LookupPrograms ORDER BY Title", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nProgramID = dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"));
                    dResult[nProgramID] = new Program(nProgramID,
                        dbRead.GetString(dbRead.GetOrdinal("Title"))
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "WebSiteURL")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "FTPURL")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "AWSBucket")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Remarks"));
                }
                return dResult;
            }
        }

        public long Save(string sDBCon)
        {
            if (State == naru.db.DBState.Unchanged)
                return ID;

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    string[] sFields = { "Title", "WebsiteURL", "FTPURL", "AWSBucket", "Remarks" };
                    SQLiteCommand dbCom = null;
                    if (State == naru.db.DBState.New)
                        dbCom = new SQLiteCommand(string.Format("INSERT INTO LookupPrograms ({0}) VALUES (@{1})", string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);
                    else
                    {
                        dbCom = new SQLiteCommand(string.Format("UPDATE LookupPrograms SET {0} WHERE ProgramID = @ProgramID", string.Join(",", sFields.Select(x => string.Format("{0} = @{1}", x)))), dbTrans.Connection, dbTrans);
                        dbCom.Parameters.AddWithValue("ProgramID", ID);
                    }

                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, WebSiteURL, "Title");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, WebSiteURL, "WebsiteURL");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, WebSiteURL, "FTPURL");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, WebSiteURL, "AWSBucket");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, WebSiteURL, "Remarks");

                    dbCom.ExecuteNonQuery();

                    if (State == naru.db.DBState.New)
                    {
                        dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                        ID = (long)dbCom.ExecuteScalar();
                    }

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }

            return ID;
        }
    }
}
