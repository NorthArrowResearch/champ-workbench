using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace HMUI.Classes
{
    public class AccessIDHelper
    {
        /// <summary>
        /// Set the primary key for an Access table after an insert statement.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="e"></param>
        /// <remarks>PGB Nov 2014. This code was adapted from Beth Massi's blog
        /// http://blogs.msdn.com/b/bethmassi/archive/2009/05/14/using-tableadapters-to-insert-related-data-into-an-ms-access-database.aspx
        /// It's needed because Access doesn't automatically return the primary key of inserted items</remarks>
        public static void SetPrimaryKey(SQLiteConnection trans, System.Data.ro  OleDbRowUpdatedEventArgs e)
        {
            if (e.Status == System.Data.UpdateStatus.Continue && e.StatementType == System.Data.StatementType.Insert)
            {
                // If this is an INSERT operation...
                System.Data.DataColumn[] pk = e.Row.Table.PrimaryKey;
                // and a primary key column exists...
                if (pk != null)// && pk.Count == 1)
                {
                    System.Data.DataColumn theCol = pk[0];
                    SQLiteCommand cmdGetIdentity = new SQLiteCommand("SELECT @@IDENTITY", trans); //trans.Connection, trans);
                    // Execute the post-update query to fetch new @@Identity
                    e.Row[theCol.ColumnName] = Convert.ToInt32(cmdGetIdentity.ExecuteScalar()); // e.Row[0]
                    e.Row.AcceptChanges();
                }
            }
        }
    }
}
