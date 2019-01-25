---
title: Download CHaMP Data
---

The CHaMP Workbench is capable of downloading files associated with CHaMP visits. You can download multiple files for multiple visits in a single operation. See the [video demonstration](#video-demonstration) at the bottom of this page.

All field files (both topo surveys and auxilliary measurements), as well as other miscellaneous files that have been attached to a visit (such as the fish habitat or hydraulic model results) can be downloaded. The files that are downloaded are always the most up to date copies, and are retrieved directly from [CHaMP Monitoring](http://champmonitoring.org). These are the same files that are available to you on various web pages when you log in do CHaMP Monitoring itself.

## Steps to Download Files

1. [Update to the latest version]({{ site.url }}/download.html#obtaining-updates) of the CHaMP Workbench.
1. It is **absolutely critical** that you always [synchronrize CHaMP Data]({{ site.url }}/Data_Menu/synchronize_champ_data.html) **before** attempting to download files*.
1. Select the visits within the main Workbench grid, for which you want to download files. You can use the filter controls on the left to narrow down to the desired visits. 
1. Select `Download API Files` from the `Data` menu.
1. Verify the number of selected visits at the top of the form.
1. Select the top level folder where you want files to be downloaded into.
1. It's typically best to select the `overwrite existing files` checkbox.
1. Click `OK`.
1. Enter your CHaMP Monitoring login credentials and click `OK`.

![download]({{ site.url }}/assets/images/file_download.png)

## Notes

* *The Workbench retrieves a list of available files and folders for each visit as part of the [synchronize CHaMP Data]({{ site.url }}/Data_Menu/synchronize_champ_data.html) tool. Therefore, the download tool only knows about the files that were available for each visit **at the point in time of the last CHaMP data synchronization**. It is therefore critical that you run the synchronize tool frequently in order for the Workbench to possess the most up to date list of available files.
* The folder and file hierarchy shown is the *super set* of all folders and files for the selected visits. i.e. items shown in the file hierarchy might not exist for all visits selected.
* The files are retrieved and downloaded from CHaMP Monitoring. This tool does not use the the CHaMP FTP site that was used in the past.

## Video Demonstration

<div class="flex-video">
 <iframe width="560" height="315" src="https://www.youtube.com/embed/3iOMO17Lhn8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>