using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Howatworks.Matrix.Service
{
    public class Program
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

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
//            .UseContentRoot(_workingFolder)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .Build();
    }
}
