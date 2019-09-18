namespace Howatworks.SubEtha.Journal.Travel
{
    public class Touchdown : JournalEntryBase
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string NearestDestination { get; set; }
        public string NearestDestination_Localised { get; set; }
        public bool PlayerControlled { get; set; }
    }
}
