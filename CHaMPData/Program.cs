using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    public class Program : naru.db.EditableNamedObject
    {
        public string m_sWebSiteURL { get; internal set; }
        public string m_sFTPURL { get; internal set; }
        public string m_sAWSBucket { get; internal set; }
        public string m_sRemarks { get; internal set; }
        public string m_sAPI { get; internal set; }

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

        public string API
        {
            get { return m_sAPI; }
            set
            {
                m_sAPI = value;
                State = naru.db.DBState.Edited;
            }
        }

        #endregion

        /// <summary>
        /// Default constructor used by bound user interface controls
        /// </summary>
        public Program() : base(0, string.Empty)
        {
            m_eState = naru.db.DBState.New;
        }

        public Program(long nProgramID, string sTitle, string sWebSiteURL, string sFTPURL, string sAWSBucket, string sAPI, string sRemarks)
            : base(nProgramID, sTitle)
        {
            Init(sWebSiteURL, sFTPURL, sAWSBucket, sAPI, sRemarks);
        }

        public Program(string sTitle, string sWebSiteURL, string sFTPURL, string sAWSBucket, string sAPI, string sRemarks)
            : base(0, sTitle)
        {
            Init(sWebSiteURL, sFTPURL, sAWSBucket, sAPI, sRemarks);
        }

        private void Init(string sWebSiteURL, string sFTPURL, string sAWSBucket, string sAPI, string sRemarks)
        {
            m_sWebSiteURL = sWebSiteURL;
            m_sFTPURL = sFTPURL;
            m_sAWSBucket = sAWSBucket;
            m_sAPI = sAPI;
            m_sRemarks = sRemarks;
            m_eState = naru.db.DBState.Unchanged;
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
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "API")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Remarks"));
                }
                return dResult;
            }
        }

        public long Save(string sDBCon)
        {
            if (State != naru.db.DBState.Unchanged)
            {
                List<Program> lPrograms = new List<Program>();
                lPrograms.Add(this);
                Save(sDBCon, lPrograms);
            }
            return ID;
        }

        public static void Save(string sDBCon, List<Program> lPrograms, List<long> lDeletedIDs = null)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                string[] sFields = { "Title", "WebsiteURL", "FTPURL", "AWSBucket", "Remarks" };
                SQLiteCommand comInsert = new SQLiteCommand(string.Format("INSERT INTO LookupPrograms ({0}) VALUES (@{1})", string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);
                SQLiteCommand comUpdate = new SQLiteCommand(string.Format("UPDATE LookupPrograms SET {0} WHERE ProgramID = @ID", string.Join(",", sFields.Select(x => string.Format("{0} = @{0}", x)))), dbTrans.Connection, dbTrans);
                SQLiteParameter pID = comUpdate.Parameters.Add("ID", System.Data.DbType.Int64);

                try
                {
                    foreach (Program aProgram in lPrograms.Where<Program>(x => x.State != naru.db.DBState.Unchanged))
                    {
                        SQLiteCommand dbCom = null;
                        if (aProgram.State == naru.db.DBState.New)
                            dbCom = comInsert;
                        else
                        {
                            dbCom = comUpdate;
                            pID.Value = aProgram.ID;
                        }

                        AddParameter(ref dbCom, "Title", System.Data.DbType.String, aProgram.Name);
                        AddParameter(ref dbCom, "WebSiteURL", System.Data.DbType.String, aProgram.WebSiteURL);
                        AddParameter(ref dbCom, "FTPURL", System.Data.DbType.String, aProgram.FTPURL);
                        AddParameter(ref dbCom, "AWSBucket", System.Data.DbType.String, aProgram.AWSBucket);
                        AddParameter(ref dbCom, "Remarks", System.Data.DbType.String, aProgram.Remarks);

                        dbCom.ExecuteNonQuery();

                        if (aProgram.State == naru.db.DBState.New)
                        {
                            dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                            aProgram.ID = (long)dbCom.ExecuteScalar();
                        }
                    }

                    if (lDeletedIDs is List<long>)
                    {
                        SQLiteCommand comDelete = new SQLiteCommand("DELETE FROM LookupPrograms WHERE ProgramID = @ID", dbTrans.Connection, dbTrans);
                        SQLiteParameter pDelete = comDelete.Parameters.Add("ID", System.Data.DbType.Int64);
                        foreach (long nID in lDeletedIDs)
                        {
                            pDelete.Value = nID;
                            comDelete.ExecuteNonQuery();
                        }
                    }

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }
        }

        private static void AddParameter(ref SQLiteCommand dbCom, string sParameterName, System.Data.DbType dbType, object objValue)
        {
            SQLiteParameter p = null;
            if (dbCom.Parameters.Contains(sParameterName))
                p = dbCom.Parameters[sParameterName];
            else
            {
                p = dbCom.Parameters.Add(sParameterName, dbType);
            }

            if (objValue == null)
                p.Value = DBNull.Value;
            else
                p.Value = objValue;
        }
    }
}
