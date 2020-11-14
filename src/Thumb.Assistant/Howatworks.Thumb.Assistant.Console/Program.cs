using System;
using System.IO;
using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Assistant.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

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
                var keyListener = scope.Resolve<ConsoleKeyListener>();

                var cts = new CancellationTokenSource();
                var reset = new ManualResetEventSlim(false);

                Task.Run(() => app.Run(cts.Token));

                keyListener.Observable.Where(k => k.Key == ConsoleKey.Escape).Subscribe(_ => reset.Set());

                // Wait forever, unless something trips the switch
                reset.Wait();

                // Cancel the token to shut any pending operations down
                cts.Cancel();
            }
        }
    }
}
