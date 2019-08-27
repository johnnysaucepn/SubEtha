using System;

namespace Howatworks.Matrix.Core.Entities
{
    public interface IGameContextEntity
    {
        DateTimeOffset TimeStamp { get; set; }
        string GameVersion { get; }
        string CommanderName { get; }
    }
}
