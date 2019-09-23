using Howatworks.SubEtha.Parser;
using log4net;
using log4net.Config;
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
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            Log.Info("Started");

            var parser = new JournalParser(true);  // Use strict parsing

            var basePath = config["JournalFolder"];

            var incrementalPattern = config["JournalPattern"];
            var incrementalFiles = Directory.EnumerateFiles(basePath, incrementalPattern, SearchOption.TopDirectoryOnly);
            foreach (var file in incrementalFiles)
            {
                Log.Info($"Parsing file '{file}'...");
                var reader = new IncrementalJournalReader(file, parser);
                // Read all entries from all files
                var _ = reader.ReadAll(DateTimeOffset.MinValue).ToList();
            }

            var realTimeFiles = config["RealTimeFilenames"].Split(';').Select(x => Path.Combine(basePath, x.Trim()));
            foreach (var file in realTimeFiles)
            {
                Log.Info($"Parsing file '{file}'...");
                var reader = new RealTimeJournalReader(file, parser);
                // Read all entries from all files
                var _ = reader.ReadAll(DateTimeOffset.MinValue).ToList();
            }
        }
    }
}
