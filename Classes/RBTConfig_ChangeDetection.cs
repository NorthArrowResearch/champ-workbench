using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void WriteToXML(System.Xml.XmlTextWriter xmlFile)
        {
            xmlFile.WriteStartElement("change_detection");
            xmlFile.WriteAttributeString("calculate", "true");

            xmlFile.WriteStartElement("error");
            xmlFile.WriteAttributeString("fis", "CHaMP_2013.fis");

            xmlFile.WriteStartElement("input");
            xmlFile.WriteAttributeString("type", "slope");
            xmlFile.WriteEndElement();

            xmlFile.WriteStartElement("input");
            xmlFile.WriteAttributeString("type", "pointdensity");
            xmlFile.WriteEndElement();

            xmlFile.WriteEndElement(); //error           

            xmlFile.WriteStartElement("dod");
            xmlFile.WriteAttributeString("type", "probabilistic");
            xmlFile.WriteAttributeString("threshold", Math.Round(m_fThreshold /100, 2).ToString());
            xmlFile.WriteAttributeString("spatialcoherence", "0");
            xmlFile.WriteEndElement();
            //Dod

            xmlFile.WriteStartElement("budget_segregations");
            foreach (string sMask in m_lBudgetMasks)
            {
                xmlFile.WriteStartElement("budget_segregation");
                xmlFile.WriteAttributeString("mask", sMask);
                xmlFile.WriteEndElement();
            }
            xmlFile.WriteEndElement(); //budget_segregations

            xmlFile.WriteEndElement(); //change_detection            
        }

    }
}
