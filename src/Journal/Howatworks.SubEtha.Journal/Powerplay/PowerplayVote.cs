using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    [ExcludeFromCodeCoverage]
    public class PowerplayVote : JournalEntryBase
    {
        public string Power { get; set; }
        public int Votes { get; set; } // TODO: assuming a total of some kind?
        public string System { get; set; }

    }
}
