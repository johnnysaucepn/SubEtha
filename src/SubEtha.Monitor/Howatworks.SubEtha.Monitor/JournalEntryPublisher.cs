using System;
using System.Reactive.Subjects;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalEntryPublisher
    {
        private readonly INewJournalEntrySource _source;

        private readonly Subject<NewJournalEntry> _subject = new Subject<NewJournalEntry>();

        public JournalEntryPublisher(INewJournalEntrySource source)
        {
            _source = source;
        }

        public void Poll()
        {
            var allEntries = _source.GetJournalEntries();
            foreach (var entry in allEntries)
                _subject.OnNext(entry);
        }

        public IObservable<NewJournalEntry> GetObservable()
        {
            return _subject;
        }

    }
}
