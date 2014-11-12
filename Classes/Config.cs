using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class Config
    {
        #region "Members"

        private string m_sTempFolder = "C:\\CHaMP\\RBTTempFolder";
        private string m_sResultsFile = "Results.xml";

        private string m_sLogFile = "Log.xml";
        private int m_nMode = 1;
        private int m_nESRIProduct;
        private int m_nArcGISLicense;
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

        #endregion
        
        #region "Properties"

        public int Mode
        {
            get { return m_nMode; }
            set { m_nMode = value; }
        }

        public int ESRIProduct
        {
            get { return m_nESRIProduct; }
            set { m_nESRIProduct = value; }
        }

        public int ArcGISLicense
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

        public Config()
        {
            m_ChangeDetection = new RBTConfig_ChangeDetection();
        }

        /// <summary>
        /// Write the current settings to an RBT input XML file
        /// </summary>
        /// <param name="xmlFile">XML text writer for the input file</param>
        /// <remarks></remarks>

        public void WriteToXML(System.Xml.XmlTextWriter xmlFile)
        {
            xmlFile.WriteStartElement("parameters");
            xmlFile.WriteElementString("rbt_mode", Mode.ToString());
            xmlFile.WriteComment("Validate Data = 1, Calculate Metrics = 10, Fix Orthogonality = 20, Create Site Geodatabase = 30, Fix Orthogonality With Minimal Validation = 40");
            xmlFile.WriteElementString("clear_temp_workspace", ClearTempWorkspaceAfter.ToString());
            xmlFile.WriteElementString("require_orthogonal_rasters", RequireOrthogDEMs.ToString());
            xmlFile.WriteElementString("raster_cell_size", CellSize.ToString("#0.00"));
            xmlFile.WriteElementString("raster_precision", Precision.ToString("#0.00"));
            xmlFile.WriteElementString("raster_buffer", RasterBuffer.ToString("#"));
            xmlFile.WriteElementString("preserve_artifacts", PreserveArtifcats.ToString());
            xmlFile.WriteElementString("xs_station_spacing", CrossSectionSpacing.ToString("#.00"));
            xmlFile.WriteElementString("zip_change_detection_results", CreateZip.ToString());
            xmlFile.WriteElementString("precision_format_string", PrecisionFormatString);
            xmlFile.WriteElementString("max_river_width", MaxRiverWidth.ToString("#"));
            xmlFile.WriteElementString("cross_section_std_filter", CrossSectionFiltering.ToString("#"));
            xmlFile.WriteElementString("min_bar_area", MinBarArea.ToString("#"));
            xmlFile.WriteElementString("thalweg_pool_weight", ThalwegPoolWeight.ToString("#"));
            xmlFile.WriteElementString("thalweg_smoothing_tolerance", ThalwegSmoothWeight.ToString("#"));
            xmlFile.WriteElementString("bank_angle_buffer_size", BankAngleBuffer.ToString("#"));
            xmlFile.WriteElementString("error_raster_point_density_kernel", ErrorRasterKernal.ToString("#"));
            xmlFile.WriteElementString("chart_width", ChartWidth.ToString("#"));
            xmlFile.WriteElementString("chart_height", ChartHeight.ToString("#"));
            xmlFile.WriteElementString("initial_cross_section_extension", InitialCrossSectionLength.ToString());
            xmlFile.WriteElementString("output_profile_values", OutputProfileValues.ToString());

            xmlFile.WriteElementString("esri_product_code", ESRIProduct.ToString());
            xmlFile.WriteComment("Engine or Desktop = 100, ArcGIS Desktop = 1, Engine = 2, ArcGIS Reader = 3, ArcGIS Server = 5");

            xmlFile.WriteElementString("esri_license_level", ArcGISLicense.ToString());
            xmlFile.WriteComment("Basic = 40, Standard = 50, Advanced = 60, Server = 30, Engine = 10, Engine Geodatabase = 20");

            xmlFile.WriteStartElement("intervals");

            xmlFile.WriteStartElement("interval");
            xmlFile.WriteAttributeString("type", "distance");
            xmlFile.WriteString("0.5");
            xmlFile.WriteEndElement();
            // interval

            xmlFile.WriteStartElement("interval");
            xmlFile.WriteAttributeString("type", "ratio");
            xmlFile.WriteAttributeString("offset", "5");
            xmlFile.WriteString("20");
            xmlFile.WriteEndElement();
            // interval

            xmlFile.WriteEndElement();
            // intervals

            m_ChangeDetection.WriteToXML(xmlFile);

            xmlFile.WriteEndElement();
            // parameters

        }
    }
}
