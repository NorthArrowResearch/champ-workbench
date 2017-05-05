---
title: Database Structure
---


## CHaMP Tables

All tables that store information from [CHaMP monitoring](http://champmonitoring.org) possess the prefix `CHAMP_`. The following list describes the key fields. There are more fields in the actual database tables, some of which are out of date and no longer used by the Workbench.

* `CHaMP_Watersheds` - List of all CHaMP watersheds.
    * Watershed ID (*this is a workbench ID and unrelated to CHaMP*)
    * Name
* `CHaMP_Sites` - List of all CHaMP sites.
    * Watershed ID
    * Site ID (*this is a workbench ID and unrelated to CHaMP*)
    * SiteName - the official CHaMP site name
* `CHaMP_Visits` - List of all CHaMP visits.
    * VisitID - this is the official CHaMP visit ID
    * SiteID
    * VisitYear - field season in which the visit occurred
    * HitchName - CHaMP hitch name
    * CrewName - CHaMP crew name
    * SampleDate - Date on which te visit was sampled
    * IsPrimary - True if this is visit is by the organization that manages the watershed.
* `CHaMP_Segments` - list of channel segments within a visit
    * Segment ID (*this is a workbench ID and unrelated to CHaMP*)
    * VisitID
    * SegmentNumber - CHaMP segment number
    * SegmentName - CHaMP segment name
* `CHaMP_ChannelUnits` - list of all channel units within each channel segment
    * ID (*this is a workbench ID and unrelated to CHaMP*)
    * Segment ID
    * ChannelUnitNumber - CHaMP channel unit number
    * Tier 1 - channel unit tier 1 designation    
    * Tier 2 - channel unit tier 2 designation
    * *Grain Size fields*

![champ tables](/assets/images/db_champ_tables.png)

## RBT Tables

Tables related to RBT runs possess the prefix `Metric_`. The key tables are:

* `Metric_SiteMetrics` - the main site level metric results.
* `RBT_Batches` - One record for each RBT batch run.
* `RBT_BatchRuns` - One record for each visit that is run within a batch run.
* `LogFiles` - One record for each visit that is run through the RBT and the log file was successfully read by the Workbench afterward.
* `LogFiles` - One record for each message in an RBT log file.
* `Metric_Definitions` - Defines each metric produced by the RBT.
    * RBTMetricID - Workbench identifier for the metric
    * Title - Workbench tile for the metric
    * MetricCalcTypeID - Sitka metric spreadsheet identifier.
    * GCDTypeID
    * RbtXPath - The XML [XPath](http://en.wikipedia.org/wiki/XPath) where the metric can be found in an RBT output file.
    * DatabaseField - The table and field in the Workbench database where the metric is stored.
    * Threshold - plus/minus tolerance when validating RBT metrics.
    * YearIntroduced - The field season when this metric was first introduced.
    * IsActiveForComparison - A value of true means that the metric is included in automated validation.
    * Description - Verbose desription of what the metric is, and how it is calculated.
    * MMLink - [Monitoring Methods](https://www.monitoringmethods.org) link.
    * RBTVersionAdded - The first version of the RBT that included this metric.
    * RBTVersionChanged - The last version of the RBT when this metric was changed.
    * Calculation - Method used to calculate the metric.

## Workbench Tables

* `VersionInfo` - Information describing the version of the database and when it was last updated.
* `VersionChangelog` - Itemizes each change to the database structure and when it was made.
