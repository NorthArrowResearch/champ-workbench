using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using HMDesktop.Classes;

namespace CHaMPWorkbench.Habitat
{
    class HabitatBatchBuilder
    {
        private Classes.CHaMPData m_CHaMPData;
        HSProjectManager m_HabitatManager;
        private System.IO.DirectoryInfo m_dHydraulicResultFolder;
        private System.IO.DirectoryInfo m_dD50Folder;
        private string m_sD50RasterFile;

        // This is the database ID of the lookup list item for CSV data source types
        private const int m_nCSVDataSourceTypeID = 61;

        public HabitatBatchBuilder(ref OleDbConnection dbWorkbench, string sHabitatDBPath, string sHydraulicResultTopLevelFolder, string sD50TopLevelFolder, string sD50RasterFile)
        {
            m_CHaMPData = new Classes.CHaMPData(ref dbWorkbench);
            m_dHydraulicResultFolder = new System.IO.DirectoryInfo(sHydraulicResultTopLevelFolder);
            m_dD50Folder = new System.IO.DirectoryInfo(sD50TopLevelFolder);
            m_sD50RasterFile = sD50RasterFile;

            m_HabitatManager = new HSProjectManager(sHabitatDBPath);
        }

        public void WipeConfData() {
            m_HabitatManager.ProjectDatabase.SimulationFISInputs.Clear();
            m_HabitatManager.ProjectDatabase.SimulationHSCInputs.Clear();
            m_HabitatManager.ProjectDatabase.ProjectVariables.Clear();
            m_HabitatManager.ProjectDatabase.ProjectDataSources.Clear();
            m_HabitatManager.ProjectDatabase.SimulationMeta.Clear();
            m_HabitatManager.ProjectDatabase.Simulations.Clear();
            m_HabitatManager.ProjectDatabase.AcceptChanges();
            HSProjectManager.Instance.Save();
        }

