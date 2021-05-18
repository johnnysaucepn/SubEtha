﻿using Howatworks.SubEtha.Parser;
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
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json;NavRoute.json;ModulesInfo.json;Cargo.json"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddCommandLine(args)
                .Build();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            Log.Info("Started");

            using var subEthaLog = SubEthaLog.LogEvents.Subscribe(log =>
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
            });

            var parser = new JournalParser(true);  // Use strict parsing

            var basePath = config["JournalFolder"];

            var logPattern = config["JournalPattern"];
            var logFiles = Directory.EnumerateFiles(basePath, logPattern, SearchOption.TopDirectoryOnly);
            foreach (var file in logFiles)
            {
                Log.Info($"Checking file '{file}'...");
                var reader = new LogJournalReader(new FileInfo(file), parser);
                // Read all entries from all files
                reader.ReadLines().ToList().ForEach(l => AttemptParse(l.Line, parser));
            }

            var liveFiles = config["RealTimeFilenames"].Split(';').Select(x => Path.Combine(basePath, x.Trim()));
            foreach (var file in liveFiles)
            {
                Log.Info($"Checking file '{file}'...");
                var reader = new LiveJournalReader(new FileInfo(file), parser);
                // Read entry from each file
                AttemptParse(reader.ReadCurrent().Line, parser);
            }
        }

        private static void AttemptParse(string line, JournalParser parser)
        {
            Log.Debug(line);
            try
            {
                parser.Parse(line);
            }
            catch (Exception ex) when (ex is UnrecognizedJournalException || ex is JournalParseException)
            {
                Log.Warn(line);
                Log.Warn(ex.Message);
            }
        }
    }
}
