# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# Name:        CHaMP Classes for GIS Tools                                    #
# Purpose:     Developed for CHaMP Program                                    #
#                                                                             #
# Author:      Kelly Whitehead                                                #
#              South Fork Research, Inc                                       #
#              Seattle, Washington                                            #
#                                                                             #
# Created:     2013-04-23                                                     #
# Version:     13.15          Modified:   2013-04-23                          #
# Copyright:   (c) Kelly Whitehead 2013                                       #
# Licence:     <your licence>                                                 #
# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
#!/usr/bin/env python

# # Import Modules # #
import os
import sys
import arcpy

# # Script Parameters # #
versionGDB = '2014.01'

# # # Classes # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
class SiteGeodatabase():
    """
    Site Geodatabase for CHaMP contains:
        Control_Network
        Survey Information
    """
    def __init__(self,filename):
        self.filename = filename
        self.fcControlNetwork = self.filename + '\\ControlNetwork'
        self.tableSurveyInfromation = self.filename + '\\SiteInfo'
        self.siteGDB = arcpy.Describe(filename)

    def test(self):
        status = 1
        if arcpy.Exists(self.fcControlNetwork) == 0:
            status = status*0
        if arcpy.Exists(self.tableSurveyInfromation)==0:
            status = status*0
        return status

    def getSiteName(self):
        sc = arcpy.SearchCursor(self.tableSurveyInfromation)
        row = sc.next()
        sitename = row.getValue("SiteName")
        del sc, row
        return sitename

class SurveyGeodatabase():
    """
    Survey Geodatabase must exist or exception on object creation.
        
        
    """
    def __init__(self,filename):

        self.filename = filename
        # Feature Datasets
        self.unprojected = filename + "\\Unprojected"
        self.projected = filename + "\\Projected"
        # Survey Data
        self.fcControl_Points = self.projected + "\\Control_Points"
        self.fcTopo_Points = self.projected + "\\Topo_Points"
        self.fcEdgeofWater_Points = self.projected + "\\EdgeofWater_Points"
        self.fcBankfull = self.projected + "\\Bankfull"
        self.fcBreaklines = self.projected + "\\Breaklines"
        self.fcErrorLines = self.projected + "\\Error_Lines"
        self.fcErrorPoints = self.projected + "\\Error_Points"
        self.fcStreamFeatures = self.projected + "\\Stream_Features"
        self.fcLiDAR_Points = self.projected + "\\LiDAR_Points"
        # Processed Data
        self.fcCenterline = self.projected + "\\Centerline"
        self.fcChannel_Units = self.projected+"\\Channel_Units"
        self.fcChannel_Units_Field = self.projected + "\\Channel_Units_Field"
        self.fcBankfull_Centerline = self.projected + "\\BankfullCL"
        self.fcSurveyExtent = self.projected + "\\Survey_Extent"
        self.fcWaterExtent = self.projected + "\\WaterExtent"
        self.fcThalweg = self.projected + "\\Thalweg"
        self.fcWIslands = self.projected + "\\WIslands"
        self.fcBIslands = self.projected + "\\BIslands"
        self.fcWCrossSections = self.projected + "\\WCrossSections"
        self.fcBCrossSections = self.projected + "\\BCrossSections"
        self.fcTINEdits = self.projected + "\\TIN_Edits"
        # Tables
        self.tblQaQcPoints = filename + "\\QaQcPoints"
        self.tblQaQcLines = filename + "\\QaQcLines"
        self.tblQaQcPolygons = filename + "\\QaQcPolygons"
        self.tblQaQcVector = filename + "\\QaQcVector"
        self.tblQaQcTIN = filename + "\\QaQcTIN"
        self.tblLog = filename + "\\Log"
        self.tblOrthogInfo = filename + "\\OrthogInfo"
        self.tblSurveyInfo = filename + "\\SurveyInfo"
        self.tblTransformations = filename + "\\Transformations"
        # Imported Raw Data
        self.fcQaQcRawPoints = filename + "\\QaQcRawPoints"
        self.tblQaQcBacksightLog = filename + "\\QaQcBacksightLog"
        self.tblQaQcUncertaintySummary = filename + "\\QaQcUncertaintySummary"
        # Raster Dataset
        self.DEM = filename + "\\DEM"
        self.DetrendedDEM =  filename + "\\Detrended"
        self.WaterDepth =  filename + "\\Water_Depth"
        self.WSEDEM =  filename + "\\WSEDEM"

        self.describe = arcpy.Describe(filename) #Return Describe object

    def validate(self):
        flag = 1
        flag = flag * arcpy.Exists(self.fcTopo_Points)

    def spatialReference(self):
        if arcpy.Exists(self.projected):
            descProjected = arcpy.Describe(self.projected)
            spatialReferenceText = descProjected.spatialReference
        else:
            spatialReferenceText = ""
        return spatialReferenceText

    def path(self):
        
        return str(self.describe.path)

    def getTransformed_FCs(self):
        originalWorkspace = arcpy.env.workspace
        arcpy.env.workspace = self.filename
        list_Transformed_FCs = arcpy.ListDatasets("Transformation*","Feature")
        arcpy.env.workspace = originalWorkspace
        return list_Transformed_FCs
    
    def get_Transformation_Tables(self):
        originalWorkspace = arcpy.env.workspace
        arcpy.env.workspace = self.filename
        list_Transformation_Tables = arcpy.ListTables("Transformation*")
        arcpy.env.workspace = originalWorkspace
        return list_Transformation_Tables

    def getRawPointData(self):
        retrun [self.fcQaQcRawPoints,self.tblQaQcBacksightLog,self.tblQaQcUncertaintySummary]

class CHaMPToolsEnvironemnt():
    """
    Creates the processing environemnt for CHaMP Tools
    """
    def __init__(self):
        self.ScratchWorkspace = os.environ["AppData"] + "\\RBT\\PythonWorkspace"


# # # Main Function # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
def main():
    pass

    return 1
# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

if __name__ == '__main__':
    # # Input Parameters # #

    # # Run Script # #
    main()

    