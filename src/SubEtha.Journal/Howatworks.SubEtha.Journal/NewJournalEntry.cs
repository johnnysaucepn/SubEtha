namespace Howatworks.SubEtha.Journal
{
    public class NewJournalEntry
    {
        public NewJournalLogFileInfo Context { get; set; }
        public IJournalEntry JournalEntry { get; }

        public NewJournalEntry(NewJournalLogFileInfo context, IJournalEntry journalEntry)
        {
            Context = context;
            JournalEntry = journalEntry;
        }
    }
}
