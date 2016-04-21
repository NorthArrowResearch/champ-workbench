using System;
using System.Collections.Generic;

namespace CHaMPWorkbench.Classes
{
    class RasterMeta
    {
        public double CellWidth { get; internal set; }
        public double CellHeight { get; internal set; }
        public string RasterUnits { get; internal set; }
        public int Rows { get; internal set; }
        public int Cols { get; internal set; }

        private double m_dLeft, m_dTop, m_dNodata;
        private int m_nDataType, m_nHasNoData;
        private string m_sSpatialReference, m_sError;

        /// <summary>
        /// This Constructor will create a new RasterMeta from a file name and attach it to a ProjectDataSourceRow
        /// </summary>
        /// <param name="sFullRasterPath"></param>
        /// <param name="rDataSource"></param>
        public RasterMeta(string sFullRasterPath)
        {
            init(sFullRasterPath);
        }

        /// <summary>
        /// This Constructor takes explicit values. It will be useful later for CSV mode.
        /// </summary>
        /// <param name="dCellHeight"></param>
        /// <param name="dCellWidth"></param>
        /// <param name="dLeft"></param>
        /// <param name="dTop"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        /// <param name="dNoData"></param>
        /// <param name="nHasNoData"></param>
        /// <param name="nDataType"></param>
        /// <param name="sUnits"></param>
        /// <param name="sSpatialReference"></param>
        /// <param name="rDataSource"></param>
        public RasterMeta(double dCellHeight, double dCellWidth, double dLeft, double dTop, int nRows, int nCols, double dNoData, int nHasNoData, int nDataType,
        string sUnits, string sSpatialReference)
        {
            init(dCellHeight, dCellWidth, dLeft, dTop, nRows, nCols, dNoData, nDataType, sUnits, sSpatialReference, String.Empty);
        }


        /// <summary>
        /// This init sets values explicitly.
        /// </summary>
        /// <param name="dCellHeight"></param>
        /// <param name="dCellWidth"></param>
        /// <param name="dLeft"></param>
        /// <param name="dTop"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        /// <param name="dNoData"></param>
        /// <param name="nHasNoData"></param>
        /// <param name="nDataType"></param>
        /// <param name="sUnits"></param>
        /// <param name="sSpatialReference"></param>
        /// <param name="rDataSource"></param>
        private void init(double dCellHeight, double dCellWidth, double dLeft, double dTop, int nRows, int nCols, double dNoData, int nDataType,
        string sUnits, string sSpatialReference, string sError)
        {
            CellHeight = dCellHeight;
            CellWidth = dCellWidth;
            m_dLeft = dLeft;
            m_dTop = dTop;
            m_dNodata = dNoData;
            Rows = nRows;
            Cols = nCols;
            m_nDataType = nDataType;
            RasterUnits = sUnits;
            m_sError = sError;
            m_sSpatialReference = sSpatialReference;
        }


        /// <summary>
        /// This Init will create a rastermeta by looking it up from a file
        /// </summary>
        /// <param name="path"></param>
        private void init(string path)
        {
            double dCellHeight, dCellWidth, dLeft, dTop, dNodata = 0;
            dCellHeight = dCellWidth = dLeft = dTop = dNodata = 0;
            int nRows, nCols, nHasNoData, nDataType;
            nRows = nCols = nHasNoData = nDataType = 0;

            System.Text.StringBuilder sUnits = new System.Text.StringBuilder(CHaMPWorkbench.Classes.RasterManager.MESSAGE_SIZE);
            System.Text.StringBuilder sSpatialReference = new System.Text.StringBuilder(CHaMPWorkbench.Classes.RasterManager.MESSAGE_SIZE);
            System.Text.StringBuilder sError = new System.Text.StringBuilder(CHaMPWorkbench.Classes.RasterManager.MESSAGE_SIZE);

            CHaMPWorkbench.Classes.RasterManager.GetRasterProperties(path, ref dCellHeight, ref dCellWidth,
                ref dLeft, ref dTop, ref nRows, ref nCols, ref dNodata,
                ref nHasNoData, ref nDataType, sUnits, sSpatialReference, sError);

            init(dCellHeight, dCellWidth, dLeft, dTop, nRows, nCols, dNodata, nDataType, sUnits.ToString(), sSpatialReference.ToString(), sError.ToString());
        }

