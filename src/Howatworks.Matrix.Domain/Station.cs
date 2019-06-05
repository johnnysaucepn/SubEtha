using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Station
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Station()
        {
        }

        private Station(string name, string type)
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
    }
}