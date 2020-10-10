using System;
using System.IO;

namespace Howatworks.SubEtha.Journal
{
    public class NewJournalLogFileInfo
    {
        public bool IsValid { get; }

        public string Filename { get; }
        public string GameVersionDiscriminator { get; }
        public DateTimeOffset HeaderTimestamp { get; }
        public DateTimeOffset LastEntry { get; }

        public NewJournalLogFileInfo(FileInfo file)
        {
            Filename = file.Name;
            IsValid = false;
        }

        public NewJournalLogFileInfo(FileInfo file, string gameVersion, DateTimeOffset timestamp, DateTimeOffset lastEntry)
        {
            IsValid = true;
            Filename = file.Name;
            GameVersionDiscriminator = gameVersion;
            HeaderTimestamp = timestamp;
            LastEntry = lastEntry;
        }

    }
}
