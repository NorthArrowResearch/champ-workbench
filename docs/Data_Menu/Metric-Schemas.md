---
title: Metric Schemas
---

Metrics can be viewed in different groupings, called schemas. 

## Metric Schema Creation

Schema names are generated directly in the SQL Workbench database and then metrics can be added to schemas using the Workbench interface.  

To generate a new schema, open the Metric_Schemas table of the Workbench database. Schemas should have an informative Title and ProgramID. All metrics within a schema must come from a single DatabaseTable.  

![Metric Schema table]({{ site.url }}/assets/images/MetricSchema_WBdb.png)



## Adding/removing metrics from Schemas

Metrics can be added and removed from schemas from Metric Definitions (Data menu)

Navigate to the Data Menu, Select Metric Definitions, and double click on an dividual metric to open the metric properties.  Click on the Schema tab and use the checkboxes to manage which schema should be visible.  Note that a metric will only be visible in the schema if it is stored in the metric table designated in the Metric_Schemas table of the Workbench db.

![Add metrics to schema window]({{ site.url }}/assets/images/MetricDefinition_schemas.png)