        public void BuildBatch(List<int> lVisitIDs, List<HabitatModelDef> lModels, ref int nSucess, ref int nError) //, int nVelocityHSCID, int nDepthHSCID, int nSubstrateHSCID
        {
            nSucess = nError = 0;

            // A Little sloppy but if you pass in a master template then it creates a project for you
            if (m_HabitatManager.ProjectDatabase.Projects.Count == 0)
            {
                dsHabitat.ProjectsRow rProject = m_HabitatManager.ProjectDatabase.Projects.NewProjectsRow();
                rProject.ProjectID = 1;
                rProject.ScaleID = 54;
                rProject.Computer = "My Computer";
                rProject.CreatedBy = "Workbench";
                rProject.Description = "";
                rProject.DateCreated = DateTime.Now;
                rProject.Title = "myProject";

                m_HabitatManager.ProjectDatabase.Projects.AddProjectsRow(rProject);
                m_HabitatManager.ProjectDatabase.Projects.AcceptChanges();
            }

            // Erase all the previous Conf data (simulations, inputs, variables etc).
            WipeConfData();


            m_CHaMPData.FillByVisitIDS(ref lVisitIDs);
            CHaMPWorkbench.Classes.RasterManager.RegisterGDAL();
            foreach (HabitatModelDef theModelDef in lModels)
            {
                foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit in m_CHaMPData.DS.CHAMP_Visits)
                {
                    // Placeholder until visits are filtered at load
                    if (!lVisitIDs.Contains(rVisit.VisitID))
                        continue;

                    // Create the one simulation for this visit
                    dsHabitat.SimulationsRow rSimulation = m_HabitatManager.ProjectDatabase.Simulations.NewSimulationsRow();
                    string sModelTitle;
                    string sModelShortName;

                    if (theModelDef.ModelType == HabitatModelDef.ModelTypes.HSI)
                    {
                        sModelTitle = m_HabitatManager.ProjectDatabase.HSI.FindByHSIID(theModelDef.Value).ShortName;
                        sModelShortName = m_HabitatManager.ProjectDatabase.HSI.FindByHSIID(theModelDef.Value).ShortName;
                        rSimulation.HSIID = theModelDef.Value;
                    }
                    else
                    {
                        sModelTitle = m_HabitatManager.ProjectDatabase.FIS.FindByFISID(theModelDef.Value).ShortName;
                        sModelShortName = m_HabitatManager.ProjectDatabase.HSI.FindByHSIID(theModelDef.Value).ShortName;
                        rSimulation.FISID = theModelDef.Value;
                    }

                    rSimulation.Title = GetSimulationName(rVisit, sModelTitle);
                    rSimulation.ShortName = GetSimulationName(rVisit, sModelShortName);
                    rSimulation.CreatedBy = Environment.UserName;
                    rSimulation.CreatedOn = DateTime.Now;
                    rSimulation.RunOn = new DateTime(1970, 1, 1);
                    rSimulation.AddIndividualOutput = true;
                    rSimulation.Folder = Paths.GetRelativePath(Paths.GetSpecificSimulationFolder(rSimulation.Title));
                    rSimulation.OutputRaster = Paths.GetRelativePath(Paths.GetSpecificOutputFullPath(rSimulation.Title, "tif"));
                    rSimulation.OutputCSV = Paths.GetRelativePath(Paths.GetSpecificOutputFullPath(rSimulation.Title, "csv"));
                    rSimulation.IsQueuedToRun = true;
                    rSimulation.CellSize = (float)0.1;

                    // Add the simulation. Now that it's XML, the ID should not need to be retrieved.
                    m_HabitatManager.ProjectDatabase.Simulations.AddSimulationsRow(rSimulation);

                    // Add the metadata we need to sort this out using scrapers.
                    AddSimulationMetaData(ref rSimulation, "visit", rVisit.VisitID.ToString());
                    AddSimulationMetaData(ref rSimulation, "site", rVisit.CHAMP_SitesRow.SiteName);
                    AddSimulationMetaData(ref rSimulation, "watershed", rVisit.CHAMP_SitesRow.CHAMP_WatershedsRow.WatershedName);
                    AddSimulationMetaData(ref rSimulation, "year", rVisit.VisitYear.ToString());

                    // TODO: Flow is a placeholder for now. Will need a real value eventually.
                    AddSimulationMetaData(ref rSimulation, "flow", "");

                    // Trigger retrieval of SimulationID;
                    //m_taSimulations.Update(rSimulation);
                    int nSimulationID = rSimulation.SimulationID;

                    // Temporary fix because the C++ cannot produce a raster when there are no raster inputs.
                    // And cannot produce a CSV when there are just rasters.
                    Boolean bRasterInputs = false;
                    bool bSimulationRecordsOK = false;

                    // Create raster data source
                    System.IO.FileInfo dD50Raster = null;
                    Classes.DataFolders.D50Raster(m_dD50Folder, m_sD50RasterFile, rVisit.VisitID, out dD50Raster);

                    if (theModelDef.ModelType == HabitatModelDef.ModelTypes.HSI)
                        bSimulationRecordsOK = AddHSISimulationChildRecords(ref rSimulation, rVisit, theModelDef.Value, ref bRasterInputs, ref dD50Raster);
                    else
                        bSimulationRecordsOK = AddFISSimulationChildRecords(ref rSimulation, rVisit, theModelDef.Value, ref bRasterInputs);


                    // Need to make a quick rasterman call in order to get the rastermeta
                    if (!string.IsNullOrEmpty(rSimulation.OutputRaster) && dD50Raster != null)
                    {
                        CHaMPWorkbench.Classes.RasterMeta rmSim = new CHaMPWorkbench.Classes.RasterMeta(dD50Raster.FullName);
                        rSimulation.RasterCellSize = rmSim.CellWidth;
                        rSimulation.RasterTop = rmSim.Top;
                        rSimulation.RasterLeft = rmSim.Left;
                        rSimulation.RasterCols = rmSim.Cols;
                        rSimulation.RasterRows = rmSim.Rows;
                        rSimulation.RasterUnits = rmSim.RasterUnits;
                        rSimulation.RasterSpatRef = rmSim.SpatialRef;
                    }
                    else
                    {
                        // No valid found for creating RasterMeta. 
                        rSimulation.SetOutputRasterNull();
                    }

                    if (bSimulationRecordsOK)
                    {
                        HSProjectManager.Instance.Save();
                        nSucess++;
                    }
                    else
                    {
                        HSProjectManager.Instance.ProjectDatabase.RejectChanges();
                        nError++;
                    }
                }
            }
            CHaMPWorkbench.Classes.RasterManager.DestroyGDAL();
        }

