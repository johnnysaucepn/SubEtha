namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class Scanned : JournalEntryBase
    {
        public string ScanType { get; set; } // TODO: enum - Cargo, Crime, Cabin, Data or Unknown
    }
}
