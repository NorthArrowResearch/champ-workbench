using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class ChannelUnit : NamedDBObject
    {
        private String m_sTier1;
        private String m_sTier2;
        private int m_nChannelUnitNumber;

        private int m_nBedrock;
        private int m_nBouldersGT256;
        private int m_nCobbles65255;
        private int m_nCoarseGravel1764;
        private int m_nFineGravel316;
        private int m_nSand0062;
        private int m_nFinesLT006;
        private int m_nSumSubstrateCover;

        public ChannelUnit(int nID, int nChannelUnitNumber, String sName, String sTier1, String sTier2)
            : base(nID, sName)
        {
            m_sTier1 = sTier1;
            m_sTier2 = sTier2;
            m_nChannelUnitNumber = nChannelUnitNumber;
        }

        public ChannelUnit(RBTWorkbenchDataSet.CHAMP_ChannelUnitsRow rUnit)
            : base(rUnit.ID, rUnit.Tier1 + " - " + rUnit.Tier2)
        {
            m_sTier1 = rUnit.Tier1;
            m_sTier2 = rUnit.Tier2;
            m_nChannelUnitNumber = rUnit.ChannelUnitNumber;

            if (!rUnit.IsBedrockNull())
                m_nBedrock= rUnit.Bedrock;
            
            if (!rUnit.IsBouldersGT256Null())
                m_nBouldersGT256 = rUnit.BouldersGT256;

            if (!rUnit.IsCobbles65255Null())
                m_nCobbles65255 = rUnit.Cobbles65255;

            if (!rUnit.IsCoarseGravel1764Null())
                m_nCoarseGravel1764 = rUnit.CoarseGravel1764;

            if (!rUnit.IsFineGravel316Null())
                m_nFineGravel316 = rUnit.FineGravel316;

            if (!rUnit.IsSand0062Null())
                m_nSand0062 = rUnit.Sand0062;

            if (!rUnit.IsFinesLT006Null())
                m_nFinesLT006 = rUnit.FinesLT006;

            if (!rUnit.IsSumSubstrateCoverNull())
                m_nSumSubstrateCover = rUnit.SumSubstrateCover;
        }

        public void WriteToXML(ref XmlTextWriter xFile)
        {
            xFile.WriteStartElement("unit");
            xFile.WriteElementString("id", ID.ToString());
            xFile.WriteElementString("unit_number", m_nChannelUnitNumber.ToString());
            xFile.WriteElementString("tier1", m_sTier1);
            xFile.WriteElementString("tier2", m_sTier2);

            if (m_nSumSubstrateCover > 0)
            {
                xFile.WriteElementString("bedrock", m_nBedrock.ToString());
                xFile.WriteElementString("bouldersgt256", m_nBouldersGT256.ToString());
                xFile.WriteElementString("cobbles65255", m_nCobbles65255.ToString());
                xFile.WriteElementString("coarsegravel1764", m_nCoarseGravel1764.ToString());
                xFile.WriteElementString("finegravel316", m_nFineGravel316.ToString());
                xFile.WriteElementString("sand0062", m_nSand0062.ToString());
                xFile.WriteElementString("fineslt006", m_nFinesLT006.ToString());
                xFile.WriteElementString("sumsubstratecolver", m_nSumSubstrateCover.ToString());
            }
            else
            {
                xFile.WriteElementString("bedrock", "");
                xFile.WriteElementString("bouldersgt256", "");
                xFile.WriteElementString("cobbles65255", "");
                xFile.WriteElementString("coarsegravel1764", "");
                xFile.WriteElementString("finegravel316","");
                xFile.WriteElementString("sand0062", "");
                xFile.WriteElementString("fineslt006", "");
                xFile.WriteElementString("sumsubstratecolver", "");
            }
            
            xFile.WriteEndElement(); // unit
        }
    }
}
