using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class RebootRepair : JournalEntryBase
    {
        public List<string> Modules { get; set; }
    }
}
