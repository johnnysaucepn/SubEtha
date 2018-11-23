using System;

namespace Howatworks.PlayerJournal.Parser
{
    internal class JournalFileInfo
    {
        public bool IsValid { get; set; }

        public string Path { get; set; }
        public string GameVersionDiscriminator { get; set; }
        public DateTimeOffset HeaderTimeStamp { get; set; }
        public DateTimeOffset LastEntryTimeStamp { get; set; }

        public JournalFileInfo(string path)
        {
            Path = path;
            IsValid = false;
        }

        public JournalFileInfo(string path, string gameVersion, DateTimeOffset timeStamp)
        {
            IsValid = true;
            Path = path;
            GameVersionDiscriminator = gameVersion;
            HeaderTimeStamp = timeStamp;
            LastEntryTimeStamp = timeStamp;
        }

        public JournalFileInfo(string path, string gameVersion, DateTimeOffset timeStamp, DateTimeOffset lastTimeStamp)
            : this(path, gameVersion, timeStamp)
        {
            LastEntryTimeStamp = lastTimeStamp;

        }


    }
}
