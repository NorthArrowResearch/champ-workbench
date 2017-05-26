---
title: 2015 Release Notes
---

### 5.0.18 - 31 Mar 2016

* Internal test release with visit metric scavenging working with new Workbench tables.

### 5.0.17 - 4 Feb 2016

* Version 24 of the database.
    * Cleaned Metric_Definitions table
    * Updated Metric_Definitions table using MetricCalcType data from CV.
    * Hidden the old metric tables
    * Added the new normalized metric tables
    * Updated cm.org contents from 3 Feb 2016
    * Deleted old queries.
    * Added Discharge and D84 metrics
* AWS Logging
* Visit filtering from CSV file.
* New validation report.
* Fixed import cm.org data when the database is completely empty.
* Model input file generation now allows setting other batches to not-queued
* Channel Unit CSV generation now matches cm.org header names.

### 5.0.16 - 15 Nov 2015

* Changes to importing CHaMP Monitoring All Measurements export to accommodate fewer records in MetricsCovariates table than AllMeasurements.

### 5.0.15 - 10 Nov 2015

* Fixed column alignment in habitat result to CSV export.

### 5.0.14 - 20 Oct 2015

* Cleaned up database structure, dropping unused tables and columns.
* Added fields to database and main grid view:
    * Organization
    * Stream Temp Logger
    * Visit Phase
    * Visit Status
    * AEM
    * Has Fish Data
    * QC Visit
    * Category Name
* Removed software options for 7Zip.
* Fixed IsPrimary to match cm.org exports.
* Model runs scavenge both target and source IDs
* New random batch selector tool
* Habitat model result export to CSV file.
* Removing inactive GCD menu

### 5.0.13 - 1 Oct 2015

* Batch habitat tool now looks for hydraulic model results CSV under `VISIT_XXXX\Hydro` rather than in HydroModelResults so that it works with both FTP and AWS data structures.

### 5.0.12 - 30 Sep 2015

* Converted habitat batch builder to look for topo and hydro files rather than use database fields.
* Habitat batch builder now has select all/none right click options.
* Used for habitat modeling with CRITFC

### 5.0.10 - 2 Sep 2015

* New code for finding and creating CHaMP Topo folder paths.

### 5.0.09 - 21 Aug 2015

* Google Map feature.
* 2015 model interactivity.

### 5.0.03 - 2 Jun 2015

* Improving status messages during FTP download.
* Implementing folder browsing on FTP download.

### 5.0.01 - 19 May 2015

* Channel Unit CSV generator added to Find By Visit ID.
* Minor file path fixes when topo data do or don't exist.
* Minor change to HSI simulation name generation.