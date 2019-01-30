using System;
using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    public class SessionState : ISessionState
    {
        public DateTimeOffset TimeStamp { get; set; }

        public string Build { get; set; }
        public string CommanderName { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        
    }
}