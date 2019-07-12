using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class GameContext
    {
        public string GameVersion { get; set; }
        public string CommanderName { get; set; }
        public bool IsLive { get; set; }

        public GameContext(string gameVersion, string commanderName)
        {
            GameVersion = gameVersion;
            CommanderName = commanderName;
            IsLive = !gameVersion.Contains("Beta");
        }
    }
}
