using System;
using System.Reactive.Linq;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public static class JournalRxExtensions
    {
        public static IObservable<TResult> OfJournalType<TResult>(this IObservable<NewJournalEntry> source)
        {
            return source.Select(x => x.JournalEntry).OfType<TResult>();
        }

        public static IObservable<NewJournalLogFileInfo> SelectContext<TResult>(this IObservable<NewJournalEntry> source)
        {
            return source.Select(x => x.Context);
        }

        public static IObservable<(NewJournalLogFileInfo Context, TResult JournalEntry)> SelectJournalContext<TResult>(this IObservable<NewJournalEntry> source) where TResult : class
        {
            return source.Where(t => t.JournalEntry is TResult).Select(x => (x.Context, x.JournalEntry as TResult));
        }
    }
}
