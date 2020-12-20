using System.IO;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalWatchActivity
    {
        public JournalWatchAction Action { get; }
        public FileInfo File { get; }

        public JournalWatchActivity(JournalWatchAction fileEvent, FileInfo file)
        {
            Action = fileEvent;
            File = file;
        }
    }
}