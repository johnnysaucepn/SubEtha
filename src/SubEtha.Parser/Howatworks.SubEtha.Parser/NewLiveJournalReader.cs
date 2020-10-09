using System;
using System.IO;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public class NewLiveJournalReader
    {
        public FileInfo File { get; }
        public NewJournalLogFileInfo Context { get; }
        private DateTimeOffset _lastSeen;

        public bool FileExists => File.Exists;

        public NewLiveJournalReader(FileInfo file)
        {
            File = file;

            Context = new NewJournalLogFileInfo(File);
        }

        public NewJournalLine ReadCurrent()
        {
            if (!File.Exists) return null;

            var modTime = File.LastWriteTimeUtc;
            if (modTime <= _lastSeen) return null;

            _lastSeen = modTime;

            using (var file = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var stream = new StreamReader(file))
                {
                    var content = stream.ReadToEnd();
                    //Log.Debug(content);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        return new NewJournalLine(Context, content);
                    }
                }
            }
            return null;
        }
    }
}
