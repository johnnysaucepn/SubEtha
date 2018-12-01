namespace Howatworks.PlayerJournal.Serialization.Status
{
    public class Status : JournalEntryBase
    {
        public int Flags
        {
            get;
            set;

        }
        public int[] Pips { get; set; }
        public string Firegroup { get; set; }

        public string GuiFocus { get; set; }
        public decimal? Fuel { get; set; }
        public decimal? Cargo { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Heading { get; set; }
    }
}
