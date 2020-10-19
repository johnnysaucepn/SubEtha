namespace Howatworks.SubEtha.Journal
{
    public class JournalLine
    {
        public JournalLogFileInfo Context { get; set; }
        public string Line { get; }

        public JournalLine(JournalLogFileInfo context, string line)
        {
            Context = context;
            Line = line;
        }
    }

}
