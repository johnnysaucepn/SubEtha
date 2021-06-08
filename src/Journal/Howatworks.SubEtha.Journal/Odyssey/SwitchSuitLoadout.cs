using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class SwitchSuitLoadout : JournalEntryBase
    {
        // WARN: this matches the contents of this event in the log, but does not match the docs
        // SuitLoadout event is undocumented as per doc v31, but seems to be the replacement for this

        [JournalName("LoadoutID")]
        public long LoadoutId { get; set; }
        public string LoadoutName { get; set; }
    }
}