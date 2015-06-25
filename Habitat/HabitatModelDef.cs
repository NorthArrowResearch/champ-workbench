using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Habitat
{
    public class HabitatModelDef : ListItem
    {
        public enum ModelTypes
        {
            FIS,
            HSI
        }

        private ModelTypes m_eModelType;
        public ModelTypes ModelType { get { return m_eModelType; } }

        public HabitatModelDef(int nModelID, ModelTypes eModelType, string sTitle, string sSpecies, string sLifeStage)
            : base(string.Format("{0} (HSI, {1}, {2})", sTitle, sSpecies, sLifeStage), nModelID)
        {
            m_eModelType = eModelType;
        }
    }
}
