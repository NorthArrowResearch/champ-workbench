using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    /// <summary>
    /// Represents a populated set of CHaMP related tables in the database
    /// </summary>
    class CHaMPData
    {
        private RBTWorkbenchDataSet m_dsWorkbench;
        RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter taWatersheds = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
        RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter taSites = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
        RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter taVisits = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
        RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter taSegments = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
        RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter taChannelUnits = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();

        public RBTWorkbenchDataSet DS { get { return m_dsWorkbench; } }

        public CHaMPData(ref System.Data.OleDb.OleDbConnection dbCon)
        {
            m_dsWorkbench = new RBTWorkbenchDataSet();

            // Load the CHaMP Workbench information about visits
            taWatersheds.Connection = dbCon;
            taSites.Connection = dbCon;
            taVisits.Connection = dbCon;
            taSegments.Connection = dbCon;
            taChannelUnits.Connection = dbCon;
        }

        public void FillByVisitIDS(ref List<int> lVisitIDs)
        {
            taWatersheds.Fill(m_dsWorkbench.CHAMP_Watersheds);
            taSites.Fill(m_dsWorkbench.CHAMP_Sites);
            taVisits.Fill(m_dsWorkbench.CHAMP_Visits);
            taSegments.Fill(m_dsWorkbench.CHaMP_Segments);
            taChannelUnits.Fill(m_dsWorkbench.CHAMP_ChannelUnits);
        }
    }
}
