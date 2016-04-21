/**
 * Global registry of filters and their query strings.
 * @type {Object}
 */
var filterArgs = {
  metric:  { selector: '', value: null },
  visit:   { selector: 'td.visitId', value: null },
  version: { selector: 'td.version, th.version', value: null },
  onlyFailures:  { selector: 'td.status', value: false },
}

// Constants for pass and fail
var passfail = {
  pass: 'Pass', 
  fail: 'Fail'
}

/**
 * Only do things when the document is ready for thing-doing
 */
$(document).ready(function() {
  // Go get the JSON data from the report:
  var JSONData = parseJSON();
  console.dir(JSONData);

  PointEditingSummary(JSONData, $('#point-editing-summary'));
  LineEditingSummary(JSONData, $('#line-editing-summary'));
  NodeEditingSummary(JSONData, $('#node-editing-summary'));

});

/******************************************************************************************

        TABLE FUNCTIONS BELOW THIS POINT

******************************************************************************************/


/**
 * PointEditingSummary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var PointEditingSummary = function(JSONData, $table){
  // Get the jQuery node associated with this table
  $tbody = $table.find('tbody');
  var summaryObj = {}

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

  // Now go through and make your actual HMTL elements for the table row.
  $.each(summaryObj, function(key,summRow){
    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(key));
    $row.append($('<td></td>').text(summRow.First));
    $row.append($('<td></td>').text(summRow.Last));

    // Do a little math, make a little sum. Get down tonight!
    var delta = (summRow.Last - summRow.First) /  summRow.First * 100;
    var added = delta >= 0 ? delta.toFixed(1) : 0;
    var deleted = delta <= 0 ? -delta.toFixed(1)  : 0;
    var edited = delta == 0 ? "No" : "Yes";
    var PointsCollected = summRow.Last == 0 && summRow.First == 0? "No" : "Yes";

    $row.append($('<td></td>').text(added));
    $row.append($('<td></td>').text(deleted));
    $row.append($('<td></td>').text(edited));
    $row.append($('<td></td>').text(PointsCollected));

    $tbody.append($row);
  })

}


/**
 * LineEditingSummary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var LineEditingSummary = function(JSONData, $table){
  // Get the jQuery node associated with this table
  $tbody = $table.find('tbody');
  var summaryObj = {}

  // Get the json data associate with this table
  dataTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "QaQcLines"});
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
        summaryObj[record.Code].First = record.Length;
      }
      if (summaryObj[record.Code].LastTime <= record.UnixTime){
        summaryObj[record.Code].LastTime = record.UnixTime
        summaryObj[record.Code].Last = record.Length;
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

  // Now go through and make your actual HMTL elements for the table row.
  $.each(summaryObj, function(key,summRow){
    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(key));
    $row.append($('<td></td>').text(summRow.First));
    $row.append($('<td></td>').text(summRow.Last));

    // Do a little math, make a little sum. Get down tonight!
    var delta = (summRow.Last - summRow.First) /  summRow.First * 100;
    var added = delta >= 0 ? delta.toFixed(1) : 0;
    var deleted = delta <= 0 ? -delta.toFixed(1)  : 0;
    var edited = delta == 0 ? "No" : "Yes";
    var PointsCollected = summRow.Last == 0 && summRow.First == 0? "No" : "Yes";

    $row.append($('<td></td>').text(added));
    $row.append($('<td></td>').text(deleted));
    $row.append($('<td></td>').text(edited));
    $row.append($('<td></td>').text(PointsCollected));

    $tbody.append($row);
  })

}


/**
 * NodeEditingSummary Table 
 * @param {[type]} JSONData [description]
 * @param {[type]} $table   [description]
 */
var NodeEditingSummary = function(JSONData, $table){
  // Get the jQuery node associated with this table
  $tbody = $table.find('tbody');
  var summaryObj = {};

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
    Nodes_Topo:"Nodes Topo"
  }; 

// var delta;
// var added;
// var deleted;

  // Now go through and make your actual HMTL elements for the table row.
  $.each(fields, function(key, name){
    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(name));
    $row.append($('<td></td>').text(first[key] || 0));
    $row.append($('<td></td>').text(last[key] || 0));

    // Do a little math, make a little sum. Get down tonight!
    delta = (last[key] - first[key]) /  first[key] * 100;
    added = delta >= 0 ? delta.toFixed(1) : 0;
    deleted = delta <= 0 ? -delta.toFixed(1)  : 0;

    if (!first.calcs)
      first.calcs = {}
    
    first.calcs[key] = {
      delta: delta,
      added: added,
      deleted: deleted
    }

    $row.append($('<td></td>').text(first.calcs[key].added));
    $row.append($('<td></td>').text(first.calcs[key].deleted));

    $tbody.append($row);
  })

  // Now we're going to do some replacing of nodes
  $('#tin-last-timestamp').html(moment(first.TIMESTAMP).format('MMMM Do YYYY, h:mm:ss a'));
  $('#tin-minutes').html(moment(last.TIMESTAMP).diff(first.TIMESTAMP, 'minutes') + " minutes");
  if (delta == 0) {
    $('#nodes-delta').html('0%');
  } else {
    $('#nodes-delta').html(first.calcs['Node_Count'].delta.toFixed(1) + '%');
  }
  if (added == 0) {
    $('#nodes-added').html('0');
    $('#nodes-deleted').html(Math.abs(last['Node_Count'] - first['Node_Count']));
  } else {
    $('#nodes-added').html(Math.abs(last['Node_Count'] - first['Node_Count']));
    $('#nodes-deleted').html('0');
  }
  $('#nodes-topo').html(last['Nodes_Topo']);
  $('#nodes-not-topo').html(last['Node_Count'] - last['Nodes_Topo']);
  $('#nodes-Zmax').html(Math.abs(last['Node_Zmax'] - first['Node_Zmax']));
  $('#nodes-Zmin').html(Math.abs(last['Node_Zmin'] - first['Node_Zmin']));
  $('#BL-length').html(last['BL_Length'] + ' meters');
  if (last['BL_Crossed'] == 0) {
    $('#BL-cross').html('0');
  } else {
    $('#BL-cross').html(last['BL_Crossed']);
  }
  
}


// -------------------------------------- HELPER METHODS BELOW THIS POINT

var parseJSON = function(){
  var x = JSON.parse($('#ReportJSONData').text());
  return x;
}