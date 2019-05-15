using Howatworks.SubEtha.Parser;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Howatworks.SubEtha.Journal.Scan
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            // TODO: In tray, we can use the Win32 add-on package that extends SpecialFolder to retrieve SavedGames directly
            var defaultJournalFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "..", "Saved Games", "Frontier Developments", "Elite Dangerous"
            );

            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = defaultJournalFolder,
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddCommandLine(args)
                .Build();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            logRepository.Threshold = Level.Warn;
            BasicConfigurator.Configure(logRepository);
            Log.Info("Started");

            var parser = new JournalParser();

            var basePath = config["JournalFolder"];
            var incrementalPattern = config["JournalPattern"];
            var incrementalFiles = Directory.EnumerateFiles(basePath, incrementalPattern, SearchOption.TopDirectoryOnly);

            foreach (var file in incrementalFiles)
            {
                Log.Info($"Parsing file {file}...");
                var reader = new IncrementalJournalReader(file, parser);
                // Read all entries from all files
                var allEntries = reader.ReadAll(DateTimeOffset.MinValue).ToList();
            }
        }
    }
}
