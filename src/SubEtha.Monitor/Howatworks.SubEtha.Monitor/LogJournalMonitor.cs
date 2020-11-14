﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.SubEtha.Monitor
{
    public class LogJournalMonitor : IJournalLineSource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogJournalMonitor));

        private readonly CustomFileWatcher _logFileWatcher;
        private readonly SortedList<DateTimeOffset, LogJournalReader> _logReaders;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        public LogJournalMonitor(IConfiguration config, IJournalReaderFactory readerFactory, DateTimeOffset? startTime = null)
        {
            startTime = startTime ?? DateTimeOffset.MinValue;
            var folder = config["JournalFolder"];
            var logPattern = config["JournalPattern"];

            _logReaders = new SortedList<DateTimeOffset, LogJournalReader>();

            _logFileWatcher = new CustomFileWatcher(folder, logPattern);

            _logFileWatcher.CreatedFiles.Subscribe(f =>
            {
                var file = new FileInfo(Path.Combine(folder, f));
                var newReader = readerFactory.CreateLogJournalReader(file);

                // As long as the files has some entries that happened after our desired start time, it's a valid source
                if (newReader.Context.LastEntry >= startTime)
                {
                    _logReaders.Add(newReader.Context.HeaderTimestamp, newReader);
                    JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(file));
                }
            });

            _logFileWatcher.DeletedFiles.Subscribe(f =>
            {
                foreach (var reader in _logReaders.Where(x => x.Value.File.Name.Equals(f)))
                {
                    _logReaders.Remove(reader.Key);
                    JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(reader.Value.File));
                }
            });
            _logFileWatcher.Start();
        }

        public IEnumerable<JournalLine> GetJournalLines()
        {
            return _logReaders
                .SelectMany(x => x.Value.ReadLines())
                .Where(l => l != null);
        }
    }
}