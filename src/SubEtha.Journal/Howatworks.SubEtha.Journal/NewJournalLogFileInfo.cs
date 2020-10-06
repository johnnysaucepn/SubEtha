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

        public NewJournalLogFileInfo(FileInfo file)
        {
            Filename = file.Name;
            IsValid = false;
        }

        public NewJournalLogFileInfo(FileInfo file, string gameVersion, DateTimeOffset timestamp)
        {
            IsValid = true;
            Filename = file.Name;
            GameVersionDiscriminator = gameVersion;
            HeaderTimestamp = timestamp;
        }

    }
}
