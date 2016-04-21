 @echo off
   rem ***********************************************************************
   rem This bat file Goes and gets the lastest (and previous) C++ libraries
   rem In order for this to work you must have installed and configured
   rem The Amazon AWS Command line utilities: https://aws.amazon.com/cli/
   rem 
   rem Contact: Philip Bailey or Matt Reimer to gain access and keys necessary
   rem
   rem ***********************************************************************
 
   echo "----------------------------------------------------"
   echo "  SYNCHING ./_Redistributable folder with S3"
   echo "----------------------------------------------------"
   if not exist "./_Redistributable" ( mkdir "./_Redistributable" )
   aws s3 sync s3://releases.northarrowresearch.com/CHaMPWorkbench/lib ../CHaMPWorkbenchLibs
   pause