using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Howatworks.Matrix.Site
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        [SuppressMessage("Simplification", "RCS1021:Convert lambda expression body to expression-body.", Justification = "Clarity and consistency")]
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddCommandLine(args);
                })
                .ConfigureLogging(logging =>
                {
                    var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var logFolder = Path.Combine(appDataFolder, "Howatworks", "Matrix", "Logs");
                    Directory.CreateDirectory(logFolder);

                    GlobalContext.Properties["logfolder"] = logFolder;

                    logging.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
