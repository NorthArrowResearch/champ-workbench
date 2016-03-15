using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    class GUTInputProperties
    {
        public decimal LowSlope { get; internal set; }
        public decimal UplandSlope { get; internal set; }
        public decimal Low_ChMargin_Slope { get; internal set; }
        public decimal High_ChMargin_Slope { get; internal set; }
        public decimal FWRelief { get; internal set; }
        public decimal Low_HADBF { get; internal set; }
        public decimal High_HADBF { get; internal set; }
        public decimal Low_BanfullDist { get; internal set; }
        public decimal HighBankfullDist { get; internal set; }
        public decimal LowRelief { get; internal set; }
        public decimal HighRelief { get; internal set; }

        public GUTInputProperties(
            decimal fLowSlope,
            decimal fUplandSlope,
            decimal fLowChMarginSlope,
            decimal fHighChMarginSlope,
            decimal fFWRelief,
            decimal fLowHADBF,
            decimal fHighHADBF,
            decimal fLowBankfullDist,
            decimal fHighBankfullDist,
            decimal fLowRelief,
            decimal fHighRelief)
        {
            LowSlope = fLowSlope;
            UplandSlope = fUplandSlope;
            Low_ChMargin_Slope = fLowChMarginSlope;
            High_ChMargin_Slope = fHighChMarginSlope;
            FWRelief = fFWRelief;
            Low_HADBF = fLowHADBF;
            High_HADBF = fHighHADBF;
            Low_BanfullDist = fLowBankfullDist;
            HighBankfullDist = fHighBankfullDist;
            LowRelief = fLowRelief;
            HighRelief = fHighRelief;
        }

        public GUTInputProperties(GUTInputProperties theProperties)
        {
            LowSlope = theProperties.LowSlope;
            UplandSlope = theProperties.UplandSlope;
            Low_ChMargin_Slope = theProperties.Low_ChMargin_Slope;
            High_ChMargin_Slope = theProperties.High_ChMargin_Slope;
            FWRelief = theProperties.FWRelief;
            Low_HADBF = theProperties.Low_HADBF;
            High_HADBF = theProperties.High_HADBF;
            Low_BanfullDist = theProperties.Low_BanfullDist;
            HighBankfullDist = theProperties.HighBankfullDist;
            LowRelief = theProperties.LowRelief;
            HighRelief = theProperties.HighRelief;
        }

        /// <summary>
        /// Write the model inputs to XML input file.
        /// </summary>
        /// <param name="xmlDoc">The model XML input file</param>
        /// <param name="nodParent">The parent XML node that will contain the input values</param>
        public void Serialize(ref System.Xml.XmlDocument xmlDoc, ref System.Xml.XmlNode nodParent)
        {
            GenerateNode(ref xmlDoc, ref nodParent, "low_slope", LowSlope);
            GenerateNode(ref xmlDoc, ref nodParent, "up_slope", UplandSlope);
            GenerateNode(ref xmlDoc, ref nodParent, "low_cm_slope", Low_ChMargin_Slope);
            GenerateNode(ref xmlDoc, ref nodParent, "up_cm_slope", High_ChMargin_Slope);
            GenerateNode(ref xmlDoc, ref nodParent, "low_hadbf", Low_HADBF);
            GenerateNode(ref xmlDoc, ref nodParent, "up_hadbf", High_HADBF);
            GenerateNode(ref xmlDoc, ref nodParent, "low_relief", LowRelief);
            GenerateNode(ref xmlDoc, ref nodParent, "up_relief", HighRelief);
            GenerateNode(ref xmlDoc, ref nodParent, "low_bf_distance", Low_BanfullDist);
            GenerateNode(ref xmlDoc, ref nodParent, "up_bf_distance", HighBankfullDist);
           GenerateNode(ref xmlDoc, ref nodParent, "fw_relief", FWRelief);
        }

        /// <summary>
        /// Write a model input to XML file
        /// </summary>
        /// <param name="xmlDoc">The model input file XML document</param>
        /// <param name="nodParent">The parent node that will contain the model input</param>
        /// <param name="sNodeName">The name of the xml node for this input</param>
        /// <param name="fValue">The value of the model input</param>
        private void GenerateNode(ref System.Xml.XmlDocument xmlDoc, ref System.Xml.XmlNode nodParent, string sNodeName, decimal fValue)
        {
            System.Xml.XmlNode nodValue = xmlDoc.CreateElement(sNodeName);
            nodValue.InnerText = fValue.ToString();
            nodParent.AppendChild(nodParent);
        }
    }
}
