---
title: Hydro Prep
---



**This feature is now deprecated and should not be used**. In 2017 hydro prep was implemented within the AWS worker engine that runs the Delft 3D hydraulics model. In other words, it should no longer be necessary to run Hydro Prep within the CHaMP Workbench.

----

The Hydro Prep process exports the following three layers from CHaMP survey geodatabases in preparation for running the [Delft 3D hydraulic model](https://oss.deltares.nl/web/delft3d):

* Thalweg
* Topographic Digital Elevation Model (DEM)
* Water surface DEM

The high level steps for running the hydro prep process are:

1. Download and setup the Hydro Prep dotNet software.
2. Within the workbench:
    1. Specify the path to the Hydro Prep dotNet software.
    2. Select the visits that you want to run.
    3. Create a batch that specifies the visits that you want to run.
    4. Run the batch of visits through the hydro prep dotNet software.
    5. Review the hydro prep results.

The remainder of this page describes these steps:

## Setup

The hydro prep process is performed by a standalone piece of software called **Hydro Prep dotNet**. This software needs to be downloaded and extracted somewhere on your computer before you can run Hydro Prep. There is no installation process. You just need to extract the files into an empty folder somewhere.

<a class="button large fa fa-cloud-download" href="http://releases.northarrowresearch.com/HydroPrepDotNet/2016_05_12_HydroPrepDotNet_1_0_00_x64.zip">&nbsp;&nbsp;Hydro Prep dotNet Software</a>

### Prerequisites

The following software must be on your computer before you can run the Hydro Prep dotNet software:

1. Windows XP or newer.
2. [Microsoft .Net Framework 4](http://www.microsoft.com/en-us/download/details.aspx?id=17851) or newer. ***(Only for windows 7 or earlier)***

## Configuration

**Before you begin** make sure that you have the topographic survey data on your computer for each of the visits that you intend to run. You should have already configured your Workbench monitoring data, input / output files and temp folders in the `Tools Options` menu.

Once you have the Hydro Prep dotNet software on your computer you need to tell the CHaMP Workbench where it can find it:

1. Open the CHaMP Workbench.
2. Select `Tools` - `Options` from the main menu.
3. Switch to the `Models` tab.
4. Click the `Browse` button beside the Hydro Prepration line and select the `HydroPrep.exe` file contained in the folder where you installed the Hydro Prep dotNet software.

## Creating Hydro Prep Batch Runs

The Hydro Prep software can be run in batch mode against multiple CHaMP surveys in one operation. 

Back in the main CHaMP Workbench window select the visits for which you want to run the Hydro Prep process. You might want to use the filter controls on the left of the screen to help narrow your search to a particular watershed, site or visit ID. Use the `SHIFT` key to select multiple visits in a contiguous block and the `CTRL` key to select a distributed set of visits.

![select visits](/assets/images/hydro_prep/select_visits.png)

From the main menu select `Tools - Delft 3D - Hydraulic Model Preparation - Build Input Files...` Provide a meaningful name for this batch of visits and also ensure that all the paths are correct. Click `OK` and the workbench will create a separate input XML file for each visit and queue the visit to be run.

![setup visits](/assets/images/hydro_prep/setup.png)

If you're unsure which visits are queued to be run through the hydro prep process, you can use `Tools - Delft 3D - Hydraulic Model Preparation - Select Batches To Run`. Expand the tree to view the necessary visits. All visits across all batches that have a checked box beside them will be run next time the hydro prep process is run.

## Running Hydro Prep

Click `Tools - Delft 3D - Hydraulic Model Preparation - Run Select Batches...` to open the form that runs the actual Hydro Prep dotNet software. The white area in the middle of the screen will show progress messages as each visit is run through the software.

By default the Hydro Prep dotNet software runs in a hidden DOS window. If you experience problems you can change the window status to `Normal` and the DOS window will become visible.

![run](/assets/images/hydro_prep/run.png)

## Hydro Prep Results

The output of the Hydro Prep process is three CSV files and a log file in the individual visit folders.

The Thalweg file contains the X, Y and Z values of each vertex in the Thalweg. The two DEM CSV files contain all non-zero values from the two corresponding rasters. These latter two files can be quite large for big CHaMP sites (but they compress down well using zip software).

![results](/assets/images/hydro_prep/results.png)

