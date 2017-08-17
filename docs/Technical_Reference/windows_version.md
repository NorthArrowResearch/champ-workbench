---
title: Discovering Microsoft Windows Architecture
---

There are many versions of Microsoft Windows available (XP, Vista, 7, 8, 8.1 etc). In addition, since Windows 7 there are two different *architectures* available; 32 bit and 64 bit. It's confusing, but the 32 bit is referred to as x86 while the 64 bit is referred to as x64. This is important because 32 bit software is generally incompatible with 64 bit software and so it's necessary to know which architectures you already have installed when contemplating new software that needs to be compatible.

So for the purpose of Habitat Model you must have a *version* of Microsoft Windows XP or newer. And you can use the instructions below to discover if you have the 32 or 64 bit *architecture* of windows.

## Steps to Dicover Microsoft Windows Architecture

1. Open Windows Explorer.
1. Navigate to your C drive (C:\).
    1. If you only see a folder called `C:\Program Files` then you have a 32 bit version of Windows.
    1. If, in addition to a folder called `C:\Program Files`, you also see a folder called `C:\Program Files (x86)` then you have a 64 bit version of Windows.
