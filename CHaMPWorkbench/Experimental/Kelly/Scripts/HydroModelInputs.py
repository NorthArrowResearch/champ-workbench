#-------------------------------------------------------------------------------
# Name:        CHaMP Hydraulic Model Input Conversion Tool
# Purpose:     Prepare Input data for CHaMP Hydro Models
#
# Author:      Kelly Whitehead
#              South Fork Research, Inc
#              Seattle, WA
#
# Created:     24/09/2013
# Version:     2.0
# Modified:    2014/07/31 Modified to run in Server Environment
#-------------------------------------------------------------------------------
#!

# # Import Modules # #
import sys
import traceback
import csv
import argparse
import arcpy
from arcpy import sa
import CHaMP

# # Script Parameters # #
NoDataValue = -9999 ## output no data value

# # Functions # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

def raster_to_csv(inputRaster,outputRasterCSV,TempGDB):
    """ Export a Raster dataset as a csv file.
    
    Inputs:
    inputRaster -- Raster to be exported
    outputRasterCSV -- Output csv file to be generated
    TempGDB -- Temporary Geodatabase used in processing.

    Return:
    none
    """
    descInputRaster = arcpy.Describe(inputRaster)
    rasterTemp = sa.Con(sa.IsNull(inputRaster),NoDataValue,inputRaster)
    rasterTemp.save(TempGDB + "\\Sample")
    tempTable = sa.Sample(rasterTemp,rasterTemp,TempGDB + '\\TempTable')
    with open(outputRasterCSV,'wb') as csvfile:
        csvRaster = csv.writer(csvfile)
        with arcpy.da.SearchCursor(tempTable,["X","Y","Sample"]) as scTempTable:
            for row in scTempTable:
                csvRaster.writerow(row)
                del row
        del scTempTable, descInputRaster
    csvfile.close()
    arcpy.Delete_management(TempGDB + "\\Sample")
    arcpy.Delete_management(TempGDB + '\\TempTable')

    return

def clearTempWorkspace(tempWorkspace):
    if arcpy.Exists(pathTempWorkspace + "\\TemporaryGDB.gdb"):
        arcpy.Delete_management(pathTempWorkspace + "\\TemporaryGDB.gdb")
    return

#def printer(string,path): # Output messages to interpreter and log file
#    f = open(path + '\\Log', 'a')
#    print string
#    f.write(string + "\n")
#    f.close()

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
if __name__ == '__main__':
    """ Test """
    # Inputs #
    parser = argparse.ArgumentParser(description = "Generate inputs for CHaMP Hydro Input Model from CHaMP Survey Data")
    parser.add_argument('SurveyGeodatabase', help="Filepath of Survey Geodatabase (ex C:\\path\\to\\Survey.gdb)")
    parser.add_argument('wsetin', help='Filepath of Water Surface TIN (ex C:\\path\\to\\wsetin)')
    parser.add_argument('OutputWorkspace', help='Path of output workspace for csv files (ex c:\\path\\to\\outputworkspace)')
    parser.add_argument('TempWorkspace', help='Path of Temporary Workspace (ex C:\\path\\to\\temp)')
    args = parser.parse_args()

    inputSurveyGDB = args.SurveyGeodatabase #sys.argv[1]
    inputWSETIN = args.wsetin #sys.argv[2]
    pathOutputWorkspace = args.OutputWorkspace #sys.argv[3]
    pathTempWorkspace = args.TempWorkspace #sys.argv[4]

    try:
        ## Preprocessing
        clearTempWorkspace(pathTempWorkspace)
        #Check Out Extensions
        arcpy.CheckOutExtension("3D")
        arcpy.CheckOutExtension("Spatial")
        #Prepare Geodatabases
        arcpy.CreateFileGDB_management(pathTempWorkspace,"TemporaryGDB")
        TempGDB = pathTempWorkspace + "\\TemporaryGDB.gdb"
        SurveyGDB = CHaMP.SurveyGeodatabase(inputSurveyGDB)

        ## Validation
        if not arcpy.Exists(SurveyGDB.fcThalweg):
            print("Thalweg not found.")
            raise Exception#ValidationException(SurveyGDB.fcThalweg)
        if not arcpy.Exists(SurveyGDB.DEM):
            print("DEM not found.")
            raise Exception#ValidationException(SurveyGDB.DEM)

        ## Processing
        # Process DEM
        outputCSVDEM = pathOutputWorkspace + "\\DEM.csv"
        raster_to_csv(SurveyGDB.DEM,outputCSVDEM,TempGDB)

        # Process WSEDEM
        outputCSVWSEDEM = pathOutputWorkspace + "\\WSEDEM.csv"
        if not arcpy.Exists(SurveyGDB.WSEDEM):
            WSEDEM = TempGDB + '//WSEDEM'
            descDEM = arcpy.Describe(SurveyGDB.DEM)
            arcpy.env.extent = descDEM.extent
            arcpy.env.snapRaster = SurveyGDB.DEM
            arcpy.TinRaster_3d(inputWSETIN,WSEDEM,"FLOAT","NATURAL_NEIGHBORS","CELLSIZE 0.1")
        else:
            WSEDEM = SurveyGDB.WSEDEM
        raster_to_csv(WSEDEM,outputCSVWSEDEM,TempGDB)

        # Process Thalweg
        descThalweg = arcpy.Describe(SurveyGDB.fcThalweg)
        with open(pathOutputWorkspace + '\\Thalweg.csv','wb') as csvfile:
            cThalweg = csv.writer(csvfile)
            scThalweg = arcpy.SearchCursor(SurveyGDB.fcThalweg)
            for row in scThalweg:
                feat = row.getValue(descThalweg.ShapeFieldName)
                for point in feat.getPart(0):
                    listValues = [point.X, point.Y]
                    cThalweg.writerow(listValues)
                    del listValues,point
                del row
            del scThalweg
        csvfile.close()
        del csvfile

    # Arcpy Exceptions
    except arcpy.ExecuteError:
        print str(arcpy.GetMessages(2))
 
    # General Exception
    except Exception as e:
       print("Python or system error occurred")
       print(e.message)
       print(traceback.format_exc())
    finally:
        arcpy.CheckInExtension("3D")
        arcpy.CheckInExtension("Spatial")