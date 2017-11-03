using System;
using SubEtha.Domain;

namespace SubEtha.Core.Entities
{
    public class SessionStateEntity : IEntity, IGameContextEntity, ISessionState
    {
        public Guid Id { get; set; }

        public GameContext GameContext { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Build { get; set; }
        public string CommanderName { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
    }
}
