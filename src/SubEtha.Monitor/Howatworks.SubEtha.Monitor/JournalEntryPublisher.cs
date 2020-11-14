using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalEntryPublisher
    {
        private readonly IJournalEntrySource _source;

        private readonly Subject<JournalEntry> _subject = new Subject<JournalEntry>();
        public IObservable<JournalEntry> Observable => _subject.AsObservable();

        public JournalEntryPublisher(IJournalEntrySource source)
        {
            _source = source;
        }

        public void Poll()
        {
            var allEntries = _source.GetJournalEntries();
            foreach (var entry in allEntries)
                _subject.OnNext(entry);
        }
    }
}
