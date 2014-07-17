namespace CHaMPWorkbench {
    
    public partial class RBTWorkbenchDataSet {
        partial class CHAMP_VisitsDataTable
        {
            public string BuildVisitDataFolder(CHAMP_VisitsRow rVisit , string sTopLevelFolder)
            {
                string sFolder = "";
                if (rVisit is CHAMP_VisitsRow)
                {
                    if (!rVisit.IsFolderNull() && !rVisit.CHAMP_SitesRow.IsFolderNull() && !rVisit.CHAMP_SitesRow.CHAMP_WatershedsRow.IsFolderNull())
                    {
                        sFolder = System.IO.Path.Combine(sTopLevelFolder, rVisit.CHAMP_SitesRow.CHAMP_WatershedsRow.Folder);
                        sFolder = System.IO.Path.Combine(sFolder, rVisit.CHAMP_SitesRow.Folder);
                        sFolder = System.IO.Path.Combine(sFolder, rVisit.Folder);
                    }
                }
                return sFolder;
            }
        }
    }
}
