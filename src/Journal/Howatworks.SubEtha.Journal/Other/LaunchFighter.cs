using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class LaunchFighter : JournalEntryBase
    {
        public string Loadout { get; set; } // TODO: enum?
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ID { get; set; } // TODO: check type
        public bool PlayerControlled { get; set; }
    }
}
