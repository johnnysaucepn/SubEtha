using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Assistant.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Assistant.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddThumbConfiguration("Assistant");
                    builder.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.UseThumbLogging(hostContext.Configuration);
                    logging.AddLog4Net();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
                {
                    var config = hostContext.Configuration;
                    builder.RegisterModule(new ThumbCoreModule(config));
                    builder.RegisterModule(new ThumbConsoleModule(config));
                    builder.RegisterModule(new AssistantModule());
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<ConsoleWorker>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
