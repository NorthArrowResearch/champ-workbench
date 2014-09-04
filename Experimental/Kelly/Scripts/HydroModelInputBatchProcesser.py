#-------------------------------------------------------------------------------
# Name:        CHaMP Hydrolic Model Input Batch Processor
# Purpose:     Prepare Input data for CHaMP Hydro Models
#
# Author:      Kelly Whitehead
#              South Fork Research, Inc
#              Seattle, WA
#
# Created:     24/09/2013
# Version:     1.0
# Modified:
#-------------------------------------------------------------------------------
#!

# # Import Modules # #
import arcpy
import sys
import csv
import os
import glob
#import HydroModelInputGenerator
import HydroModelInputs
import time
import traceback
import CHaMP

# # Script Parameters # #
pathTempFolder = r"C:\ArcTempDirectory"

def main(directorySource,directoryOutput):
    # Headers for Log
    printer("Start of Batch Process",directoryOutput)
    printer(str(time.asctime()),directoryOutput)

    # Loop through source directory
    for site in os.listdir(directorySource):
        directoryCurrent = directorySource + '\\' +  site
        listsurveyGDB = glob.glob(directoryCurrent + "\\*.gdb")
        liststationXLSX = "null" ##glob.glob(directoryCurrent + '\\*.xls')
        if len(listsurveyGDB) == 1: ##and len(liststationXLSX) == 1:
            SurveyGDB = CHaMP.SurveyGeodatabase(listsurveyGDB[0])
            stationXLSX = "null" ##liststationXLSX[0] + "\\Sheet1$"
            #nameDEM = surveyGDB + '\\DEM'
            #nameWSEDEM = surveyGDB + '\\WSEDEM'
            #nameThalweg = surveyGDB + '\\Projected\\Thalweg'
            #nameCenterline = surveyGDB + '\\Projected\\Centerline'
            #nameEdgeofWaterPoints = surveyGDB + '\\Projected\\EdgeofWater_Points'
            #nameErrorPoints = surveyGDB + '\\Projected\\Error_Points'
            #nameStreamFeatures = surveyGDB + '\\Projected\\Stream_Features'
            #nameWaterExtent = surveyGDB + '\\Projected\\WaterExtent'
            #nameSurveyExtent = surveyGDB + '\\Projected\\Survey_Extent'

            #listTransects = nameErrorPoints + ";" + nameStreamFeatures
            outputFolder = directoryOutput + '\\' + str(site)
            if arcpy.Exists(outputFolder):
                pass
            else:
                os.makedirs(outputFolder)
            try:
                printer("   " + site + ": START",directoryOutput)
                #HydroModelInputGenerator.main(str(site),nameDEM,nameWSEDEM,nameThalweg,nameCenterline,listTransects,nameEdgeofWaterPoints,nameSurveyExtent,nameWaterExtent,stationXLSX,outputFolder,pathTempFolder,"FALSE")
                
                
                printer("   " + site + ": COMPLETE",directoryOutput)
            except:
                printer("   " + site + ": EXCEPTION",directoryOutput)
                # Get the geoprocessing error messages
                msgs = arcpy.GetMessage(0)
                msgs += arcpy.GetMessages(2)
                # Return gp error messages for use with a script tool
                arcpy.AddError(msgs)
                # Print gp error messages for use in Python/PythonWin
                printer("***" + msgs,directoryOutput)
                # Get the traceback object
                tb = sys.exc_info()[2]
                tbinfo = traceback.format_tb(tb)[0]
                # Concatenate information together concerning the error into a
                #   message string
                pymsg = tbinfo + "\n" + str(sys.exc_type)+ ": " + str(sys.exc_value)
                # Return python error messages for use with a script tool
                arcpy.AddError(pymsg)
                # Print Python error messages for use in Python/PythonWin
                printer( pymsg + "***",directoryOutput)
        else:
            printer("   " + str(site) + ": Data Incomplete",directoryOutput)

    printer("Batch Complete",directoryOutput)
    printer(str(time.asctime()),directoryOutput)

def printer(string,path): # Output messages to interpreter and log file
    f = open(path + '\\BatchLog.txt', 'a')
    print string
    f.write(string + "\n")
    f.close()
    arcpy.AddMessage(str(string))

if __name__ == '__main__':
    inputSourceDirectory = sys.argv[1]
    inputOutputDirectory = sys.argv[2]

    main(inputSourceDirectory,inputOutputDirectory)
