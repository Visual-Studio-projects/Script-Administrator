<img align="left" src="CS/Images/App.ico" width="64px" >

# Documentum Script Administrator

[![Join the chat at https://gitter.im/DocumentumScriptAdministrator/Lobby](https://badges.gitter.im/DocumentumScriptAdministrator/Lobby.svg)](https://gitter.im/DocumentumScriptAdministrator/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE "MIT License Copyright © Anthony Duguid")
![current_build WebTop_6.7](https://img.shields.io/badge/current_build-WebTop_6.7-red.svg)

This Documentum scripting tool was created in Microsoft Visual Studio Community 2017 C#/WPF Ribbon. Multiple scripts can be contained in one file then run as one command against the idql32/64. It then refreshes the grid with the results file that is created by the idql tool. It was created to help with scripting Documentum databases. This application must be run on the Documentum Content Server in order to work properly.

<h1 align="center">
  <img src="CS/Images/ReadMe/DocScriptMainForm.png" alt="MyApp" />
</h1>

<br>

## Table of Contents
- <a href="#dependencies">Dependencies</a>
- <a href="#glossary-of-terms">Glossary of Terms</a>
- <a href="#functionality">Functionality</a>

<br>

<a id="user-content-dependencies" class="anchor" href="#dependencies" aria-hidden="true"> </a>
## Dependencies
|Software                        |Dependency                 |
|:-------------------------------|:--------------------------|
|[Microsoft Visual Studio Community 2017](https://www.visualstudio.com/vs/whatsnew/)|Solution|
|[Microsoft Ribbon for WPF](https://www.microsoft.com/en-us/download/details.aspx?id=11877)|Project|
|[Microsoft SQL Server CE 3.5](https://www.microsoft.com/en-au/download/details.aspx?id=5783)|Database|
|[SQL Server Compact Toolbox](https://marketplace.visualstudio.com/items?itemName=ErikEJ.SQLServerCompactSQLiteToolbox)|Database|
|[Sandcastle](https://github.com/EWSoftware/SHFB)|API documentation|
|[Log4Net](https://www.nuget.org/packages/log4net/) |Error Logging |
|[Documentum](http://documentum.opentext.com/documentum/)|Script|
|[www.IconArchive.com](http://www.iconarchive.com/show/silk-icons-by-famfamfam.html)|Icons|
|[Snagit](http://discover.techsmith.com/snagit-non-brand-desktop/?gclid=CNzQiOTO09UCFVoFKgod9EIB3g)|Read Me|VBA, VSTO|
|Badges ([Library](https://shields.io/), [Custom](https://rozaxe.github.io/factory/), [Star/Fork](http://githubbadges.com))|Read Me|VBA, VSTO|

<br>

<a id="user-content-glossary-of-terms" class="anchor" href="#glossary-of-terms" aria-hidden="true"> </a>
## Glossary of Terms

| Term                      | Meaning                                                                                  |
|:--------------------------|:-----------------------------------------------------------------------------------------|
|DocScript |Documentum Script Administrator|
|DQL |Documentum Query Language is used to query Documentum which is a content management system used to create, manage, deliver, and archive all types of content from text documents and spreadsheets to digital images, HTML, and XML components. DQL uses syntax that is a superset of ANSI-standard SQL (Structured Query Language) DQL statements operate on objects and sometimes on tables/rows but SQL statements operate only on tables/rows. Part of DQL statements are translated automatically into SQL before being executed by the eContent Server|
|API |Application Programming Interface is a collection of methods prescribed by an application program by which another application can send requests to it. For example, Server API methods are executed by the Documentum Desktop to access the Content Server|
|idql |A scripting application for DQL located in the working directory on the content server| 
|iapi |A scripting application for API located in the working directory on the content server|
|WPF|Windows Presentation Foundation (or WPF) is a graphical subsystem by Microsoft for rendering user interfaces in Windows-based applications. WPF, previously known as "Avalon", was initially released as part of .NET Framework 3.0 in 2006. WPF uses DirectX. WPF attempts to provide a consistent programming model for building applications and separates the user interface from business logic. It resembles similar XML-oriented object models, such as those implemented in XUL and SVG. WPF employs XAML, an XML-based language, to define and link various interface elements.[1] WPF applications can be deployed as standalone desktop programs or hosted as an embedded object in a website. WPF aims to unify a number of common user interface elements, such as 2D/3D rendering, fixed and adaptive documents, typography, vector graphics, runtime animation, and pre-rendered media. These elements can then be linked and manipulated based on various events, user interactions, and data bindings.|

<br>

<a id="user-content-functionality" class="anchor" href="#functionality" aria-hidden="true"> </a>
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

##	Quick Access ToolBar
The following are all functions of the quick access toolbar.
 
###	Refresh
* This refreshes the results column from the results file in the datagrid
### Save
* This saves the grid to a csv file

## Application Menu
The following are all functions of the application menu.
 
###	About
* The about form 
###	How To…
* Opens the as built documentation.
###	API Doc…
* Opens the API documentation.
###	Settings
* All the dropdown lists and default values are stored here and can be edited.
###	Copy
* Copies the command line script
###	Exit
* Closes the application.
