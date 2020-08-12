using log4net;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.IO;
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

        private readonly Matcher _matcher;

        public CustomFileWatcher(string folder, string pattern)
        {
            _matcher = new Matcher(StringComparison.InvariantCultureIgnoreCase).AddInclude(pattern);
            _journalWatcher = new FileSystemWatcher(folder)
            {
                EnableRaisingEvents = false
            };

            CreatedFiles = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    h => _journalWatcher.Created += h,
                    h => _journalWatcher.Created -= h
                )
                .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
                .Select(x => x.EventArgs.FullPath)

                // Treat renamed files as ones that have been created in one place and deleted in another
                .Merge(
                    Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                        h => _journalWatcher.Renamed += h,
                        h => _journalWatcher.Renamed -= h
                    )
                    .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
                    .Select(x => x.EventArgs.FullPath)
                )

                .Do(x => Log.Debug($"Seen new file '{x}'"));

            ChangedFiles = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    h => _journalWatcher.Changed += h,
                    h => _journalWatcher.Changed -= h
                )
                .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
                .Select(x => x.EventArgs.FullPath)
                .Do(x => Log.Debug($"Seen modified file '{x}'"));

            DeletedFiles = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    h => _journalWatcher.Deleted += h,
                    h => _journalWatcher.Deleted -= h
                )
                .Where(x => _matcher.Match(x.EventArgs.Name).HasMatches)
                .Select(x => x.EventArgs.FullPath)

                // Treat renamed files as ones that have been created in one place and deleted in another
                .Merge(
                    Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                        h => _journalWatcher.Renamed += h,
                        h => _journalWatcher.Renamed -= h
                    )
                    .Where(x => _matcher.Match(x.EventArgs.OldName).HasMatches)
                    .Select(x => x.EventArgs.OldFullPath)
                )

                    .Do(x => Log.Debug($"Seen deletion of file '{x}'"));

            Errors = Observable.FromEventPattern<ErrorEventHandler, ErrorEventArgs>(
                        h => _journalWatcher.Error += h,
                        h => _journalWatcher.Error -= h
                   )
                   .Select(x => x.EventArgs.GetException())
                   .Do(x => Log.Error("Seen file error", x));
        }

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
