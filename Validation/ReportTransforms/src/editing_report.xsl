<?xml version="1.0" encoding="UTF8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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
          <xsl:call-template name="header" /><!-- jumbotron -->
            
          <!--  Background --> 
          <h2>Background</h2>
          <p>Once a survey has been completed and imported from the instrument file into GIS, there are several critical steps in post-processing a survey that involve manual editing.  This report provides descriptive information of the changes to the point cloud from which a surface was generated, the lines present in the survey, and changes to the TIN surface.</p> 

          <p>Since editing can be performed at different times during the point collection and post-processing workflow these summaries are not an indicator of survey quality.</p>
            
          <!-- Point and Line Editing -->   
          <div class="panel panel-default"> 
              <div class="panel-heading"><div class="panel-title"><h2>Point and line editing</h2></div></div>
              <div class="panel-body">
                  <p>Crews have the opportunity to adjust point descriptions and generate lines during post processing.</p>
                  <h3>Total Points: <xsl:value-of select="sum(/surveyGDB/table[tablename='QaQcPoints'])"/></h3>
              </div>
          </div>

          <!-- Point Editing Table -->
          <div class="metric">
              <h3>Point Editing Summary</h3>
              <table class="table">
                  <thead>
                      <tr>
                          <th>Edit Summary</th><!-- /surveyGDB/table/tablename[QaQcPoints]/Code -->
                          <th>Count, First</th><!-- based on first TIMESTAMP -->
                          <th>Count, Last</th><!-- based on last TIMESTAMP -->
                          <th>% Added</th><!-- points added=(Last-First/Last+First), points deleted=0 -->
                          <th>% Deleted</th><!-- points added=0, points deleted=(Last-First/Last+First) -->
                          <th>Edited</th><!-- Were points added or deleted? Yes/No -->
                          <th>Points Collected</th><!-- Were points collected? Yes/No -->
                      </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>control points</td> 
                      <td>26</td> 
                      <td>26</td> 
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>tp</td>
                      <td>423</td>
                      <td>416</td>
                      <td>0</td>
                      <td>2%</td>
                      <td>Yes</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>tb</td>
                      <td>53</td>
                      <td>88</td>
                      <td>39%</td>
                      <td>0</td>
                      <td>Yes</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>q</td>
                      <td></td>
                      <td></td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>No</td>
                    </tr>
                    <tr>
                      <td>rw</td>
                      <td>54</td>
                      <td>53</td>
                      <td>0</td>
                      <td>1%</td>
                      <td>Yes</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>swg</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>No</td>
                    </tr>
                    <tr>
                      <td>to</td>
                      <td>17</td>
                      <td>17</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>tos</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>No</td>
                    </tr>
                    <tr>
                      <td>u</td>
                      <td>12</td>
                      <td>12</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>wg</td>
                      <td>69</td>
                      <td>69</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>ws</td>
                      <td>10</td>
                      <td>10</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                  </tbody>
              </table>
          </div>
        
          <!-- Line Editing Table -->
          <div class="metric">
              <h3>Line Editing Summary</h3>
              <table class="table">
                  <thead>
                      <tr>
                          <th>Edit Summary</th><!-- /surveyGDB/table/tablename[QaQcLines]/Code -->
                          <th>Total Length, First</th><!-- based on first TIMESTAMP -->
                          <th>Total length, Last</th><!-- based on last TIMESTAMP -->
                          <th>% Added</th><!-- Length added = (Last-First/Last+First), Length deleted = 0 -->
                          <th>% Deleted</th><!-- Length dded = 0, Length deleted = (Last-First/Last+First) -->
                          <th>Edited</th><!-- Were line lengths added or deleted? Yes/No -->
                          <th>Lines Collected</th><!-- were line records collected? Yes/No -->
                      </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>bf</td> 
                      <td>28</td> 
                      <td>28</td> 
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>bl</td>
                      <td>1389</td>
                      <td>2051</td>
                      <td>32%</td>
                      <td>0</td>
                      <td>Yes</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>br</td>
                      <td>43</td>
                      <td>43</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>lw</td>
                      <td>609</td>
                      <td>609</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>mw</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>No</td>
                    </tr>
                    <tr>
                      <td>rw</td>
                      <td>666</td>
                      <td>654</td>
                      <td>0</td>
                      <td>2%</td>
                      <td>Yes</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>swg</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>No</td>
                    </tr>
                    <tr>
                      <td>tb</td>
                      <td>560</td>
                      <td>917</td>
                      <td>0</td>
                      <td>0</td>
                      <td>Yes</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>to</td>
                      <td>152</td>
                      <td>152</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                    <tr>
                      <td>wg</td>
                      <td>393</td>
                      <td>393</td>
                      <td>0</td>
                      <td>0</td>
                      <td>No</td>
                      <td>Yes</td>
                    </tr>
                  </tbody>
              </table>
          </div>    
                  
                  
          <!-- TIN editing -->
          <div class="panel panel-default"> 
            <div class="panel-heading"><div class="panel-title"><h2>Tin Editing</h2></div></div>
            <div class="panel-body">
              <!-- variables derived from TIN summary table, using JS? -->    
              <p><xsl:value-of select="/surveyGDB/table/tablename[SurveyInfo]/record/FinalTin" /> is the final TIN and it was last processed on [LAST TIMESTAMP] in [FIRST - LAST TIMESTAMP] minutes.</p>
              <p>TINs are made of triangular facets that have nodes where the facet endpoints meet. These nodes can be generated from points collected by crews or artificial nodes that were generated by GIS during the TIN generation.</p>
              <p>In this survey, edits included the addition of [40] nodes and deletion of [5] nodes for a net change of [2%].  [62%] of the nodes originate from Topo Points.  There are [XX] topo points that are not nodes in TIN. [1] node was changed in a breakline.  A dam [was] removed during TIN editing and a facet [was] removed during TIN editing. TIN editing changed the minimum node elevation by [0m] and the maximum node elevation by [0m].</p>
              <p>There are [XX] m of breaklines that were used in the TIN generation and XX were crossed at the time of final TIN validation.</p>    
            </div>
            
            <!-- TIN node Editing summary table -->
            <div class="metric"> 
              <h3>Node Editing Summary</h3>    
              <table class="table">
                <thead>
                  <tr>
                    <th>Edit Summary</th><!-- /surveyGDB/table/tablename[QaQcTIN]/Code -->
                    <th>First TIN</th><!-- based on first TIMESTAMP -->
                    <th>Last TIN</th><!-- based on last TIMESTAMP -->
                    <th>% Addition</th><!-- points added=(Last-First/Last+First), pointes deleted=0 -->
                    <th>% Deletion</th><!-- points added=0, points deleted=(Last-First/Last+First) -->
                  </tr>
                </thead>
                <tbody>
                    <tr>
                      <td>Total nodes</td>
                      <td>1334</td>
                      <td>1696</td>
                      <td>12%</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Node Zmin</td>
                      <td>625</td>
                      <td>625</td>
                      <td>0</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Node Zmax</td>
                      <td>632</td>
                      <td>632</td>
                      <td>0</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Nodes Topo</td>
                      <td>840</td>
                      <td>941</td>
                      <td>5%</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Nodes Interpolated</td>
                      <td>393</td>
                      <td>393</td>
                      <td>0</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Breakline Length</td>
                      <td>5081</td>
                      <td>6203</td>
                      <td>2%</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Dams</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Crossed Breaklines</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                      <td>0</td>
                    </tr>
                    <tr>
                      <td>Facets</td>
                      <td>2395</td>
                      <td>2395</td>
                      <td>0</td>
                      <td>0</td>
                    </tr>
                </tbody>
              </table>
            </div>
              
            <!-- Image Snapshots -->
            <div class="panel panel-default"> 
                <h2>Review Images</h2>
                <div class="panel-body">
                    <img src="../src/img/johnday_sample.png"></img>
                    <p>TIN before editing</p>
                </div>
                <div class="panel-body">
                    <img  src="../src/img/johnday_sample.png"></img>
                    <p>TIN after editing</p>
                </div>
                <div class="panel-body">
                    <img  src="../src/img/johnday_sample.png"></img>
                    <p>WSETIN</p>
                </div>
                <div class="panel-body">
                    <img id="img-review" src="../src/img/johnday_sample.png"></img>
                    <p>Image from Crews</p>
                </div>
            </div>
              
          </div>
        </div>
        <xsl:call-template name="javascript" />
        <xsl:call-template name="JSON" />
      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">
      <div class="jumbotron">
              <div class="row">
                  <div class="col-md-6">
                      <h1>Survey Editing Report</h1>
                      <h3>Created on: </h3>
                      <div class="row">
                        <div class="col-md-6">
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
              <div class="col-md-6"><img class="img"></img></div> <!-- site map? -->
       </div> 
  </xsl:template>

   <xsl:template name="JSON">
     <script id="ReportJSONData" type="application/json"><xsl:value-of select="/report/json"/></script>
   </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/editing_report.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/editing_report.css?__inline=true" rel="stylesheet" />
  </xsl:template>
</xsl:stylesheet>