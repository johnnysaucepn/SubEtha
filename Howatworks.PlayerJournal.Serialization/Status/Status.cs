using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Status
{
    public class Status : JournalEntryBase
    {
        public StatusFlags Flags { get; set; }
        public List<int> Pips { get; set; }
        public string Firegroup { get; set; } // TODO: check spelling

        public string GuiFocus { get; set; }
        public decimal? Fuel { get; set; }
        public decimal? Cargo { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Heading { get; set; } // TODO: check data type

        public bool HasFlag(StatusFlags flag)
        {
            return (Flags & flag) != 0;
        }
    }
}
