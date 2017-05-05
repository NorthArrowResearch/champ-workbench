using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class RBTConfig_ChangeDetection
    {
        private double m_fThreshold;
        private List<string> m_lBudgetMasks;

        public RBTConfig_ChangeDetection()
        {
            m_lBudgetMasks = new List<string>();
            m_lBudgetMasks.Add("tier1channelunits");
            m_lBudgetMasks.Add("tier2channelunits");
            m_lBudgetMasks.Add("channel");
            m_lBudgetMasks.Add("bankfull_union");

            m_fThreshold = 80;
        }

        public double Threshold
        {
            get { return m_fThreshold; }
            set { m_fThreshold = value; }
        }

        public void ClearMasks()
        {
            m_lBudgetMasks.Clear();
        }

        public void AddMask(string sMask)
        {
            if (!string.IsNullOrWhiteSpace(sMask))
                if (!m_lBudgetMasks.Contains(sMask))
                    m_lBudgetMasks.Add(sMask);
        }

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc)
        {
            XmlNode nodCD = xmlDoc.CreateElement("change_detection");
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodCD, "calculate", "true");

            XmlNode nodError = naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodCD, "error");
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodError, "fis", "CHaMP_2013.fis");

            XmlNode nodSlope = naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodError, "input");
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodSlope, "type", "slope");

            XmlNode nodPointDensity = naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodError, "input");
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodPointDensity, "type", "pointdensity");

            XmlNode nodDoD = naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodCD, "dod");
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodDoD, "type", "probabilistic");
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodDoD, "threshold", Math.Round(m_fThreshold / 100, 2).ToString());
            naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodDoD, "spatialcoherence", "0");

            XmlNode nodBS = naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodCD, "budget_segregations");
            foreach (string sMask in m_lBudgetMasks)
            {
                XmlNode nodMask = naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodBS, "budget_segregation");
                naru.xml.XMLHelpers.AddAttribute(ref xmlDoc, ref nodMask, "mask", sMask);
            }

            return nodCD;
        }
    }
}
