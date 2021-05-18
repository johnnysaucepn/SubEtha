# README #

## SubEtha for Elite: Dangerous

[![AppVeyor build status](https://img.shields.io/appveyor/ci/johnnysaucepn/subetha/master)](https://ci.appveyor.com/project/johnnysaucepn/subetha/branch/master)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/johnnysaucepn/subetha/master)](https://ci.appveyor.com/project/johnnysaucepn/subetha/build/tests?branch=master)
[![Coverlet code coverage](https://img.shields.io/codecov/c/github/johnnysaucepn/SubEtha/master)](https://codecov.io/gh/johnnysaucepn/SubEtha)

At its core, this is a set of .NET (Standard/Core) packages and tools for parsing and consuming Elite: Dangerous Player's
Journal log events, allowing the creation of new tools for tracking player activity.

The intent is to free developers from the burden of maintaining repetitive parsing and processing code, particularly when
new iterations of the journal spec are published.

Layered upon this, there are [standalone sample tools](https://github.com/johnnysaucepn/SubEtha.Apps) that illustrate
the use of these packages and offers a re-usable architecture to add new capabilities - this should provide a starter kit
for developing journal-based apps.

### Howatworks.SubEtha.Journal

This contains only C#-serializable representations of Elite: Dangerous journal entry types, as listed in the official
documentation. This library has no dependencies on any logging or JSON-parsing libraries. Where there are discrepancies
between the generated logs and the documentation, this is noted in the source.

### Howatworks.SubEtha.Parser

Taking a step up from the raw data types, this allows the creation of readers for individual log files (both the ongoing
rolling files and the constantly-replaced files such as `status.json`) and the ability to retrieve the structured data
types on demand.

### Howatworks.SubEtha.Monitor

This is a higher level again, providing a fully-operational file monitor that will track the core log files for updates,
and trigger events that your application can process however it wishes. In addition, it exposes these updates as a sequence
using [Reactive Extensions](https://www.nuget.org/packages/System.Reactive/).

### Howatworks.SubEtha.Bindings

This contains structured data types for the various game control bindings, and methods to access that information per
device.

## Installation

You can get started by consuming the [NuGet packages](https://www.nuget.org/packages?q=Howatworks.SubEtha) in your own
projects, or by building and running the [sample apps](https://github.com/johnnysaucepn/SubEtha.Apps).

Build the packages using Visual Studio 2019+, or at the command line using:
```
dotnet tool restore
dotnet cake
```

## Dependencies

SubEtha and associated tools make use of the following libraries and applications:
* log4net
* GitVersion for semantic versioning
* Newtonsoft.Json for journal serialization
* xUnit.net for testing

## Release History

* 0.1
  * First public release
* 0.5
  * Rearchitected to use Reactive Extensions (Rx) to simplify data flow
* 0.7
  * Migrated apps to separate repository
* 0.8
  * Removed dependency on log4net for libraries

  
Distributed under the MIT license. See ``LICENSE.md`` for more information.
