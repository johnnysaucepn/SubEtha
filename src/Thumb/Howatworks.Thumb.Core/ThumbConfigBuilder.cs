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
        private readonly string _defaultAppStoragePath;
        private readonly string _defaultJournalPath;
        private readonly string _defaultBindingsPath;

        public ThumbConfigBuilder(string appName)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // In Windows, we can use the P/Invoke Shell32 package that exposes KnownFolders to retrieve SavedGames directly

            string savedGamesPath;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                savedGamesPath = Shell32.SHGetKnownFolderPath(Shell32.KNOWNFOLDERID.FOLDERID_SavedGames);
            }
            else
            {
                savedGamesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Saved Games");
            }

            _defaultAppStoragePath = Path.Combine(appData, "Howatworks", appName);
            _defaultJournalPath = Path.Combine(savedGamesPath, "Frontier Developments", "Elite Dangerous");
            _defaultBindingsPath = Path.Combine(appData, "Frontier Developments", "Elite Dangerous", "Options", "Bindings");
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
                ["ActiveWindowTitle"] = "Elite - Dangerous (CLIENT)",
                ["JournalMonitorStateFolder"] = _defaultAppStoragePath,
                ["LogFolder"] = _defaultAppStoragePath
        };
            var env = Environment.GetEnvironmentVariable("HOSTINGENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables("THUMB_")
                .AddInMemoryCollection(defaultConfig)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", true)
                .Build();

            return config;
        }
    }
}