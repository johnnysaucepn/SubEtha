﻿using log4net;
using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
namespace Howatworks.Assistant.WebSockets
{
    public enum ConnectionChange
    {
        Connected,
        Disconnected
    }
}