        private bool AddSimulationMetaData(ref dsHabitat.SimulationsRow rSimulation, string sKey, string sValue)
        {
            dsHabitat.SimulationMetaRow rMeta = m_HabitatManager.ProjectDatabase.SimulationMeta.NewSimulationMetaRow();
            rMeta.SimulationsRow = rSimulation;
            rMeta.TheKey = sKey;
            rMeta.TheValue = sValue;

            m_HabitatManager.ProjectDatabase.SimulationMeta.AddSimulationMetaRow(rMeta);
            m_HabitatManager.ProjectDatabase.SimulationMeta.AcceptChanges();
            return true;
        }

        private bool AddHSISimulationChildRecords(ref dsHabitat.SimulationsRow rSimulation, RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit, int nHSIID, ref bool bRasterInputs, ref System.IO.FileInfo dD50Raster)
        {
            // This is a temporary list to store the project variables. It is checked to ensure that
            // all are valid before they are actually added to the project.
            List<dsHabitat.ProjectVariablesRow> lProjectVariables = new List<dsHabitat.ProjectVariablesRow>();

            dsHabitat.HSIRow rHSI = m_HabitatManager.ProjectDatabase.HSI.FindByHSIID(nHSIID);

            AddSimulationMetaData(ref rSimulation, "species", m_HabitatManager.ProjectDatabase.LookupListItems.FindByItemID(rHSI.SpeciesID).ItemName);
            AddSimulationMetaData(ref rSimulation, "lifestage", m_HabitatManager.ProjectDatabase.LookupListItems.FindByItemID(rHSI.LifestageID).ItemName);

            // Loop over all the input curves and create the necessary project data sources and inputs
            dsHabitat.ProjectDataSourcesRow rCSVDataSource = null;
            foreach (dsHabitat.HSICurvesRow rHSICurveRow in rHSI.GetHSICurvesRows())
            {
                dsHabitat.VariablesRow rVariable = m_HabitatManager.ProjectDatabase.Variables.FindByVariableID(rHSICurveRow.HSCRow.HSCVariableID);
                dsHabitat.ProjectVariablesRow rProjectVariable = null;

                if (rVariable.VariableName.ToLower().Contains("substrate") || rVariable.VariableName.ToLower().Contains("d50"))
                {

                    if (dD50Raster != null)
                    {
                        dsHabitat.ProjectDataSourcesRow rSubstrateSource = BuildAndCopyProjectDataSource(rVisit.VisitID, "SubstrateRaster", dD50Raster.FullName, false, "raster");
                        // Create project variable
                        rProjectVariable = BuildProjectVariable(rVisit.VisitID, "D50", rHSICurveRow.HSCRow.HSCName, rHSICurveRow.HSCRow.UnitID, rHSICurveRow.HSCRow.VariablesRow.VariableID, rSubstrateSource.DataSourceID);
                        bRasterInputs = true;
                    }
                }
                else
                {
                    // Create a Data Source for the CSV
                    if (rCSVDataSource == null)
                    {
                        System.IO.FileInfo dHydroCSVFile = null;
                        if (Classes.DataFolders.HydroResultCSV(m_dHydraulicResultFolder, rVisit.VisitID, out dHydroCSVFile))
                            rCSVDataSource = BuildAndCopyProjectDataSource(rVisit.VisitID, "Delft 3D CSV Output", dHydroCSVFile.FullName, true, "csv");
                    }

                    // Only proceed and build the project variable if the CSV data source was successfully built.
                    if (rCSVDataSource != null)
                    {
                        if (rVariable.VariableName.ToLower().Contains("velocity"))
                            rProjectVariable = BuildProjectVariable(rVisit.VisitID, "Velocity.Magnitude", rHSICurveRow.HSCRow.HSCName, rHSICurveRow.HSCRow.UnitID, rHSICurveRow.HSCRow.VariablesRow.VariableID, rCSVDataSource.DataSourceID);
                        else
                            rProjectVariable = BuildProjectVariable(rVisit.VisitID, "Depth", rHSICurveRow.HSCRow.HSCName, rHSICurveRow.HSCRow.UnitID, rHSICurveRow.HSCRow.VariablesRow.VariableID, rCSVDataSource.DataSourceID);
                    }
                }

                if (rProjectVariable != null)
                {
                    lProjectVariables.Add(rProjectVariable);
                    // Insert the Simulation HSC input
                    dsHabitat.SimulationHSCInputsRow rSimHSCInput = m_HabitatManager.ProjectDatabase.SimulationHSCInputs.NewSimulationHSCInputsRow();
                    rSimHSCInput.SimulationsRow = rSimulation;
                    rSimHSCInput.HSICurvesRow = rHSICurveRow;
                    rSimHSCInput.HSOutputPath = Paths.GetRelativePath(Paths.GetSpecificOutputHSFullPath(rSimulation.Title, rProjectVariable.Title));
                    rSimHSCInput.HSPreparedPath = Paths.GetRelativePath(Paths.GetSpecificPreparedHSFullPath(rSimulation.Title, rProjectVariable.Title));
                    rSimHSCInput.ProjectVariablesRow = rProjectVariable;
                    m_HabitatManager.ProjectDatabase.SimulationHSCInputs.AddSimulationHSCInputsRow(rSimHSCInput);
                }
            }

            if (lProjectVariables.Count == rHSI.GetHSICurvesRows().Count<dsHabitat.HSICurvesRow>())
            {
                m_HabitatManager.ProjectDatabase.SimulationHSCInputs.AcceptChanges();
                return true;
            }
            else
            {
                m_HabitatManager.ProjectDatabase.SimulationHSCInputs.RejectChanges();
                return false;
            }
        }

