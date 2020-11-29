using System;

namespace Howatworks.Matrix.Data.Entities
{
    public interface IGameContextEntity
    {
        DateTimeOffset TimeStamp { get; set; }
        string GameVersion { get; }
        string CommanderName { get; }
    }
}
