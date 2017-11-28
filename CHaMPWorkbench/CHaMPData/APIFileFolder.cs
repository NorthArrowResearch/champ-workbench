using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using naru.xml;
using naru.db;


namespace CHaMPWorkbench.CHaMPData
{
    /// <summary>
    /// CREATE TABLE CHaMP_VisitFileFolders (FolderID INTEGER PRIMARY KEY NOT NULL, VisitID INTEGER NOT NULL REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE, Name TEXT NOT NULL, URL TEXT NOT NULL, Description TEXT, IsField INTEGER NOT NULL DEFAULT (0), IsFile INTEGER NOT NULL DEFAULT (0));
    /// </summary>



    public class APIFileFolder : naru.db.EditableNamedObject
    {
        public long VisitID { get; internal set; }
        public string URL { get; internal set; }
        public string Description { get; internal set; }
        public bool IsFile { get; internal set; }
        public bool IsField { get; internal set; }

        public APIFileFolder(string name, string url, string desc, bool isfile, bool isfield, DBState eState)
            : base(0, name, eState)
        {
            URL = url;
            Description = desc;
            IsFile = isfile;
            IsField = isfield;
        }

        public static List<APIFileFolder> Load(string sDBCon, long nVisitID)
        {
            List<APIFileFolder> APIFileFolders = new List<APIFileFolder>();
            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM CHaMP_VisitFileFolders WHERE VisitID = @VisitID", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    APIFileFolders.Add(new APIFileFolder(
                        naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Name")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "URL")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueStr(ref dbRead, "Description")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "IsFile")
                        , naru.db.sqlite.SQLiteHelpers.GetSafeValueBool(ref dbRead, "IsField")
                        , DBState.Unchanged));
                }
            }
            return APIFileFolders;
        }

        public static void Save(long visitID, SQLiteTransaction dbTrans, List<APIFileFolder> lFileFolders, List<long> lDeletedIDs = null)
        {
            string[] sFields = { "VisitID", "Name", "URL", "Description", "IsField", "IsFile" };

            // We don't have good ids here so we wipe everything and recreate it every time
            SQLiteCommand dbCom = new SQLiteCommand("DELETE FROM CHaMP_VisitFileFolders WHERE VisitID = @VisitID", dbTrans.Connection, dbTrans);
            dbCom.Parameters.AddWithValue("VisitID", visitID);
            dbCom.ExecuteNonQuery();

            // Note that this insert query is slightly unique and doesn't include the in-memory ID.
            SQLiteCommand comInsert = new SQLiteCommand(string.Format("INSERT INTO CHaMP_VisitFileFolders ({0}) VALUES (@{1})",
                string.Join(",", sFields), string.Join(", @", sFields)), dbTrans.Connection, dbTrans);

            comInsert.Parameters.Add("FolderID", System.Data.DbType.Int64);

            foreach (APIFileFolder aFileFolder in lFileFolders.Where<APIFileFolder>(x => x.State != naru.db.DBState.Unchanged))
            {
                AddParameter(ref comInsert, "VisitID", System.Data.DbType.Int64, visitID);
                AddParameter(ref comInsert, "Name", System.Data.DbType.String, aFileFolder.Name);
                AddParameter(ref comInsert, "URL", System.Data.DbType.String, aFileFolder.URL);
                AddParameter(ref comInsert, "Description", System.Data.DbType.String, aFileFolder.Description);
                AddParameter(ref comInsert, "IsField", System.Data.DbType.Boolean, aFileFolder.IsField);
                AddParameter(ref comInsert, "IsFile", System.Data.DbType.Boolean, aFileFolder.IsFile);

                try
                {
                    comInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write("stop");
                }

                if (aFileFolder.State == naru.db.DBState.New && aFileFolder.ID < 1)
                {
                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    aFileFolder.ID = (long)dbCom.ExecuteScalar();
                }
            }

        }
        /// <summary>
        /// Create an appropriate XML node for these objects
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc)
        {
            XmlNode nodUnit;
            if (IsField && IsFile) nodUnit = xmlDoc.CreateElement("FieldFile");
            else if (IsField && !IsFile) nodUnit = xmlDoc.CreateElement("FieldFolder");
            else if (!IsField && IsFile) nodUnit = xmlDoc.CreateElement("File");
            else nodUnit = xmlDoc.CreateElement("Folder");

            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "Name", Name);
            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "Description", Description);
            XMLHelpers.AddNode(ref xmlDoc, ref nodUnit, "URL", URL);


            return nodUnit;
        }

    }
}