        private bool AddFISSimulationChildRecords(ref dsHabitat.SimulationsRow rSimulation, RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit, int nFISID, ref bool bRasterInputs)
        {
            // This is a temporary list to store the project variables. It is checked to ensure that
            // all are valid before they are actually added to the project.
            List<dsHabitat.ProjectVariablesRow> lProjectVariables = new List<dsHabitat.ProjectVariablesRow>();

            dsHabitat.FISRow rFIS = m_HabitatManager.ProjectDatabase.FIS.FindByFISID(nFISID);

            // Loop over all the input curves and create the necessary project data sources and inputs
            dsHabitat.ProjectDataSourcesRow rCSVDataSource = null;

            string sFISRuleFile = Paths.GetAbsolutePath(rFIS.FISRuleFile);
            if (System.IO.File.Exists(sFISRuleFile))
            {
                FISRuleFile fis = new FISRuleFile(sFISRuleFile);
                foreach (string sInput in fis.FISInputs)
                {
                    dsHabitat.ProjectVariablesRow rProjectVariable = null;

                    if (string.Compare(sInput, "velocity", true) == 0 || string.Compare(sInput, "depth", true) == 0)
                    {
                        // Create a Data Source for the CSV
                        if (rCSVDataSource == null)
                        {
                            System.IO.FileInfo dHydroCSVFile = null;
                            if (Classes.DataFolders.HydroResultCSV(m_dHydraulicResultFolder, rVisit.VisitID, out dHydroCSVFile))
                            {
                                rCSVDataSource = BuildAndCopyProjectDataSource(rVisit.VisitID, "Delft 3D CSV Output", dHydroCSVFile.FullName, true, "csv");
                            }
                        }

                        if (rCSVDataSource != null)
                        {
                            if (sInput.ToLower().Contains("velocity"))
                            {
                                rProjectVariable = BuildProjectVariable(rVisit.VisitID, "Velocity.Magnitude", "Velocity", 5, 22, rCSVDataSource.DataSourceID);
                            }
                            else
                                rProjectVariable = BuildProjectVariable(rVisit.VisitID, "Depth", "Depth", 8, 8, rCSVDataSource.DataSourceID);
                        }

                    }
                    else if (string.Compare(sInput, "grainsize_mm", true) == 0)
                    {
                        System.IO.FileInfo dD50Raster = null;
                        if (Classes.DataFolders.D50Raster(m_dD50Folder, m_sD50RasterFile, rVisit.VisitID, out dD50Raster))
                        {
                            // Create raster data source
                            dsHabitat.ProjectDataSourcesRow rSubstrateSource = BuildAndCopyProjectDataSource(rVisit.VisitID, "SubstrateRaster", dD50Raster.FullName, false, "raster");

                            // Create project variable
                            rProjectVariable = BuildProjectVariable(rVisit.VisitID, "D50", "D50", 12, 12, rSubstrateSource.DataSourceID);
                            bRasterInputs = true;
                        }
                    }
                    else
                        throw new Exception("Unhandled FIS input name '" + sInput + "'.");

                    if (rProjectVariable != null)
                    {
                        lProjectVariables.Add(rProjectVariable);

                        // Insert the Simulation HSC input
                        dsHabitat.SimulationFISInputsRow rSimInput = m_HabitatManager.ProjectDatabase.SimulationFISInputs.NewSimulationFISInputsRow();
                        rSimInput.SimulationsRow = rSimulation;
                        rSimInput.FISInputName = sInput;
                        rSimInput.FISPreparedPath = Paths.GetRelativePath(Paths.GetSpecificPreparedHSFullPath(rSimulation.Title, rProjectVariable.Title));
                        rSimInput.ProjectVariablesRow = rProjectVariable;
                        m_HabitatManager.ProjectDatabase.SimulationFISInputs.AddSimulationFISInputsRow(rSimInput);
                    }
                }

                if (lProjectVariables.Count == fis.FISInputs.Count)
                {
                    m_HabitatManager.ProjectDatabase.SimulationHSCInputs.AcceptChanges();
                    return true;
                }
                else
                {
                    m_HabitatManager.ProjectDatabase.SimulationHSCInputs.RejectChanges();
                    return false;
                }
            }
            else
                return false;
        }

