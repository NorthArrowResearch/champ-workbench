using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class BatchInputfileBuilder : InputFileBuilder
    {
        private OleDbConnection m_dbCon;
        private List<int> m_lFieldSeasons;
        private Boolean m_bCalculateMetrics;
        private Boolean m_bChangeDetection;
        private String m_sBatchName;
        private String m_sDefaultInputFileName;

        private RBTWorkbenchDataSet m_ds;

        public BatchInputfileBuilder(OleDbConnection dbCon, List<short> lFieldSeasons, Classes.Config rbtConfig, Classes.Outputs rbtOutputs)
            : base(rbtConfig, rbtOutputs)
        {
            m_ds = new RBTWorkbenchDataSet();
            RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter daW = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            daW.Connection = dbCon;
            daW.Fill(m_ds.CHAMP_Watersheds);

            RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter daS = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            daS.Connection = dbCon;
            daS.Fill(m_ds.CHAMP_Sites);
            
            foreach (short nVisitYear in lFieldSeasons)
            {
                RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter daV = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
                daV.Connection = dbCon;
                daV.ClearBeforeFill = false;
                daV.FillByVisitYear(m_ds.CHAMP_Visits, nVisitYear);

                RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSeg = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
                daSeg.Connection = dbCon;
                daSeg.ClearBeforeFill = false;
                daSeg.FillByVisitYear(m_ds.CHaMP_Segments, nVisitYear);

                RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daC = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
                daC.Connection = dbCon;
                daC.ClearBeforeFill = false;
                daC.FillByVisitYear(m_ds.CHAMP_ChannelUnits, nVisitYear);
            }
            m_dbCon = dbCon;
        }

        public String Run(String sBatchName, String sDefaultInputFileName, String sParentTopoDataFolder , Boolean bCalculateMetrics, Boolean bChangeDetection, Boolean bMakeDEMOrthogonal, bool bIncludeOtherVisits)
        {
            foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit in m_ds.CHAMP_Visits)
            {
                string sVisitTopofolder = m_ds.CHAMP_Visits.BuildVisitDataFolder(rVisit, sParentTopoDataFolder);
                string sInputFile =  m_ds.CHAMP_Visits.BuildVisitDataFolder(rVisit, this.m_Outputs.OutputFolder);

                if (System.IO.Directory.Exists(sVisitTopofolder))
                {
                   XmlTextWriter xmlInput;
                   CreateFile(sInputFile, out xmlInput);
 
                    Visit v = new Visit(rVisit, bCalculateMetrics, bChangeDetection, bChangeDetection || bMakeDEMOrthogonal);
                    v.WriteToXML(ref xmlInput , sVisitTopofolder);

                    if (bIncludeOtherVisits)
                    {
                        foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rOtherVisit in m_ds.CHAMP_Visits)
                        {
                            if (rOtherVisit.VisitID != rVisit.VisitID)
                            {
                                Visit vOther = new Visit(rOtherVisit, false, bChangeDetection && rOtherVisit.IsPrimary,  bChangeDetection || bMakeDEMOrthogonal);
                                vOther.WriteToXML(ref xmlInput , sVisitTopofolder);
                            }
                        }
                    }

                    // Write the end of the file
                    CloseFile(ref xmlInput);
                }
            }

            return "";
        }
    }
}
