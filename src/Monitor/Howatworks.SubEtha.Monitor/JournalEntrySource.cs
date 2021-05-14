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

        public IEnumerable<JournalEntry> GetJournalEntries()
        {
            return _lineSources
                .SelectMany(s => s.GetJournalLines())
                .Select(l =>
                {
                    try
                    {
                        return new JournalEntry(l.Context, _parser.Parse(l.Line));
                    }
                    catch (JournalParseException e)
                    {
                        Debug.WriteLine($"'{l.Context.Filename}': {e.Message}");
                        Debug.WriteLine(e.JournalFragment);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"'{l.Context.Filename}': {e.Message}");
                    }
                    return null;
                })
                .Where(x => x?.Entry != null && x.Entry.Timestamp >= _startTime);
        }
    }
}
