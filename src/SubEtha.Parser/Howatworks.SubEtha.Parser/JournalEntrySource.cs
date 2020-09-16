using Howatworks.SubEtha.Journal;
using log4net;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace Howatworks.SubEtha.Parser
{
    public class JournalEntrySource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalEntrySource));
        private readonly IJournalParser _parser;

        public JournalEntrySource(IJournalParser parser)
        {
            _parser = parser;
        }

        public IObservable<(NewJournalLogFileInfo Context, IJournalEntry JournalEntry)> GetJournalEntries(IObservable<NewJournalLine> observable)
        {
            return observable.Select(l =>
            {
                try
                {
                    return (l.Context, JournalEntry: _parser.Parse(l.Line));
                }
                catch (JournalParseException e)
                {
                    Log.Error($"'{l.Context.File.FullName}': {e.JournalFragment}");
                    Log.Error(e.Message);
                }
                catch (UnrecognizedJournalException e)
                {
                    Log.Warn($"'{l.Context.File.FullName}': {e.JournalFragment}");
                    Log.Warn(e.Message);
                }
                return (null, null);

            })
            .Where(x => x.JournalEntry != null);
        }

    }
}
