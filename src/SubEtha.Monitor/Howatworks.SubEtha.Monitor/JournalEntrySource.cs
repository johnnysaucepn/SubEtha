using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalEntrySource : INewJournalEntrySource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalEntrySource));
        private readonly IJournalParser _parser;
        private readonly DateTimeOffset _startTime;
        private readonly INewJournalLineSource[] _lineSources;

        public JournalEntrySource(IJournalParser parser, DateTimeOffset startTime, params INewJournalLineSource[] lineSources)
        {
            _lineSources = lineSources;
            _parser = parser;
            _startTime = startTime;
        }

        public IEnumerable<NewJournalEntry> GetJournalEntries()
        {
            return _lineSources
                .SelectMany(s => s.GetJournalLines())
                .Select(l =>
                {
                    try
                    {
                        return new NewJournalEntry(l.Context, _parser.Parse(l.Line));
                    }
                    catch (JournalParseException e)
                    {
                        Log.Error($"'{l.Context.Filename}': {e.JournalFragment}");
                        Log.Error(e.Message);
                    }
                    catch (UnrecognizedJournalException e)
                    {
                        Log.Warn($"'{l.Context.Filename}': {e.JournalFragment}");
                        Log.Warn(e.Message);
                    }
                    return null;
                })
                .Where(x => x != null && x.JournalEntry != null && x.JournalEntry.Timestamp >= _startTime);
        }

    }
}
