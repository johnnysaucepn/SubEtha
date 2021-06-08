using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class RenameSuitLoadout : JournalEntryBase
    {
        [JournalName("SuitID")]
        public long SuitId { get; set; } // TODO: check this
        public string SuitName { get; set; } // TODO: check this - localised?
        [JournalName("LoadoutID")]
        public long LoadoutId { get; set; }
        public string LoadoutName { get; set; }
    }
}