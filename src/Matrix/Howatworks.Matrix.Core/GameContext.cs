using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Core
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal sealed class GameContext : IEquatable<GameContext>
    {
        public string GameVersion { get; }
        public string CommanderName { get; }

        public GameContext()
        {
        }

        public GameContext(string gameVersion, string commanderName)
        {
            GameVersion = gameVersion;
            CommanderName = commanderName;
        }

        public bool Equals(GameContext other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(GameVersion, other.GameVersion, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(CommanderName, other.CommanderName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is GameContext other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCultureIgnoreCase.GetHashCode(GameVersion) * 397) ^
                       StringComparer.InvariantCultureIgnoreCase.GetHashCode(CommanderName ?? string.Empty);
            }
        }

        public static bool operator ==(GameContext left, GameContext right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GameContext left, GameContext right)
        {
            return !Equals(left, right);
        }
    }
}
