using System;
using System.IO;

namespace Howatworks.SubEtha.Parser
{
    public class NewJournalLogFileInfo
    {
        public bool IsValid { get; set; }

        public FileInfo File { get; set; }
        public string GameVersionDiscriminator { get; set; }
        public DateTimeOffset HeaderTimestamp { get; set; }

        public NewJournalLogFileInfo(FileInfo file)
        {
            File = file;
            IsValid = false;
        }

        public NewJournalLogFileInfo(FileInfo file, string gameVersion, DateTimeOffset timestamp)
        {
            IsValid = true;
            File = file;
            GameVersionDiscriminator = gameVersion;
            HeaderTimestamp = timestamp;
        }

    }
}
