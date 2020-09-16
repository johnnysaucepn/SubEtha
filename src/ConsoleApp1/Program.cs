using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using Microsoft.Extensions.Configuration;
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

            var startOfYear = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
            while (true)
            {
                GetABunchOfStuff(parser, monitor, startOfYear);
                /*Log.Debug("WAIT");
                GetABunchOfStuff(parser, monitor, startOfYear);
                Log.Debug("WAIT");
                GetABunchOfStuff(parser, monitor, startOfYear);*/

                //Console.WriteLine(items);

                Console.ReadKey();
            }
        }

        private static void GetABunchOfStuff(JournalParser parser, NewJournalMonitor monitor, DateTimeOffset startOfYear)
        {
            monitor.GetJournalEntries()
                //.Where(x => x.Context.HeaderTimestamp > startOfYear)
                .Select(l =>
                {
                    try
                    {
                        var journalEntry = parser.Parse(l.Line);
                        return (line: l, journal: journalEntry);
                    }
                    catch (JournalParseException e)
                    {
                        Log.Error($"'{l.Context.File.FullName}': {e.JournalFragment}");
                        Log.Error(e.Message);
                    }
                    catch (UnrecognizedJournalException e)
                    {
                        Log.Warn($"'{l.Context.File.FullName}': {e.JournalFragment}");
                        Log.Warn(e.Message);
                    }
                    return (null, null);

                })
                .Where(x=>x.line != null)
            //.Count().Subscribe(Console.WriteLine);
            //.Subscribe(x => Log.Debug($"{x.Context.Path}: {x.Line.Substring(0, Math.Min(50, x.Line.Length))}"));
            .Subscribe(x => Log.Info($"{x.line.Context.File.Name} {x.journal.Timestamp:g}: {x.journal.Event}"));
        }
    }
}
