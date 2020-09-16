namespace Howatworks.SubEtha.Parser
{
    public class NewJournalLine
    {
        public NewJournalLogFileInfo Context { get; set; }
        public string Line { get; }

        public NewJournalLine(NewJournalLogFileInfo context, string line)
        {
            Context = context;
            Line = line;
        }
    }

}
