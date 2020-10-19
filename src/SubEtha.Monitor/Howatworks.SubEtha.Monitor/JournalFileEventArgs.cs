using System;
using System.IO;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalFileEventArgs : EventArgs
    {
        public FileInfo File { get; }

        public JournalFileEventArgs(FileInfo file)
        {
            File = file;
        }
    }
}