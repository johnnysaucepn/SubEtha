using System;
using System.IO;
using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Plugin.Matrix;

namespace Howatworks.Thumb.Matrix.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var appStoragePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Howatworks", "Thumb", "Matrix"
            );
            var config = new ThumbConfigBuilder(appStoragePath).Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
            builder.RegisterModule(new MatrixPluginModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ThumbApp>();
                app.Start();
                System.Console.ReadKey();
                app.Stop();
            }
        }
    }
}
