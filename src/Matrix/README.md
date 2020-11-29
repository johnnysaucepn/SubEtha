# README #

## SubEtha Matrix for Elite: Dangerous

*Matrix* is a standalone application that monitors Commander activity in real-time,
relaying ship movements (and other status information) back to a central server (by default,
this is hosted at [matrix.howatworks.com](https://matrix.howatworks.com)).

Since this is independent of in-game activity, Commanders can be co-ordinated even when
running in Solo or Private Group mode.

The software supports Commanders being part of named groups, however user interface to support
assigning Commanders to groups is under development.

The server software uses ASP.NET Identity with JWT bearer tokens to secure account credentials.

## Installation and usage

The default application (*Howatworks.Matrix.Wpf*) is a Windows-only, .NET Core 3.1
system tray app, which will remain unobtrusive and consume as little resource as possible.

This application is provided as an MSI installer for installation on target machines.

However, there is also a Console version which should run on any Core-supported platform, as long as it has access to the journal log files.

Distributed under the MIT license. See ``LICENSE.md`` for more information.
