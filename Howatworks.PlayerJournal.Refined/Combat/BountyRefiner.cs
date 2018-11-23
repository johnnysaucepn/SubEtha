using Howatworks.PlayerJournal.Serialization.Combat;

namespace Howatworks.PlayerJournal.Refined.Combat
{
    public class BountyRefiner : IJournalRefiner<Bounty, BountyRefined>
    {
        public BountyRefined Refine(Bounty raw)
        {
            return new BountyRefined();
        }
    }
}