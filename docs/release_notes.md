---
title: Release Notes
weight: 9
---

### 7.0.06 - 9 Mar 2017

* Fixed several custom visit bugs
    * Incorporating 64bit versions of FileGDB API DLLs
    * Creating custom watershed and site IDs now works
    * Validating that visit ID doesn't already exist now checks 64 bit int
    * Custom visits now defeault to the "custom visit" program
* Creating multiple model input files now closes data reader to allow multiple visit input files to be generated.
* Fixing NWIS data retrieval when USGS site code is less than 8 digits.
* Filtering main visit data grid by organization.
* Re-introduced James' GCD results review tool.
* Topo metric report updates for 2017

### 7.0.05 - 8 Mar 2017

* Program API now authenticates on corresponding Keystone API - production or QA

### 7.0.04 - 8 Mar 2017

* API URLs defaulting to production
* API URL is read only in the user interface
* Corrected developer config file tags for GeoOptix user name
* Renamed `XMLResultTag` field in `Metric_Definitions` database table

### 7.0.03 - 6 Mar 2017

* Fixed channel unit tier1 and tier2 mix-up during API sync.

### 7.0.02 - 2 Mar 2017

* Second beta release with SQLite database.
    * Dropped the `CHaMP_Segments` database table.
* Synchronizing CHaMP data via the API and not cm.org Access DB exports
* FTP data download now operates on multiple visits.
* FTP data download now includes `CrewUploadedSurveyGDB.zip`

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

### 4.0.24 - 18 Nov 2014

* Fixed selection of correct visit in the single XML builder when launched from the Find By Visit ID tool.

### 4.0.23 - 17 Nov 2014

* "Force Primary" now an option when creating model input XML files.
* Cross section spacing is now a configurable model input XML file option.

### 4.0.22 - 14 Nov 2014

* Single visit input file only makes the first visit change detection = true.
* Checkbox for copying the single input XML file path to the clipboard.
* Find By Visit can now launch the single input XML builder.
* Tool tips for the find visit by ID.
* Incorporated some XPath validation code from Sitka.
* Made water surface TINs optional via both single and batch input builders.

### 4.0.19 - 12 Nov 2014

* New model mode for "fix orthogonality with minimal validation". Needed for Bridge Creek analysis where survey GDBs are missing certain * CHaMP layers.
* New DOS window management in batch engine. User interface added, but still not working properly.
* Updated release for Joe Wheaton
* New CHaMP mode for hydro model prep.
    * sample date now written to Visit tag
    * safer handling of missing files in scavenger.

### 4.0.18 - 11 Nov 2014

* single model input file builder now using monitoring/inputoutput/temp folders from settings.

### 4.0.14 - 11 Nov 2014

* Added checkbox to scavenge visit topo info so that user has the option whether visits with missing topo data are set to NULL. Useful if the user has hand entered some topo data paths and wants to keep them.

### 4.0.13 - 9 Nov 2014

* CHaMP Visit ID now part of model input XML file. This ID is also scavenged during results.
* VisitID added to Metric_SiteMetrics and also Metric_ChangeDetection.
* Bedload field added to the channel unit table and scavenged from cmorg database export.

### 4.0.12 - 7 Nov 2014

* New Find Visit By ID Feature
* This also now has a feature for building a model batch directly from the form.
* Visit ID is now added as a tag to the model input XML
* Occular grain size measurements are now added to the model input XML.

### 4.0.10 - 4 Nov 2014

* Preparing the Database for Deployment now has additional option for clearing manual, validation metrics. This is separate from clearing model generated metrics.

### 4.0.9 - 30 Oct 2014

* New user interface for doing result file XPath validation.
