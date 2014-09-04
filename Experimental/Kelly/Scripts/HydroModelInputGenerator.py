#-------------------------------------------------------------------------------
# Name:        CHaMP Hydraulic Model Input Conversion Tool
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
from arcpy import sa
import sys
import csv
import winsound
import math
# # Script Parameters # #


# # Functions # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
def main(strSiteID,rasterDEM,rasterWSEDEM,fcThalweg,fcCenterline,listFCTransects,fcEdgeofWater,fcSurveyExtent,fcWaterExtent,csvStationTable,outputWorkspace,tempWorkspace,boolGDB):
    printer("Site: " + str(strSiteID),outputWorkspace)

    # Parse Paths
    pathTableDEM = tempWorkspace + "\\TempTableDEM"
    pathTableWSEDEM = tempWorkspace + '\\TempTableWSEDEM'

    # Create Output GDB
    if boolGDB:
        outGDB = outputWorkspace + '\\TransectGDB.gdb'
        if arcpy.Exists(outGDB):
            arcpy.Delete_management(outGDB)
        arcpy.CreateFileGDB_management(outputWorkspace,"TransectGDB")
        pathSave = outputWorkspace + "\\TransectGDB.gdb\\"
    else:
        pathSave = "in_memory\\"

    # Clear Temp Workspace
    if arcpy.Exists(pathTableDEM):
       arcpy.Delete_management(pathTableDEM)
    if arcpy.Exists(pathTableWSEDEM):
       arcpy.Delete_management(pathTableWSEDEM)

    # Convert DEM to CSVs
    if arcpy.Exists(rasterDEM):
        tableDEM = sa.Sample(rasterDEM,rasterDEM,pathTableDEM)
        with open(outputWorkspace + "\\DEM.csv",'wb') as csvfile:
            cDEM = csv.writer(csvfile)
            scDEM = arcpy.SearchCursor(tableDEM)
            for row in scDEM:
                listValues = [row.getValue("X"),row.getValue("Y"),row.getValue("DEM_1")]
                cDEM.writerow(listValues)
                del listValues
            del scDEM
        csvfile.close()
        del csvfile

    # Convert WSEDEM to CSV
    if arcpy.Exists(rasterWSEDEM):
        tableWSEDEM = sa.Sample(rasterWSEDEM,rasterWSEDEM,pathTableWSEDEM)
        with open(outputWorkspace + '\\WSEDEM.csv','wb') as csvfile:
            cWSEDEM = csv.writer(csvfile)
            scWSEDEM = arcpy.SearchCursor(tableWSEDEM)
            for row in scWSEDEM:
                listValues = [row.getValue("X"),row.getValue("Y"),row.getValue("WSEDEM_1")]
                cWSEDEM.writerow(listValues)
                del listValues
            del scWSEDEM
        csvfile.close()
        del csvfile

    # Convert Thalweg to CSV
    if arcpy.Exists(fcThalweg):
        descThalweg = arcpy.Describe(fcThalweg)
        with open(outputWorkspace + '\\Thalweg.csv','wb') as csvfile:
            cThalweg = csv.writer(csvfile)
            scThalweg = arcpy.SearchCursor(fcThalweg)
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

    # # Transects # #
    if arcpy.Exists(csvStationTable):
        # Parse Paths
        fcTransectsRaw = pathSave + 'TransectPointsRaw'
        fcTransects = pathSave + "Transect_Pins"
        fcTransectsEoW = pathSave + "Transect_EOW_Points"
        fcTransectLines = pathSave + "TransectLines"
        fcTransectLinesClipped = pathSave + "TransectLinesClip"
        fcTransectSegments = pathSave + "TransectSegments"
        fcTransectConstructor = pathSave +"TransectLineConstructor"
        fcTransectRouted = pathSave + "TransectLineConstructorRouted"
        fcStations = pathSave + "Station_Points"
        tableStations = pathSave + "Stations"
        lyrStations = "layerStations"
        tvTableStations = "tableviewTableStations"

        listfc = [fcTransectsRaw,fcTransects,fcTransectLines,fcTransectLinesClipped]
        for item in listfc:
            if arcpy.Exists(item):
                arcpy.Delete_management(item)
        del item, listfc

        # Transect Pin Points
        arcpy.Merge_management(listFCTransects,fcTransectsRaw)
        arcpy.Select_analysis(fcTransectsRaw,fcTransects,""" ("DESCRIPTION" LIKE 'zt%' OR "DESCRIPTION" LIKE 't%') AND "DESCRIPTION" <> 'tos' """)
        arcpy.AddField_management(fcTransects,"TransectID","SHORT")
        descTransects = arcpy.Describe(fcTransects)
        ucTransects = arcpy.UpdateCursor(fcTransects)
        for row in ucTransects:
            feat = row.getValue(descTransects.ShapeFieldName)
            point = feat.getPart()
            strTransectID = row.getValue("DESCRIPTION")
            row.setValue("TransectID",int(filter(lambda x: x.isdigit(), strTransectID)))
            ucTransects.updateRow(row)
            del row
        del ucTransects
        Bankside(fcTransects,fcEdgeofWater,fcCenterline,fcSurveyExtent,pathSave)

        # Transect EOW Points
        arcpy.PointsToLine_management(fcTransects,fcTransectLines,"TransectID")
        arcpy.Clip_analysis(fcTransectLines,fcWaterExtent,fcTransectLinesClipped)
        arcpy.FeatureVerticesToPoints_management(fcTransectLinesClipped,fcTransectsEoW,"BOTH_ENDS")
        Bankside(fcTransectsEoW,fcEdgeofWater,fcCenterline,fcSurveyExtent,pathSave)

        arcpy.AddField_management(fcTransectLinesClipped,"left_X","DOUBLE")
        arcpy.AddField_management(fcTransectLinesClipped,"left_Y","DOUBLE")
        arcpy.AddField_management(fcTransectLinesClipped,"right_X","DOUBLE")
        arcpy.AddField_management(fcTransectLinesClipped,"right_Y","DOUBLE")
        arcpy.AddField_management(fcTransectLinesClipped,"Bearing","DOUBLE")
        arcpy.AddField_management(fcTransectLinesClipped,"Distance","DOUBLE")

        descTransectsEoW = arcpy.Describe(fcTransectsEoW)
        ucTansectClipped = arcpy.UpdateCursor(fcTransectLinesClipped)
        for transect in ucTansectClipped:
            transectID = transect.getValue("TransectID")
            transectLength = transect.getValue("Shape_Length")
            scPoint = arcpy.SearchCursor(fcTransectsEoW,""" "TransectID" = """ + str(transectID) + """ AND "BankSide" = 'Left' """)
            for row in scPoint:
                feat = row.getValue(descTransectsEoW.ShapeFieldName)
                pnt = feat.getPart()
                transect.left_X = pnt.X
                transect.left_Y = pnt.Y
                x1 = pnt.X
                y1 = pnt.Y
                del row, feat
            del scPoint
            scPoint = arcpy.SearchCursor(fcTransectsEoW,""" "TransectID" = """ + str(transectID) + """ AND "Bankside" = 'Right' """)
            for row in scPoint:
                feat = row.getValue(descTransectsEoW.ShapeFieldName)
                pnt = feat.getPart()
                transect.right_X = pnt.X
                transect.right_Y = pnt.Y
                x2 = pnt.X
                y2 = pnt.Y
                del row, feat
            del scPoint
            transect.Bearing = math.atan2((x2-x1),(y2-y1))
            transect.Distance = transectLength + 0.5 * transectLength
            ucTansectClipped.updateRow(transect)
            del transect
        del ucTansectClipped

        arcpy.BearingDistanceToLine_management(fcTransectLinesClipped,fcTransectConstructor,"left_X","left_Y","Distance","METERS","Bearing","RADS","RHUMB_LINE","TransectID")
        arcpy.AddField_management(fcTransectConstructor,"FromDist","DOUBLE")
        arcpy.CalculateField_management(fcTransectConstructor,"FromDist","0")
        arcpy.CreateRoutes_lr(fcTransectConstructor,"TransectID",fcTransectRouted,"TWO_FIELDS","FromDist","Shape_Length")

        # Transect Segments
        arcpy.SplitLineAtPoint_management(fcTransectLines,fcTransectsEoW,fcTransectSegments,"0.1 Meters")
        Bankside(fcTransectSegments,fcEdgeofWater,fcCenterline,fcSurveyExtent,pathSave)

        # Station Distances
        ### load csv to table
        arcpy.TableToTable_conversion(csvStationTable,pathSave.rstrip("\\"),"Stations")
        ## Zero Distances To the Left Edge of Water.
        arcpy.AddField_management(tableStations,"DistanceFromLEW","DOUBLE")
        scTableStations = arcpy.SearchCursor(tableStations)
        listTransectIDtemp = []
        for item in scTableStations:
            listTransectIDtemp.append(item.TransectNumber)
            del item
        del scTableStations
        setTransectIDs = set(listTransectIDtemp)
        listTransectIDs = list(setTransectIDs)
        printer('TransectCount: ' + str(len(listTransectIDs)),outputWorkspace)

        for transectID in listTransectIDs:
            where = """ "TransectNumber" = """ + str(transectID)
            ucTableStations = arcpy.UpdateCursor(tableStations,where,"","","StationNumber A")
            for station in ucTableStations:
                if station.StationNumber == 1:
                    valueZeroDistance = station.Distance
                station.DistanceFromLEW = station.Distance - valueZeroDistance
                ucTableStations.updateRow(station)
                del station
            del ucTableStations
        ### Create Event Table based on distances
        arcpy.MakeRouteEventLayer_lr(fcTransectRouted, "TransectID", tableStations, "TransectNumber POINT DistanceFromLEW" , lyrStations)
        arcpy.CopyFeatures_management(lyrStations,fcStations)
        ### Create Event Layer
        ### export x y to CSV

        # Write To Transect Pins CSV
        descTransects = arcpy.Describe(fcTransects)
        with open(outputWorkspace + '\\Transect_Pins.csv','wb') as csvfile:
            cTransects = csv.writer(csvfile)
            scTransects = arcpy.SearchCursor(fcTransects)
            for row in scTransects:
                feat = row.getValue(descTransects.ShapeFieldName)
                point = feat.getPart()
                listValues = [str(row.getValue("TransectID")),row.getValue("BankSide"),point.X,point.Y]
                cTransects.writerow(listValues)
                del listValues,point
                del row
            del scTransects
        csvfile.close()
        del csvfile

        # Write to Transect EoW Points CSV
        descTransectsEoW = arcpy.Describe(fcTransectsEoW)
        with open(outputWorkspace + '\\Transect_WaterEdge.csv','wb') as csvfile:
            cTransectsEoW = csv.writer(csvfile)
            scTransectsEoW = arcpy.SearchCursor(fcTransectsEoW)
            for row in scTransectsEoW:
                feat = row.getValue(descTransectsEoW.ShapeFieldName)
                point = feat.getPart()
                listValues = [str(row.getValue("TransectID")),row.getValue("BankSide"),point.X,point.Y]
                cTransectsEoW.writerow(listValues)
                del listValues,point
                del row
            del scTransectsEoW
        csvfile.close()
        del csvfile

        # Write Stations Points to CSV
        descStations = arcpy.Describe(fcStations)
        with open(outputWorkspace + '\\Transect_Stations.csv','wb') as csvfile:
            cStations = csv.writer(csvfile)
            cStations.writerow(["SiteID","TransectNumber","StationNumber","X","Y","Velocity","Depth"])
            scStations = arcpy.SearchCursor(fcStations)
            for row in scStations:
                if int(row.TransectNumber) > 0:
                    feat = row.getValue(descStations.ShapeFieldName)
                    point = feat.getPart()
                    listValues = [row.SiteID,row.TransectNumber,row.StationNumber,point.X,point.Y,row.Velocity,row.Depth]
                    cStations.writerow(listValues)
                    del point, listValues
                del row
            del scStations
        csvfile.close()
        del csvfile

    # Clean Up
    printer("Complete",outputWorkspace)
    return 1

