namespace Howatworks.SubEtha.Journal.Exploration
{
    public class Screenshot : JournalEntryBase
    {
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string System { get; set; } // Note: name
        public string Body { get; set; } // Note: name
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Heading { get; set; } // TODO: check data type
    }
}
