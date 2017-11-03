using System.Diagnostics.CodeAnalysis;

namespace SubEtha.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Body
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
    }
}