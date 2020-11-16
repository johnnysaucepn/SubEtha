using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    // TODO: get sample for PowerplayVoucher
    public class PowerplayVoucher : JournalEntryBase
    {
        public string Power { get; set; }
        public List<string> Systems { get; set; } // TODO: check data type
    }
}
