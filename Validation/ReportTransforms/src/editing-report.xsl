<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="report">
    <html class="no-js" lang="en">
      <head> 
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Survey Editing Report</title>
        <xsl:call-template name="stylesheet" />
      </head>
      <body>
        <div class="container">
          <xsl:call-template name="header" /><!-- jumbotron -->
          <!--  Background --> 
          <h2>Background</h2>
          <p>Once a survey has been completed and imported from the instrument file into GIS, there are several critical steps in post-processing a survey that involve manual editing.  This report provides descriptive information of the changes to the point cloud from which a surface was generated, the lines present in the survey, and changes to the TIN surface.</p> 

          <p>Since editing can be performed at different times during the point collection and post-processing workflow these summaries are not an indicator of survey quality.</p>    
          <!-- Point and Line Editing -->   
          <div class="panel panel-default"> 
              <div class="panel-heading"><div class="panel-title"><h2>Point and line editing</h2></div></div>
              <div class="panel-body">
                  <p>Crews hav ethe opportunity to adjust point descriptions and generate lines during post processing.</p>
                  <h3>Total Points: <xsl:value-of select="sum(/surveyGDB/table[tablename='QaQcPoints'])"/></h3>
              </div>
          </div>

          <!-- Point and Line Editing Table -->
          <div class="metric">
              <table class="table">
                  <thead>
                      <tr>
                          <th>Edit Summary</th>
                          <th>First</th>
                          <th>Last</th>
                          <th>% added</th>
                          <th>% deleted</th>
                          <th>Edited</th>
                          <th>Points collected</th>
                      </tr>
                  </thead>
                  <tbody>
                      <xsl:for-each select="/surveyGDB/table/tablename/record" />
                        <xsl:sort select="TIMESTAMP" />
                        <xsl:choose>
                            <xsl:when test="position() = 1">
                                <xsl:value-of select="Count" />
                            </xsl:when>
                        </xsl:choose>
                      
                  </tbody>
              </table>
          </div>    
                  
          <!-- TIN editing -->
            <h3><xsl:value-of select="TinName" /> is the final TIN and it was processed on <xsl:value-of select="TinProcessDate" /> by [?????].</h3>
            <p><xsl:value-of select="TINCount" /> TINs were generated during a processing time of [0.6] minutes and changes were [MINIMAL]</p>
            <p>Edits included the addition of [40] nodes and deletion of [5] nodes with a total change of [2%].  [62%] of the nodes originate from Topo Points.  There are [XX] topo points that are not nodes in TIN. [1] node was changed in a breakline.  A dam [was] removed during TIN editing and a facet [was] removed during TIN editing.   TIN editing changed the minimum node elevation by [0m] and the maximum node elevation by [0m].</p>
            <p>There are [XX] m of breaklines that were used in the TIN generation and [XX] were crossed at the time of final TIN validation.</p>
              
            <div class="metric">  
                  <h3>TIN Integrity Metrics</h3>        
                  <table class="table">
                    <thead>
                      <tr>
                        <th>Visit ID</th>
                        <xsl:for-each select="watershed-report[MetricID='33']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                        <xsl:for-each select="watershed-report[MetricID='45']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                        <xsl:for-each select="watershed-report[MetricID='37']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                        <xsl:for-each select="watershed-report[MetricID='139']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                        <xsl:for-each select="watershed-report[MetricID='137']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                        <xsl:for-each select="watershed-report[MetricID='3264']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                        <xsl:for-each select="watershed-report[MetricID='3263']">
                          <th><xsl:value-of select="Title" /></th>
                        </xsl:for-each>
                      </tr>
                    </thead>
                    <tbody>
                        <tr>
                          <xsl:for-each select="watershed-report[1]">
                            <td><xsl:value-of select="VisitID"/></td>
                          </xsl:for-each>  
                          <xsl:for-each select="watershed-report[MetricID='33']">   
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                          <xsl:for-each select="watershed-report[MetricID='45']">
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                          <xsl:for-each select="watershed-report[MetricID='37']">
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                          <xsl:for-each select="watershed-report[MetricID='139']">
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                          <xsl:for-each select="watershed-report[MetricID='137']">
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                          <xsl:for-each select="watershed-report[MetricID='3264']">
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                          <xsl:for-each select="watershed-report[MetricID='3263']">
                            <td><xsl:value-of select="AvgOfMetricValue"/></td>
                          </xsl:for-each>
                        </tr>
                    </tbody>
                  </table>
              </div>    
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
      <div class="jumbotron">
          <xsl:template select="surveyGDB">
              <div class="row">
                  <div class="col-md-6">
                      <h1>Survey Editing Report</h1>
                      <h3>Created on: </h3>
                      <div class="row">
                        <div class="col-md-6">
                          <h2><xsl:value-of select="/surveyGDB/table[tablename='SurveyInfo']/record/Watershed" /></h2>    
                          <ul class="list-unstyled">
                            <li>Visit ID: <xsl:value-of select="/surveyGDB/table[tablename='SurveyInfo']/record/VisitID" /></li>
                            <li>Site Name: <xsl:value-of select="/surveyGDB/table[tablename='SurveyInfo']/record/SiteID" /></li><!-- xml attributes? -->
                            <li>Sampled: <xsl:value-of select="/surveyGDB/table[tablename='SurveyInfo']/record/SurveyDate" /></li><!-- xml attributes? -->
                            <li>Survey GDB Name: <xsl:value-of select="surveyGDB/filename" /></li>
                          </ul>
                        </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-6"><img class="img"></img></div> <!-- site map? -->
           </xsl:template> 
       </div> 
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/watershed.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/watershed.css?__inline=true" rel="stylesheet" />
  </xsl:template>
</xsl:stylesheet>