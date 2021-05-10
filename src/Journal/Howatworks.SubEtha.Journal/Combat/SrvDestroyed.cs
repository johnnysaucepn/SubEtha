using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Combat
{
    [ExcludeFromCodeCoverage]
    [JournalName("SRVDestroyed")]
    public class SrvDestroyed : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ID { get; set; } // TODO: check type
    }
}
