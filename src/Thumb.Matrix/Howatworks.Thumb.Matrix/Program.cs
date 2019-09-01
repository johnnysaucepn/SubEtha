using System;
using System.IO;
using System.Windows.Forms;
using Autofac;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Forms;
using Howatworks.Thumb.Matrix.Core;

namespace Howatworks.Thumb.Matrix
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
                "Howatworks", "Thumb", "Matrix"
            );
            var config = new ThumbConfigBuilder(appStoragePath).Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbFormsModule(config));
            builder.RegisterModule(new MatrixModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<MatrixApp>();
                app.Initialize();

                app.Start();
                var context = new MatrixApplicationContext(app);
                Application.Run(context);
                app.Stop();
            }
        }
    }
}
