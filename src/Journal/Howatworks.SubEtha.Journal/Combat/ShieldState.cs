using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Combat
{
    [ExcludeFromCodeCoverage]
    public class ShieldState : JournalEntryBase
    {
        public bool ShieldsUp { get; set; } // Note: sample suggests bool, docs say int
    }
}
