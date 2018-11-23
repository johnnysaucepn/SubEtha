namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class CommunityGoalReward : JournalEntryBase
    {
        public string Name { get; set; }
        public string System { get; set; }
        public int Reward { get; set; }
    }
}
