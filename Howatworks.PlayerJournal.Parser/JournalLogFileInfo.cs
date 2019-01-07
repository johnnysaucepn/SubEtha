using System;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalLogFileInfo
    {
        public bool IsValid { get; set; }

        public string Path { get; set; }
        public string GameVersionDiscriminator { get; set; }
        public DateTimeOffset HeaderTimeStamp { get; set; }

        public JournalLogFileInfo(string path)
        {
            Path = path;
            IsValid = false;
        }

        public JournalLogFileInfo(string path, string gameVersion, DateTimeOffset timeStamp)
        {
            IsValid = true;
            Path = path;
            GameVersionDiscriminator = gameVersion;
            HeaderTimeStamp = timeStamp;
        }

    }
}
