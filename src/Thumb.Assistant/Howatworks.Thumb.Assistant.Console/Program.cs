using System;
using System.IO;
using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Assistant.Core;

namespace Howatworks.Thumb.Assistant.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var config = new ThumbConfigBuilder("Assistant").Build();

            var logger = new Log4NetThumbLogging(config);
            logger.Configure();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
            builder.RegisterModule(new AssistantModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<AssistantApp>();
               
                app.Initialize();
                app.StartMonitoring();
                app.Shutdown();
            }
        }
    }
}
