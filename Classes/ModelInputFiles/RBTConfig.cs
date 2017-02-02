using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using naru.xml;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public class RBTConfig
    {
        public enum RBTModes
        {
            Validate_Data = 1,
            Calculate_Metrics = 10,
            Fix_Orthogonality = 20,
            Create_Site_Geodatabase = 30,
            Hydraulic_Model_Preparation = 50,
            GCD_Analysis = 60
        };

        private string m_sTempFolder = "C:\\CHaMP\\RBTTempFolder";
        private string m_sResultsFile = "Results.xml";

        private string m_sLogFile = "Log.xml";
        private RBTModes m_nMode = RBTModes.Calculate_Metrics;
        private long m_nESRIProduct;
        private long m_nArcGISLicense;
        private string m_sPrecisionFormatString = "0.0####";
        private int m_nChartHeight = 1000;
        private int m_nChartWidth = 1000;
        private bool m_bClearTempWorkspaceAfter = true;
        private bool m_bRequireOrthogonalDEMS = true;
        private bool m_bPreserveArtifacts = true;
        private bool m_bCreateZip = false;
        private double m_fCellSize = 0.1;
        private int m_nPrecision = 1;
        private int m_nRasterBuffer = 10;
        private double m_fCrossSectionSpacing = 0.5;
        private double m_fCrossSectionStation = 0.1;
        private int m_nMaxRiverWidth = 100;
        private int m_nCrossSectionFiltering = 4;
        private int m_nMinBarArea = 3;
        private int m_nThalwegPoolWeight = 100;
        private double m_nThalwegSmoothingTolerance = 2;
        private int m_nErrorRasterKernal = 5;
        private int m_nBankAngleBuffer = 5;
        private bool m_bOutputProfileValues = false;

        private double m_fInitialCrossSectionLength = 50;

        private RBTConfig_ChangeDetection m_ChangeDetection;

        #region "Properties"

        public RBTModes Mode
        {
            get { return m_nMode; }
            set { m_nMode = value; }
        }

        public long ESRIProduct
        {
            get { return m_nESRIProduct; }
            set { m_nESRIProduct = value; }
        }

        public long ArcGISLicense
        {
            get { return m_nArcGISLicense; }
            set { m_nArcGISLicense = value; }
        }

        public string PrecisionFormatString
        {
            get { return m_sPrecisionFormatString; }
            set { m_sPrecisionFormatString = value; }
        }

        public int ChartHeight
        {
            get { return m_nChartHeight; }
            set { m_nChartHeight = value; }
        }

        public int ChartWidth
        {
            get { return m_nChartWidth; }
            set { m_nChartWidth = value; }
        }

        public bool ClearTempWorkspaceAfter
        {
            get { return m_bClearTempWorkspaceAfter; }
            set { m_bClearTempWorkspaceAfter = value; }
        }

        public bool RequireOrthogDEMs
        {
            get { return m_bRequireOrthogonalDEMS; }
            set { m_bRequireOrthogonalDEMS = value; }
        }

        public bool PreserveArtifcats
        {
            get { return m_bPreserveArtifacts; }
            set { m_bPreserveArtifacts = value; }
        }

        public bool CreateZip
        {
            get { return m_bCreateZip; }
            set { m_bCreateZip = value; }
        }

        public double CellSize
        {
            get { return m_fCellSize; }
            set { m_fCellSize = value; }
        }

        public int Precision
        {
            get { return m_nPrecision; }
            set { m_nPrecision = value; }
        }

        public int RasterBuffer
        {
            get { return m_nRasterBuffer; }
            set { m_nRasterBuffer = value; }
        }

        public double CrossSectionSpacing
        {
            get { return m_fCrossSectionSpacing; }
            set { m_fCrossSectionSpacing = value; }
        }

        public double CrossSectionStationSpacing
        {
            get { return m_fCrossSectionStation; }
            set { m_fCrossSectionStation = value; }
        }

        public int MaxRiverWidth
        {
            get { return m_nMaxRiverWidth; }
            set { m_nMaxRiverWidth = value; }
        }

        public int CrossSectionFiltering
        {
            get { return m_nCrossSectionFiltering; }
            set { m_nCrossSectionFiltering = value; }
        }

        public int MinBarArea
        {
            get { return m_nMinBarArea; }
            set { m_nMinBarArea = value; }
        }

        public int ThalwegPoolWeight
        {
            get { return m_nThalwegPoolWeight; }
            set { m_nThalwegPoolWeight = value; }
        }

        public double ThalwegSmoothWeight
        {
            get { return m_nThalwegSmoothingTolerance; }
            set { m_nThalwegSmoothingTolerance = value; }
        }

        public int ErrorRasterKernal
        {
            get { return m_nErrorRasterKernal; }
            set { m_nErrorRasterKernal = value; }
        }

        public int BankAngleBuffer
        {
            get { return m_nBankAngleBuffer; }
            set { m_nBankAngleBuffer = value; }
        }

        public double InitialCrossSectionLength
        {
            get { return m_fInitialCrossSectionLength; }
            set { m_fInitialCrossSectionLength = value; }
        }

        public bool OutputProfileValues
        {
            get { return m_bOutputProfileValues; }
            set { m_bOutputProfileValues = value; }
        }

        public string TempFolder
        {
            get { return m_sTempFolder; }
            set { m_sTempFolder = value; }
        }

        public string ResultsFile
        {
            get { return m_sResultsFile; }
            set { m_sResultsFile = value; }
        }

        public string LogFile
        {
            get { return m_sLogFile; }
            set { m_sLogFile = value; }
        }

        public RBTConfig_ChangeDetection ChangeDetectionConfig
        {
            get { return m_ChangeDetection; }
        }

        #endregion

        public RBTConfig()
        {
            m_ChangeDetection = new RBTConfig_ChangeDetection();
        }

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc)
        {
            XmlNode nodParameters = xmlDoc.CreateElement("parameters");

           XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "rbt_mode", ((int)Mode).ToString());

            string sModes = "";
            foreach (RBTModes eMode in Enum.GetValues(typeof(RBTModes)))
                sModes += eMode.ToString().Replace("_", " ") + " = " + ((int)eMode).ToString() + ", ";
            nodParameters.AppendChild(xmlDoc.CreateComment(sModes.Substring(0, sModes.Length - 2)));

            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "clear_temp_workspace", ClearTempWorkspaceAfter.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "require_orthogonal_rasters", RequireOrthogDEMs.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "raster_cell_size", CellSize.ToString("#0.00"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "raster_precision", Precision.ToString("#0.00"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "raster_buffer", RasterBuffer.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "preserve_artifacts", PreserveArtifcats.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "xs_station_spacing", CrossSectionStationSpacing.ToString("#.00"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "zip_change_detection_results", CreateZip.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "precision_format_string", PrecisionFormatString);
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "max_river_width", MaxRiverWidth.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "cross_section_std_filter", CrossSectionFiltering.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "min_bar_area", MinBarArea.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "thalweg_pool_weight", ThalwegPoolWeight.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "thalweg_smoothing_tolerance", ThalwegSmoothWeight.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "bank_angle_buffer_size", BankAngleBuffer.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "error_raster_point_density_kernel", ErrorRasterKernal.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "chart_width", ChartWidth.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "chart_height", ChartHeight.ToString("#"));
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "initial_cross_section_extension", InitialCrossSectionLength.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "output_profile_values", OutputProfileValues.ToString());

            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "esri_product_code", ESRIProduct.ToString());
            nodParameters.AppendChild(xmlDoc.CreateComment("Engine or Desktop = 100, ArcGIS Desktop = 1, Engine = 2, ArcGIS Reader = 3, ArcGIS Server = 5"));

            XMLHelpers.AddNode(ref xmlDoc, ref nodParameters, "esri_license_level", ArcGISLicense.ToString());
            nodParameters.AppendChild(xmlDoc.CreateComment("Basic = 40, Standard = 50, Advanced = 60, Server = 30, Engine = 10, Engine Geodatabase = 20"));

            XmlNode nodIntervals = xmlDoc.CreateElement("intervals");
            nodParameters.AppendChild(nodIntervals);

            nodIntervals.AppendChild(CreateIntervalNode(ref xmlDoc, ref nodIntervals, "distance", m_fCrossSectionSpacing));

            XmlNode nodRatio = CreateIntervalNode(ref xmlDoc, ref nodIntervals, "ratio", 20.0);
            XMLHelpers.AddAttribute(ref xmlDoc, ref nodRatio, "offset", "5");

            nodParameters.AppendChild(m_ChangeDetection.CreateXMLNode(ref xmlDoc));

            return nodParameters;
        }

        private XmlNode CreateIntervalNode(ref XmlDocument xmlDoc, ref XmlNode nodIntervals, string sType, double fValue)
        {
            XmlNode nodInterval = XMLHelpers.AddNode(ref xmlDoc, ref nodIntervals, "Interval", fValue.ToString());
            XMLHelpers.AddAttribute(ref xmlDoc, ref nodInterval, "type", sType);
            return nodInterval;
        }
    }
}
