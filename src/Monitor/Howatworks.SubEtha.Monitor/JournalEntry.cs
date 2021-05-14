namespace Howatworks.SubEtha.Journal
{
    public class JournalEntry
    {
        public JournalLogFileInfo Context { get; set; }
        public IJournalEntry Entry { get; }

        public JournalEntry(JournalLogFileInfo context, IJournalEntry journalEntry)
        {
            Context = context;
            Entry = journalEntry;
        }
    }
}
