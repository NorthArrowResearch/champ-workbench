using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace CHaMPWorkbench
{
    public class ListItem
    {
        private string m_sText;
        private int m_nValue;

        public ListItem(string sText, int nValue)
        {
            m_sText = sText;
            m_nValue = nValue;
        }

        public string Text { get { return m_sText; } }

        public int Value { get { return m_nValue; } }

        public override string ToString()
        {
            return m_sText;
        }

        public static int LoadComboWithListItems(ref System.Windows.Forms.ComboBox cbo, string sDBCon, string sSQL, long nSelectID = 0)
        {
            cbo.Items.Clear();

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    Int32 nID = (Int32)dbRead.GetValue(0);
                    int nIn = cbo.Items.Add(new ListItem(dbRead.GetString(1), (int)nID));
                    if (nID == nSelectID)
                        cbo.SelectedIndex = nIn;
                }
            }

            return cbo.Items.Count;
        }

        public static void SelectItem(ref System.Windows.Forms.ComboBox cbo, int nID)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (cbo.Items[i] is ListItem)
                {
                    if (((ListItem)cbo.Items[i]).Value == nID)
                    {
                        cbo.SelectedIndex = i;
                        return;
                    }
                }
            }
        }
    }

    public class CheckedListItem : ListItem
    {
        private Boolean m_bChecked;

        public Boolean Checked { get; set; }

        public CheckedListItem(string sText, int nValue, Boolean bChecked = true)
            : base(sText, nValue)
        {
            m_bChecked = bChecked;
        }

        public static int LoadComboWithListItems(ref System.Windows.Forms.CheckedListBox lst, string sDBCon, string sSQL, bool bCheckItems)
        {
            lst.Items.Clear();

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    int nID = 0;
                    if (dbRead.GetFieldType(0) == Type.GetType("System.Int16"))
                        nID = (int) dbRead.GetInt16(0);
                    else if (dbRead.GetFieldType(0) == Type.GetType("System.Int32"))
                        nID = dbRead.GetInt32(0);
                    else if (dbRead.GetFieldType(0) == Type.GetType("System.Int64"))
                        nID = (int)dbRead.GetInt64(0);
                    else
                        throw new Exception("Unhandled field type in column 0");

                    int nIn = lst.Items.Add(new ListItem(dbRead.GetString(1), nID));
                    lst.SetItemChecked(nIn, bCheckItems);
                }
            }

            return lst.Items.Count;
        }
    }
}
