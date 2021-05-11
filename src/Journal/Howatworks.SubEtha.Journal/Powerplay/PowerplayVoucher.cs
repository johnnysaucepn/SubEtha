using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    // TODO: get sample for PowerplayVoucher
    [ExcludeFromCodeCoverage]
    public class PowerplayVoucher : JournalEntryBase
    {
        public string Power { get; set; }
        public List<string> Systems { get; set; } // TODO: check data type
    }
}
