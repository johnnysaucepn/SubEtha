using System;
using System.Diagnostics;
using System.IO;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public class LiveJournalReader : IJournalReader
    {
        public FileInfo File { get; }
        public JournalLogFileInfo Context { get; }

        // Track the timestamp on the last item we saw, so we only report changes
        private DateTimeOffset _lastSeen;
        private readonly IJournalParser _parser;

        public LiveJournalReader(FileInfo file, IJournalParser parser)
        {
            File = file;
            _parser = parser;

            Context = new JournalLogFileInfo(File);
        }

        public JournalResult<JournalLine> ReadCurrent()
        {
            if (!File.Exists)
            {
                return JournalResult.Failure<JournalLine>($"File '{File.Name}' not found");
            }

            using (var file = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var stream = new StreamReader(file))
                {
                    var content = stream.ReadToEnd();

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return JournalResult.Failure<JournalLine>($"File '{File.Name}' contains no content");
                    }

                    try
                    {
                        // Skip any out-of-order entries
                        var (_, timestamp) = _parser.ParseCommonProperties(content);
                        if (timestamp > _lastSeen)
                        {
                            _lastSeen = timestamp;
                            return JournalResult.Success(new JournalLine(Context, content));
                        }
                    }
                    catch (JournalParseException e)
                    {
                        return JournalResult.Failure<JournalLine>($"Unable to read content of {File.Name}: {e}");
                    }
                }
            }
            return JournalResult.Failure<JournalLine>($"File '{File.Name}' contains no new content");
        }
    }
}

