---
title: Release Notes
---

### 7.0.07 - 3 May 2017

* Metric scraping of Python topo metrics from XML files
* Minor fixes to metric review features (checking for duplicate columns in CrossTab query)
* Misc fixes to visit details form to handle missing information (channel units and metrics etc)

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