---
title: Metric Definitions
---

# Metric Definitions

Select `Metric Definitions` ![Data storage]({{ site.url }}/assets/images/MetricDefinition.png) from the Data menu for detailed information about metrics. Metrics can be filtered using the Model, Schema, and search box to the left of the window that appears.

![MetricDef_Window]({{ site.url }}/assets/images/MetricDef_Window.png)

Double click on an individual metric to open additional details. Changes to metric details are **only stored locally** and never synchronized with CHaMP Monitoring. Changes may be lost or create conflicts when updates to the workbench database occur so it's important to backup your Workbench database.

There are 4 tabs to navigate metric metadata:

**Data storage:** Provides property details of individual metrics:

![Data storage]({{ site.url }}/assets/images/MetricDef_DataStorage.png)

**Schemas:** Allows assignment of metrics to schemas:

![Schema List]({{ site.url }}/assets/images/MetricDef_Schemas.png)

**Metadata:**  Provides links to metric definitions:

![Metadata]({{ site.url }}/assets/images/MetricDef_Metadata.png)

**Validation:**  Allows update of thresholds used for validation:

![Validation]({{ site.url }}/assets/images/MetricDef_Validation.png)

## Metric Attributes

- ID: (MetricID) Unique identifier of metrics within the Workbench db.  
- Title: Common metric name  
- Short Name: Metric Abbreviation  
- Model: Model source of the metric.  Sometimes called "Engine".   
- Active:    
- XPath: location of metric within the source model xml file.  
- Documentation: primary link to metric metadata and definition.  
- DataType:   
- Threshold: Allowed tolerance from Validation metric values.  
- Precision: Preferred decimal place precision  
- Minimum: Minimum allowed value.  Used to identify outliers during validation.  
- Maximum: Maximum allowed value. Used to identify outliers during validation.  
- Updated: Date of last metadata update.  
