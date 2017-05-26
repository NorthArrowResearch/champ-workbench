---
title: Download
---

The CHaMP Workbench is a standalone, desktop software application that supports the [Columbia Habitat Monitoring Program](http://champmonitoring.org). To install it you download and run the `setup.exe` application linked below. This will install the Workbench in your user profile folder. Read the [release notes](/release_notes) for information on the latest features and updates.

## Prerequisites

1. Windows XP or newer. Must be [64 bit version](/technical_reference/guides/windows_version).
2. [Microsoft .Net Framework 4](http://www.microsoft.com/en-us/download/details.aspx?id=17851) or newer. ***(Only for windows 7 or earlier)***
4. 150Mb free disk space for the software, plus enough space to store cross input and out datasets.
6. ***NEW*** [64-bit Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-ca/download/details.aspx?id=48145). *NOTE: You must install the 64-bit version ( 
vc_redist.x64.exe). If you have previously installed the 32-bit version you still need to install the 64-bit version.*

## Download CHaMP Workbench Software

<a class="btn btn-large btn-primary fa fa-cloud-download" href="http://releases.northarrowresearch.com/CHaMPWorkbench/setup.exe">&nbsp;&nbsp;CHaMP Workbench Software</a>

## Workbench Database

In 2017 the Workbench has been updated to use [SQLite](https://www.sqlite.org/) as the underlying database instead of Microsoft Access. This simplifies deployment and improves performance. There are no special requirements associated with SQLite. The Workbench comes with everything that is needed to interact with SQLite database. See the section on [Working with SQLite databases]() if you want to interact with the Workbench database directly.

The Workbench now installs the latest version of the database along with the software. You no longer have to download it separately.

Once the latest version of the Workbench software is installed and opened, click the File menu and choose "Create New Workbench Database..." Choose a location to save the database file and then start working with the Workbench.

![create new database](/images/create_database.png)

Note that the Workbench is **only** compatible with the latest version of the database. You can use Microsoft Access to transfer information from older versions of the database to the latest version.

## Installation Process

Click on the link above to download the software setup.exe file. Certain internet browsers will warn about the potential risks of downloading executable files (see below). Check that the warning refers to the CHaMP Workbench software and then click to keep the file.

![Warning](/images/warning.png)

Once the `setup.exe` file download is complete, double click the file to run the installation routine and follow the prompts to complete the process. There are no options or choices during the installation.

The software does not require Administrator privileges to install and does not place any files in the `C:\Program Files` folder. The entire application is stored in the user's profile folder.

## Obtaining Updates

The software automatically checks for newer versions every time the software is launched. When a newer version is detected the user is prompted to install it.

![Update Offer](../images/update_offer.png)

Note that clicking cancel turns off the automated check permanently and the software will no longer prompt when newer versions are available. 

There is also a manual "Check for Updates" feature on the Help menu in the software.

![Update Menu](../images/checkforupdates.png)
