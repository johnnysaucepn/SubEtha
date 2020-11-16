using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class Promotion : JournalEntryBase
    {
        // TODO: enumerate these?
        public int Combat { get; set; }
        public int Trade { get; set; }
        public int Explore { get; set; }
        public int CQC { get; set; }
        public int Federation { get; set; }
        public int Empire { get; set; }
    }
}
