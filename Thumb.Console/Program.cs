using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Thumb.Core;

namespace Thumb.Console
{
    internal static class Program
    {
        private static void Main()
        {
            // TODO: In tray, we can use the Win32 add-on package that extends SpecialFolder to retrieve SavedGames directly
            var defaultJournalFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "..", "Saved Games", "Frontier Developments", "Elite Dangerous"
            );

            var defaultBindingsFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Frontier Developments", "Elite Dangerous", "Options", "Bindings"
            );

            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = defaultJournalFolder,
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json",
                ["UpdateInterval"] = new TimeSpan(0, 0, 5).ToString(),
                ["BindingsFolder"] = defaultBindingsFolder,
                ["BindingsFilename"] = "Custom.binds",
                ["ActiveWindowTitle"] = "Elite - Dangerous (CLIENT)"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddJsonFile("journalmonitor.json")
                .Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ThumbApp>();
                app.Start();
                System.Console.ReadKey();
                app.Stop();
            }

        }
    }
}
