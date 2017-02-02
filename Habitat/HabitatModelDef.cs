using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Habitat
{
    public class HabitatModelDef : naru.db.NamedObject
    {
        public enum ModelTypes
        {
            FIS,
            HSI
        }

        private ModelTypes m_eModelType;
        public ModelTypes ModelType { get { return m_eModelType; } }

        private string m_sSpecies;
        private string m_sLifeStage;

        public HabitatModelDef(long nModelID, ModelTypes eModelType, string sTitle, string sSpecies, string sLifeStage)
            : base(nModelID, sTitle)
        {
            m_eModelType = eModelType;
            m_sSpecies = sSpecies;
            m_sLifeStage = sLifeStage;
        }

        public override string ToString()
        {
            string sType = "HSI";
            if (m_eModelType == ModelTypes.FIS)
                sType="FIS";
            
            return string.Format("{0} ({1}, {2}, {3})", base.Name, sType, m_sSpecies, m_sLifeStage);
        }
    }
}
