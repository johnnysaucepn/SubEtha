# README #

## SubEtha for Elite: Dangerous

[![AppVeyor build status](https://ci.appveyor.com/api/projects/status/25t6x52w4r3gw6vr/branch/master)](https://ci.appveyor.com/project/johnnysaucepn/subetha/branch/master)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/johnnysaucepn/subetha/master.png)](https://ci.appveyor.com/project/johnnysaucepn/subetha/build/tests?branch=master)
[![Coverlet code coverage](https://ci.appveyor.com/api/projects/johnnysaucepn/subetha/artifacts/AnalysisResults/badge_linecoverage.png?branch=master)](https://ci.appveyor.com/api/projects/johnnysaucepn/subetha/artifacts/AnalysisResults/summary.htm?branch=master)

At its core, this is a set of .NET (Standard, Core and Framework) packages and tools for parsing and consuming Elite: Dangerous Player's Journal log events, allowing the creation of new tools for tracking player progress.

The intent is to free developers from the burden of maintaining repetitive parsing and processing
code, particularly when new iterations of the journal spec are published.

Layered upon this, there are standalone tools (called *Thumb Matrix* and *Thumb Assistant*) that illustrate the use of these packages and offers a re-usable architecture to add new capabilities - this should provide a starter kit for developing journal-based apps.

### Howatworks.SubEtha.Journal

This contains only C#-serializable representations of Elite: Dangerous journal entry types, as listed in the official documentation.
No dependencies on any JSON-parsing library. Where there are discrepancies between the generated logs and the documentation, this is noted in the source.

### Howatworks.SubEtha.Parser

Taking a step up from the raw data types, this allows the creation of readers for individual log files (both the ongoing rolling files and the constantly-replaced files such as `status.json`) and the ability to retrieve the structured data types on demand.

### Howatworks.SubEtha.Monitor

This is a higher level again, providing a fully-operational file monitor that will track the core log files for updates, and trigger events that your application can process however it wishes.

### Howatworks.SubEtha.Bindings

This contains structured data types for the various game control bindings, and methods to access that information per device.

### Howatworks.Thumb.Matrix and Howatworks.Thumb.Assistant

These working applications use the libraries to illustrate some of the possibilities.

*Matrix* watches for certain status changes of ships and locations, and submits them to a central ASP.NET Core server - this allows groups of ships to be monitored in real-time.

*Assistant* operates as a virtual control panel - by checking for controls bound to keys, it can provide a Websocket-powered web front-end to ship operations. In addition, since it also monitors journal events, it can dynamically switch control options according to the situation.

**See README files in each application's source folder for more information**

## Installation

You can get started by consuming the [NuGet packages](https://www.nuget.org/packages?q=Howatworks.SubEtha) in your own projects, or by building and running the Thumb application in this repository. By default, Thumb makes use of both the sample plugins - see below for more details.

## Dependencies

SubEtha and associated tools make use of the following libraries and applications:
* log4net
* Entity Framework Core and PostgreSQL for Matrix web site
* ASP.NET Core for web service hosting
* Autofac for dependency injection
* GitVersion for semantic versioning
* InputSimulatorStandard for virtual key presses
* Newtonsoft.Json for journal serialization
* xUnit.net for testing

## Release History

* 0.1
  * First public release
  
Distributed under the MIT license. See ``LICENSE.md`` for more information.
