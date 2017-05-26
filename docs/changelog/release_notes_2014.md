---
title: 2014 Release Notes
---

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
