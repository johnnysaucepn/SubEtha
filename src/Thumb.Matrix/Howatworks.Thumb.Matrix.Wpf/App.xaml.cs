using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Matrix.Core;
using Howatworks.Thumb.Wpf;

namespace Howatworks.Thumb.Matrix.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _tb;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var config = new ThumbConfigBuilder("Matrix").Build();

            var logger = new Log4NetThumbLogging(config);
            logger.Configure();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbWpfModule(config));
            builder.RegisterModule(new MatrixModule());
            builder.RegisterModule(new MatrixWpfModule());

            _container = builder.Build();

            using (var scope = _container.BeginLifetimeScope())
            {
                var app = _container.Resolve<MatrixApp>();

                _tb = (TaskbarIcon)FindResource("ThumbTrayIcon");
                LoadTaskbarIcon();

                Task.Run(() =>
                {
                    app.Run(_cts.Token);
                });
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _cts.Cancel();
            _tb.Visibility = Visibility.Hidden;
            _tb.Dispose();
        }

        public void LoadTaskbarIcon()
        {
            var trayVm = _container.Resolve<TrayIconViewModel>();
            var authDialog = _container.Resolve<AuthenticationDialog>();

            trayVm.OnExitApplication += (s, e) => Application.Current.Shutdown();
            trayVm.OnAuthenticationRequested += (s, e) => authDialog.Show();

            _tb.DataContext = trayVm;
            _tb?.BringIntoView();
        }
    }
}
