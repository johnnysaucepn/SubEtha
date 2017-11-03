using System;

namespace Howatworks.PlayerJournal.Parser
{
    internal class JournalFileInfo
    {
        public bool IsValid { get; set; }

        public string Path { get; set; }
        public string GameVersion { get; set; }
        public DateTime HeaderTimeStamp { get; set; }
        public DateTime LastEntryTimeStamp { get; set; }

        public JournalFileInfo(string path)
        {
            Path = path;
            IsValid = false;
        }

        public JournalFileInfo(string path, string gameVersion, DateTime timeStamp)
        {
            IsValid = true;
            Path = path;
            GameVersion = gameVersion;
            HeaderTimeStamp = timeStamp;
            LastEntryTimeStamp = timeStamp;
        }

        public JournalFileInfo(string path, string gameVersion, DateTime timeStamp, DateTime lastTimeStamp)
            : this(path, gameVersion, timeStamp)
        {
            LastEntryTimeStamp = lastTimeStamp;

        }


    }
}
