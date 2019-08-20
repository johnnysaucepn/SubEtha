using System;
using System.Windows.Forms;
using Autofac;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Forms;

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

            var config = new ThumbConfigBuilder().Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbFormsModule(config));
            var container = builder.Build();

            var context = new ThumbTrayApplicationContext(container);
            Application.Run(context);
        }
    }
}
