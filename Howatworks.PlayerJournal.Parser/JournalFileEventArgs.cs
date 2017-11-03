using System;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalFileEventArgs : EventArgs
    {
        public JournalFileEventArgs(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}