        private string GetSimulationName(RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit, string sHSIName)
        {
            // CBW5583-34565_VISIT_217
            string sSiteName = Paths.RemoveDangerousCharacters(rVisit.CHAMP_SitesRow.SiteName);
            sHSIName = Paths.RemoveDangerousCharacters(sHSIName);

            string sResult = string.Format("{0}_VISIT_{1}_{2}", sSiteName, rVisit.VisitID.ToString(), sHSIName);
            return sResult;
        }

        /// <summary>
        /// Creates the record in the database for the project data source (CSV or raster)
        /// </summary>
        /// <param name="sDataSourceName">Habitat project data source name</param>
        /// <param name="sOriginalPath">Full, absolute path to the original file (CSV or TIF)</param>
        /// <param name="bCopySingleFile">When true this routine copies on the original file path. When false it wildcards the file name .* and copies all files.</param>
        /// <param name="sProjectInputType">"csv" or "raster". Used to lookup the lookuplist item ID in the habitat database</param>
        /// <returns>Remember that this is used for both CSV and raster data sources</returns>
        private dsHabitat.ProjectDataSourcesRow BuildAndCopyProjectDataSource(int nVisitID, string sDataSourceName, string sOriginalPath, Boolean bCopySingleFile, string sProjectInputType)
        {
            sDataSourceName = string.Format(String.Format("{0}_VisitID{1}", sDataSourceName, nVisitID));
            // check that the project data source does not already exist for this combination of visit ID and data source.
            string sProjectDataSourcePath = Paths.GetSpecificInputFullPath(sDataSourceName, System.IO.Path.GetExtension(sOriginalPath));
            string sRelativeProjectDataSourcePath = Paths.GetRelativePath(sProjectDataSourcePath);
            foreach (dsHabitat.ProjectDataSourcesRow rDS in m_HabitatManager.ProjectDatabase.ProjectDataSources)
            {
                if (string.Compare(sDataSourceName, rDS.Title) == 0)
                    return rDS;
            }

            if (!System.IO.File.Exists(sOriginalPath))
                return null;

            int i = 0;
            string sFileName;
            bool bAnyFilesExist = false;
            do
            {
                sFileName = System.IO.Path.GetFileNameWithoutExtension(sProjectDataSourcePath);
                if (i > 0)
                    sFileName = string.Format("{0}_{1}", sFileName, i);

                sProjectDataSourcePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(sProjectDataSourcePath), sFileName);
                sProjectDataSourcePath = System.IO.Path.ChangeExtension(sProjectDataSourcePath, System.IO.Path.GetExtension(sOriginalPath));
                string sFolder = System.IO.Path.GetDirectoryName(sProjectDataSourcePath);
                if (System.IO.Directory.Exists(sFolder))
                {
                    string[] sMatch = System.IO.Directory.GetFiles(sFolder, System.IO.Path.ChangeExtension(sFileName, "*"));
                    bAnyFilesExist = sMatch.Count<string>() > 0;
                }
                i++;

            } while (bAnyFilesExist);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sProjectDataSourcePath));

            // TIF rasters require all the files to be copied.
            string sCopySource;
            if (bCopySingleFile)
                sCopySource = sOriginalPath;
            else
                sCopySource = System.IO.Path.ChangeExtension(sOriginalPath, "*");

            string[] sMatchingFiles = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(sCopySource), System.IO.Path.GetFileName(sCopySource));
            foreach (string sFileToCopy in sMatchingFiles)
            {
                string sCleanName = System.IO.Path.GetFileNameWithoutExtension(sProjectDataSourcePath);
                string sOrigFileName = System.IO.Path.GetFileNameWithoutExtension(sFileToCopy);
                string sFileMiddle = "";
                string sExtension = System.IO.Path.GetExtension(sFileToCopy);

                if (sOrigFileName.IndexOf('.') >= 0)
                {
                    sFileMiddle = sOrigFileName.Substring(sOrigFileName.IndexOf('.'));
                    sExtension = sFileMiddle + sExtension;
                }

                string sDestinationFile = System.IO.Path.ChangeExtension(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(sProjectDataSourcePath), sCleanName), sExtension);

                if (System.IO.File.Exists(sDestinationFile))
                    bAnyFilesExist = true;


                System.IO.File.Copy(sFileToCopy, sDestinationFile, false);
                if (!System.IO.File.Exists(sDestinationFile))
                    return null;
            }

            int nDataSourceTypeID = GetLookupListItemID("Project Input Types", sProjectInputType);

            dsHabitat.ProjectDataSourcesRow rDataSource = m_HabitatManager.ProjectDatabase.ProjectDataSources.NewProjectDataSourcesRow();
            rDataSource.OriginalPath = sOriginalPath;
            rDataSource.CreatedOn = DateTime.Now;
            rDataSource.DataSourceTypeID = nDataSourceTypeID;
            rDataSource.ProjectPath = Paths.GetRelativePath(sProjectDataSourcePath);
            rDataSource.Title = sDataSourceName;

            if (nDataSourceTypeID == m_nCSVDataSourceTypeID)
            {
                rDataSource.XField = "X";
                rDataSource.YField = "Y";
            }

            m_HabitatManager.ProjectDatabase.ProjectDataSources.AddProjectDataSourcesRow(rDataSource);
            return rDataSource;
        }


        private dsHabitat.ProjectVariablesRow BuildProjectVariable(int nVisitID, string sValueField, string sVariableTitle, int nUnitID, int nVariableID, int nProjectDataSourceID)
        {
            sVariableTitle = string.Format("VisitID {0} - {1}", nVisitID, sVariableTitle);

            foreach (dsHabitat.ProjectVariablesRow rPV in m_HabitatManager.ProjectDatabase.ProjectVariables.Rows)
            {
                if (string.Compare(sVariableTitle, rPV.Title, true) == 0)
                    return rPV;
            }

            dsHabitat.ProjectVariablesRow rProjectVariable = m_HabitatManager.ProjectDatabase.ProjectVariables.NewProjectVariablesRow();
            rProjectVariable.DataSourceID = nProjectDataSourceID;
            rProjectVariable.Title = sVariableTitle;
            rProjectVariable.UnitsID = nUnitID;
            rProjectVariable.VariableID = nVariableID;

            if (string.IsNullOrWhiteSpace(sValueField))
                rProjectVariable.SetValueFieldNull();
            else
                rProjectVariable.ValueField = sValueField;

            m_HabitatManager.ProjectDatabase.ProjectVariables.AddProjectVariablesRow(rProjectVariable);
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
            foreach (dsHabitat.LookupListItemsRow rItem in m_HabitatManager.ProjectDatabase.LookupListItems)
                if (rItem.LookupListsRow.ListName.ToLower().Contains(sListName.ToLower()))
                    if (rItem.ItemName.ToLower().Contains(sItemWildCard.ToLower()))
                        return rItem.ItemID;

            return -1;
        }
    }
}
