﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;

namespace CHaMPWorkbench.Classes
{
    public class InputFileBuilder_Helper
    {

        public static void RefreshVisitPaths(string sDBCon, ref Dictionary<long, string> dVisits, ref System.Windows.Forms.ListBox lstVisits)
        {
            lstVisits.Items.Clear();

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, V.VisitYear" +
                   " FROM (CHAMP_Watersheds AS W INNER JOIN CHAMP_Sites AS S ON W.WatershedID = S.WatershedID) INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID" +
                   " WHERE (V.VisitYear Is Not Null) AND (V.VisitID Is Not Null) AND (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null)", dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long nVisitID = (long)dbRead["VisitID"];
                    if (dVisits.Keys.Contains<long>(nVisitID))
                    {
                        System.IO.DirectoryInfo dVisitTopoFolder = null;

                        string sPath = string.Format("{0}\\{1}\\{2}\\VISIT_{3}", dbRead["VisitYear"], dbRead["WatershedName"].ToString().Replace(" ", ""), dbRead["SiteName"].ToString().Replace(" ", ""), nVisitID);
                        lstVisits.Items.Add(new naru.db.NamedObject((long)dbRead["VisitID"], sPath));
                    }
                }
            }

        }

        public static void BrowseFolder(string sFormTitle, string sMessage, ref TextBox txt)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = sMessage;

            if (!string.IsNullOrEmpty(txt.Text))
            {
                if (System.IO.Directory.Exists(txt.Text))
                {
                    frm.SelectedPath = txt.Text;
                }
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Directory.Exists(frm.SelectedPath))
                {
                    txt.Text = frm.SelectedPath;
                }
            }
        }
    }
}
