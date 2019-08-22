using System;
using System.IO;
using System.Windows.Forms;
using Autofac;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Forms;
using Howatworks.Thumb.Assistant.Core;

namespace Howatworks.Thumb.Assistant
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var appStoragePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Howatworks", "Thumb", "Assistant"
            );
            var config = new ThumbConfigBuilder(appStoragePath).Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbFormsModule(config));
            builder.RegisterModule(new AssistantModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var assistantApp = scope.Resolve<AssistantApp>();
                assistantApp.Startup();

                var thumbApp = scope.Resolve<ThumbApp>();

                var context = new ThumbTrayApplicationContext(thumbApp);
                Application.Run(context);
            }
        }
    }
}
