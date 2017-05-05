namespace HMDesktop.dsHabitatTableAdapters
{
    partial class HSCTableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            //HMDesktop.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }

    partial class HSITableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            //HMDesktop.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }

    partial class SimulationsTableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            //HMDesktop.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }

    public partial class ProjectDataSourcesTableAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>PGB Nov 2014. 
        /// Access doesn't populate the primary key field for inserted items so you
        /// have to write code to retrieve it. This is important if you want to proceed and
        /// add child records to another table straight away and use hte primary key from the
        /// parent table. So this code captures the row updated event on the table adaptor's internal
        /// ADO Data adaptor and retrieves the ID using a separate command. This approach was
        /// adapted from Beth Massi's blog
        /// http://blogs.msdn.com/b/bethmassi/archive/2009/05/14/using-tableadapters-to-insert-related-data-into-an-ms-access-database.aspx
        /// 
        /// To use this code, add an event handler on the table adaptor row updated event
        /// in the class the owns the dataset. Example: 
        ///      m_daDataSources.Adapter.RowUpdated += new System.Data.OleDb.OleDbRowUpdatedEventHandler(m_daDataSources._adapter_RowUpdated);
        ///</remarks>
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            // TODOPGB
            //HMDesktop.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }
}

namespace HMDesktop
{
    
}
namespace HMDesktop
{


    public partial class dsHabitat
    {
    }
}
namespace HMDesktop {
    
    
    public partial class dsHabitat {
    }
}
