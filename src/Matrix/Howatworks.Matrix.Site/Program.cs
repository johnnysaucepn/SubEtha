using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Matrix.Site
{
    public static class Program
    {
        private static string _appDataFolder;
        private static string _workingFolder;
        private static string _logFolder;

        public static void Main(string[] args)
        {
            _appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _workingFolder = Path.Combine(_appDataFolder, "Howatworks", "Matrix");
            _logFolder = Path.Combine(_workingFolder, "Logs");
            Directory.CreateDirectory(_logFolder);

            GlobalContext.Properties["logfolder"] = _logFolder;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));


            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<Startup>();

    }
}
