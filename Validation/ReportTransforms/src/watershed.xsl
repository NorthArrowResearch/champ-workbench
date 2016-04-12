<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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
              <div class="metric">  
                  <h3>Bankful Metrics</h3>        
                  <table class="table">
                    <thead>
                      <tr>
                        <th>Visit ID</th>
                        <th>Field Season</th>
                        <xsl:for-each select="/report/metrics/metric[display_parent_group='Bankfull' and display_child_group='SiteSize']">
                            <th><xsl:value-of select="name" /></th>
                        </xsl:for-each>
                      </tr>
                    </thead>
                    <tbody>
                        <xsl:for-each select="/report/metrics/metric[display_parent_group='Bankfull' and display_child_group='SiteSize']/visits/visit">
                        <!-- <xsl:variable name="results" select="results" /> -->
                        <tr>
                            <td class="visitId"><xsl:value-of select="visit_id" /></td>
                            <td><xsl:value-of select="field_season" /></td>
                            <td><xsl:value-of select="results/result/value" /></td>
                        </tr>
                        </xsl:for-each>
                    </tbody>
                  </table>
              </div>
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <th>Field Season</th>
                            <xsl:for-each select="/report/metrics/metric[display_parent_group='Wetted' and display_child_group='SiteSize']">
                                <th><xsl:value-of select="name" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead> 
                        <tbody>
                            <xsl:for-each select="/report/metrics/metric[display_parent_group='Wetted' and display_child_group='SiteSize']/visits/visit">
                                <tr>
                                    <td class="visitId"><xsl:value-of select="visit_id" /></td>
                                    <td class="visitId"><xsl:value-of select="field_season" /></td>
                                    <xsl:for-each select="results/result">
                                        <td><xsl:value-of select="value" /></td>
                                    </xsl:for-each>
                                </tr>
                            </xsl:for-each>
                        </tbody>
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
              <div class="metric">  
                  <h3>Bankful Metrics</h3>        
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                                <xsl:for-each select="/report/metrics/metric[display_parent_group='Bankfull' and display_child_group='SiteShape']">
                                <th><xsl:value-of select="name" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <xsl:for-each select="/report/metrics/metric/visits/visit">
                            <xsl:variable name="results" select="results" />
                            <tr>
                                <td class="visitId">
                                    <xsl:value-of select="visit_id" />
                                </td>
                                <xsl:for-each select="/report/metrics/metric[display_parent_group='Bankfull' and display_child_group='SiteShape']">
                                    <td>
                                        <xsl:value-of select="results/result/value" />
                                    </td>
                                </xsl:for-each>    
                            </tr>
                            </xsl:for-each>
                        </tbody>
                      </table>
              </div>
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                                <xsl:for-each select="/report/metrics/metric[display_parent_group='Wetted' and display_child_group='SiteShape']">
                                <th><xsl:value-of select="name" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <xsl:for-each select="/report/metrics/metric/visits/visit">
                            <xsl:variable name="results" select="results" />
                            <tr>
                                <td class="visitId">
                                    <xsl:value-of select="visit_id" />
                                </td>
                                <xsl:for-each select="/report/metrics/metric[display_parent_group='Wetted' and display_child_group='SiteShape']">
                                    <td>
                                        <xsl:value-of select="results/result/value" />
                                    </td>
                                </xsl:for-each>    
                            </tr>
                            </xsl:for-each>
                        </tbody>
                      </table>
                  </div>
               </div>
            </div>
        </div>
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
                            <h2>John Day</h2><!-- XSL: watershed name -->
                        </div>
                        <div class="col-md-4">
                            <ul class="list-unstyled">
                                <li>Current RBT Version: </li>
                                <li>Report Generated: </li>
                            </ul>
                        </div>
                      </div>
                  </div>
                  <div class="col-md-6"><img class="img"></img> <!-- watershed map -->
                  </div>
              </div> 
           </div>
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/watershed.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/watershed.css?__inline=true" rel="stylesheet" />
  </xsl:template>
</xsl:stylesheet>