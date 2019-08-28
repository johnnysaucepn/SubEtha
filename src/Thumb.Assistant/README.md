# README #

## SubEtha Thumb Assistant for Elite: Dangerous

*Thumb Assistant* is a standalone application that operates as a virtual control panel, removing
the need to memorize arcane key combinations under pressure.

### User Interface

The UI is (currently) implemented as a Bootstrap web front-end hosted inside the application itself. This allows any device capable of using Websockets (almost all modern browsers, mobile, tablets) to act as the remote control.

The UI can, by default, be found at [http://<hostname>:8985/index.html](http://<hostname>:8985/index.html) when the application is running.

In the future, it should be possible for any Websocket-capable device to interact with the Assistant application, whether software or hardware switches.

### Usage of the SubEtha libraries

One other advantage of using SubEtha libraries to monitor journal files is that the UI can dynamically switch to display the appropriate controls for the current status - e.g. the operations available in Supercruise are very different for those in Combat or Analysis modes.

Additionally, the SubEtha binding parsing allows the application to detect the active keybindings for actions - disabling those that are unavailable, and sending the correct keystrokes without additional configuration. All that is required is for the keys to be bound.

## Installation and usage

The default application (*Howatworks.Thumb.Assistant*) is a Windows-only, .NET Framework 4.7.2
system tray app, which will remain unobtrusive and consume as little resource as possible.

This application is provided as an MSI installer for installation on target machines.

However, there is also a Console version, written in .NET Core 2.1 and should run on any
Core-supported platform, as long as it has access to the journal log files.

Distributed under the MIT license. See ``LICENSE.md`` for more information.
