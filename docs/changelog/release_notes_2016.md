---
title: 2016 Release Notes
---

### 6.0.12 29 Nov 2016

* Hot fix to previous metric review change. Putting parameters in the correct order for Microsoft Access query.

### 6.0.11 - 29 Nov 2016

* Fixing metric review query to select the newest metric result value (ignoring results that don't include any values for the specific metric).

### 6.0.10 - 9 Nov 2016

* Three digit formatting on metric columns in the metric review data grid.for analysis by

### 6.0.09 - 17 Oct 2016

* Fixing HTML validation report to exclude tier 1, 2 and channel unit metrics. This report only works with visit level metrics.
* Minor bug fixes to the user query management user interface.

### 6.0.08 - 14 Oct 2016

* New User feedback feature for capturing and storing metric review comments in workbench database.
* New Metric Review feature for reviewing metric values by program.
    * Allows custom plots using any XY combination of metrics associated with the program
    * Users can enter feedback as they are viewing plots
    * Metric grid of all metrics for the selected program
* Master database version 32.

### 6.0.07 - 5 Aug 2016

* Channel unit CSV generation
    * Updated CSV columns to match 2016 broker file format.
    * New batch CSV generation feature
* Log file scavenging now stores the run date-time, model version and batch run ID when reading a log file. The first two are always read and populated in the database. The batch run ID is only populated when a log file is scavenged as part of a Workbench batch run. Simply scavenging log files from disk will not populate this field.
* Master database version 31.

### 6.0.06 - 23 June 2016

* FTP switching between AWS And CHaMP
* Tier 1 and 2 as well as channel unit metric scavenging
* New visit filtering:
    * site name
    * stream name
    * Primary / non-primary
    * Default to no watersheds selected.
* User queries.

### 6.0.05 - 3 June 2016

* Changing `MinDatabaseVersion` software setting scope from `User` to `Application`.

### 6.0.04 - 3 June 2016

* Metric validation report model version filtering fixed.
* Cleaned-up the new dynamic report selection form.
* Fixes to habitat batch builder
    * FIS models now possible.
    * automated variable naming improved
* Database Version 29
    * Habitat metric results query
    * Hiding unused queries

### 6.0.03 - 30 May 2016

* Workbench now ships with the latest database and helps users create a new copy of the database if they don't have one.
* Visit properties for viewing channel units, metrics, logs.
* Add custom visits to the database.
* Scavenge habitat metrics into the workbench database.
* Updated to the latest reports.
* Historical discharge viewer.
* Exporting and loading selections of visits to CSV file.
* Fixed field season filter listbox (was duplicating values)
* Version 28 0f the database:
    * Reversing Bankfull and Other child metric grouping list items.
    * Dropping Result_Old and SourceFile fields from Metric_Results table
    * New Metric_Habitat table with relationships
    * New species and life stage lookup list with values
    * Split HSI and FIS into two separate model types in the lookup list
    * Dropped IsBridge field from CHaMP_Visits
    * Added Remarks field to CHaMP_Visits
    * Added new Metric_Plots table to support visit details in workbench

### 6.0.02 - 19 May 2016

* Uses Database version 27.
* Database version checking at startup.
* First public version for the 2016 field season.
* CHaMP Protocol ID imported from cm.org exports
* Unified model run process.
    * Batch generator filters for visits associated with protocols that have topo data.
    * Checkbox for making the new batch the only queued batch.
* Hydro Prep integration.
* Validation reports
    * Manual Metric Validation Report
    * Watershed Report
* Normalized metric database tables & scavenging
* Min/ Max value fields associated with metric definitions.
* Metric grouping lookups.