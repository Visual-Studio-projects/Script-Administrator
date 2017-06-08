# DocumentumScript
This Documentum scripting tool was created in Visual Studio 2017 C#/WPF Ribbon. Multiple scripts can be contained in one file then run as one command against the idql32/64. It then refreshes the grid with the results file that is created by the idql tool.


<h1 align="center">
  <img src="Images/main_form.png" alt="MyApp" />
</h1>

## Overview
This application was created to help with scripting Documentum databases.  Multiple scripts can be contained in one file then run as one command. This application must be run on the Documentum Content Server in order to work properly.

## Dependencies
|Software                        |Dependency                 |
|:-------------------------------|:--------------------------|
|[Microsoft Visual Studio 2017](https://www.visualstudio.com/vs/whatsnew/)|Solution|
|[Microsoft SQL Server CE 3.5](https://www.microsoft.com/en-au/download/details.aspx?id=5783)|Database|
|[Sandcastle](https://github.com/EWSoftware/SHFB)|API documentation|
|[Word Processor](https://www.libreoffice.org/)|As Built documentation|

## Glossary of Terms

| Term                      | Meaning                                                                                  |
|:--------------------------|:-----------------------------------------------------------------------------------------|
|DocScript |Documentum Script Administrator|
|DQL |Documentum Query Language|
|API |Application Programming Interface|
|idql |A scripting application for DQL located in the working directory on the content server| 
|iapi |A scripting application for API located in the working directory on the content server|

## Functionality
Listed below is the detailed functionality of this application and its components.  

## Main Form
###	Run
* This button will first copy the script from the script file directory to the working directory.  Then it will execute the script file.
###	User Name
* This account must be an administrator in Documentum.
###	Password
* The password for the administrator in Documentum
###	Script Type
* This is the type of script you can run.  i.e. idql32.exe for a 32 bit machine and idql64 for a 64 bit machine.
###	Docbase
* A specific docbase must be selected
###	Script File
* This file contains one or more than one script files.  It has to be copied to the working directory to run.
* The script file can not have any spaces in the file name
###	Results File
* This is the file that is created after the script file is run.  Unless the path to this file is indicated the default path is the working directory.
###	Working Directory
* This is the location of the script type e.g. idql64.exe, iapi64.exe
