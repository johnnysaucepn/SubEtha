using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Thumb.Forms
{
    public class ThumbTrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private readonly IThumbApp _thumbApp;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbTrayApplicationContext));
        private IProgress<DateTimeOffset?> _progressHandler;

        private System.Threading.Timer _updateTimer;
        private readonly ResourceManager _resources;

        public ThumbTrayApplicationContext(IThumbApp thumbApp, ResourceManager resources)
        {
            _thumbApp = thumbApp;
            _resources = resources;
            Application.ApplicationExit += Cleanup;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var exitLabel = _resources.GetString("ExitLabel");
            var notifyIconDefaultLabel = _resources.GetString("NotifyIconDefaultLabel");
            var notifyIconLastUpdatedLabel = _resources.GetString("NotifyIconLastUpdatedLabel");
            var notifyIconNeverUpdatedLabel = _resources.GetString("NotifyIconNeverUpdatedLabel");
            var thumbIcon = (Icon)_resources.GetObject("ThumbIcon");

            var exitMenuItem = new MenuItem(exitLabel, (sender, args) => Application.Exit());

            // Initialize Tray Icon
            _trayIcon = new NotifyIcon
            {
                Icon = thumbIcon,
                ContextMenu = new ContextMenu(new[] {exitMenuItem}),
                Visible = true,
                Text = notifyIconDefaultLabel
            };

            _progressHandler = new Progress<DateTimeOffset?>(_ =>
            {
                var lastChecked = _thumbApp.LastChecked() ?? DateTimeOffset.UtcNow;
                var lastEntry = _thumbApp.LastEntry();
                _trayIcon.Text = lastEntry.HasValue
                    ? string.Format(
                        notifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                        lastEntry.Value.LocalDateTime.ToString("g"),
                        lastChecked.LocalDateTime.ToString("g"))
                    : notifyIconNeverUpdatedLabel;
            });

            try
            {
                _progressHandler.Report(null);

                _updateTimer = new System.Threading.Timer(UpdateProgress, null, 0, 10000);
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
            _updateTimer?.Dispose();
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }
    }
}