#  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
def Bankside(fcInFeatures,fcEdgeofWater,fcCentrline,fcSurveyExtent,pathSave):

    fcTempLines = pathSave + "TempLines"
    fcBankPolygons = pathSave + 'BankPolygons'
    lyrBankPolygons = "LayerBankPolygons"
    lyrEdgeofWaterPoints = 'LayerEdgeofWaterPoints'
    lyrInFeatures = "LayerInFeatures"

    listfc = [fcTempLines,fcBankPolygons,lyrBankPolygons,lyrEdgeofWaterPoints,lyrInFeatures]
    for item in listfc:
        if arcpy.Exists(item):
            arcpy.Delete_management(item)
    del item, listfc

    arcpy.AddField_management(fcInFeatures,"BankSide","TEXT",'','',10)
    ##Prepare Polygons
    arcpy.FeatureToLine_management([fcCentrline,fcSurveyExtent],fcTempLines)
    arcpy.ExtendLine_edit(fcTempLines)
    arcpy.FeatureToPolygon_management(fcTempLines,fcBankPolygons)
    ## Make Features
    arcpy.MakeFeatureLayer_management(fcBankPolygons,lyrBankPolygons)
    arcpy.MakeFeatureLayer_management(fcEdgeofWater,lyrEdgeofWaterPoints)
    arcpy.MakeFeatureLayer_management(fcInFeatures,lyrInFeatures)
    ## Initialize Bank Polygon
    arcpy.SelectLayerByAttribute_management(lyrBankPolygons, "NEW_SELECTION", """  "OBJECTID" = 1 """)
    ## Count lw
    arcpy.SelectLayerByLocation_management(lyrEdgeofWaterPoints,"INTERSECT",lyrBankPolygons,'',"NEW_SELECTION")
    arcpy.SelectLayerByAttribute_management(lyrEdgeofWaterPoints,"SUBSET_SELECTION",""" "DESCRIPTION" = 'lw' """)
    countLW = int(arcpy.GetCount_management(lyrEdgeofWaterPoints).getOutput(0))
    arcpy.AddMessage("LW:" + str(countLW))
    ##Count rw
    arcpy.SelectLayerByLocation_management(lyrEdgeofWaterPoints,"INTERSECT",lyrBankPolygons,'',"NEW_SELECTION")
    arcpy.SelectLayerByAttribute_management(lyrEdgeofWaterPoints,"SUBSET_SELECTION",""" "DESCRIPTION" = 'rw' """)
    countRW = int(arcpy.GetCount_management(lyrEdgeofWaterPoints).getOutput(0))
    arcpy.AddMessage("RW:" + str(countRW))
    ## Calculate Bank Field
    if countLW > countRW:
        arcpy.AddMessage("Case1 Left")
        arcpy.SelectLayerByLocation_management(lyrInFeatures,"COMPLETELY_WITHIN",lyrBankPolygons,"","NEW_SELECTION")
        arcpy.CalculateField_management(lyrInFeatures,"BankSide",'"Left"')
        arcpy.SelectLayerByAttribute_management(lyrInFeatures,"SWITCH_SELECTION")
        arcpy.CalculateField_management(lyrInFeatures,"BankSide",'"Right"')
        arcpy.SelectLayerByAttribute_management(lyrInFeatures,"CLEAR_SELECTION")
    else:
        arcpy.AddMessage("Case2 Right")
        arcpy.SelectLayerByLocation_management(lyrInFeatures,"COMPLETELY_WITHIN",lyrBankPolygons,"","NEW_SELECTION")
        arcpy.CalculateField_management(lyrInFeatures,"BankSide",'"Right"')
        arcpy.SelectLayerByAttribute_management(lyrInFeatures,"SWITCH_SELECTION")
        arcpy.CalculateField_management(lyrInFeatures,"BankSide",'"Left"')
        arcpy.SelectLayerByAttribute_management(lyrInFeatures,"CLEAR_SELECTION")

    arcpy.SelectLayerByLocation_management(lyrInFeatures,"INTERSECT",fcCentrline,'',"NEW_SELECTION",) ## Temporary Logic... Works well for transect segments
    arcpy.CalculateField_management(lyrInFeatures,"BankSide", '"InChannel"')
    return 1
