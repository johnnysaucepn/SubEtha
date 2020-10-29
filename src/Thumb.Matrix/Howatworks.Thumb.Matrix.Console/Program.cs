using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Matrix.Core;

namespace Howatworks.Thumb.Matrix.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var config = new ThumbConfigBuilder("Matrix").Build();

            var logger = new Log4NetThumbLogging(config);
            logger.Configure();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
            builder.RegisterModule(new MatrixModule());
            builder.RegisterModule(new MatrixConsoleModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<MatrixApp>();
                var client = scope.Resolve<HttpUploadClient>();

                var cts = new CancellationTokenSource();
                var reset = new ManualResetEventSlim(false);

                Task.Run(()=>
                {
                    app.Run(cts.Token);
                });

                // Wait forever, unless something trips the switch
                reset.Wait();

                // Cancel the token to shut any pending operations down
                cts.Cancel();
            }
        }

    }
}
