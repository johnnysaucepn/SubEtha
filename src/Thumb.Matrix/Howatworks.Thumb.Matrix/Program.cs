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
            builder.RegisterModule(new MatrixWinFormsModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var context = scope.Resolve<MatrixApplicationContext>();
                Application.Run(context);
            }
        }
    }
}
