using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Cooked.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PInvoke;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using static log4net.Appender.ManagedColoredConsoleAppender;

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

            var parser = new JournalParser();
            var monitor = new NewJournalMonitor(config, parser);
            var source = new JournalEntrySource(parser);

            var startOfYear = new DateTimeOffset(2019, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var publisher = source.GetJournalEntries(monitor.GetJournalLines(), startOfYear).Publish();

            //publisher.Source.Subscribe(x => Log.Info($"{x.Context.Filename} {x.JournalEntry.Timestamp:g}: {x.JournalEntry.Event} {x.JournalEntry.GetType().Name}"));
            publisher.Where(x => x.JournalEntry.Event.Equals("Music")).Subscribe(x => Log.Info($"#{x.Context.Filename} {x.JournalEntry.Timestamp:g}: {x.JournalEntry.Event}"));
            publisher.Where(x => x.JournalEntry.Event.Equals("Shutdown")).Subscribe(x => Log.Info($"*{x.Context.Filename} {x.JournalEntry.Timestamp:g}: {x.JournalEntry.Event}"));
            publisher.Where(x => x.JournalEntry.Event.Equals("ShipTargeted")).Subscribe(s => Log.Info($"@{s.Context.Filename} {s.JournalEntry.Event}"));

            publisher.Where(x => x.JournalEntry is ShipTargeted)
                .Select(j=>new ShipTargetedCooked(j.Context, j.JournalEntry as ShipTargeted))
                .Subscribe(s => Log.Info($":{s.Context.Filename} {s.Ship.Text}"));

            while (true)
            {
                using (publisher.Connect())
                {
                    Console.ReadKey();
                }
            }
        }

        /*private static void GetABunchOfStuff(NewJournalMonitor monitor, JournalEntrySource source, DateTimeOffset startOfYear)
        {
            source.GetJournalEntries(monitor.GetJournalLines())
            //.Where(x => x.Context.HeaderTimestamp > startOfYear)
            .Where(x => x.JournalEntry is ShipTargeted)
            .Select(t => new ShipTargetedCooked(t.Context, t.JournalEntry as ShipTargeted))
            .Subscribe(s => Log.Info(JsonConvert.SerializeObject(s)));
            //.Count().Subscribe(Console.WriteLine);
            //.Subscribe(x => Log.Info($"{x.Context.File.Name} {x.JournalEntry.Timestamp:g}: {x.JournalEntry.Event} {x.JournalEntry.GetType().Name}"));
        }*/

        /*private static void Listen(RawJournalEntryPublisher source)
        {
            
            source.Source.Subscribe(x => Log.Info($"{x.Context.Filename} {x.JournalEntry.Timestamp:g}: {x.JournalEntry.Event} {x.JournalEntry.GetType().Name}"));
        }*/
        
    }
}
