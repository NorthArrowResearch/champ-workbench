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
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table light">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <xsl:for-each select="watershed-report[MetricID='31']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='34']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='35']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='157']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='155']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='40']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='30']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='68']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='28']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3373']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <tr>
                              <xsl:for-each select="watershed-report[1]">
                                <td><xsl:value-of select="VisitID"/></td>
                              </xsl:for-each>  
                              <xsl:for-each select="watershed-report[MetricID='31']">   
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='34']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='35']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='157']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='155']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='40']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='30']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                                <xsl:for-each select="watershed-report[MetricID='68']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                                <xsl:for-each select="watershed-report[MetricID='28']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                                <xsl:for-each select="watershed-report[MetricID='3373']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                            </tr>
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
                  <div>    
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <xsl:for-each select="watershed-report[MetricID='151']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='149']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='145']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='143']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='78']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='90']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='139']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='32']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                          </tr>
</thead>
                        <tbody>
                            <tr>
                              <xsl:for-each select="watershed-report[1]">
                                <td><xsl:value-of select="VisitID"/></td>
                              </xsl:for-each>  
                              <xsl:for-each select="watershed-report[MetricID='151']">   
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='149']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='145']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='143']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='78']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='90']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='139']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='32']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                            </tr>
</tbody>
                      </table>
                  </div>
              </div>
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table light">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <xsl:for-each select="watershed-report[MetricID='169']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='167']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='163']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='161']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='71']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='79']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='157']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='30']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <tr>
                              <xsl:for-each select="watershed-report[1]">
                                <td><xsl:value-of select="VisitID"/></td>
                              </xsl:for-each>  
                              <xsl:for-each select="watershed-report[MetricID='169']">   
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='167']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='163']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='161']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='71']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='79']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='157']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                                <xsl:for-each select="watershed-report[MetricID='30']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                            </tr>
                        </tbody>
</table>
                  </div>
              </div>
          </div>
          <!-- Side Channels metrics -->   
          <div class="panel panel-default">   
              <div class="panel-heading"><div class="panel-title"><h2>Side Channels</h2></div></div>
              <div class="panel-body">
                <p>Summarize and explain what the 'Side Channels' means, and the types of metrics included in the summary report.</p>
              </div>
              <div class="metric">  
                  <h3>Bankful Metrics</h3>      
                  <div>    
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <xsl:for-each select="watershed-report[MetricID='89']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='139']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3271']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3269']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3283']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3281']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3277']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3275']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='32']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <tr>
                              <xsl:for-each select="watershed-report[1]">
                                <td><xsl:value-of select="VisitID"/></td>
                              </xsl:for-each>  
                              <xsl:for-each select="watershed-report[MetricID='89']">   
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='139']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3271']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3269']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3283']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3281']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3277']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3275']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='32']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                            </tr>
                          </tbody>
                      </table>
                  </div>
              </div>
              <div class="metric">
                  <h3>Wetted Metrics</h3>      
                  <div>    
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <xsl:for-each select="watershed-report[MetricID='79']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='157']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3289']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3287']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3301']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3299']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3295']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3293']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='30']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='6679']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3374']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3375']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <tr>
                              <xsl:for-each select="watershed-report[1]">
                                <td><xsl:value-of select="VisitID"/></td>
                              </xsl:for-each>  
                              <xsl:for-each select="watershed-report[MetricID='79']">   
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='157']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3289']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3287']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3301']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3299']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3295']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3293']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='30']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='6679']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3374']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3375']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                            </tr>
                        </tbody>
                      </table>
                  </div>
              </div>
          </div>
          <!-- Channel Features metrics -->
          <div class="panel panel-default">   
              <div class="panel-heading"><div class="panel-title"><h2>Channel Features</h2></div></div>
              <div class="panel-body">
                <p>Summarize and explain what 'Channel Features' means, and the types of metrics included.</p>
              </div>
              <div class="metric">  
                  <h3>Channel Feature Metrics</h3>      
                  <div>    
                      <table class="table">
                        <thead>
                          <tr>
                            <th>Visit ID</th>
                            <xsl:for-each select="watershed-report[MetricID='19']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='20']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3359']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='23']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='22']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='14']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='15']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3358']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='18']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='17']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='9']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='10']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='3357']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                            <xsl:for-each select="watershed-report[MetricID='13']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='12']">
                              <th><xsl:value-of select="Title" /></th>
                            </xsl:for-each>
                          </tr>
                        </thead>
                        <tbody>
                            <tr>
                              <xsl:for-each select="watershed-report[1]">
                                <td><xsl:value-of select="VisitID"/></td>
                              </xsl:for-each>  
                              <xsl:for-each select="watershed-report[MetricID='19']">   
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='20']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3359']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='23']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='22']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='14']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='15']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3358']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='18']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='17']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='9']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='10']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='3357']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='13']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                              <xsl:for-each select="watershed-report[MetricID='12']">
                                <td><xsl:value-of select="AvgOfMetricValue"/></td>
                              </xsl:for-each>
                            </tr>
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