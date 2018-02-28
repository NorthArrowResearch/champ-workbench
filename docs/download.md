---
title: Download
weight: 1
---

The CHaMP Workbench is a standalone, desktop software application that supports the [Columbia Habitat Monitoring Program](https://www.champmonitoring.org). To install it you download and run the `setup.exe` application linked below. This will install the Workbench in your user profile folder. Read the [release notes](release_notes.html) for information on the latest features and updates.

## Prerequisites

1. [64 bit version](technical_reference/guides/windows_version.html) of Windows (XP or newer).
2. [Microsoft .Net Framework 4.6.2](https://www.microsoft.com/en-us/download/details.aspx?id=53344) or newer.
3. 150Mb free disk space for the software, plus enough space to store input and out datasets.
4. [64-bit Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-ca/download/details.aspx?id=48145). *NOTE: You must install the **64-bit** version ( 
  vc_redist.x64.exe). If you have previously installed the 32-bit version you still need to install the 64-bit version.*

## Download CHaMP Workbench Software

<a class="button large fa fa-cloud-download" href="http://releases.northarrowresearch.com/CHaMPWorkbench/setup.exe">&nbsp;&nbsp;CHaMP Workbench Software</a>

## Workbench Database

In 2017 the Workbench has been migrated to [SQLite](https://www.sqlite.org/) instead of Microsoft Access. This simplifies deployment and improves performance. There are no special requirements associated with SQLite. The Workbench comes with everything that is needed to interact with SQLite database. See the section on [Working with SQLite databases](Technical_Reference/working_with_sqlite_databases.html) if you want to interact with the Workbench database directly.

The Workbench software now comes with the latest version of the database. You no longer have to download the database separately. Once the latest version of the Workbench software is installed and open, click the File menu and choose "Create New Workbench Database..." Choose a location to save the database file and then start working with the Workbench software.

![create new database](/assets/images/create_database.png)

Note that the Workbench is **only** compatible with the latest version of the database. You can use Microsoft Access to transfer information from older versions of the database to the latest version. Contact the Workbench software developers (info@northarrowresearch.com) if you need help with this.

## Installation Process

Click on the link above to download the software `setup.exe` file. Certain internet browsers will warn about the potential risks of downloading executable files (see below). Check that the warning refers to the CHaMP Workbench software and then click to keep the file.

![Warning](/assets/images/warning.png)

Once the `setup.exe` file download is complete, double click the file to run the installation routine and follow the prompts to complete the process. There are no options or choices during the installation.

The software does not require Administrator privileges to install and does not place any files in the `C:\Program Files` folder. The entire application is stored in the user's profile folder.

## Obtaining Updates

The software automatically checks for newer versions every time the software is launched. When a newer version is detected the user is prompted to install it.

![Update Offer](/assets/images/update_offer.png)

Note that clicking cancel turns off the automated check permanently and the software will no longer prompt when newer versions are available. 

There is also a manual "Check for Updates" feature on the Help menu in the software.

![Update Menu](/assets/images/checkforupdates.png)
