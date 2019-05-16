using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    public class CrewLaunchFighter : JournalEntryBase
    {
        public string Crew { get; set; } // TODO: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ID { get; set; } // TODO: check type
    }
}
