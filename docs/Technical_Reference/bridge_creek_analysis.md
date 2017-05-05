---
title: Bridge Creek Analysis
---

## Introduction

This page describes the process of running multiple GCD analyses on several topographic surveys in Bridge Creek. The intent is to leverage several CHaMP software tools to automate much of this work given its repetitive nature.

The process outlined below describes how the CHaMP Workbench can automate the generation of RBT input XML files and then the running of the RBT against these files to perform the regular CHaMP GCD analysis. The result is a series of GCD projects - together with all the change detection and budget segregations analyses already completed - that can be opened in the regular GCD Desktop software for review.

## Workflow

1a) Prepare your data. You need a separate file geodatabase (GDB) for each visit (CHaMP terminology for a survey). Ideally you should create each GDB just a like a CHaMP survey with all the layers named and structured precisely like a CHaMP survey. However, this is not mandatory and see step 3a.

1b) Organize your data. I recommend that you organize your data in the following folder structure. Field Season would be the year (2011, 2012 etc) Watershed will always be the same. Site name should be the CHaMP site names or use your own for non-CHaMP surveys (Avoid numbers at the start, avoid spaces and special characters).

 FieldSeason\Watershed\Site\Visit\Topo\<file GDB>

2a) Download the latest CHaMP Workbench software.

2b) Obtain the latest CHaMP Workbench database from me. 

3a) Edit the Workbench database contents to ensure that the correct sites and visits exist for your Bridge Creek data. Note that some of the Bridge Creek visits (the CHaMP term for a survey) are already in the database because they are part of CHaMP. However, you will need to add records to the tables CHaMP_Sites, CHaMP_Visits, CHaMP_Segments, and CHaMP_ChannelUnits for those Bridge surveys that are unrelated to CHaMP. I will produce a YouTube video showing you how to do this.




3b) Edit the Workbench database and ensure that all your visits have their "primary" field set to true. The CHaMP GCD only considers primary visits (as opposed to crew variability visits etc.)

4a) Download the latest CHaMP RBT Console. Note that this version of the RBT can be configured to do a standard "CHaMP" GCD change detection analysis along with several budget segregations. This will create a GCD project along with all the appropriate inputs, outputs etc. Contact Philip Bailey for a copy of the latest RBT Console software.

4b) Set the path to the RBT executable (rbt.exe) in the Workbench under Tools/Options.




5a) Use the CHaMP Workbench to generate RBT Input files for each of your CHaMP visits. Make sure that you turn the change detection on and the metric calculation off!




5b) Edit any of the RBT input files for visits that have non-standard CHaMP layer names. For example, if your wetted extent polygon layer is not called "WaterExtent" then you can edit the data dictionary at the top of the RBT input file and provide the correct name. This is also where you enter your error raster name for a survey if you have one.




6) Use the Workbench to queue up the appropriate RBT batch files for your Bridge Creek sites. Tools/RBT/Select RBT Batches to Run.




7) Run the RBT through the Workbench (Tools/RBT/Run Queued Batches). This will loop through all the RBT input files and run the CHaMP GCD analysis. 




8a) Review the RBT log files in the Workbench database and make sure there were no errors.

8b) Open ArcGIS and review the GCD results. Each RBT run creates a folder called "GCD" inside which it creates a GCD project. These projects can be opened in the GCD. They contain the DEM surveys, DoDs and budget segregations.

8c) If you turned on "Scavenge Metric Results" during step 7 the GCD results are also imported into the Workbench database and available in Access. This is handy for sorting and filtering etc to help look for interesting/problematic sites.
