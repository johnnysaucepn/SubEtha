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

        public JournalLogFileInfo(FileInfo file)
        {
            IsValid = false;
            Filename = file.Name;
            GameVersionDiscriminator = string.Empty;
            HeaderTimestamp = DateTimeOffset.MinValue;
        }

        public JournalLogFileInfo(FileInfo file, string gameVersion, DateTimeOffset timestamp)
        {
            IsValid = true;
            Filename = file.Name;
            GameVersionDiscriminator = gameVersion;
            HeaderTimestamp = timestamp;
        }
    }
}
