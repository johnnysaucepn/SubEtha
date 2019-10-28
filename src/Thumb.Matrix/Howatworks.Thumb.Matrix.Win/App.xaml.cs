using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace Howatworks.Thumb.Matrix.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _tb;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _tb = (TaskbarIcon) FindResource("ThumbTrayIcon");
            _tb?.BringIntoView();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _tb.Visibility = Visibility.Hidden;
            _tb.Dispose();
        }
    }
}
