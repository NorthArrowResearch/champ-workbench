/**
 * Only do things when the document is ready for thing-doing
 */
$(document).ready(function() {
  // Go get the JSON data from the report:
  var JSONData = parseJSON();
  console.dir(JSONData);

  SiteSizeSummary(JSONData, $('#site-size-summary'));

});

/******************************************************************************************

        TABLE FUNCTIONS BELOW THIS POINT

******************************************************************************************/

var SiteSizeSummary = function(JSONData, $table) {

    // Get the json data associate with this grouping of metrics
    var dataTable = JSONData.report.metrics.metric;
    var targetMetrics = _.where(JSONData.report.metrics.metric, {
                display_parent_group : 'Bankfull', display_child_group: 'SiteShape'});

    // Populate a list with the metric names
    var metricNames = _.pluck(targetMetrics, 'name');

    // create arrays of values for populating the table
    var visitID_key = "visit_id"; 
    //var season_key = "field_Season";
    var results_key = "results";
    visitID_list = BuildVisitIDArray(targetMetrics, visitID_key);
    //season_list = buildVisitIDArray(targetMetrics, season_key);
    results_list = BuildVisitIDArray(targetMetrics, results_key);
    console.log(visitID_list);
    console.log(results_list);

    // append table header columns
    var $thead = $('<thead />');
    var $header_row = $('<tr />');
    for (i=0; i<metricNames.length; i++) {
      $header_row.append($('<th></th>').text(metricNames[i]))
      $thead.append($header_row);
    }
    console.log($thead);

    // append tbody content
    $tbody = $('<tbody />');
    var $row = $('<tr />');
    $row.append($('<td></td>').text(visitID_list[0])); //Add visit ID column
    for (i=0; i<metricNames.length; i++) {
      $row.append($('<td class="number-column"></td>').text(results_list[i]));
      $tbody.append($row);
    }

    $thead.append($tbody);

}

// -------------------------------------- HELPER METHODS BELOW THIS POINT

var parseJSON = function(){
  var x = JSON.parse($('#ReportJSONData').html());
  return x;
}

var BuildVisitIDArray = function(targetMetrics, key) {
  var attrList = [];
  for(var i=0; i<targetMetrics.length; i++) {
    var visitRecords = targetMetrics[i].visits;
    for(visit in visitRecords) {
      attrList.push(_.pluck(visitRecords[visit], key));
      }
    }
  return attrList
} 