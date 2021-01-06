using log4net;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Monitor
{
    /// <summary>
    /// The standard FileSystemWatcher class has several limitations, such as case-sensitivity (even on Windows).
    /// Also, for our needs, we don't generally care about the mechanics of renaming files, so abstract them away and build in useful logging.
    /// </summary>
    public class CustomFileWatcher
    {
        public IObservable<string> ChangedFiles { get; }
        public IObservable<string> CreatedFiles { get; }
        public IObservable<string> DeletedFiles { get; }
        public IObservable<Exception> Errors { get; }
        public IObservable<(string oldPath, string newPath)> RenamedFiles { get; }
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomFileWatcher));

        private readonly FileSystemWatcher _journalWatcher;

        public string Folder { get; }
        public string Pattern { get; }
        private readonly Matcher _matcher;

        public CustomFileWatcher(string folder, string pattern)
        {
            Folder = folder;
            Pattern = pattern;
            _matcher = new Matcher(StringComparison.InvariantCultureIgnoreCase).AddInclude(Pattern);
            _journalWatcher = new FileSystemWatcher(Folder)
            {
                EnableRaisingEvents = false,
                NotifyFilter = NotifyFilters.LastWrite
            };

            CreatedFiles =
                GetCreatedFiles()
                // Treat renamed files as ones that have been created in one place and deleted in another
                .Merge(GetIncomingRenamedFiles())
                // Include files that exist at the time the watcher is created
                .Merge(GetExistingFiles())
                .Do(x => Log.Warn($"Seen new file '{x}'"));

            ChangedFiles =
                GetModifiedFiles()
                .Do(x => Log.Warn($"Seen modified file '{x}'"));

            DeletedFiles =
                GetDeletedFiles()
                // Treat renamed files as ones that have been created in one place and deleted in another
                .Merge(GetOutgoingRenamedFiles())
                .Do(x => Log.Warn($"Seen deletion of file '{x}'"));

            Errors = GetErrors()
                   .Do(x => Log.Error("Seen file error", x));
        }

        private IObservable<string> GetCreatedFiles() =>
            Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => _journalWatcher.Created += h,
                h => _journalWatcher.Created -= h
            )
            .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
            .Select(x => x.EventArgs.FullPath);

        private IObservable<string> GetModifiedFiles() =>
            Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => _journalWatcher.Changed += h,
                h => _journalWatcher.Changed -= h
            )
            .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
            .Select(x => x.EventArgs.FullPath);

        private IObservable<string> GetDeletedFiles() =>
            Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => _journalWatcher.Deleted += h,
                h => _journalWatcher.Deleted -= h
            )
            .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
            .Select(x => x.EventArgs.FullPath);

        private IObservable<string> GetExistingFiles()
        {
            return Observable.Create<string>(o =>
            {
                foreach (var file in Directory.EnumerateFiles(Folder)
                    .Select(Path.GetFileName)
                    .Where(f => _matcher.Match(f).HasMatches))
                {
                    o.OnNext(file);
                }
                o.OnCompleted();
                return Disposable.Empty;
            });
        }

        private IObservable<string> GetOutgoingRenamedFiles() =>
            Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                h => _journalWatcher.Renamed += h,
                h => _journalWatcher.Renamed -= h
            )
            .Where(x => _matcher.Match(x.EventArgs.OldName).HasMatches)
            .Select(x => x.EventArgs.OldFullPath);

        private IObservable<string> GetIncomingRenamedFiles() =>
            Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                h => _journalWatcher.Renamed += h,
                h => _journalWatcher.Renamed -= h
            )
            .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
            .Select(x => x.EventArgs.FullPath);

        private IObservable<Exception> GetErrors() =>
            Observable.FromEventPattern<ErrorEventHandler, ErrorEventArgs>(
                h => _journalWatcher.Error += h,
                h => _journalWatcher.Error -= h
            )
            .Select(x => x.EventArgs.GetException());

        public void Start()
        {
            _journalWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _journalWatcher.EnableRaisingEvents = false;
        }
    }
}
