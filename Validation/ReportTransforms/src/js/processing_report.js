/**
 * Only do things when the document is ready for thing-doing
 */
$(document).ready(function() {

  // Go get the JSON data from the report:
  var JSONData = parseJSON();
  console.dir(JSONData);

  ChannelFeatureSummary(JSONData, $('#channel-feature-summary'));

});


/******************************************************************************************

        TABLE FUNCTIONS BELOW THIS POINT

******************************************************************************************/

/**
 * ChannelFeatureSummary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var ChannelFeatureSummary = function(JSONData, $table){
  // Get the jQuery node associated with this table
  $tbody = $table.find('tbody');
  var summaryObj = {};

  // Get the json data associate with this table
  dataTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "QaQcPolygons"});
  records = dataTable.records.record;

  var first;
  var last;

  // We have to figure out what the first and last values should be
  $.each(records, function(recordKey,record){
    // Add a convenient unix timestamp that makes sorting easier
    record.unixTime = moment(record.TIMESTAMP).unix();
    // move our first and last pointer if the record we're looking at is newer/older
    if (!first || first.unixTime >= record.unixTime){
      first = record;
    }
    if (!last || last.unixTime <= record.unixTime){
      last = record;
    }
  })


  // These are the properties we want to print to the table.
  // It's done this way so that you can specify different names than 
  // the raw fieldname.
  var fields = {
    ChannelUnitsCount:"Number of Channel Unit Polygons",
    ChannelUnitsUnique: "Number of Unique Channel Units",
    WaterExtentCount:"Number of Water Extent Polygons"

    //BankfullExtentCount: "Number of Bankfull Extent Polygons"
  };


  // Now go through and make your actual HMTL elements for the table row.
  $.each(fields, function(key, name){
    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(name));
    $row.append($('<td></td>').text(first[key] || 0));

    $tbody.append($row);
  })

  // Now we're going to do some replacing of nodes
  $('#poly-water-extent').html(first['WaterExtentCount']);
  $('#unique-channel-units').html(first['ChannelUnitsUnique']);

}

// -------------------------------------- HELPER METHODS BELOW THIS POINT

var parseJSON = function() {
  var x = JSON.parse($('#ReportJSONData').text());
  return x;
}