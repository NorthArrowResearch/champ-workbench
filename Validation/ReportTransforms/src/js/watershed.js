
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


  var table_metric = JSONData.report.metrics.metric;
  var target_metrics = _.where(table_metric, {
                      display_parent_group : 'Bankfull', display_child_group: 'SiteShape'
                      });
  var target_metric_names = _.pluck(target_metrics, "name");


  function buildVisitIDArray(target_metrics, key) {
    var attrList = [];
    for(var i=0; i<target_metrics.length; i++) {
      var visit_records = target_metrics[i].visits;
      for(v in visit_records) {
        attrList.push(_.pluck(visit_records[v], key));
      } 
    }
    return attrList
  }


   var visitID_key = "visit_id"; 
   var season_key = "field_Season";
   visitID_list = buildVisitIDArray(target_metrics, visitID_key);
   season_list = buildVisitIDArray(target_metrics, season_key);

   console.log(visitID_list);
   console.log(season_list);

   function zip() {
        var args = [].slice.call(arguments);
        var shortest = args.length==0 ? [] : args.reduce(function(a,b){
            return a.length<b.length ? a : b
        });
        return shortest.map(function(_,i){
            return args.map(function(array){return array[i]})
        });
    }

   visitID_list_test = visitID_list[0];
   season_list_test = season_list[0];
   zipped_array = zip(visitID_list_test, season_list_test);
   console.log(visitID_list_test);
   console.log(season_list_test);
   console.log(zipped_array);

 var template = ''
   <table  id="surveyinfo" cclass="table">
       <tbody>
           <tr>
               <td>Vertical error notes:</td>
               <td><%=  %></td>
               <td>Horizontal error notes:</td>
               <td><%= %></td>
           </tr>
           <% for (var index = 0; index < employeeList.length; index++){ %>
           <% var employee = employeeList[index]; %>
           <% var compensation = employee.hours * employee.pay; %>
           <tr>
               <td><%= employee.name %></td>
               <td><%= employee.position %></td>
               <td><%= employee.pay %></td>
               <td><%= employee.hours %></td>
               <td><%= employee.type %></td>
               <td><%= compensation %></td>
           </tr>
           <% } %>
       </tbody>
   </table>
   ";"

   var output = _.template(template, { employeeList : employeeList } );

   $("#surveyinfo").html(output);

});