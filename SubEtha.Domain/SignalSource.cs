using System.Diagnostics.CodeAnalysis;

namespace SubEtha.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class SignalSource
    {
        public LocalisedString Type { get; set; }
        public int Threat { get; set; }

        public SignalSource()
        {
        }

        public SignalSource(LocalisedString type, int threat)
        {
            Type = type;
            Threat = threat;
        }
    }
}