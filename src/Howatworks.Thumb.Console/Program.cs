using Autofac;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var config = new ThumbConfigBuilder().Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
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
