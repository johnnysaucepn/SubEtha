using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;

namespace Howatworks.SubEtha.Bindings.Scan
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var defaultBindingsFolder = Path.Combine(
                appData, "Frontier Developments", "Elite Dangerous", "Options", "Bindings"
            );

            var defaultConfig = new Dictionary<string, string>
            {
                ["BindingsFolder"] = defaultBindingsFolder,
                ["BindingsPattern"] = "*.binds",
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddCommandLine(args)
                .Build();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            Log.Info("Started");

            var folder = config["BindingsFolder"];

            // Instead of using the full monitor, just enumerate files and parse them one by one
            var pattern = config["BindingsPattern"];
            var files = Directory.EnumerateFiles(folder, pattern, SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                Log.Info($"Checking file '{file}'...");

                // Load the .binds file
                try
                {
                    var bindings = new BindingSetReader(new FileInfo(file)).Read();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }
}
