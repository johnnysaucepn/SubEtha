using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using PInvoke;

namespace Howatworks.Thumb.Core
{
    public class ThumbConfigBuilder
    {
        private readonly string _defaultJournalPath;
        private readonly string _defaultBindingsPath;
        private readonly string _defaultMonitorPath;

        public ThumbConfigBuilder()
        {
            // In Windows, we can use the P/Invoke Shell32 package that exposes KnownFolders to retrieve SavedGames directly

            string savedGamesPath;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                savedGamesPath = Shell32.SHGetKnownFolderPath(Shell32.KNOWNFOLDERID.FOLDERID_SavedGames);
            }
            else
            {
                savedGamesPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "..", "Saved Games"
                );
            }

            _defaultJournalPath = Path.Combine(savedGamesPath, "Frontier Developments", "Elite Dangerous");

            _defaultBindingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Frontier Developments", "Elite Dangerous", "Options", "Bindings"
            );

            _defaultMonitorPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Howatworks", "Thumb"
            );
        }

        public IConfiguration Build()
        {
            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = _defaultJournalPath,
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json",
                ["UpdateInterval"] = new TimeSpan(0, 0, 5).ToString(),
                ["BindingsFolder"] = _defaultBindingsPath,
                ["BindingsFilename"] = "Custom.3.0.binds",
                ["ActiveWindowTitle"] = "Elite - Dangerous (CLIENT)",
                ["JournalMonitorStateFolder"] = _defaultMonitorPath
        };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddJsonFile("config.json")
                .Build();

            return config;
        }
    }
}