namespace Howatworks.PlayerJournal.Serialization.Combat
{
    public class EscapeInterdiction : JournalEntryBase
    {
        public string Interdictor { get; set; } // NOTE: Commander name
        public bool IsPlayer { get; set; }
    }
}
