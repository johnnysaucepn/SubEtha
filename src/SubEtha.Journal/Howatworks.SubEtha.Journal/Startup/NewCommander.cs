using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Startup
{
    [ExcludeFromCodeCoverage]
    public class NewCommander : JournalEntryBase
    {
        public string Name { get; set; } // NOTE: Commander name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string FID { get; set; }     // NOTE: Player ID
        public string Package { get; set; }
    }
}
