namespace CHaMPWorkbench.Habitat
{


    public partial class dsHabitat
    {
    }
}

namespace CHaMPWorkbench.Habitat.dsHabitatTableAdapters {
    partial class SimulationHSCInputsTableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            HMUI.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }

    partial class ProjectVariablesTableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            HMUI.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }

    partial class ProjectDataSourcesTableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            HMUI.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }
    
    public partial class SimulationsTableAdapter
    {
        public void _adapter_RowUpdated(dynamic sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            HMUI.Classes.AccessIDHelper.SetPrimaryKey(this.Connection, e);
        }
    }
}
