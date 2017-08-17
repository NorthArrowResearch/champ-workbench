---
title: Metric Tables
---

## Metric Tables

Tables related to RBT runs possess the prefix `Metric_`. The key tables are:

- `Metric_SiteMetrics` - the main site level metric results.
- `RBT_Batches` - One record for each RBT batch run.
- `RBT_BatchRuns` - One record for each visit that is run within a batch run.
- `LogFiles` - One record for each visit that is run through the RBT and the log file was successfully read by the Workbench afterward.
- `LogFiles` - One record for each message in an RBT log file.
- `Metric_Definitions` - Defines each metric produced by the RBT.
  - RBTMetricID - Workbench identifier for the metric
  - Title - Workbench tile for the metric
  - MetricCalcTypeID - Sitka metric spreadsheet identifier.
  - GCDTypeID
  - RbtXPath - The XML [XPath](http://en.wikipedia.org/wiki/XPath) where the metric can be found in an RBT output file.
  - DatabaseField - The table and field in the Workbench database where the metric is stored.
  - Threshold - plus/minus tolerance when validating RBT metrics.
  - YearIntroduced - The field season when this metric was first introduced.
  - IsActiveForComparison - A value of true means that the metric is included in automated validation.
  - Description - Verbose desription of what the metric is, and how it is calculated.
  - MMLink - [Monitoring Methods](https://www.monitoringmethods.org) link.
  - RBTVersionAdded - The first version of the RBT that included this metric.
  - RBTVersionChanged - The last version of the RBT when this metric was changed.
  - Calculation - Method used to calculate the metric.