using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    [JournalName("DockSRV")]
    public class DockSrv : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ID { get; set; } // TODO: check type
    }
}
