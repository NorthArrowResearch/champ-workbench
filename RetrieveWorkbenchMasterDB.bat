@echo off

SET VERSION=44

rem ***********************************************************************
rem In order for this to work you must have installed and configured
rem The Amazon AWS Command line utilities: https://aws.amazon.com/cli/
rem 
rem Contact: Philip Bailey or Matt Reimer to gain access and keys necessary
rem
rem ***********************************************************************


aws s3 cp s3://releases.northarrowresearch.com/CHaMPWorkbench/MasterDatabase/WorkbenchMaster_V%VERSION%.zip ./CHaMPWorkbench/WorkbenchMaster.zip 
pause