namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class CommitCrime : JournalEntryBase
    {
        // TODO: enumerate?
        public string CrimeType { get; set; }
        public string Faction { get; set; }

        #region Optional
        public string Victim { get; set; }
        public long Fine { get; set; }
        public long Bounty { get; set; }
        #endregion
    }
}
