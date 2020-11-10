using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Body : IEquatable<Body>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Docked { get; set; }

        public Body()
        {
        }

        public Body(string name, string type)
        {
            Name = name;
            Type = type;
            Docked = false;
        }

        public Body(string name, string type, bool docked)
            : this(name, type)
        {
            Docked = docked;
        }

        public bool Equals(Body other)
        {
            if (!string.Equals(Name, other.Name)) return false;
            if (!string.Equals(Type, other.Type)) return false;
            if (Docked != other.Docked) return false;

            return true;
        }
    }
}