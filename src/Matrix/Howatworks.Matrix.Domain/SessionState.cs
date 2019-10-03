using System;

namespace Howatworks.Matrix.Domain
{
    public class SessionState : ISessionState, ICloneable<SessionState>
    {
        public DateTimeOffset TimeStamp { get; set; }

        public string Build { get; set; }
        public string CommanderName { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }

        public SessionState Clone()
        {
            return new SessionState
            {
                TimeStamp = this.TimeStamp,
                Build = this.Build,
                CommanderName = this.CommanderName,
                GameMode = this.GameMode,
                Group = this.Group
            };
        }
    }
}