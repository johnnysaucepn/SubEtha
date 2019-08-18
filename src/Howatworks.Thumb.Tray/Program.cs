using System;
using System.Windows.Forms;
using Howatworks.Thumb.Forms;

namespace Howatworks.Thumb.Tray
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

            var context = new ThumbTrayApplicationContext();
            Application.Run(context);
        }
    }
}
