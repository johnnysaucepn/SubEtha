namespace Howatworks.SubEtha.Journal.Combat
{
    public class EscapeInterdiction : JournalEntryBase
    {
        public string Interdictor { get; set; } // NOTE: Commander name
        public bool IsPlayer { get; set; }
    }
}
