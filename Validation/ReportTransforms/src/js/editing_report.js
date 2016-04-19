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

var parseJSON = function(){
  var x = JSON.parse($('#ReportJSONData').html());
  return x;
}

/**
 * Only do things when the document is ready for thing-doing
 */
$(document).ready(function() {
  // Go get the JSON data from the report:
  var JSONData = parseJSON();
  console.dir(JSONData);

  var test = JSONData.surveyGDB.table.tablename[0].record.Watershed;
  console.log(test);
  
    
  // extract list of values from a survey GDB table   
  function extract(record, property) {
      if (Array.isArray(record)) {
       return _.pluck(record, property);
      } else {
        return record[property];
        }
      }

  var table = JSONData.surveyGDB.table.tablename;
    
  var records = _.find(table, function(i) {
    return i['#text'] === 'QaQcPoints\n      ';
  });

  var timestamps = extract(records.record, 'TIMESTAMP');
  var codes = extract(records.record, 'Code');
  var counts = extract(records.record, 'Count');

  console.log(timestamps);
  console.log(codes);
  console.log(counts);

  });

//  var template = ''
//    <table  id="surveyinfo" cclass="table">
//        <tbody>
//            <tr>
//                <td>Vertical error notes:</td>
//                <td><%=  %></td>
//                <td>Horizontal error notes:</td>
//                <td><%= %></td>
//            </tr>
//            <% for (var index = 0; index < employeeList.length; index++){ %>
//            <% var employee = employeeList[index]; %>
//            <% var compensation = employee.hours * employee.pay; %>
//            <tr>
//                <td><%= employee.name %></td>
//                <td><%= employee.position %></td>
//                <td><%= employee.pay %></td>
//                <td><%= employee.hours %></td>
//                <td><%= employee.type %></td>
//                <td><%= compensation %></td>
//            </tr>
//            <% } %>
//        </tbody>
//    </table>
//    ";

//    $(document).ready(function() {
//    var output = _.template(template, { employeeList : employeeList } );
//
//    $("#surveyinfo").html(output);
//    });

    
    
  // Remove zero padding on version numbers
  $('#version-filter option, thead th.version span').each(function(){
    var version = $(this).text();
    $(this).html(version.replace(/\.0/g, '.'));
  });

  $('td.version').each(function(){
    if ($(this).attr('data-status') == passfail.pass){
      $(this).addClass('pass');
    }
    else if ($(this).attr('data-status') == passfail.fail){
      $(this).addClass('fail');
    }
  });
  $('select#metric-filter').selectize().on('change', function(e){
    filterArgs.metric.value = e.target.selectize.getValue();
    filter();
  });
  $('select#visit-filter').selectize().on('change', function(e){
    filterArgs.visit.value = e.target.selectize.getValue();
    filter();
  });
  $('select#version-filter').selectize().on('change', function(e){
    filterArgs.version.value = e.target.selectize.getValue();
    filter();
  });
  $('input#onlyFailures').on('change', function(e){    
    filterArgs.onlyFailures.value =  $(this).is(':checked');
    console.log(filterArgs.onlyFailures.value);
    filter();
  });


/**
 * Decide what to show and then show (or hide) it. Duh.
 * @return {[type]} [description]
 */
var filter = function(){
  // Show all the things
  $('div.metric').removeClass('hide');
  $('table tbody tr').removeClass('hide');
  $(filterArgs.version.selector).removeClass('hide');

  // Go through and hide some of the sections
  $('div.metric').each(function(){
    var hideMetric = false;
    if (filterArgs.metric.value && filterArgs.metric.value.length > 0){
      hideMetric = true;
      var metric = $(this).attr('data');
      $.each(filterArgs.metric.value, function(n, val){
        if (val == metric){
          hideMetric = false;
        }
      })
    }

    // Go through and hide some of the rows
    // ------------------------------------------------
    var rowCount = 0;
    $(this).find('table tr').each(function(){
      var hideRow = false;
      if ($(this).parent('thead').length == 0 && 
          filterArgs.visit.value && 
          filterArgs.visit.value.length > 0){

        hideRow = true;
        var visit = $(this).find(filterArgs.visit.selector).attr('data');
        $.each(filterArgs.visit.value, function(n, val){
          if (val == visit){
            hideRow = false;
          }
        })
      }

      // Go through and hide some of the Columns
      // ------------------------------------------------
      $(this).find(filterArgs.version.selector).each(function(n,row){
        var hideCol = false;
        if (!hideRow && filterArgs.version.value && filterArgs.version.value.length > 0){
          hideCol = true;
          var version = $(this).attr('data');
          $.each(filterArgs.version.value, function(n, val){
            if (val == version){
              hideCol = false;
            }
          })
        }

        if (hideCol){
          $(this).addClass('hide');
        }
      });

      // Only show rows and columns containing failures'
      // ------------------------------------------------
      if ($(this).parent('thead').length == 0 && 
          !hideRow && filterArgs.onlyFailures.value && 
          filterArgs.onlyFailures.value == true){
        hideRow = true;
        $(this).find('td.version span.status').each(function(){
          if (!$(this).parent('td').hasClass('hide') && 
              $(this).text() == passfail.fail ){ 
            hideRow = false;
          }
        });
      }

      if (hideRow) $(this).addClass('hide');
      else rowCount++
    });

    // Now we make a decision to hide (or not) the metric
    if (hideMetric || rowCount <= 1)
      $(this).addClass('hide');

  });





}