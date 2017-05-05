using System.Runtime.InteropServices;

namespace CHaMPWorkbench.Classes
{
    class RasterManager
    {
        public const int MESSAGE_SIZE = 1024;
        public const string m_sRasterManDLLName = "RasterManager.dll";
        public enum Raster_SymbologyStyle
        {
            GSS_DEM = 1,  // DEM
            GSS_DoD = 2,  // DoD
            GSS_Error = 3,  // Error
            GSS_Hlsd = 4,  // HillShade
            GSS_PtDens = 5,  // PointDensity
            GSS_SlopeDeg = 6,  // SlopeDeg
            GSS_SlopePer = 7,  // SlopePC
            GSS_Unknown = 8,  // This one is for when the user doesn't enter it.
        };


        [DllImport(m_sRasterManDLLName, EntryPoint = "RegisterGDAL", CallingConvention = CallingConvention.Cdecl)]
        public extern static int RegisterGDAL();

        [DllImport(m_sRasterManDLLName, EntryPoint = "DestroyGDAL", CallingConvention = CallingConvention.Cdecl)]
        public extern static int DestroyGDAL();

        public static int CreatePNG(string sInputRaster, string sOutputPNG, int nImageQuality, int nLongAxisPixels, int nOpacity, Raster_SymbologyStyle eRasterType)
        {
            return CreatePNG(sInputRaster, sOutputPNG, nImageQuality, nLongAxisPixels, nOpacity, (int)eRasterType);
        }


        [DllImport(m_sRasterManDLLName, EntryPoint = "CreatePNG", CallingConvention = CallingConvention.Cdecl)]
        private extern static int CreatePNG(string sInputRaster, string sOutputPNG, int nImageQuality, int nLongAxisPixels, int nOpacity, int eRasterType);

        [DllImport(m_sRasterManDLLName, EntryPoint = "GetRasterProperties", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetRasterProperties(
        string sFullPath,
        ref double fCellHeight,
        ref double fCellWidth,
        ref double fLeft,
        ref double fTop,
        ref int nRows,
        ref int nCols, ref double fNoData,
        ref int nHasNoData,
        ref int nDataType,
        System.Text.StringBuilder sUnits,
        System.Text.StringBuilder sSpatialReference,
        System.Text.StringBuilder sError);

        // extern "C" RM_DLL_API int IsConcurrent(const char * csRaster1, const char * csRaster2, char * sErr)
        [DllImport(m_sRasterManDLLName, EntryPoint = "IsConcurrent", CallingConvention = CallingConvention.Cdecl)]
        public extern static int IsConcurrent(string sRaster1, string sRaster2, ref System.Text.StringBuilder error);



    }
}
