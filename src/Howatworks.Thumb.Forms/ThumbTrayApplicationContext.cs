using System;
using System.Windows.Forms;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Thumb.Forms
{
    public class ThumbTrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private readonly ThumbApp _thumbApp;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbTrayApplicationContext));
        private IProgress<DateTimeOffset?> _progressHandler;

        private System.Threading.Timer _updateTimer;

        public ThumbTrayApplicationContext(ThumbApp thumbApp)
        {
            _thumbApp = thumbApp;
            Application.ApplicationExit += Cleanup;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var exitMenuItem = new MenuItem(Resources.ExitLabel, (sender, args) => Application.Exit());

            // Initialize Tray Icon
            _trayIcon = new NotifyIcon
            {
                Icon = Resources.ThumbIcon,
                ContextMenu = new ContextMenu(new[] {exitMenuItem}),
                Visible = true,
                Text = Resources.NotifyIconDefaultLabel
            };

            _progressHandler = new Progress<DateTimeOffset?>(_ =>
            {
                var lastChecked = _thumbApp.LastChecked() ?? DateTimeOffset.UtcNow;
                var lastEntry = _thumbApp.LastEntry();
                _trayIcon.Text = lastEntry.HasValue
                    ? string.Format(
                        Resources.NotifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                        lastEntry.Value.LocalDateTime.ToString("g"),
                        lastChecked.LocalDateTime.ToString("g"))
                    : Resources.NotifyIconNeverUpdatedLabel;
            });

            try
            {
                _progressHandler.Report(null);

                _updateTimer = new System.Threading.Timer(UpdateProgress, null, 0, 10000);

                _thumbApp?.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                Cleanup(this, null);
                throw;
            }
        }

        private void UpdateProgress(object state)
        {
            _progressHandler.Report(_thumbApp.LastEntry());
        }

        private void Cleanup(object sender, EventArgs e)
        {
            _thumbApp?.Stop();
            _updateTimer?.Dispose();
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }
    }
}