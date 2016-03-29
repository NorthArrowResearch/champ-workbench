<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">


<html class="no-js" lang="en">
  <head> 
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>RBT Validation Report</title>
    <xsl:call-template name="stylesheet" />
  </head>
  <body>
    <div class="container-fluid">
        <div class="jumbotron">
          <div class="row">
              <div class="col-md-6">
                  <h1>Watershed Validation Report</h1>
                  <small class="text-muted">Topographic Survey Metrics</small>
                  <div class="row">
                    <div class="col-md-3">
                        <h2>Methow</h2> <!-- XSL: watershed name -->
                    </div>
                    <div class="col-md-3">
                        <ul>
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
        <div class="metric" data-toggle="collapse" href="#{generate-id(metric-name)}">
          <h3>Site Size</h3> <!-- XSL: watershed name -->
          <div id="{generated-id(metric-name)}" class="panel-collapse collapse">    
              <table class="table">
                <thead>
                  <tr>
                    <th></th>
                    <th></th>
                    <th></th>
                    <!-- use xsl to find all metrics for other column heads -->
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <!-- all the other metrics, etc. -->
                    </tr>
                </tbody>
              </table>
          </div>
        </div>
    </div>
  </body>
    
  <xsl:template name="javascript">
    <script src="tmp/watershed.js?__inline=true"></script>
  </xsl:template>
    
  <xsl:template name="stylesheet">
    <link href="tmp/watershed.css?__inline=true" rel="stylesheet" />
  </xsl:template>