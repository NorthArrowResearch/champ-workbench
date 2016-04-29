/**
 * Only do things when the document is ready for thing-doing
 */
$(document).ready(function() {

  // Go get the JSON data from the report:
  var JSONData = parseJSON();
  console.dir(JSONData);

  ChannelFeatureSummary(JSONData, $('#channel-feature-summary'));
  PointEditingSummary(JSONData, $('#point-editing-summary'));
  NodeEditingSummary(JSONData, $('#node-editing-summary'));
  BacksightLogSummary(JSONData, $('#backsight-log-summary'))
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
    ChannelUnitsCount:"# of Channel Unit Polygons",
    ChannelUnitsUnique: "# of Unique Channel Units",
    WaterExtentCount:"# of Water Extent Polygons"
    //BankfullExtentCount: "Number of Bankfull Extent Polygons"
  };


  // Now go through and make your actual HMTL elements for the table row.
  $.each(fields, function(key, name){
    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(name));
    $row.append($('<td></td>').text(records[key] || 0));

    $tbody.append($row);
  })

  // Now we're going to do some replacing of nodes
  $('#poly-water-extent').html(records['WaterExtentCount']);
  $('#unique-channel-units').html(records['ChannelUnitsUnique']);

}


/**
 * PointEditingSummary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var PointEditingSummary = function(JSONData, $table){
  var summaryObj = {};

  // Get the json data associate with this table
  dataTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "QaQcPoints"});
  records = dataTable.records.record;

  // We have to collect the records by Code.
  $.each(records, function(key,record){
    record.unixTime = moment(record.TIMESTAMP).unix();
    // This Code exists in our summary object.
    if (summaryObj[record.Code]){
      // If we're looking at something newer than the newest or older than the oldest
      // then reset those points.
      if (summaryObj[record.Code].FirstTime >= record.UnixTime){
        summaryObj[record.Code].FirstTime = record.UnixTime;
        summaryObj[record.Code].First = record.Count;
      }
      if (summaryObj[record.Code].LastTime <= record.UnixTime){
        summaryObj[record.Code].LastTime = record.UnixTime
        summaryObj[record.Code].Last = record.Count;
      }
    }
    // We don't have a record for this code yet. Create one. 
    else{
      summaryObj[record.Code] = {
        First: record.Count,
        FirstTime: record.unixTime,
        Last: record.Count,
        LastTime: record.unixTime,
      }
    }
  })
  
  // Find codes and counts for the span IDs
  var dict_code = {}; //build a dictionary

  $.each(summaryObj, function(in_key,summRow){
    if (in_key == 'bm###') {
      dict_code['bm'] = summRow.Last;
    }
    if (in_key == 'br'){
      dict_code['br'] = summRow.Last;
    }
    if (in_key == 'cp###') {
      dict_code['cp'] = summRow.Last;
    }
    if (in_key == 'ws') {
      dict_code['ws'] = summRow.Last;
    }
    
  })

  $('#bar-points').html(dict_code['br']);
  $('#wse-points').html(dict_code['ws']);
  $('#bm-points').html(dict_code['bm']);
  $('#cp-points').html(dict_code['cp']);

}

/**
 * NodeEditingSummary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var NodeEditingSummary = function(JSONData, $table){

  // Get the json data associate with this table
  dataTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "QaQcTIN"});
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

  // These are the properties we want to print to the table
  // it's done this way so that you can specify different names than the
  // raw fieldname
  var fields = {
    BL_Crossed:"Breakline Crossed",
    BL_Length:"Breakline Length",
    Node_Count:"Node Count",
    Node_Zmax:"Node Zmax",
    Node_Zmin:"Node Zmin",
    Nodes_Interpolated:"Nodes Interpolated",
    Nodes_Topo:"Nodes Topo",
    Dams:"Dams",
    Facets:"Facets"
  }; 

  // Now we're going to do some replacing of nodes
  if (last['Dams'] == 'False') {
    $('#dams-remove').html('was not');
  } else {
    $('#dams-remove').html('was');
  }
  if (last['Facets'] == 'False') {
    $('#facets-remove').html('was not');
  } else {
    $('#facets-remove').html('was');
  }
}

/**
 * BackSightLogSummary Summary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var BacksightLogSummary = function(JSONData, $table){
  var summaryObj = {};

  // Get the json data associate with this table
  dataTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "QaQcBacksightLog"});
  records = dataTable.records.record;

  // Find unique occupied point records
  var backsight_pnts = [];

  $.each(records, function(key, record) {
      if ($.inArray(record.OC , backsight_pnts) === -1) {
          backsight_pnts.push(record.OC);
      }
  });

  $('#occ-points').html(backsight_pnts.length);

}


// -------------------------------------- HELPER METHODS BELOW THIS POINT

var parseJSON = function() {
  var x = JSON.parse($('#ReportJSONData').text());
  return x;
}