using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class SignalSource : IEquatable<SignalSource>
    {
        public LocalisedString Type { get; set; }
        public int? Threat { get; set; }

        public SignalSource()
        {
        }

        public SignalSource(LocalisedString type, int? threat)
        {
            Type = type;
            Threat = threat;
        }

        public bool Equals(SignalSource other)
        {
            if (other == null) return false;

            if (!string.Equals(Type, other.Type)) return false;
            if (!Equals(Threat, other.Threat)) return false;

            return true;
        }
    }
}