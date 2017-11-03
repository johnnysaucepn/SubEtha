using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Powerplay
{
    public class PowerplayVoucher : JournalEntryBase
    {
        public string Power { get; set; }
        // TODO: confirm type
        public Dictionary<string, int> Systems { get; set; }
    }
}
