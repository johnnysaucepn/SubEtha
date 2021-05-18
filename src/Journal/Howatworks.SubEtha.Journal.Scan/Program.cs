using Howatworks.SubEtha.Parser;
using Howatworks.SubEtha.Parser.Logging;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using PInvoke;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Howatworks.SubEtha.Journal.Scan
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            var savedGames = Shell32.SHGetKnownFolderPath(Shell32.KNOWNFOLDERID.FOLDERID_SavedGames);
            var defaultJournalFolder = Path.Combine(
                savedGames, "Frontier Developments", "Elite Dangerous"
            );

            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = defaultJournalFolder,
                ["LogPattern"] = "Journal.*.log",
                ["LiveFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json;NavRoute.json;ModulesInfo.json;Cargo.json"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddCommandLine(args)
                .Build();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            Log.Info("Started");

            // Subscribe to loggable events we might be interested in
            SubEthaLog.LogEvent += SubEthaLog_LogEvent;

            var parser = new JournalParser(true);  // Use strict parsing
            var basePath = config["JournalFolder"];

            // Instead of using the full monitor, just enumerate files and parse them one by one
            var logPattern = config["LogPattern"];
            var logFiles = Directory.EnumerateFiles(basePath, logPattern, SearchOption.TopDirectoryOnly);

            foreach (var file in logFiles)
            {
                Log.Info($"Checking file '{file}'...");
                var reader = new LogJournalReader(new FileInfo(file), parser);
                // Read all entries from all files
                reader.ReadLines().ToList().ForEach(l => Validate(l.Line, parser));
            }

            // Get all the live files (that get replaced as they are updated)
            var liveFiles = config["LiveFilenames"].Split(';').Select(x => Path.Combine(basePath, x.Trim()));

            foreach (var file in liveFiles)
            {
                Log.Info($"Checking file '{file}'...");
                var reader = new LiveJournalReader(new FileInfo(file), parser);
                // Read entry from each file
                Validate(reader.ReadCurrent().Line, parser);
            }

            // Unsub from loggable events
            SubEthaLog.LogEvent -= SubEthaLog_LogEvent;
        }

        private static void SubEthaLog_LogEvent(object sender, SubEthaLogEvent log)
        {
            switch (log)
            {
                case var d when log.Level == SubEthaLogLevel.Debug:
                    Log.Debug($"{d.Source}: {d.Message}");
                    break;
                case var w when log.Level == SubEthaLogLevel.Warn:
                    Log.Warn($"{w.Source}: {w.Message}", w.Exception);
                    break;
                case var e when log.Level == SubEthaLogLevel.Error:
                    Log.Error($"{e.Source}: {e.Message}", e.Exception);
                    break;
                default:
                    Log.Info($"{log.Source}: {log.Message}");
                    break;
            }
        }

        private static void Validate(string line, JournalParser parser)
        {
            Log.Debug(line);
            try
            {
                var _ = parser.Parse(line);
            }
            catch (Exception ex) when (ex is UnrecognizedJournalException || ex is JournalParseException)
            {
                Log.Warn(line);
                Log.Warn(ex.Message);
            }
        }
    }
}
