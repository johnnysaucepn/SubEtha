using System;
using System.Windows.Forms;

namespace Thumb.Tray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var context = new ThumbTrayApplicationContext();
            Application.Run(context);
        }
    }
}
