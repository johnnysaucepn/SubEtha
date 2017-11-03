using Autofac;
using Thumb.Core;

namespace Thumb.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule());
            builder.RegisterModule(new ThumbConsoleModule());
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
