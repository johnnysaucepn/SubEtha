using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Station : IEquatable<Station>
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Station()
        {
        }

        public Station(string name, string type)
        {
            Name = name;
            Type = type;
            // TODO: economics and factions
        }

        public static Station Create(string name, string type)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            return new Station(name, type ?? string.Empty);
        }

        public bool Equals(Station other)
        {
            if (other == null) return false;

            if (!string.Equals(Name, other.Name)) return false;
            if (!string.Equals(Type, other.Type)) return false;

            return true;
        }
    }
}