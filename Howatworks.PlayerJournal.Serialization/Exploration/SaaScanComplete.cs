using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    [JournalName("SAAScanComplete")]
    public class SaaScanComplete : JournalEntryBase
    {
        public string BodyName { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
        public int ProbesUsed { get; set; }
        public int EfficiencyTarget { get; set; }
    }
}
