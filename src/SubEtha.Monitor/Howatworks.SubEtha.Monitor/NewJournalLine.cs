using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
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
