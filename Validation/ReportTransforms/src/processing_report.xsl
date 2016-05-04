<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="surveyGDB">
    <html class="no-js" lang="en">
      <head> 
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Survey Processing Report</title>
        <xsl:call-template name="stylesheet" />
      </head>
      <body>
        <div class="container">
          <xsl:call-template name="header" />

          <!-- Survey Information -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Survey Information</h2></div></div>
            <div class="panel-body">
                <p>The site was surveyed using a <span class="response"><xsl:value-of select="/surveyGDB/tables/table/records/record/InstrumentModel" /></span>  by the <span class="response"><xsl:value-of select="/surveyGDB/tables/table/records/record/Organization" /></span> crew <span class="response"><xsl:value-of select="/surveyGDB/tables/table/records/record/Crew" /></span>.  LiDAR [was/was not] merged with the survey.  <span id="bm-points">[0]</span> benchmarks were shot in with <span id="cp-points">[0]</span> control point(s).  <span id="occ-points">[0]</span> locations were occupied. After leaving the field site [answer q1 survey quality report].</p>
                <p>See the Survey Quality Report for detailed information about the workflow, backsights, and point quality from the survey.</p>   
            </div>
            <!-- Survey Information table -->
            <div class="metric">
              <h3 class="table-name">Survey Quality Summary</h3> 
              <table class="table">
                <thead>
                  <tr>
                    <th>Error Type</th>
                    <th>Present in Survey</th>
                    <th>Survey Notes</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td class="notes">Vertical</td>
                    <td>[answer to q2]</td>
                    <td>[answer to q2b]</td>
                  </tr>
                  <tr>
                    <td class="notes">Horizontal</td>
                    <td>[answer to q3]</td>
                    <td>[answer to q3b]</td>
                  </tr>
                  <tr>
                    <td class="notes">Point Quality</td>
                    <td>[answer to q4]</td>
                    <td>[answer to q4b]</td>
                  </tr> 
                </tbody>
              </table>
              <div class="panel-body"><p>Additional Survey Quality notes: </p> </div>
            </div>
          </div>
            
          <!-- Point Editing, Line Editing, and Surface Generation -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h3>Point Editing, Line Editing, and Surface Generation</h3></div></div>
              <div class="panel-body"><!-- from QaQcTIN -->
                <p>There [were/were not] rod height busts to resolve during processing [answer q5 survey qualit report]. ' <span class="response"><xsl:value-of select="/surveyGDB/tables/table/records/record/FinalTIN" /></span>' was the final TIN generated. The survey edits were described as [answer q4 TIN integrity report] and the TIN quality was described as [answer q8 TIN integrity report]. Additional information about the editing that occurred during post-processing of the survey can be found in the TIN Integrity Report.</p>   
              </div>
              <xsl:for-each select="/surveyGDB/tables/table[name='MapImages']/records/record[Title='Water Surface']">
                <div class="panel-body">
                  <img src="{FilePath}" />
                  <p><xsl:value-of select="Title" /></p>
                  <p></p>
                </div>
              </xsl:for-each>
          </div>
            
          <!-- Channel Features -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h3>Channel Features</h3></div></div>
              <div class="panel-body"><!-- from QaQcPolygons -->
                <p>The survey has <span id="poly-water-extent">[0]</span> water extent polygon(s) and <span id="unique-channel-units">[0]</span> unique channel units. [answer q1 processing summary]. Water flow connects all wetted areas in  <span class="response">[answer q3 processing report]</span>. According to the processing crew, the topography beyond the survey and the extent size is described as:  <span class="response">[answer q2 processing summary]</span>.  <span id="bar-points">[0]</span> bar points were collected at the site and <span id="wse-points">[0]</span> water surface shots were collected.</p> 
              </div>
              <xsl:for-each select="/surveyGDB/tables/table[name='MapImages']/records/record[Title='Bankful XSections']">
                <div class="panel-body">
                  <img src="{FilePath}" />
                  <p><xsl:value-of select="Title" /></p>
                  <p></p>
                </div>
              </xsl:for-each>
              
              
              <!-- Channel Feature table -->
            <div class="metric">
              <div class="row">
                <div class="col-md-6">
                  <h3 class="table-name">Channel Feature Summary</h3>
                  <table id="channel-feature-summary" class="table">
                    <tbody>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>

          <!-- Point Density -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Point Density</h2></div></div>
              <div class="panel-body">
                <p>Point density (pts/m<sup>2</sup>) is calculated using a 5m circular radius throughout the site. It is useful for evaluating sampling effort, which can be useful for understanding and improving survey workflow, site complexity, and areas of interest. Point density also contributes to the uncertainty in the topographic representation of land surface and can affect the accuracy of metrics and models generated from DEMs. It is one of the error rasters used in the CHaMP fuzzy inference model for geomorphic change detection products. See Bangen et al. 2016. Error modeling of DEMs from topographic surveys of rivers using fuzzy inference systems. Water Resources Research DOI:10.1002/2015WR018299
                </p>
                <p>According to the crew, the point densities were described as: <span class="response">[answer q5 processing summary]</span>.</p>   
              </div>
              <xsl:for-each select="/surveyGDB/tables/table[name='MapImages']/records/record[FilePath='MapImages\PlainDEM.jpg']">
                <div class="panel-body">
                  <img src="{FilePath}" />
                  <p><xsl:value-of select="Title" /></p>
                  <p></p>
                </div>
              </xsl:for-each>
          </div>
            
          <!-- Error Surface -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Error Surface</h2></div></div>
              <div class="panel-body">
                <p>The error surface raster is generated using a fuzzy inference system (FIS) to combine DEM Slope, Point Density, Interpolation Error, 3-D Point Quality and Roughness input rasters and represents the uncertainty of the topographic representation of a site. The FIS approach allows error to vary spatially across a site, depending on the workflow, instrumentation, and topography that is described in the input rasters. In CHaMP, the error surface is generated using:</p>
                <ul>
                  <li>2 inputs (DEM Slope and Point Density)</li>
                  <li>4 inputs (DEM Slope, Point Density, Interpolation Error and 3-D Point Quality)</li>
                  <li>5 inputs (all input rasters)</li>
                </ul>
                <p>depending on input availability, as instrumentation and processing tools have changed over the years. See Bangen et al. 2016. Error modeling of DEMs from topographic surveys of rivers using fuzzy inference systems. Water Resources Research DOI:10.1002/2015WR018299.</p>
                <p>According to the processing crew, the topographic uncertainty was described as  <span class="response">[notes q4 procesisng summary]</span>.</p>   
              </div>
              <xsl:for-each select="/surveyGDB/tables/table[name='MapImages']/records/record[FilePath='MapImages\ChannelUnits.jpg']">
                <div class="panel-body">
                  <img src="{FilePath}" />
                  <p><xsl:value-of select="Title" /></p>
                  <p></p>
                </div>
              </xsl:for-each>
            </div>


          <!-- Processing Summary -->
          <div class="panel panel-default">
            <div class="panel-heading"><div class="panel-title"><h2>Processing Summary</h2></div></div>
              <div class="panel-body">
                <p>This survey was  <span class="response">[answer q6 processing summary ]</span> to process because  <span class="response">[answer q7 processing summary]</span>.</p>
                <p>Additional processing notes for future reference:</p>
                <ul>
                  <li> <span class="response">[notes q7 processing summary]</span></li>
                  <li>According to the processing crew, recommended areas to resurvey or collect more points would be  <span class="response">[answer q8]</span></li>
                </ul>
              </div>  
          </div>
        </div>
        <xsl:call-template name="javascript" />
        <xsl:call-template name="JSON" />
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
    <div class="jumbotron jumbo-blue">
      <h1>Survey Processing Report</h1>
      <h3>Created on: <xsl:value-of select="/surveyGDB/xmlcreated" /></h3>
      <div class="row">
        <div class="col-md-12">
          <div class="row">
            <div class="col-md-12">
              <h2><xsl:value-of select="/surveyGDB/table/tablename/record/Watershed" /></h2> 
              <h2>Site Name: <xsl:value-of select="/surveyGDB/tables/table/records/record/SiteID" /></h2>
              <ul class="list-unstyled">
                <li>Visit ID: <xsl:value-of select="/surveyGDB/tables/table/records/record/VisitID" /></li>
                <li>Sampled: <xsl:value-of select="/surveyGDB/tables/table/records/record/SurveyDate" /></li>
                <li>Visit Type: <xsl:value-of select="/surveyGDB/tables/table/records/record/VisitType" /></li>
                <li>Survey Quality: </li>
                <li>Instrument Type: <xsl:value-of select="/surveyGDB/tables/table/records/record/SurveyInstrument" /></li>
                <li>Projection: <xsl:value-of select="/surveyGDB/tables/table/records/record/Projection" /></li>
                <li>Survey GDB Name: <xsl:value-of select="/surveyGDB/filename" /></li>
                <li>Process with Toolbar version: <xsl:value-of select="/surveyGDB/toolbarVersion" /></li>
                <li>Total processing time: </li>
              </ul>
             </div>
          </div>
        </div>
      </div>
    </div>
  </xsl:template>
    
  <xsl:template name="JSON">
    <script id="ReportJSONData" type="application/json"><xsl:value-of select="/surveyGDB/JSON"/></script>
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/processing_report.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/processing_report.css?__inline=true" rel="stylesheet" />
  </xsl:template>

</xsl:stylesheet>