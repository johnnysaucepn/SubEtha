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
            var appStoragePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Howatworks", "Thumb", "Assistant"
            );
            var config = new ThumbConfigBuilder(appStoragePath).Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
            builder.RegisterModule(new AssistantPluginModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var assistantApp = scope.Resolve<AssistantJournalProcessorPlugin>();
                assistantApp.Startup();

                var thumbApp = scope.Resolve<ThumbApp>();
                thumbApp.Start();
                System.Console.ReadKey();
                thumbApp.Stop();
            }
        }
    }
}
