using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.UserQueries
{
    public partial class frmQueryProperties : Form
    {
        private string DBCon { get; set; }
        public int ID { get; internal set; }

        public frmQueryProperties(string sDBCon, int nID = 0)
        {
            InitializeComponent();
            DBCon = sDBCon;
            ID = nID;
        }

        private void frmQueryProperties_Load(object sender, EventArgs e)
        {

            if (ID > 0)
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();

                    OleDbCommand dbCom = new OleDbCommand("SELECT Title, QueryText, Remarks FROM User_Queries WHERE QueryID = @QueryID", dbCon);
                    dbCom.Parameters.AddWithValue("@QueryID", ID);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    if (dbRead.Read())
                    {
                        txtTitle.Text = dbRead.GetString(dbRead.GetOrdinal("Title"));
                        txtQueryText.Text = dbRead.GetString(dbRead.GetOrdinal("QueryText"));
                        txtRemarks.Text = dbRead.GetString(dbRead.GetOrdinal("Remarks"));
                    }
                }
            }
        }

        private void cmdVerify_Click(object sender, EventArgs e)
        {
            VerifyQuery(DBCon, txtQueryText.Text, true);
        }

        /// <summary>
        /// Check that a particular query string is valid and not nefarious
        /// </summary>
        /// <param name="bShowSuccessMessage"></param>
        /// <returns>This method needs to be static so that the code that
        /// actually runs the user query (see bottom of file) can access it.</returns>
        private static bool VerifyQuery(string sDBCon, string sSQL, bool bShowSuccessMessage)
        {
            List<string> ForbiddenStrings = new List<string> { "insert", "update", "delete", "drop", "alter" };
            string sMessageBoxTitle = "Validation Failed";

            if (string.IsNullOrEmpty(sSQL))
            {
                MessageBox.Show("You must enter a query string to proceed.", sMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            foreach (string sWord in ForbiddenStrings)
            {
                if (sSQL.ToLower().Contains(sWord))
                {
                    MessageBox.Show(string.Format("The query text contains the forbidden word '{0}' The following words are not allowed in the query text string {1}.", sWord, string.Join(", ", ForbiddenStrings)), sMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            try
            {
                using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                    dbCom.ExecuteScalar();

                    if (bShowSuccessMessage)
                    {
                        MessageBox.Show("Query verification successful.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The query string failed to complete with the following error: " + ex.Message, sMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = null;

                    if (ID < 1)
                        dbCom = new OleDbCommand("INSERT INTO User_Queries (Title, QueryText, Remarks, CreatedBy) VALUES (@Title, @QueryText, @Remarks, @CreatedBy)", dbCon);
                    else
                        dbCom = new OleDbCommand("UPDATE User_Queries SET Title = @Title, QueryText = @QueryText, Remarks = @Remarks WHERE QueryID = @QueryID", dbCon);

                    dbCom.Parameters.AddWithValue("@Title", txtTitle.Text);

                    OleDbParameter pQueryText = dbCom.Parameters.Add("@QueryText", OleDbType.VarChar);
                    pQueryText.Value = txtQueryText.Text;
                    pQueryText.Size = txtQueryText.Text.Length;

                    OleDbParameter pRemarks = dbCom.Parameters.Add("@Remarks", OleDbType.VarChar);
                    if (string.IsNullOrEmpty(txtRemarks.Text))
                        pRemarks.Value = DBNull.Value;
                    else
                    {
                        pRemarks.Value = txtRemarks.Text;
                        pRemarks.Size = txtRemarks.Text.Length;
                    }

                    if (ID < 1)
                        dbCom.Parameters.AddWithValue("@CreatedBy", System.Environment.UserName);
                    else
                        dbCom.Parameters.AddWithValue("@QueryID", ID);

                    int nAffected = dbCom.ExecuteNonQuery();
                    if (nAffected == 1)
                    {
                        dbCom = new OleDbCommand("SELECT @@Identity FROM User_Queries", dbCon);
                        ID = (int)dbCom.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving User Query", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("You must provide a title for this user query.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!VerifyQuery(DBCon, txtQueryText.Text, false))
                return false;

            return true;
        }

        public static void RunUserQuery(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                UserQueryTag uqTag = (sender as ToolStripMenuItem).Tag as UserQueryTag;
                if (VerifyQuery(uqTag.DBCon, uqTag.SQL, false))
                {
                    SaveFileDialog frm = new SaveFileDialog();
                    frm.Title = "Save User Query To CSV File";
                    frm.Filter = "Comma Separated Value File (*.csv)|*.csv";

                    frm.InitialDirectory = System.IO.Path.GetDirectoryName(MainForm.GetDatabasePathFromConnectionString(uqTag.DBCon));
                    frm.FileName = uqTag.DefaultFileName;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int nRecords = 0;
                        using (OleDbConnection dbCon = new OleDbConnection(uqTag.DBCon))
                        {
                            dbCon.Open();

                            OleDbDataAdapter dbDA = new OleDbDataAdapter(uqTag.SQL, dbCon);
                            DataTable dt = new DataTable();
                            nRecords = dbDA.Fill(dt);

                            StringBuilder sb = new StringBuilder();
                            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                            sb.AppendLine(string.Join(",", columnNames));

                            foreach (DataRow row in dt.Rows)
                            {
                                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                                sb.AppendLine(string.Join(",", fields));
                            }

                            System.IO.File.WriteAllText(frm.FileName, sb.ToString());
                        }

                        if (System.IO.File.Exists(frm.FileName))
                        {
                            if (MessageBox.Show(string.Format("{0} line(s) written to file. Do you want to open the file produced?", nRecords), "User Query Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                                == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(frm.FileName);
                            }
                        }
                    }
                }
            }
        }

        public class UserQueryTag
        {
            public string DBCon { get; internal set; }
            public string SQL { get; internal set; }
            public int ID { get; internal set; }
            public string Title { get; internal set; }

            public string DefaultFileName
            {
                get
                {
                    string sResult = Title;
                    foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                        sResult = sResult.Replace(c.ToString(), "");
                    
                    sResult = sResult.Replace("-", "");
                    sResult = sResult.Replace("_", "");
                    sResult = sResult.Replace(" ", "");
                    sResult = sResult.Replace(".", "");
                    sResult = sResult.Replace("!", "");
                    sResult = sResult.Replace("@", "");
                    sResult = sResult.Replace("#", "");
                    sResult = sResult.Replace("$", "");
                    sResult = sResult.Replace("%", "");
                    sResult = sResult.Replace("^", "");
                    sResult = sResult.Replace("&", "");
                    sResult = sResult.Replace("*", "");
                    sResult = sResult.Replace("(", "");
                    sResult = sResult.Replace(")", "");
                    sResult = sResult.Replace("+", "");
                    sResult = sResult.Replace("=", "");
                    sResult = sResult.Replace("'", "");
                    sResult = sResult.Replace("~", "");
                    sResult = sResult.Replace("`", "");
                    sResult = sResult.Replace("{", "");
                    sResult = sResult.Replace("}", "");
                    sResult = sResult.Replace("[", "");
                    sResult = sResult.Replace("]", "");
                    sResult = sResult.Replace(";", "");
                    sResult = sResult.Replace(",", "");

                    return sResult;
                }
            }

            public UserQueryTag(string sDBCon, string sSQL, int nID, string sTitle)
            {
                DBCon = sDBCon;
                SQL = sSQL;
                ID = nID;
                Title = sTitle;
            }
        }
    }
}
