using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class LaunchDrone : JournalEntryBase
    {
        public string Type { get; set; } // TODO: enum ("Hatchbreaker", "FuelTransfer", "Collection", "Prospector", "Repair", "Research", "Decontamination")
    }
}
