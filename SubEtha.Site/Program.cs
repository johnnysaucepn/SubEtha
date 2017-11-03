using System;
using System.IO;
using System.Reflection;
using Autofac;
using Howatworks.Configuration;
using log4net;
using log4net.Config;
using Microsoft.Owin.Hosting;
using Owin;

namespace SubEtha.Site
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var workingFolder = Path.Combine(appDataFolder, "Howatworks", "SubEtha");
            var logFolder = Path.Combine(workingFolder, "Logs");
            Directory.CreateDirectory(logFolder);

            GlobalContext.Properties["logfolder"] = logFolder;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var builder = new ContainerBuilder();
            builder.RegisterModule(new SiteAutofacModule());
            var container = builder.Build();

            var configLoader = container.Resolve<IConfigLoader>();
            var section = configLoader.GetConfigurationSection("SubEtha.Site");
            var url = section.Get<string>("SiteBinding") ?? "http://+:8984/SubEtha/Site";

            // Start OWIN host
            using (WebApp.Start(new StartOptions(url), app =>
            {
                //app
                //.UseFacebookAuthentication(ConfigurationManager.AppSettings["facebookAppID"], ConfigurationManager.AppSettings["facebookAppSecret"])
                //.UseStaticFiles("/HalBrowser");

                app
                    .UseAutofacMiddleware(container)
                    .UseNancy(config => {
                        config.Bootstrapper = new SubEthaBootstrapper();
                    });
            }))
            {
                Log.Info($"{url} started");
                Console.WriteLine($"{url} started");
                Console.ReadLine();

            }
        }
    }
}
