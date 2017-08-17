---
title: Release Notes
weight: 99
---

### 7.0.19 - ????

* Fixed int/long data type bug in user queries
* CHaMP Data Sync
    * Reports when the API URL is incorrect
    * Retrieves Site UTM, latitude and longitude

### 7.0.18 - 28 Jul 2017

* Metric download now reporting on errors
	* Insufficient permissions
	* Visits without the specified metric schema defined
	* generic `BadRequest`
* Experimental copy of metrics between schemas

### 7.0.17 - 19 Jul 2017

* AWS Automation user interface enhancements
	* Buttons to AWS services
	* Tooltips
* Fixing some broken online help URLs

### 7.0.16 - 18 Jul 2017

* Revised metric schemas and metric definitions 
* Metric review plot working again
* Keystone credentials persist between tools while the Workbench remains open
* New database upgrade architecture
* The abiltiy to trigger AWS Automation workers

### 7.0.15 - 8 Jul 2017

* Fixing database lock issue during metric download
* Including latest master database version
* Stream, site, watershed, year now columns in metric grid

### 7.0.13 - 28 Jun 2017

* Upgraded to x64 RasterMan 6.4.0
* Changed Habitat Batch Builder to search all directories for substrate rasters, not just the top level.

### 7.0.12 - 2 Jun 2017

* Metric definition column ordering and sizing
* Revised master database, with cleaned out metrics.

### 7.0.10 - 1 Jun 2017

* Confirmation message when checking for updates and the software is already up to date.
* Metric Defintions:
	* Reordered Metric Definitions grid columns
	* Help links available in grid.
	* Metric Definition XML export fix to include precision.
* Moved `Upload Topo Data` feature to experimental menu.

### 7.0.08 - 26 May 2017

* Internal development releases to desolve missing `System.Net.Html.dll` in deployment.

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

## Prior Years

* [2016 Release Notes]({{site.url}}/changelog/release_notes_2016.html)
* [2015 Release Notes]({{site.url}}/changelog/release_notes_2015.html)
* [2014 Release Notes]({{site.url}}/changelog/release_notes_2014.html)
