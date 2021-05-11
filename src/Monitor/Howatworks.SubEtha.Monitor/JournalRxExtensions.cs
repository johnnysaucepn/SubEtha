using System;
using System.Reactive.Linq;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public static class JournalRxExtensions
    {
        public static IObservable<TResult> OfJournalType<TResult>(this IObservable<JournalEntry> source)
        {
            return source.Select(x => x.Entry).OfType<TResult>();
        }

        public static IObservable<JournalLogFileInfo> SelectContext(this IObservable<JournalEntry> source)
        {
            return source.Select(x => x.Context);
        }

        public static IObservable<(JournalLogFileInfo Context, TResult JournalEntry)> SelectJournalContext<TResult>(this IObservable<JournalEntry> source) where TResult : class
        {
            return source.Where(t => t.Entry is TResult).Select(x => (x.Context, x.Entry as TResult));
        }
    }
}
