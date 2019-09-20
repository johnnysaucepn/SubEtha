using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace Howatworks.Thumb.Forms
{
    public class ThumbTrayUserInterface : IDisposable
    {
        private NotifyIcon _trayIcon;
        private readonly Func<DateTimeOffset?> _getLastChecked;
        private readonly Func<DateTimeOffset?> _getLastEntry;
        private readonly string _notifyIconDefaultLabel;
        private readonly string _notifyIconNeverUpdatedLabel;
        private readonly string _notifyIconLastUpdatedLabel;
        private readonly string _exitLabel;
        private readonly Icon _icon;

        private IProgress<DateTimeOffset?> _progressHandler;

        private Timer _updateTimer;
        private MenuItem _exitMenuItem;

        public event EventHandler OnExitRequested = delegate { };

        public ThumbTrayUserInterface(Func<DateTimeOffset?> getLastChecked,
            Func<DateTimeOffset?> getLastEntry,
            Icon icon,
            string exitLabel,
            string notifyIconDefaultLabel, string notifyIconNeverUpdatedLabel, string notifyIconLastUpdatedLabel)
        {
            _getLastChecked = getLastChecked;
            _getLastEntry = getLastEntry;
            _notifyIconDefaultLabel = notifyIconDefaultLabel;
            _notifyIconNeverUpdatedLabel = notifyIconNeverUpdatedLabel;
            _notifyIconLastUpdatedLabel = notifyIconLastUpdatedLabel;
            _exitLabel = exitLabel;
            _icon = icon;
        }

        public void Initialize()
        {
            _exitMenuItem = new MenuItem(_exitLabel, (sender, args) => OnExitRequested(this, new EventArgs()));

            // Initialize Tray Icon
            _trayIcon = CreateTrayIcon();

            _progressHandler = CreateProgressHandler();
            _progressHandler.Report(null);

            _updateTimer = new Timer(x => _progressHandler.Report(_getLastEntry()), null, 0, 10000);
        }

        private IProgress<DateTimeOffset?> CreateProgressHandler()
        {
            return new Progress<DateTimeOffset?>(_ =>
            {
                var lastChecked = _getLastChecked() ?? DateTimeOffset.UtcNow;
                var lastEntry = _getLastEntry();
                _trayIcon.Text = lastEntry.HasValue
                    ? string.Format(
                        _notifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                        lastEntry.Value.LocalDateTime.ToString("g"),
                        lastChecked.LocalDateTime.ToString("g"))
                    : _notifyIconNeverUpdatedLabel;
            });
        }

        private NotifyIcon CreateTrayIcon()
        {
            return new NotifyIcon
            {
                Icon = _icon,
                ContextMenu = new ContextMenu(new[] { _exitMenuItem }),
                Visible = true,
                Text = _notifyIconDefaultLabel
            };
        }

        public void Dispose()
        {
            _updateTimer?.Dispose();
            _exitMenuItem?.Dispose();

            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _trayIcon?.Dispose();
        }
    }
}