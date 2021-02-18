using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Assistant.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("Simplification", "RCS1021:Convert lambda expression body to expression-body.", Justification = "Clarity and consistency")]
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
                    services.AddHostedService<AssistantBackgroundService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
