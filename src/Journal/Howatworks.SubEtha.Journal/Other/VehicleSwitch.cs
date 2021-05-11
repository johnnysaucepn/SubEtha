using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class VehicleSwitch : JournalEntryBase
    {
        public string To { get; set; } // TODO: enum?  Mothership/Fighter
    }
}