        public double Top
        {
            get { return m_dTop; }
        }
        public double Left
        {
            get { return m_dLeft; }
        }
        public double Right
        {
            get { return m_dLeft + Width; }
        }
        public double Bottom
        {
            get { return m_dTop - Height; }
        }
        public double Height
        {
            get { return Rows * Math.Abs(CellHeight); }
        }
        public double Width
        {
            get { return Cols * Math.Abs(CellWidth); }
        }
        public string Error
        {
            get { return m_sError; }
        }
        public string SpatialRef
        {
            get { return m_sSpatialReference; }
        }
        /// <summary>
        /// Concurrency Test for RasterMetas
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <returns></returns>
        public bool IsConcurrent(RasterMeta otherExtent)
        {

            bool bIsConcurrent = false;

            if (Left == otherExtent.Left)
            {
                if (Right == otherExtent.Right)
                {
                    if (Top == otherExtent.Top)
                    {
                        if (Bottom == otherExtent.Bottom)
                        {
                            bIsConcurrent = true;
                        }
                    }
                }
            }
            return bIsConcurrent;
        }

        /// <summary>
        ///  Returns true if the DEM raster is orthogonal to the specified precision
        /// </summary>
        /// <returns></returns>
        /// <remarks>Only true if the corner coordinates can be wholy divisible by the precision.
        /// ie. if a value of 2 is supplied then all four corners must be able to be divided by 2 with
        /// no fractions returned.</remarks>
        public bool IsDivisible()
        {
            bool bResult = false;
            if ((Top / Math.Abs(CellHeight)) % 1 == 0)
            {
                if ((Left / CellWidth) % 1 == 0)
                {
                    if ((Right / CellWidth) % 1 == 0)
                    {
                        if ((Bottom / Math.Abs(CellHeight)) % 1 == 0)
                        {
                            bResult = true;
                        }
                    }
                }
            }
            return bResult;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <param name="fCellSize"></param>
        /// <returns></returns>
        /// <remarks>PGB 16 Oct 2015. The CellHeight returned by the property is negative and so
        /// needs to be made positive for comparison. also, the member properties return type
        /// double which does not equate to true when compared to the same value as a single.</remarks>
        public bool IsOrthogonal(RasterMeta otherExtent)
        {

            bool bResult = false;

            if (IsDivisible())
            {
                if (otherExtent.IsDivisible())
                {
                    bResult = true;
                }
            }
            return bResult;
        }

        /// <summary>
        /// Test every raster meta in a list with every other one once.
        /// </summary>
        /// <param name="lRasterMetas"></param>
        /// <returns></returns>
        public static bool IsConcurrentCombinator(List<RasterMeta> lRasterMetas)
        {
            for (int i = 0; i < lRasterMetas.Count - 1; i++)
            {
                for (int j = i + 1; j < lRasterMetas.Count; j++)
                {
                    if (!lRasterMetas[i].IsConcurrent(lRasterMetas[j]))
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        /// <summary>
        /// Test every raster meta in a list with every other one once.
        /// </summary>
        /// <param name="lRasterMetas"></param>
        /// <returns></returns>
        public static bool IsOrthogonalCombinator(List<RasterMeta> lRasterMetas)
        {
            for (int i = 0; i < lRasterMetas.Count - 1; i++)
            {
                for (int j = i + 1; j < lRasterMetas.Count; j++)
                {
                    if (!lRasterMetas[i].IsOrthogonal(lRasterMetas[j]))
                    {
                        return false;
                    }
                }
            }
            return true;

        }


        /// <summary>
        /// Test every raster meta in a list with every other one once.
        /// </summary>
        /// <param name="lRasterMetas"></param>
        /// <returns></returns>
        public static bool IsSameSpatialRef(List<RasterMeta> lRasterMetas)
        {
            for (int i = 0; i < lRasterMetas.Count - 1; i++)
            {
                for (int j = i + 1; j < lRasterMetas.Count; j++)
                {
                    if (String.Compare(lRasterMetas[i].SpatialRef.ToLower(), lRasterMetas[j].SpatialRef.ToLower()) != 0)
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        public RasterMeta Union(RasterMeta otherMeta)
        {
            RasterMeta rmResult = null;

            if (IsOrthogonal(otherMeta))
            {
                if (IsConcurrent(otherMeta))
                    return this;
                else
                {
                    // Basic properties
                    double fLeft = Math.Min(this.Left, otherMeta.Left);
                    double fTop = Math.Max(this.Top, otherMeta.Top);

                    int nCols = (int)(Math.Max(this.Right, otherMeta.Right) / (double)CellWidth);
                    int nRows = (int)(Math.Min(this.Bottom, otherMeta.Bottom) / (double)Math.Abs(CellHeight));

                    rmResult = new RasterMeta(this.CellHeight, this.CellWidth, fLeft, fTop, nRows, nCols, this.m_dNodata, this.m_nHasNoData, this.m_nDataType, this.RasterUnits, this.m_sSpatialReference);
                }
            }

            return rmResult;
        }

        public RasterMeta Intersect(RasterMeta otherMeta)
        {
            RasterMeta rmResult = null;

            if (IsOrthogonal(otherMeta))
            {
                if (IsConcurrent(otherMeta))
                    return this;
                else
                {
                    // Test for overlap
                    // http://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
                    if (this.Left < otherMeta.Right && this.Right > otherMeta.Left && this.Top < otherMeta.Bottom && this.Bottom > otherMeta.Top)
                    {
                        // Basic properties
                        double fLeft = Math.Max(this.Left, otherMeta.Left);
                        double fTop = Math.Min(this.Top, otherMeta.Top);

                        int nCols = (int)(Math.Min(this.Right, otherMeta.Right) / (double)CellWidth);
                        int nRows = (int)(Math.Max(this.Bottom, otherMeta.Bottom) / (double)Math.Abs(CellHeight));

                        rmResult = new RasterMeta(this.CellHeight, this.CellWidth, fLeft, fTop, nRows, nCols, this.m_dNodata, this.m_nHasNoData, this.m_nDataType, this.RasterUnits, this.m_sSpatialReference);
                    }
                }
            }

            return rmResult;
        }


        /// <summary>
        /// Precision as an Integer. Positive numbers are decimals of 10. Negative numbers mean 0.1 0.01 etc.
        /// </summary>
        /// <param name="fVal"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetPrecision(double fVal)
        {
            string numberAsString = fVal.ToString();
            int indexOfDecimalPoint = numberAsString.IndexOf(".");

            if ((fVal == 0))
            {
                return 0;
            }
            //Decimal place
            if ((indexOfDecimalPoint != -1))
            {
                int numberOfDecimals = numberAsString.Substring(indexOfDecimalPoint + 1).Length;
                // Small decimal points are rare so throw out a warning.
                if ((numberOfDecimals > 10))
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("WARNING: Possible Double/Float precision problem. Number: {0}", numberAsString));
                }
                return -(numberOfDecimals);
            }
            else
            {
                // No decimal found. We're into powers of 10 here.
                dynamic numberOfTens = 0;
                while (numberAsString.Substring(numberAsString.Length - numberOfTens - 1) == "0")
                {
                    numberOfTens += 1;
                }
                return numberOfTens;
            }
        }

    }
}
