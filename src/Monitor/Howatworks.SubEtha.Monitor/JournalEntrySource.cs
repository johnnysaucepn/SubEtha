using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalEntrySource : IJournalEntrySource
    {
        private readonly IJournalParser _parser;
        private readonly DateTimeOffset _startTime;
        private readonly IJournalLineSource[] _lineSources;

        public JournalEntrySource(IJournalParser parser, DateTimeOffset startTime, params IJournalLineSource[] lineSources)
        {
            _lineSources = lineSources;
            _parser = parser;
            _startTime = startTime;
        }

        public IEnumerable<JournalResult<JournalEntry>> GetJournalEntries()
        {
            return _lineSources
                .SelectMany(s => s.GetJournalLines())
                .Select(lineResult =>
                {
                    if (lineResult.IsSuccess)
                    {
                        var line = lineResult.Value;
                        var entryResult = _parser.Parse(line.Line);
                        if (entryResult.IsSuccess)
                        {
                            return JournalResult.Success(new JournalEntry(line.Context, entryResult.Value));
                        }
                        else
                        {
                            return JournalResult.Failure<JournalEntry>($"{entryResult.Message} in '{line.Context.Filename}': {line.Line}");
                        }
                    }
                    else
                    {
                        return JournalResult.Failure<JournalEntry>(lineResult.Message);
                    }
                })
                // Exclude items that would be successful if they weren't out of date
                .Where(x => x.IsFailure || x.Value.Entry.Timestamp >= _startTime);
        }
    }
}
