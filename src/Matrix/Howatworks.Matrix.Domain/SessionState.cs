using System;

namespace Howatworks.Matrix.Domain
{
    public class SessionState : ISessionState, ICloneable<SessionState>, IStateComparable<SessionState>
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

        public bool HasChangedSince(SessionState state)
        {
            if (state == null) return false;

            if (Build != state.Build) return true;
            if (!Equals(CommanderName, state.CommanderName)) return true;
            if (!Equals(GameMode, state.GameMode)) return true;
            if (!Equals(Group, state.Group)) return true;
             
            return false;
        }
    }
}