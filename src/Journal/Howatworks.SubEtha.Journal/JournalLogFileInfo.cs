using System;
using System.IO;

namespace Howatworks.SubEtha.Journal
{
    public class JournalLogFileInfo
    {
        public bool IsValid { get; }

        public string Filename { get; }
        public string GameVersionDiscriminator { get; }
        public DateTimeOffset HeaderTimestamp { get; }
        public DateTimeOffset LastEntry { get; }

        public JournalLogFileInfo(FileInfo file)
        {
            Filename = file.Name;
            IsValid = false;
        }

        public JournalLogFileInfo(FileInfo file, string gameVersion, DateTimeOffset timestamp, DateTimeOffset lastEntry)
        {
            IsValid = true;
            Filename = file.Name;
            GameVersionDiscriminator = gameVersion;
            HeaderTimestamp = timestamp;
            LastEntry = lastEntry;
        }
    }
}
