namespace Howatworks.SubEtha.Journal.Combat
{
    // TODO: no sample
    public class HullDamage : JournalEntryBase
    {
        public decimal Health { get; set; }
        public bool PlayerPilot { get; set; }
        public bool Fighter { get; set; }
    }
}
