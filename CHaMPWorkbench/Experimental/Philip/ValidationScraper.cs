using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Xml;
using System.Text.RegularExpressions;

namespace CHaMPWorkbench.Experimental.Philip
{
    public class ValidationScraper
    {
        public static void Run()
        {
            bool bClearFirst = false;

            switch (System.Windows.Forms.MessageBox.Show("Do you want to delete all existing validation log messages before proceeding?", Properties.Resources.MyApplicationNameLong, System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                 System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2))
            {
                case System.Windows.Forms.DialogResult.Cancel:
                    return;

                case System.Windows.Forms.DialogResult.Yes:
                    bClearFirst = true;
                    break;
            }


            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select top level folder containing validation.xml files:";

            if (frm.ShowDialog() == DialogResult.Cancel)
                return;


            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    if (bClearFirst)
                    {
                        SQLiteCommand comDelete = new SQLiteCommand("DELETE FROM LogFiles", dbCon);
                        comDelete.ExecuteNonQuery();
                    }

                    List<long> MissingVisits = new List<long>();
                    int nCount = 0;
                    foreach (string logPath in System.IO.Directory.GetFiles(frm.SelectedPath, "validation.xml", System.IO.SearchOption.AllDirectories))
                    {
                        // Get the Visit ID from the path
                        Match matchVisitID = Regex.Match(logPath, "VISIT_([0-9]+)");
                        long nVisitID = long.Parse(matchVisitID.Groups[1].Value);


                        SQLiteCommand comSelect = new SQLiteCommand("SELECT VisitID FROM CHaMP_Visits WHERE VisitID = @VisitID", dbTrans.Connection, dbTrans);
                        comSelect.Parameters.AddWithValue("VisitID", nVisitID);
                        long nCheckVisitID = naru.db.sqlite.SQLiteHelpers.GetScalarID(ref comSelect);
                        if (nCheckVisitID < 1)
                        {
                            MissingVisits.Add(nCheckVisitID);
                            continue;
                        }

                        SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO LogFiles (Status, LogFilePath, VisitID, DateRun, ModelVersion)" +
                                                    " VALUES (@Status, @LogFilePath, @VisitID, @DateRun, @ModelVersion)", dbTrans.Connection, dbTrans);

                        XmlDocument xmlLog = new XmlDocument();
                        xmlLog.Load(logPath);

                        dbCom.Parameters.AddWithValue("Status", xmlLog.SelectSingleNode("/TopoValidation/Status/Overall").InnerText);
                        dbCom.Parameters.AddWithValue("LogFilePath", logPath);
                        dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                        dbCom.Parameters.AddWithValue("DateRun", xmlLog.SelectSingleNode("TopoValidation/Meta/DateCreated").InnerText);
                        dbCom.Parameters.AddWithValue("ModelVersion", xmlLog.SelectSingleNode("TopoValidation/Meta/Version").InnerText);
                        dbCom.ExecuteNonQuery();

                        long nLogID = naru.db.sqlite.SQLiteHelpers.GetLastInsertID(ref dbTrans);

                        dbCom = new SQLiteCommand("INSERT INTO LogMessages (LogID, TargetVisitID, SourceVisitID, MessageType, LogMessage, LogSolution)" +
                                        " VALUES (@LogID, @VisitID, @VisitID, @MessageType, @LogMessage, @LogSolution)", dbTrans.Connection, dbTrans);

                        dbCom.Parameters.AddWithValue("LogID", nLogID);
                        dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                        SQLiteParameter pMessageType = dbCom.Parameters.Add("MessageType", System.Data.DbType.String);
                        SQLiteParameter pMessage = dbCom.Parameters.Add("LogMessage", System.Data.DbType.String);
                        SQLiteParameter pLogSolution = dbCom.Parameters.Add("LogSolution", System.Data.DbType.String);

                        foreach (XmlNode nodText in xmlLog.SelectNodes("TopoValidation/Layers/Layer/Tests/Test"))
                        {
                            pMessageType.Value = nodText.SelectSingleNode("Status").InnerText;

                            if (string.IsNullOrEmpty(nodText.SelectSingleNode("Message").InnerText))
                                pMessage.Value = DBNull.Value;
                            else
                                pMessage.Value = nodText.SelectSingleNode("Message").InnerText;

                            pLogSolution.Value = nodText.ParentNode.ParentNode.SelectSingleNode("Name").InnerText;
                            dbCom.ExecuteNonQuery();
                        }
                        nCount++;
                    }

                    dbTrans.Commit();
                    MessageBox.Show(string.Format("{0} validation logs scraped successfully. {1} validation logs skipped because the corresponding visit doesn't exist in workbench", nCount, MissingVisits.Count), Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }
        }
    }
}
