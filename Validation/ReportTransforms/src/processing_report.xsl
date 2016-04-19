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
                <p>The site was surveyed using a <xsl:value-of select="/surveyGDB/table/tablename/record/InstrumentModel" />  by the <xsl:value-of select="/surveyGDB/table/tablename/record/Organization" /> crew <xsl:value-of select="/surveyGDB/table/tablename/record/Crew" />.  LiDAR [was/was not] merged with the survey.  It was projected to [UTM Zone 11N]. The visit type was - <xsl:value-of select="/surveyGDB/table/tablename/record/VisitType" />. [5] benchmarks were shot in and [11] control points were used.  [X] locations were occupied.  Edits [were/were not] made to the raw instrument files prior to import into GIS.</p>   
            </div>

            <!-- Survey Information table -->
            <div class="metric">
              <table class="table">
                <tbody>
                  <tr>
                    <td>Vertical error notes:</td>
                    <td>Sample notes about vertical error in the survey.</td><!-- from new questions on survey report review -->
                  </tr>
                  <tr>
                    <td>Horizontal error notes:</td>
                    <td>More sample notes about horizontal error in the survey.</td><!-- from new questions on survey report review -->
                  </tr>  
                </tbody>
              </table>
            </div>
          </div>
            
          <!-- Point Editing, Line Editing, and Surface Generation -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h3>Point Editing, Line Editing, and Surface Generation</h3></div></div>
              <div class="panel-body"><!-- from QaQcTIN -->
                <p>[tin0] was the final TIN generated. The survey edits were described as [XXXX] because [XXX]. The crew rated the TIN quality as [XX] </p>   
              </div>
              <div class="panel-body">
                <img  src="../src/img/johnday_sample.png" />
                <p>TIN surface image</p>
                <p></p>
              </div>
          </div>
            
          <!-- Channel Features -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h3>Channel Features</h3></div></div>
              <div class="panel-body"><!-- from QaQcPolygons -->
                <p>There were [2] water extent polygons and [21] unique channel units. Bars [were] evident at the site and water surface shots [were] used.</p>   
              </div>
              <div class="panel-body">
                <img  src="../src/img/johnday_sample.png" />
                <p>Water Depth and Channel Units image</p>
                <p></p>
              </div>
              
              <!-- Channel Feature table -->
            <div class="metric">
              <table class="table">
                <tbody>
                  <tr>
                    <td># of Channel Units</td> <!-- from QaQcPolygons -->
                    <td></td> <!-- ChannelUnitsCount (associated with last TIMESTAMP) -->
                  </tr>
                  <tr>
                    <td>Number of Water Extent Polygons</td> <!-- from QaQcPolygons -->
                    <td></td> <!-- WaterExtentCount (associated with last TIMESTAMP) -->
                  </tr>
                  <tr>
                    <td>Number of Bankfull Extent Polygons</td> <!-- from QaQcPolygons??? -->
                    <td></td> <!-- (associated with last TIMESTAMP) -->
                  </tr>  
                </tbody>
              </table>
            </div>

          <!-- Point Density -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Point Density</h2></div></div>
              <div class="panel-body">
                <p>Boilerplate information abou the survey's point density data. Two to three sentences on what it is, why we care, how to change it, etc.</p>   
              </div>
              <div class="panel-body">
                <img  src="../src/img/johnday_sample.png" />
                <p>Point Density Image</p>
                <p></p>
              </div>
          </div>
            
          <!-- Error Surface -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Error Surface</h2></div></div>
              <div class="panel-body">
                <p>Boilerplate information about the survey's error surface data. Two to three sentences on what it is, why we care, how to improve the quality, etc.</p>   
              </div>
              <div class="panel-body">
                <img  src="../src/img/johnday_sample.png" />
                <p>Error Surface Image</p>
                <p></p>
              </div>
          </div>
        
        </div>
        <xsl:call-template name="javascript" />
        <xsl:call-template name="JSON" />
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
    <div class="jumbotron">
      <h1>Survey Processing Report</h1>
      <h3>Created on: </h3>
      <div class="row">
        <div class="col-md-12">
          <div class="row">
            <div class="col-md-12">
              <h2><xsl:value-of select="/surveyGDB/table/tablename/record/Watershed" /></h2> 
              <h2>Site Name: <xsl:value-of select="/surveyGDB/table/tablename/record/SiteID" /></h2>
              <ul class="list-unstyled">
                <li>Visit ID: <xsl:value-of select="/surveyGDB/table/tablename/record/VisitID" /></li>
                <li>Sampled: <xsl:value-of select="/surveyGDB/table/tablename/record/SurveyDate" /></li>
                <li>Visit Type: <xsl:value-of select="/surveyGDB/table/tablename/record/VisitType" /></li>
                <li>Instrument Type: <xsl:value-of select="/surveyGDB/table/tablename/record/SurveyInstrument" /></li>
              </ul>
              <h2>Processing</h2>
              <ul class="list-unstyled"></ul>
                <li>Survey GDB Name: <xsl:value-of select="/surveyGDB/filename" /></li>
                <li>Process with Toolbar version </li>
                <li>Total processing time: </li>
             </div>
          </div>
        </div>
      </div>
    </div>
  </xsl:template>
    
  <xsl:template name="JSON">
    <script id="ReportJSONData" type="application/json"><xsl:value-of select="/report/json"/></script>
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/processing_report.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/processing_report.css?__inline=true" rel="stylesheet" />
  </xsl:template>

</xsl:stylesheet>