using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class ScanOrganic : JournalEntryBase
    {
        public string ScanType { get; set; } // TODO: enum Log, Sample, Analyse
        public string Genus { get; set; } // TODO: check if localised
        public string Species { get; set; } // TODO: check type, check if localised
        public long SystemAddress { get; set; }
        public int Body { get; set; } // TODO: check type, assume this corresponds to BodyID?
    }
}