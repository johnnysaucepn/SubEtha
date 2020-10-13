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
        private readonly IJournalParser _parser;

        public bool FileExists => File.Exists;

        public NewLiveJournalReader(FileInfo file, IJournalParser parser)
        {
            File = file;
            _parser = parser;

            Context = new NewJournalLogFileInfo(File);
        }

        public NewJournalLine ReadCurrent()
        {
            if (!File.Exists) return null;

            using (var file = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var stream = new StreamReader(file))
                {
                    var content = stream.ReadToEnd();
                    var (_, timestamp) = _parser.ParseCommonProperties(content);
                    //Log.Debug(content);
                    if (timestamp > _lastSeen)
                    {
                        _lastSeen = timestamp;
                        return new NewJournalLine(Context, content);
                    }
                }
            }
            return null;
        }
    }
}
