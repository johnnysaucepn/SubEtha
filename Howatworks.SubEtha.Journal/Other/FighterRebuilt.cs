using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    public class FighterRebuilt : JournalEntryBase
    {
        public string Loadout { get; set; } // TODO: enum?
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ID { get; set; } // TODO: check type
    }
}