# ------------------------------------------------------------------------------
def printer(string,path): # Output messages to interpreter and log file
    f = open(path + '\\Log', 'a')
    print string
    f.write(string + "\n")
    f.close()

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
if __name__ == '__main__':
   # Inputs #
   inputSiteID = sys.argv[1]
   inputDEM = sys.argv[2] ## Optional
   inputWSEDEM = sys.argv[3] ## Optional
   inputThalwegFC = sys.argv[4] ## Optional
   inputCenterlineFC = sys.argv[5]
   inputTransectsFCs = sys.argv[6]
   inputEoWFC = sys.argv[7]
   inputSurveyExtentFC = sys.argv[8]
   inputWaterExtentFC = sys.argv[9]
   inputStationTable = sys.argv[10]
   inputOutputLocation = sys.argv[11]
   inputTempWorkspace = sys.argv[12]
   inputSavetoGDB = sys.argv[13]

   # Run Main #
   main(inputSiteID,inputDEM,inputWSEDEM,inputThalwegFC,inputCenterlineFC,inputTransectsFCs.split(";"),inputEoWFC,inputSurveyExtentFC,inputWaterExtentFC,inputStationTable,inputOutputLocation,inputTempWorkspace,inputSavetoGDB)

   #Freq = 2500 # Set Frequency To 2500 Hertz
   #Dur = 1000 # Set Duration To 1000 ms == 1 second
   #winsound.Beep(Freq,Dur)
