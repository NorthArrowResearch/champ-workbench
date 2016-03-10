//  D3 source for a whisker plot 
//  This was stolen from : http://bl.ocks.org/jensgrubert/7789216

// Dummy Data. Eventually we will read this directly from the RBT data
var data = [
  [ ["2011"], [20000,9879,5070,7343,9136,7943,10546,9385,8669,4000] ],
  [ ["2012"], [15000,9323,9395,8675,5354,6725,10899,9365,8238,7446] ],
  [ ["2013"], [8000,3294,17633,12121,4319,18712,17270,13676,6587,16754] ],
  [ ["2014"], [20000,5629,5752,7557,5125,5116,5828,6014,5995,8905] ]
]
  // using an array of arrays with
  // data[n][2] 
  // where n = number of columns in the csv file 
  // data[i][0] = name of the ith column
  // data[i][1] = array of values of ith column

var labels = true; // show the text labels beside individual boxplots?

var margin = {top: 30, right: 50, bottom: 70, left: 50};
var  width = 800 - margin.left - margin.right;
var height = 400 - margin.top - margin.bottom;
  
var min = Infinity,
    max = -Infinity;

$(document).ready(function() {

  // Figure out the max and min of the graph
  $.each(data, function(i,x) {
    $.each(x[1], function(j,v) {   
      if (v > max) max = v;
      if (v < min) min = v; 
    });
  });
  
  var chart = d3.box()
    .whiskers(iqr(1.5))
    .height(height) 
    .domain([min, max])
    .showLabels(labels);

  var svg = d3.select("#plot").append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom)
    .attr("class", "box")    
    .append("g")
    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
  
  // the x-axis
  var x = d3.scale.ordinal()     
    .domain( data.map(function(d) { console.log(d); return d[0] } ) )     
    .rangeRoundBands([0 , width], 0.7, 0.3);    

  var xAxis = d3.svg.axis()
    .scale(x)
    .orient("bottom");

  // the y-axis
  var y = d3.scale.linear()
    .domain([min, max])
    .range([height + margin.top, 0 + margin.top]);
  
  var yAxis = d3.svg.axis()
    .scale(y)
    .orient("left");

  // draw the boxplots  
  svg.selectAll(".box")    
      .data(data)
    .enter().append("g")
    .attr("transform", function(d) { return "translate(" +  x(d[0])  + "," + margin.top + ")"; } )
      .call(chart.width(x.rangeBand())); 
  
        
  // add a title
  svg.append("text")
        .attr("x", (width / 2))             
        .attr("y", 0 + (margin.top / 2))
        .attr("text-anchor", "middle")  
        .style("font-size", "18px") 
        //.style("text-decoration", "underline")  
        .text("Some Title for the plot");
 
   // draw y axis
  svg.append("g")
        .attr("class", "y axis")
        .call(yAxis)
    .append("text") // and text1
      .attr("transform", "rotate(-90)")
      .attr("y", 6)
      .attr("dy", ".71em")
      .style("text-anchor", "end")
      .style("font-size", "16px") 
      .text("y axis label");    
  
  // draw x axis  
  svg.append("g")
      .attr("class", "x axis")
      .attr("transform", "translate(0," + (height  + margin.top + 10) + ")")
      .call(xAxis)
    .append("text")             // text label for the x axis
        .attr("x", (width / 2) )
        .attr("y",  10 )
    .attr("dy", ".71em")
        .style("text-anchor", "middle")
    .style("font-size", "16px") 
        .text("x axis label"); 

// Returns a function to compute the interquartile range.
function iqr(k) {
  return function(d, i) {
    var q1 = d.quartiles[0],
        q3 = d.quartiles[2],
        iqr = (q3 - q1) * k,
        i = -1,
        j = d.length;
    while (d[++i] < q1 - iqr);
    while (d[--j] > q3 + iqr);
    return [i, j];
  };
}
});
