using Howatworks.SubEtha.Journal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Parser
{
    public class NewLiveJournalReader
    {
        public FileInfo File { get; }
        public NewJournalLogFileInfo Context { get; }
        private DateTimeOffset _lastSeen;

        //public IObservable<NewJournalLine> JournalEntries { get; internal set; }

        public bool FileExists => File.Exists;

        public NewLiveJournalReader(FileInfo file)
        {
            File = file;

            Context = new NewJournalLogFileInfo(File);
            
            //JournalEntries = Observable.Interval(TimeSpan.FromSeconds(5)).SelectMany(_ => ReadNext());
        }

        public IObservable<NewJournalLine> GetObservable()
        {
            return Observable.Create<NewJournalLine>(observer =>
            {
                foreach (var y in ReadNext())
                {
                    observer.OnNext(y);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IEnumerable<NewJournalLine> ReadNext()
        {
            // TODO: handle non-existent files

            var modTime = File.LastWriteTimeUtc;
            if (modTime <= _lastSeen) yield break;

            _lastSeen = modTime;

            using (var file = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        var content = stream.ReadToEnd();
                        //Log.Debug(content);
                        if (!string.IsNullOrWhiteSpace(content)) yield return new NewJournalLine(Context, content);
                    }
                }
            }
        }
    }
}
