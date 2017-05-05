---
title: Getting Started
weight: 1
---

This page describes the initial workflow to get up and running with the CHaMP Workbench software. All new users should perform these steps at least once to ensure that they have the latest data from [CHaMP](http://champmonitoring.org) and that the Workbench database is aware of the data on their system.

### Download the Workbench Software

1. Check the [downloads page](/download) to ensure that you have the pre-requisites installed.
1. [Download](/download) the Workbench software.
    1. Click the link to download the `setup.exe` file.
    1. double click to run the `setup.exe` file. This will install the software.

**Note:** You can check for new versions and update the software at any time by choosing `Check For Updates` on the help menu. See the [download](/download) page for more detailed instructions.

### Create CHaMP Workspace

It is strongly recommended that you create a workspace for the files related to the Workbench. This will be used as the *home* location for monitoring data and model inputs and outputs used by the Workbench. This workspace should be a folder on your computer, preferably at a high level (i.e. not nested inside too many other folders) and **with no spaces in the path**. Note that if you use a folder on your desktop or a folder inside your Windows profile, you need to ensure that your Windows user name does not include any spaces or periods! We recommend the following workspace folder structure:

```bash
C:\CHaMP                   <- main workspace folder
C:\CHaMP\MonitoringData    <- folder for storing raw CHaMP monitoring data (topo and hydro files from cm.org)
C:\CHaMP\InputOutputFiles  <- folder for RBT and other model inputs and outputs
C:\CHaMP\Temp              <- temp folder for models
```

Using the folder structure above is strongly recommended. The Workbench will then create and populate folders under `MonitoringData` and `InputOutputFiles` that mirror the CHaMP FTP site folder structure. i.e. Once you have downloaded several datasets your folder structure might look like:

```bash
C:\CHaMP
C:\CHaMP\MonitoringData
C:\CHaMP\MonitoringData\2011
C:\CHaMP\MonitoringData\2011\Asotin\ASW00001-CC-F1P2BR\VISIT_213\Topo\SurveyGDB.zip
C:\CHaMP\MonitoringData\2014\JohnDay\OJD03458-000534\VISIT_2645\Hydro\HydroModelResults.
zip
...
C:\CHaMP\InputOutputFiles
C:\CHaMP\Temp
```

### Download the Workbench Database

Download the [Workbench Microsoft Access database]( 
http://releases.northarrowresearch.com/CHaMPWorkbench/WorkbenchDatabase.zip) and unzip the file into the top level of the workspace created in the previous step.

![folders](/images/folders.png)

### Refresh CHaMP Data

The Workbench maintains a record of all CHaMP watersheds, sites, visits and channel units. Given that visits are occuring to CHaMP sites during the field season, it's important to refresh the contents of the Workbench database periodically.

The Workbench uses one of the data exports from the CHaMP Monitoring as the source of this information. 

1. Login to [http://champmonitoring.org](http://champmonitoring.org) and switch to the `Data Exports` tab.
1. Download the `All Measurements` Access database export. **TODO: The Complete these instructions once the CHaMP monitoring web site is no longer broken.**
1. UnZip the all measurements Access database export.
1. Click `Import CHaMP...` In the Workbench from the `Data` menu.
1. Browse to the location where you unzipped the **All Measurements" Access database.
1. Click OK.

![cm.org](/images/cmorg_allmeasurements.png)

### Download CHaMP Data - Option 1 Many - All Visits

If you plan on working with lots of CHaMP visits (e.g. an entire watershed) then it's recommended that you use an FTP software client such as [WinSCP](http://winscp.net/) or [FileZilla](https://filezilla-project.org) to retrieve the data. There are [CHaMP-specific instructions for using WinSCP](/Technical_Reference/Guides/WinSCP_Quick_How_To_Guide.pdf).

You **MUST** download the **ByYear** version of the data and place the files inside the workspace `MonitoringData` folder created in the previous step.

### Download CHaMP Data - Option 2 Select Visits

If you intend to only work with a small number of visits, or if you already have a copy of all the data and you just want to refresh files for one or more visits then use the following inidividual visit download approach.

1. Open the Workbench software. If you just downloaded the software you can press the Windows Key and start typing `CHaMP Workbench`. The CHaMP Workbench software should appear.
1. Open the Workbench database by choosing `Open` from the File menu. Browse to the Workbench workspace and choose the Access database that you unzipped in an earlier step. The list of all CHaMP visits should populate.
1. Use the filter controls on the left to browse to the desired visit:
    1. Enter a Visit ID if you know it
    1. Use the field season / watershed / site controls to narrow in on the visit.
    *Tip:* If you want to find all visits to the same site, then right click on any visit and choose `Filter for all visits to this site`.
1. Right click and choose `Download`. In the window that appears, select the files that you want to download. Click the top level node in the hierarchy if you want all the files related to the visit.
    ~[Download](/images/download.png)
      *Note: Not all visit files are availabled on the CHaMP monitoring FTP site. For an accurate list of what's available you should use an FTP client as describe in Option 1 of this step.*
1. Click the `Start Download` button to retrieve the files. Note how the workbench will recreate the same folder structure in your workspace as is used on the CHaMP monitoring FTP site.

### UnZip the CHaMP Data

The CHaMP data comes as a series of zip archives. Before you can use the data you have to unzip the various files.

1. Choose "Unpack..." from the main Workbench menu.
1. Review the inputs in the window that appears.
    1. Ensure that the top level folder points to your CHaMP workspace.
    1. Turn off the hydraulic model result if you don't intend to use these files.
1. Click OK. The tool will search through your folder structure and unzip any files that match the search patterns.
*Note that the files are unzipped into the same folder as the zip archives. Specifying a different location for the unzip process is considered an advanced topic.*

*Note:* You can repeat this process each time that you download a set of visit files using the Workbench download tool, or simply unzip the relevant files individually.

![Unpacker](/images/unpacker.png)

### Refresh Data Paths

The Workbench database keeps track of which visit topo data nd hydraulic model files are available on your computer. Follow these steps to refresh the Workbench database and ensure that it is aware of the latest changes:

1. Click `Update Topo...` from the `Data` menu in the Workbench.
1. Ensure that the top level folder points to your CHaMP workspace.
1. Make sure that you choose the `Field Season\Watershed\Site\Visit` folder structure option.  This is the only folder structure that currently works with *all* the Workbench features.
1. Click `Run`.

This tool does not change any files on the operating system. It simply updates the relevant visit records in the database with the path to the relevant files.

![file paths](/images/file_paths.png)

At this point the Workbench, database and folder structure are all in a state ready to be used. You can use the Workbench features to search for visits, run the RBT etc. Or simply use the database to achieve your own research. A good next step is to read the page on [basic navigation](/technical_reference/basic_navigation) and how to work with the main Workbench visit information.