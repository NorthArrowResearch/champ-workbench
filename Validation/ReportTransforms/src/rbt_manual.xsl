<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:key name="metric" match="/report/metrics/metric/name/text()" use="." />
  <xsl:key name="visit" match="/report/metrics/metric/visits/visit/visit_id/text()" use="." />
  <xsl:key name="version" match="/report/metrics/metric/visits/visit/results/result/version/text()" use="." />

  <xsl:template match="report">
    <html class="no-js" lang="en">
      <head> 
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>RBT Validation Report</title>
        <xsl:call-template name="stylesheet" />
      </head>
      <body>
        <div class="container">
          <xsl:call-template name="header" />
          <xsl:for-each select="metrics/metric">
            <xsl:variable name="tolerance" select="tolerance" />
            <div class="metric">
              <xsl:attribute name="data"><xsl:value-of select="name"/></xsl:attribute>
              <h2><xsl:value-of select="name"/> &#160;
                  <xsl:choose>
                    <xsl:when test="normalize-space(unit)"><span>(<xsl:value-of select="unit"/>) &#160;</span></xsl:when>
                    <xsl:otherwise></xsl:otherwise>
                  </xsl:choose>
                <span class="tolerance">( ±<xsl:value-of select="tolerance * 100"/>% )</span>
              </h2>
              <table class="table">
                <thead>
                  <tr>
                    <th>VID</th>
                    <th>Visit</th>
                    <th class="manual">
                      <span class="">Manual</span>
                    </th>
                    <xsl:for-each select="/report/metrics/metric/visits/visit/results/result/version/text()[generate-id() = generate-id(key('version',.)[1])]">
                      <th class="version">
                        <xsl:attribute name="data"><xsl:value-of select="."/></xsl:attribute>
                        <span class=""><xsl:value-of select="."/></span>
                      </th>
                    </xsl:for-each>
                  </tr>
                </thead>
                <tbody>
                  <xsl:for-each select="visits/visit">
                  <xsl:variable name="results" select="results" />
                  <tr>
                    <td class="visitId">
                      <xsl:attribute name="data"><xsl:value-of select="visit_id"/></xsl:attribute>
                      <xsl:value-of select="visit_id"/>
                    </td>
                    <td class="visitName">
                      VISIT_<xsl:value-of select="visit_id"/>, 
                      <xsl:value-of select="watershed_name"/>, 
                      <xsl:value-of select="field_season"/>
                    </td>
                    <td class="manualResult"><xsl:value-of select="manual_result"/></td>
                    <xsl:for-each select="/report/metrics/metric/visits/visit/results/result/version/text()[generate-id() = generate-id(key('version',.)[1])]">
                      <xsl:variable name="version" select="." />
                      <td class="version">
                        <xsl:attribute name="data-status">
                          <xsl:value-of select="$results/result[version=$version]/status"/>
                        </xsl:attribute>
                        <xsl:attribute name="data">
                          <xsl:value-of select="."/>
                        </xsl:attribute>
                        <xsl:if test="$results/result/version/text()=$version">
                          <span class="value">
                            <xsl:value-of select="$results/result[version=$version]/value"/>
                          </span>
                          <span class="tolerance">
                            ±<xsl:value-of select="$tolerance * 100"/>%
                          </span>
                          <span class="status">
                            <xsl:value-of select="$results/result[version=$version]/status"/>
                          </span>
                        </xsl:if>
                      </td>
                    </xsl:for-each>
                  </tr>
                  </xsl:for-each>
                </tbody>
              </table>
            </div>
          </xsl:for-each>
        </div>
        <xsl:call-template name="javascript" />
        <xsl:call-template name="JSON" />
      </body>
      
    </html>
  </xsl:template>


  <xsl:template name="header">
    <div class="jumbotron">
      <h1 class="display-3">RBT Validation Report</h1>
      <p class="lead">Date: <xsl:value-of select="date"/></p>
      <div class="row">
        <div class="col-sm-3">
          <select multiple="true" id="metric-filter" name="Metric" placeholder="Metric">
            <xsl:for-each select="/report/metrics/metric/name/text()[generate-id() = generate-id(key('metric',.)[1])]">
              <option>
                <xsl:attribute name="value">
                  <xsl:value-of select="."/>
                </xsl:attribute>
                <xsl:value-of select="."/>
              </option>              
            </xsl:for-each>
          </select>
        </div>
        <div class="col-sm-3">
          <select multiple="true" id="visit-filter" name="Visit" placeholder="Visit">
            <xsl:for-each select="/report/metrics/metric/visits/visit/visit_id/text()[generate-id() = generate-id(key('visit',.)[1])]">
              <option>
                <xsl:attribute name="value">
                  <xsl:value-of select="."/>
                </xsl:attribute>
                <xsl:value-of select="."/>
              </option>              
            </xsl:for-each>
          </select>
        </div>
        <div class="col-sm-3">
          <select multiple="true" id="version-filter" name="Version" placeholder="Version">
            <xsl:for-each select="/report/metrics/metric/visits/visit/results/result/version/text()[generate-id() = generate-id(key('version',.)[1])]">
              <option>
                <xsl:attribute name="value">
                  <xsl:value-of select="."/>
                </xsl:attribute>
                <xsl:value-of select="."/>
              </option>              
            </xsl:for-each>
          </select>
        </div>
        <div class="col-sm-3">
          <label>
            <input id="onlyFailures" type="checkbox" name="failures" /> Only Failures
          </label>
        </div>
      </div>
    </div>
  </xsl:template>

  <xsl:template name="JSON">
    <script id="ReportJSONData" type="application/json"><xsl:value-of select="/report/json"/></script>
  </xsl:template>

  <xsl:template name="javascript">
    <script src="tmp/rbt_manual.js?__inline=true"></script>
  </xsl:template>

  <xsl:template name="stylesheet">
    <link href="tmp/rbt_manual.css?__inline=true" rel="stylesheet" />
  </xsl:template>

</xsl:stylesheet>

