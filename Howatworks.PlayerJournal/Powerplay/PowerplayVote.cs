using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Powerplay
{
    public class PowerplayVote : JournalEntryBase
    {
        public string Power { get; set; }
        // TODO: don't know what this is
        public List<string> Votes { get; set; }
        public string System { get; set; }

    }
}
