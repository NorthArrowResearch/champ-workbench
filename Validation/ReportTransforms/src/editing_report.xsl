<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:key name="key-record" match="surveyGDB/table/tablename/record" use="Code" />

  <xsl:template match="surveyGDB">
    <html class="no-js" lang="en">
      <head> 
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Survey Editing Report</title>
        <xsl:call-template name="stylesheet" />
      </head>
      <body>
        <div class="container">
          <xsl:call-template name="header" />
            
          <!--  Background --> 
          <h2>Background</h2>
          <p>Once a survey has been completed and imported from the instrument file into GIS, there are several critical steps in post-processing a survey that involve manual editing.  This report provides descriptive information of the changes to the point cloud from which a surface was generated, the lines present in the survey, and changes to the TIN surface.</p> 
          <p>Since editing can be performed at different times during the point collection and post-processing workflow these summaries are not an indicator of survey quality.</p> 

          <!-- Point and Line Editing -->   
          <div class="panel panel-default"> 
              <div class="panel-heading"><div class="panel-title"><h2>Point and line editing</h2></div></div>
              <div class="panel-body">
                  <p>Crews have the opportunity to adjust point descriptions and generate lines during post processing.</p>
                  <h3>Total Points: <xsl:value-of select="sum(/surveyGDB/table/tablename/record/Count)"/></h3>
              </div>
          </div>

          <!-- Survey Information -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Survey Information</h2></div></div>
            <div class="panel-body">
                <p>The site was surveyed using a [TopCon Magnet v2.5.1] [Total Station] by [ELR] crew [CGSHS].  LiDAR [was] merged with the survey.  It was projected to [UTM Zone 11N]. The visit was [new/successfully reoccupied]. [5] Benchmarks were shot in and [11] control points were used.  [X] locations were occupied.  [No/yes] edits were made to the raw instrument files prior to import into GIS.</p>   
            </div>

          <!-- Survey Information table -->
          <div class="metric">
            <table class="table">
              <tbody>
                <tr>
                  <td>Vertical error notes:</td>
                  <td></td>
                </tr>
                <tr>
                  <td>Horizontal error notes:</td>
                  <td></td>
                </tr>  
              </tbody>
            </table>
          </div>
            
            <!-- TIN Editing Table -->
            <div class="metric">      
              <table class="table">
                <thead>
                  <tr>
                      <th></th>
                      <th>Edit Summary</th>
                      <th>First Tin</th>
                      <th>Last Tin</th>
                      <th>Additions</th>
                      <th>Deletions</th>
                  </tr>
                </thead>
                <tbody>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </tbody>
              </table>
            </div>    

          </div>
        </div>
        <xsl:call-template name="JSON" />
        <xsl:call-template name="javascript" />
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
    <div class="jumbotron">
      <h1>Survey Editing Report</h1>
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
    <script id="ReportJSONData" type="application/json"><xsl:value-of select="/surveyGDB/json"/></script>
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/editing_report.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/editing_report.css?__inline=true" rel="stylesheet" />
  </xsl:template>

</xsl:stylesheet>