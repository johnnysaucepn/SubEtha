using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Combat
{
    [ExcludeFromCodeCoverage]
    public class EscapeInterdiction : JournalEntryBase
    {
        public string Interdictor { get; set; } // NOTE: Commander name
        public bool IsPlayer { get; set; }
    }
}
