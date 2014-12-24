using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CHaMPWorkbench.Habitat
{
    class HabitatBatchBuilder
    {
        private OleDbConnection m_dbWorkbenchCon;
        private OleDbConnection m_dbHabitatCon;

        private Classes.CHaMPData m_CHaMPData;
        HMUI.Classes.HSProjectManager m_HabitatManager;
        private System.IO.DirectoryInfo m_dMonitoringDatafolder;

        private dsHabitat m_dsHabitat;

        private dsHabitatTableAdapters.SimulationsTableAdapter m_taSimulations;
        private dsHabitatTableAdapters.SimulationHSCInputsTableAdapter m_taSimulationHSInputs;
        private dsHabitatTableAdapters.ProjectDataSourcesTableAdapter m_taProjectDataSources;
        private dsHabitatTableAdapters.ProjectVariablesTableAdapter m_taProjectVariables;

        public HabitatBatchBuilder(ref OleDbConnection dbWorkbench, string sHabitatDBPath, string sMonitoringDataFolder)
        {
            m_CHaMPData = new Classes.CHaMPData(ref dbWorkbench);
            m_HabitatManager = new HMUI.Classes.HSProjectManager(sHabitatDBPath);
            m_dMonitoringDatafolder = new System.IO.DirectoryInfo(sMonitoringDataFolder);

            m_dsHabitat = new dsHabitat();
            LoadHabitatLookupData();
 
            // Create any table adapters that are used to insert new records
            m_taSimulations = new dsHabitatTableAdapters.SimulationsTableAdapter();
            m_taSimulations.Connection = m_HabitatManager.ProjectDatabaseConnection;
            m_taSimulations.Adapter.RowUpdated += new System.Data.OleDb.OleDbRowUpdatedEventHandler(m_taSimulations._adapter_RowUpdated);

            m_taProjectDataSources = new dsHabitatTableAdapters.ProjectDataSourcesTableAdapter();
            m_taProjectDataSources.Connection = m_HabitatManager.ProjectDatabaseConnection;
            m_taProjectDataSources.Adapter.RowUpdated += new System.Data.OleDb.OleDbRowUpdatedEventHandler(m_taProjectDataSources._adapter_RowUpdated);

            m_taProjectVariables = new dsHabitatTableAdapters.ProjectVariablesTableAdapter();
            m_taProjectVariables.Connection = m_HabitatManager.ProjectDatabaseConnection;
            m_taProjectVariables.Adapter.RowUpdated += new System.Data.OleDb.OleDbRowUpdatedEventHandler(m_taProjectVariables._adapter_RowUpdated);

            m_taSimulationHSInputs = new dsHabitatTableAdapters.SimulationHSCInputsTableAdapter();
            m_taSimulationHSInputs.Connection = m_HabitatManager.ProjectDatabaseConnection;
            // Do not need primary key after records inserted.
        }

        private void LoadHabitatLookupData()
        {
           // Fill the lookup list tables
            dsHabitatTableAdapters.LookupListsTableAdapter taLists = new dsHabitatTableAdapters.LookupListsTableAdapter();
            taLists.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taLists.Fill(m_dsHabitat.LookupLists);

            dsHabitatTableAdapters.LookupListItemsTableAdapter taListItems = new dsHabitatTableAdapters.LookupListItemsTableAdapter();
            taListItems.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taListItems.Fill(m_dsHabitat.LookupListItems);
            
            //Units
            dsHabitatTableAdapters.UnitsTableAdapter taUnits = new dsHabitatTableAdapters.UnitsTableAdapter();
            taUnits.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taUnits.Fill(m_dsHabitat.Units);

            // Variables
            dsHabitatTableAdapters.VariablesTableAdapter taVariables = new dsHabitatTableAdapters.VariablesTableAdapter();
            taVariables.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taVariables.Fill(m_dsHabitat.Variables);

            // HSI
            dsHabitatTableAdapters.HSITableAdapter taHSI = new dsHabitatTableAdapters.HSITableAdapter();
            taHSI.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taHSI.Fill(m_dsHabitat.HSI);

            // HSC
            dsHabitatTableAdapters.HSCTableAdapter taHSC = new dsHabitatTableAdapters.HSCTableAdapter();
            taHSC.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taHSC.Fill(m_dsHabitat.HSC);

            // HSI Curves
            dsHabitatTableAdapters.HSICurvesTableAdapter taHSICurves = new dsHabitatTableAdapters.HSICurvesTableAdapter();
            taHSICurves.Connection = m_HabitatManager.ProjectDatabaseConnection;
            taHSICurves.Fill(m_dsHabitat.HSICurves);
        }

        public void BuildBatch(List<int> lVisitIDs, int nHSIID, ref int nSucess, ref int nError) //, int nVelocityHSCID, int nDepthHSCID, int nSubstrateHSCID
        {
            nSucess = nError = 0;

            m_CHaMPData.FillByVisitIDS(ref lVisitIDs);

            //dsHabitatTableAdapters.SimulationsTableAdapter taSimulations = new dsHabitatTableAdapters.SimulationsTableAdapter();
            //taSimulations.Connection = m_HabitatManager.ProjectDatabaseConnection;

            dsHabitat.HSIRow rHSI = m_dsHabitat.HSI.FindByHSIID(nHSIID);

            foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit in m_CHaMPData.DS.CHAMP_Visits)
            {
                // Placeholder until visits are filtered at load
                if (!lVisitIDs.Contains(rVisit.VisitID))
                    continue;

                // Create the one simulation for this visit
                dsHabitat.SimulationsRow rSimulation = m_dsHabitat.Simulations.NewSimulationsRow();
                rSimulation.Title = GetSimulationName(rVisit);
                rSimulation.CreatedBy = Environment.UserName;
                rSimulation.CreatedOn = DateTime.Now;
                rSimulation.AddIndividualOutput = true;
                rSimulation.Folder = HMUI.Classes.Paths.GetRelativePath(HMUI.Classes.Paths.GetSpecificSimulationFolder(rSimulation.Title));
                rSimulation.HSIID = nHSIID;
                rSimulation.HSISourcePath = HMUI.Classes.Paths.GetRelativePath(HMUI.Classes.Paths.GetSpecificOutputFullPath(rSimulation.Title));
                rSimulation.IsQueuedToRun = true;
                rSimulation.VisitID = rVisit.VisitID;
                m_dsHabitat.Simulations.AddSimulationsRow(rSimulation);
                // Trigger retrieval of SimulationID;
                m_taSimulations.Update(rSimulation);
                nSucess++;

                // Loop over all the input curves and create the necessary project data sources and inputs
                dsHabitat.ProjectDataSourcesRow rCSVDataSource = null;
                foreach (dsHabitat.HSICurvesRow rHSICurveRow in rHSI.GetHSICurvesRows())
                {
                    dsHabitat.VariablesRow rVariable = m_dsHabitat.Variables.FindByVariableID(rHSICurveRow.HSCRow.HSCVariableID);
                    dsHabitat.ProjectVariablesRow rProjectVariable = null;

                    if (rVariable.VariableName.ToLower().Contains("substrate"))
                    {
                        // Create raster data source

                        m_taProjectDataSources.Update(m_dsHabitat.ProjectDataSources);

                        // Create project variable
                        rProjectVariable = BuildProjectVariable("", rHSICurveRow.HSCRow, rCSVDataSource.DataSourceID);
                    }
                    else
                    {
                        // Create a Data Source for the CSV
                        if (rCSVDataSource == null)
                        {
                            rCSVDataSource = BuildCSVDataSource("Delft 3D CSV Output", rVisit);
                            //m_taProjectDataSources.Update(m_dsHabitat.ProjectDataSources);
                        }

                        if (rVariable.VariableName.ToLower().Contains("velocity"))
                            rProjectVariable = BuildProjectVariable("Velocity.Magnitude", rHSICurveRow.HSCRow, rCSVDataSource.DataSourceID);
                        else
                            rProjectVariable = BuildProjectVariable("Depth", rHSICurveRow.HSCRow, rCSVDataSource.DataSourceID);
                    }

                    // Update the project variables table to trigger retrieval of primary keys
                    m_taProjectVariables.Update(m_dsHabitat.ProjectVariables);

                    // Insert the Simulation HSC input
                    dsHabitat.SimulationHSCInputsRow rSimHSCInput = m_dsHabitat.SimulationHSCInputs.NewSimulationHSCInputsRow();
                    rSimHSCInput.SimulationsRow = rSimulation;
                    rSimHSCInput.HSICurvesRow = rHSICurveRow;
                    rSimHSCInput.HSOutputPath = HMUI.Classes.Paths.GetRelativePath(HMUI.Classes.Paths.GetSpecificOutputHSFullPath(rSimulation.Title, rProjectVariable.Title));
                    rSimHSCInput.HSPreparedPath = HMUI.Classes.Paths.GetRelativePath(HMUI.Classes.Paths.GetSpecificPreparedHSFullPath(rSimulation.Title, rProjectVariable.Title));
                    rSimHSCInput.ProjectVariablesRow = rProjectVariable;
                    m_dsHabitat.SimulationHSCInputs.AddSimulationHSCInputsRow(rSimHSCInput);
                }

                // This update can be done outside the loop because the IDs are not needed elsewhere
                m_taSimulationHSInputs.Update(m_dsHabitat.SimulationHSCInputs);
            }
        }

        private string GetSimulationName(RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit)
        {
            string sResult = string.Format("{0}, {1}, {2}",
                rVisit.CHAMP_SitesRow.CHAMP_WatershedsRow.WatershedName,
                rVisit.VisitYear.ToString(),
                rVisit.CHAMP_SitesRow.SiteName);

            if (!rVisit.IsHitchNameNull())
                sResult += ", " + rVisit.HitchName;

            if (!rVisit.IsCrewNameNull())
                sResult += ", " + rVisit.CrewName;

            return sResult;
        }

        private dsHabitat.ProjectDataSourcesRow BuildCSVDataSource(string sDataSourceName, RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit)
        {
            string sOriginalCSVPath = System.IO.Path.Combine(m_dMonitoringDatafolder.FullName, rVisit.VisitYear.ToString(), rVisit.CHAMP_SitesRow.CHAMP_WatershedsRow.Folder, rVisit.CHAMP_SitesRow.Folder);
            sOriginalCSVPath = System.IO.Path.Combine(sOriginalCSVPath, rVisit.HydraulicModelCSV);
            //if (!System.IO.File.Exists(sOriginalCSVPath))
            //    return null;

            string sInputPath = HMUI.Classes.Paths.GetSpecificInputFullPath(sDataSourceName, "csv");
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sOriginalCSVPath));

            //System.IO.File.Copy(sOriginalCSVPath, sInputPath, true);
            //if (!System.IO.File.Exists(sInputPath))
            //    return null;

            int nDataSourceTypeID = GetLookupListItemID("Project Input Types", "csv");

            dsHabitat.ProjectDataSourcesRow rDataSource = m_dsHabitat.ProjectDataSources.NewProjectDataSourcesRow();
            rDataSource.OriginalPath = sOriginalCSVPath;
            rDataSource.CreatedOn = DateTime.Now;
            rDataSource.DataSourceTypeID = nDataSourceTypeID;
            rDataSource.ProjectPath = HMUI.Classes.Paths.GetRelativePath(sInputPath);
            rDataSource.Title = sDataSourceName;
            m_dsHabitat.ProjectDataSources.AddProjectDataSourcesRow(rDataSource);
            m_taProjectDataSources.Update(rDataSource);

            return rDataSource;
        }

        private dsHabitat.ProjectVariablesRow BuildProjectVariable(string sValueField, dsHabitat.HSCRow rHSC, int nProjectDataSourceID)
        {
            dsHabitat.ProjectVariablesRow rProjectVariable = m_dsHabitat.ProjectVariables.NewProjectVariablesRow();
            rProjectVariable.DataSourceID = nProjectDataSourceID;
            rProjectVariable.Title = rHSC.HSCName;
            rProjectVariable.UnitsID = rHSC.UnitID;
            rProjectVariable.VariableID = rHSC.HSCVariableID;

            if (string.IsNullOrWhiteSpace(sValueField))
                rProjectVariable.SetValueFieldNull();
            else
                rProjectVariable.ValueField = sValueField;

            m_dsHabitat.ProjectVariables.AddProjectVariablesRow(rProjectVariable);
            return rProjectVariable;
        }

        /// <summary>
        /// Find the lookup list item ID using the wildcard for the list and item name
        /// </summary>
        /// <param name="sListName"></param>
        /// <param name="sItemWildCard"></param>
        /// <param name="hData"></param>
        /// <returns></returns>
        private int GetLookupListItemID(string sListName, string sItemWildCard)
        {
            foreach (dsHabitat.LookupListItemsRow rItem in m_dsHabitat.LookupListItems)
                if (rItem.LookupListsRow.ListName.ToLower().Contains(sListName.ToLower()))
                    if (rItem.ItemName.ToLower().Contains(sItemWildCard.ToLower()))
                        return rItem.ItemID;

            return -1;
        }
    }
}
