---
title: Discovering Microsoft Access Architecture
---

There are many versions of Microsoft Access available (97, 2010, 2013 etc). In addition, there are two different *architectures* available; 32 bit and 64 bit. It's confusing, but the 32 bit is referred to as x86 while the 64 bit is referred to as x64. This is important because 32 bit software is generally incompatible with 64 bit software and so it's necessary to know which architectures you already have installed when contemplating new software that needs to be compatible.

So for the purpose of the CHaMP Workbench you must have *version* 2010 or newer of Microsoft Access. And you can use the instructions below to discover if you have the 32 or 64 bit *architecture* of Microsoft Access.

## Steps to Discover Microsoft Access Architecture.

1. Open Microsoft Access. You should be presented with options for opening and existing or creating a new database.
1. Click *Open Other Files* at the bottom left. (If you already have an Access database open, you can simply click on the *File* menu, top left).
1. Click *Account* in the middle of the left pane.
1. Click *About Access*.
1. Read the version information at the top of the screen. It will say *32-bit* or *64-bit*.

![Access Version](/images/access_version.png)