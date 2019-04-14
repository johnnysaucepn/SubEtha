using System;

namespace Howatworks.SubEtha.Monitor
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