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
  var pntCount = 0;
  $.each(summaryObj, function(key,summRow){
    var zero = 0.0;
    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(key));
    $row.append($('<td class="number-column"></td>').text(summRow.First));
    $row.append($('<td class="number-column"></td>').text(summRow.Last));

    // Do a little math, make a little sum. Get down tonight!
    var result = ((summRow.Last - summRow.First) /  summRow.First * 100);
    if (isNaN(result)) {
      var delta = 0.0;
    } else {
      var delta = result;
    }

    var added = delta >= 0 ? delta.toFixed(1) : zero.toFixed(1);
    var deleted = delta <= 0 ? delta.toFixed(1) : zero.toFixed(1);
    var edited = delta == 0 ? "No" : "Yes";
    var PointsCollected = summRow.Last == 0 && summRow.First == 0? "No" : "Yes";

    $row.append($('<td class="number-column"></td>').text(added));
    $row.append($('<td class="number-column"></td>').text(deleted));
    $row.append($('<td></td>').text(edited));
    $row.append($('<td></td>').text(PointsCollected));

    $tbody.append($row);

    pntCount = pntCount + parseFloat(summRow.Last);
  })
  
  $('#points-total').html(pntCount);
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
        First: record.Length,
        FirstTime: record.unixTime,
        Last: record.Length,
        LastTime: record.unixTime,
      }
    }
  })


  // Now go through and make your actual HMTL elements for the table row.
  $.each(summaryObj, function(key,summRow){
    var zero = 0.0;
    var summRow_first = parseFloat(summRow.First).toFixed(2);
    var summRow_last = parseFloat(summRow.Last).toFixed(2);

    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(key));
    $row.append($('<td class="number-column"></td>').text(summRow_first));
    $row.append($('<td class="number-column"></td>').text(summRow_last));

    // Do a little math, make a little sum. Get down tonight!
    var result = (summRow.Last - summRow.First) /  summRow.First * 100;
    if (isNaN(result)) {
      var delta = 0.0;
    } else {
      var delta = result;
    }

    var added = delta >= 0 ? delta.toFixed(2) : zero.toFixed(2);
    var deleted = delta <= 0 ? delta.toFixed(2) : zero.toFixed(2);
    var edited = delta == 0 ? "No" : "Yes";
    var PointsCollected = summRow.Last == 0 && summRow.First == 0? "No" : "Yes";

    $row.append($('<td class="number-column"></td>').text(added));
    $row.append($('<td class="number-column"></td>').text(deleted));
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

  // Get the json data associated with the SurveyInfo table
  surveyInfoTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "SurveyInfo"});
  surveyRecord = surveyInfoTable.records.record;
  finalTINname = surveyRecord.FinalTIN;

  // Get the json data associated with the QaQcTIN table
  tinTable = _.findWhere(JSONData.surveyGDB.tables.table, {name: "QaQcTIN"});
  tinRecords = tinTable.records.record;
  records = _.filter(tinRecords, function(i) {return i.TIN_Name == finalTINname;});

  var first;
  var last;

  // We have to figure out what the first and last values should be
  $.each(records, function(recordKey,record){
    // Add a convenient unix timestamp that makes sorting easier
    record.unixTime = moment(record.TIMESTAMP).unix();
    // move our feirst and last pointer if the record we're looking at is newer/older
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
    BL_Count:"BL_Count",
    BL_Length:"Breakline Length",
    BL_Crossed:"Breakline Crossed",
    Node_Count:"Node Count",
    Node_Zmax:"Node Zmax",
    Node_Zmin:"Node Zmin",
    Nodes_Interpolated:"Nodes Interpolated",
    Nodes_Topo:"Nodes Topo",
    Dams:"Dams",
    Facets:"Facets"
  }; 



  // Now go through and make your actual HMTL elements for the table row.
  $.each(fields, function(key, name){
    var zero = 0.00;

    // format key values for table, if numeric
    if(first[key] == 'True' || first[key] == 'False') {
      var first_key_ready = first[key];
      var last_key_ready = last[key];
    } else {
      var first_key = parseFloat(first[key]);
      var last_key = parseFloat(last[key]);
      var first_key_rnd = first_key.toFixed(2);
      var last_key_rnd = last_key.toFixed(2);
      var first_key_ready = parseFloat(first_key_rnd);
      var last_key_ready = parseFloat(last_key_rnd);
    }

    var $row = $('<tr/>');
    // Easy rows just print values from the summary table
    $row.append($('<td></td>').text(name));
    $row.append($('<td class="number-column"></td>').text(first_key_ready || "NA"));
    $row.append($('<td class="number-column"></td>').text(last_key_ready || "NA"));

    // Do a little math, make a little sum. Get down tonight!
    delta = (last[key] - first[key]) /  first[key] * 100;
    added = delta >= 0 ? delta.toFixed(2) : zero.toFixed(2);
    deleted = delta <= 0 ? delta.toFixed(2)  : zero.toFixed(2);

    if (!first.calcs)
      first.calcs = {}
    
    first.calcs[key] = {
      delta: delta,
      added: added,
      deleted: deleted
    }

    $row.append($('<td class="number-column"></td>').text(first.calcs[key].added));
    $row.append($('<td class="number-column"></td>').text(first.calcs[key].deleted));

    $tbody.append($row);
  })



  // Now we're going to do some replacing of nodes
  $('#final-tin').html(finalTINname);
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
  $('#nodes-Zmax').html((Math.abs(last['Node_Zmax'] - first['Node_Zmax'])).toFixed(2));
  $('#nodes-Zmin').html((Math.abs(last['Node_Zmin'] - first['Node_Zmin'])).toFixed(2));
  $('#bl-length').html(last['BL_Length'] + ' meters');
  if (last['BL_Crossed'] == 0) {
    $('#bl-cross').html('0');
  } else {
    $('#bl-cross').html(last['BL_Crossed']);
  }
  if (last['BL_Count'] == 0) {
    $('#bl-count').html('0');
  } else {
    $('#bl-count').html(last['BL_Count']);
  }
  if (last['Dams'] == 'False') {
    $('#dams-remove').html('were not');
  } else {
    $('#dams-remove').html('were');
  }
  if (last['Facets'] == 'False') {
    $('#facets-remove').html('were not');
  } else {
    $('#facets-remove').html('were');
  }

}


// -------------------------------------- HELPER METHODS BELOW THIS POINT

var parseJSON = function(){
  var x = JSON.parse($('#ReportJSONData').text());
  return x;
}