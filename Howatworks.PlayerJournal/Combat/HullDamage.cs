namespace Howatworks.PlayerJournal.Combat
{
    public class HullDamage : JournalEntryBase
    {
        public decimal Health { get; set; }
        public bool PlayerPilot { get; set; }
        public bool Fighter { get; set; }
    }
}
