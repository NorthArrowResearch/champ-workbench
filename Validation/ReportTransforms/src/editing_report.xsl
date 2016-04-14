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

          <!-- Point and Line Editing Table -->
          <div class="metric">
              <table class="table">
                  <thead>
                      <tr>
                          <th></th>
                          <th>Edit Summary</th>
                          <th>First</th>
                          <th>Last</th>
                          <th>Added</th>
                          <th>Deleted</th>
                          <th>Edited</th>
                          <th>Points collected</th>
                      </tr>
                  </thead>
                  <tbody>
                      <xsl:for-each select="/surveygdb/table/tablename/record [count (key('key-record', ./Code)[1] | .) = 1]">
                          <xsl:variable name="this" select="." />
                          <xsl:for-each select="key('key-record', $this/Code)">
                            <tr>
                              <td></td>    
                              <td><xsl:value-of select="Code"/></td>
                              <xsl:variable name="first">
                                <xsl:for-each select="key('key-record', $this/Code)">
                                  <xsl:sort select="TIMESTAMP"/>
                                  <xsl:choose>
                                    <xsl:when test="position() = 1">
                                      <td><xsl:value-of select="Count"/></td>
                                    </xsl:when>
                                    </xsl:choose>
                                  </xsl:for-each>
                              </xsl:variable>
                              <xsl:variable name="last" > 
                                <xsl:for-each select="key('key-record',$this/Code)">
                                  <xsl:sort select="TIMESTAMP" />
                                  <xsl:choose>
                                    <xsl:when test="position() = last()">
                                      <td><xsl:value-of select="Count" /></td>
                                    </xsl:when>
                                  </xsl:choose>
                                </xsl:for-each>
                              </xsl:variable>
                              <xsl:variable name="diff" select="$first - $last" />
                                <xsl:choose>
                                  <xsl:when test="$diff >= 0">
                                    <td><xsl:value-of select="$diff" /></td>
                                    <td>0</td>;
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td>0</td>
                                    <td><xsl:value-of select="$diff" /></td>;
                                  </xsl:otherwise>
                                </xsl:choose>        
                            </tr>
                          </xsl:for-each>
                      </xsl:for-each>
                  </tbody>
              </table>
          </div>
                  
          <!-- TIN Editing -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Tin editing</h2></div></div>
            <div class="panel-body">
              <p>[tin] is the final TIN and it was process on [TIMESTAMP] in [time] minutes.</p>
              <p>TINs are made of triangular facets that have nodes where the facet endpoints meet. These nodes can be generated from points collected by crews or artificial nodes that were generated by GIS during the TIN generation.</p>
              <p>In this survey, edits included the addition of [40] nodes and deletion of [5] nodes for a net change of [2%].  [62%] of the nodes originate from Topo Points.  There are [XX] topo points that are not nodes in TIN. [1] node was changed in a breakline.  A dam [was] removed during TIN editing and a facet [was] removed during TIN editing. TIN editing changed the minimum node elevation by [0m] and the maximum node elevation by [0m].</p>
              <p>There are [XX m of breaklines that were used in the TIN generation and XX were crossed at the time of final TIN validation.</p>    
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
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
    <div class="jumbotron">
      <h1>Survey Editing Report</h1>
      <h3>Created on: </h3>
      <div class="row">
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <h2><xsl:value-of select="/surveyGDB/table/tablename/record/Watershed" /></h2>    
              <ul class="list-unstyled">
                <li>Visit ID: <xsl:value-of select="/surveyGDB/table/tablename/record/VisitID" /></li>
                <li>Site Name: <xsl:value-of select="/surveyGDB/table/tablename/record/SiteID" /></li>
                <li>Sampled: <xsl:value-of select="/surveyGDB/table/tablename/record/SurveyDate" /></li>
                <li>Survey GDB Name: <xsl:value-of select="/surveyGDB/filename" /></li>
              </ul>
            </div>
          </div>
        </div>
     </div>
     <div class="col-md-6"><img class="img"></img></div>
     </div> 
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/editing_report.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/editing_report.css?__inline=true" rel="stylesheet" />
  </xsl:template>

</xsl:stylesheet>