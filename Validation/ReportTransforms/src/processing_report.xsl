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
                <p>The site was surveyed using a <xsl:value-of select="/surveyGDB/tables/table/records/record/InstrumentModel" />  by the <xsl:value-of select="/surveyGDB/tables/table/records/record/Organization" /> crew <xsl:value-of select="/surveyGDB/tables/table/records/record/Crew" />.  LiDAR [was/was not] merged with the survey.  The survey uses a <xsl:value-of select="/surveyGDB/tables/table/records/record/Projection" /> projection. The type of visit was - <xsl:value-of select="/surveyGDB/tables/table/records/record/VisitType" />. <span id="bm-points">[0]</span> benchmarks were shot in with <span id="cp-points">[0]</span> control point(s).  <span id="occ-points">[0]</span> locations were occupied.  Edits [were/were not] made to the raw instrument files prior to import into GIS.</p>   
            </div>

            <!-- Survey Information table -->
            <div class="metric">
              <table class="table">
                <tbody>
                  <tr>
                    <td class="notes">Vertical error notes:</td>
                    <td>Sample notes about vertical error in the survey.</td><!-- from new questions on survey report review -->
                  </tr>
                  <tr>
                    <td class="notes">Horizontal error notes:</td>
                    <td>More sample notes about horizontal error in the survey.</td><!-- from new questions on survey report review -->
                  </tr>  
                </tbody>
              </table>
            </div>
            
          <!-- Point Editing, Line Editing, and Surface Generation -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h3>Point Editing, Line Editing, and Surface Generation</h3></div></div>
              <div class="panel-body"><!-- from QaQcTIN -->
                <p>'<xsl:value-of select="/surveyGDB/tables/table/records/record/FinalTIN" />' was the final TIN generated. The survey edits were described as [XXXX] because [XXXX]. The crew rated the TIN quality as [XX] </p>   
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
                <p>The survey has <span id="poly-water-extent">[0]</span> water extent polygon(s) and <span id="unique-channel-units">[0]</span> unique channel units. <span id="bar-points">[0]</span> bar points were collected at the site and <span id="wse-points">[0]</span> water surface shots were collected.</p> 
              </div>
              <xsl:for-each select="/surveyGDB/tables/table[name='MapImages']/records/record[Title='Bankful XSections']">
                <div class="panel-body">
                  <img src="{FilePath}" />
                  <p><xsl:value-of select="Title" /></p>
                  <p></p>
                </div>
              </xsl:for-each>
              </div>
              
              <!-- Channel Feature table -->
            <div class="metric">
              <div class="row">
                <div class="col-md-6">
                  <h3>Channel Feature Summary</h3>
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
                <p>Boilerplate information abou the survey's point density data. Two to three sentences on what it is, why we care, how to change it, etc.</p>   
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
                <p>Boilerplate information about the survey's error surface data. Two to three sentences on what it is, why we care, how to improve the quality, etc.</p>   
              </div>
              <xsl:for-each select="/surveyGDB/tables/table[name='MapImages']/records/record[FilePath='MapImages\ChannelUnits.jpg']">
                <div class="panel-body">
                  <img src="{FilePath}" />
                  <p><xsl:value-of select="Title" /></p>
                  <p></p>
                </div>
              </xsl:for-each>
            </div>
          </div>
        <xsl:call-template name="javascript" />
        <xsl:call-template name="JSON" />
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
    <div class="jumbotron">
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
                <li>Instrument Type: <xsl:value-of select="/surveyGDB/tables/table/records/record/SurveyInstrument" /></li>
              </ul>
              <h2>Processing</h2>
              <ul class="list-unstyled"></ul>
                <li>Survey GDB Name: <xsl:value-of select="/surveyGDB/filename" /></li>
                <li>Process with Toolbar version: <xsl:value-of select="/surveyGDB/toolbarVersion" /></li>
                <li>Total processing time: </li>
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