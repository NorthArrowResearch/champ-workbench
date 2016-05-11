<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:key name="metric" match="/report/metrics/metric/name/text()" use="." />
  <xsl:key name="visit" match="/report/metrics/metric/visits/visit" use="visit_id/text()" />
  <xsl:key name="result" match="/report/metrics/metric/visits/visit/results/result" use="concat(../../visit_id, '|', ../../../../display_parent_group, '|', ../../../../display_child_group)" />
  <xsl:key name="watershed" match="/report/metrics/metric/visits/visit/watershed_name/text()" use="." />
  <xsl:key name="season" match="/report/metrics/metric/visits/visit/field_season/text()" use="." />

  <xsl:template match="report">
    <html class="no-js" lang="en">
      <head> 
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Watershed Summary Report</title>
        <xsl:call-template name="stylesheet" />
      </head>
      <body>
        <div class="container">
          <xsl:call-template name="header" /><!-- jumbotron -->
            
          <!-- Site Size metrics -->   
          <div class="panel panel-default"> 
              <div class="panel-heading"><div class="panel-title"><h2>Site Size</h2></div></div>
              <div class="panel-body">
                <p>Summarize and explain what 'Site Size' means, and the types of metrics included.</p>
              </div>
              <!-- Bankfull metrics -->
              <div class="metric">  
                  <h3>Bankfull Metrics</h3>
                  <div>
                      <table id="site-size-summary" class="table">
                      </table>
                  </div>      
              </div>
              <!-- Wetted metrics -->
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table">
                      </table>
                  </div>
               </div>
          </div>
            
          <!-- Site Shape metrics -->
          <div class="panel panel-default"> 
              <div class="panel-heading"><div class="panel-title"><h2>Site Shape</h2></div></div>
              <div class="panel-body">
                <p>Summarize and explain what 'Site Shape' means, and the types of metrics included.</p>
              </div>
              <!-- Bankfull Metrics -->
              <div class="metric">  
                  <h3>Bankfull Metrics</h3>        
                      <table class="table">
                      </table>
              </div>
              <!-- Wetted Metrics -->
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table">
                      </table>
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
              <h1>Watershed Summary Report</h1>
              <h2 class="text-muted">Topographic Survey Metrics</h2>
              <div class="row">
                <div class="col-md-4">
                  <h2><xsl:value-of select="/report/metrics/metric[1]/visits/visit[1]/watershed_name"/></h2>
                </div>
                <div class="col-md-4">
                  <ul class="list-unstyled">
                    <li>Current RBT Version: <xsl:value-of select="/report/metrics/metric[1]/visits/visit[1]/results/result/version"/></li>
                    <li>Report Generated: </li>
                  </ul>
                </div>
              </div>
              <div class="row">
                <div class="col-sm-4">
                  <select multiple="true" id="watershed-filter" name="Watershed" placeholder="Watershed">
                    <xsl:for-each select="/report/metrics/metric/visits/visit/watershed_name/text()[generate-id() = generate-id(key('watershed',.)[1])]">
                      <option>
                        <xsl:attribute name="value">
                          <xsl:value-of select="."/>
                        </xsl:attribute>
                        <xsl:value-of select="."/>
                      </option>              
                    </xsl:for-each>
                  </select>
                </div>
                <div class="col-sm-4">
                  <select multiple="true" id="season-filter" name="Season" placeholder="Season">
                    <xsl:for-each select="/report/metrics/metric/visits/visit/field_season/text()[generate-id() = generate-id(key('season',.)[1])]">
                      <option>
                        <xsl:attribute name="value">
                          <xsl:value-of select="."/>
                        </xsl:attribute>
                        <xsl:value-of select="."/>
                      </option>              
                    </xsl:for-each>
                  </select>
                </div>
              </div>
            </div>
            <div class="col-md-6"><img class="img" src="../src/img/johnday_sample.png"></img> <!-- watershed map -->
            </div>
        </div>
    </div>
  </xsl:template>

  <xsl:template name="JSON">
    <script id="ReportJSONData" type="application/json"><xsl:value-of select="/report/json"/></script>
  </xsl:template>    

  <xsl:template name="javascript">
    <script src="tmp/watershed.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/watershed.css?__inline=true" rel="stylesheet" />
  </xsl:template>
</xsl:stylesheet>