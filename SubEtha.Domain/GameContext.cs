using System.Diagnostics.CodeAnalysis;

namespace SubEtha.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class GameContext
    {
        public string GameVersion { get; set; }
        public string User { get; set; }
        public bool IsLive { get; set; }

        public GameContext(string gameVersion, string user)
        {
            GameVersion = gameVersion;
            User = user;
            IsLive = !gameVersion.Contains("Beta");
        }
    }
}
