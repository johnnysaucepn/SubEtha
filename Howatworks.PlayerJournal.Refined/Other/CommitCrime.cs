namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class CommitCrime : JournalEntryBase
    {
        // TODO: enumerate?
        public string CrimeType { get; set; }
        public string Faction { get; set; }

        #region Optional
        public string Victim { get; set; }
        public int Fine { get; set; }
        public int Bounty { get; set; }
        #endregion
    }
}
