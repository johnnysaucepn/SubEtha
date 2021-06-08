using System;
using System.Collections.Generic;
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

        public IEnumerable<JournalEntry> GetJournalEntries()
        {
            return _lineSources
                .SelectMany(s => s.GetJournalLines())
                .Select(l => _parser.Parse(l))
                .Where(x => EntryIsValid(x) && EntrySince(x));
        }

        private bool EntrySince(JournalEntry x)
        {
            return x.Entry?.Timestamp >= _startTime;
        }

        private static bool EntryIsValid(JournalEntry x)
        {
            return x.Entry != null;
        }
    }
}
