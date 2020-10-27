using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class StarSystem : IEquatable<StarSystem>
    {
        public string Name { get; set; }
        public decimal[] Coords { get; set; }

        public StarSystem()
        {
        }

        public StarSystem(string name, IEnumerable<decimal> coords)
        {
            Name = name;
            Coords = coords.Concat(Enumerable.Repeat(0m, 3)).Take(3).ToArray();
        }

        public bool Equals(StarSystem other)
        {
            if (other == null) return false;

            if (!string.Equals(Name, other.Name)) return false;
            if (!SimpleComparer.SequenceEquals(Coords, other.Coords)) return false;

            return true;
        }

    }
}