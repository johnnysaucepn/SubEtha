using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Cooked.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using PInvoke;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // In Windows, we can use the P/Invoke Shell32 package that exposes KnownFolders to retrieve SavedGames directly

            string savedGamesPath = Shell32.SHGetKnownFolderPath(Shell32.KNOWNFOLDERID.FOLDERID_SavedGames);

            var defaultJournalPath = Path.Combine(savedGamesPath, "Frontier Developments", "Elite Dangerous");

            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = defaultJournalPath,
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .Build();

            var startOfYear = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var parser = new JournalParser();
            var readerFactory = new JournalReaderFactory(parser);
            var logMonitor = new LogJournalMonitor(config, readerFactory, startOfYear);
            var liveMonitor = new LiveJournalMonitor(config, readerFactory);
            var source = new JournalEntrySource(parser, startOfYear, logMonitor, liveMonitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.GetObservable().Publish();

            //publication.Subscribe(x => Log.Info($"{x.Context.Filename} {x.JournalEntry.Timestamp:g}: {x.JournalEntry.Event} {x.JournalEntry.GetType().Name}"));
            publication.Where(x => x.Entry is Music).Subscribe(x => Log.Info($"#{x.Context.Filename} {x.Entry.Timestamp:g}: {x.Entry.Event}"));
            publication.Where(x => x.Entry is Shutdown).Subscribe(x => Log.Info($"*{x.Context.Filename} {x.Entry.Timestamp:g}: {x.Entry.Event}"));
            publication.Where(x => x.Entry is ShipTargeted).Subscribe(s => Log.Info($"@{s.Context.Filename} {s.Entry.Event}"));

            publication.Where(x => x.Entry is ShipTargeted)
                .Select(j => new ShipTargetedCooked(j.Context, j.Entry as ShipTargeted))
                .Subscribe(s => Log.Info($":{s.Context.Filename} {s.Ship.Text}"));


            Console.WriteLine("Connecting");
            var sub = publication.Connect();

            while (true)
            {
                Console.WriteLine("Waiting");
                Console.ReadKey();
                publisher.Poll();
            }

            sub.Dispose();
        }

        
    }
}
