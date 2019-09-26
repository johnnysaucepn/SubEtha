using System;
using System.Windows.Forms;
using Howatworks.Thumb.Assistant.Core;
using Howatworks.Thumb.Forms;
using log4net;

namespace Howatworks.Thumb.Assistant
{
    public class AssistantApplicationContext : ApplicationContext
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantApplicationContext));

        private readonly AssistantApp _app;
        private readonly ThumbTrayControl _tray;

        public AssistantApplicationContext(AssistantApp app)
        {
            _app = app;

            _tray = new ThumbTrayControl(GetLastChecked, GetLastEntry,
                Resources.ThumbIcon,
                Resources.ExitLabel,
                Resources.NotifyIconDefaultLabel, Resources.NotifyIconNeverUpdatedLabel, Resources.NotifyIconLastUpdatedLabel);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            try
            {
                _tray.Initialize();

                _tray.OnExitRequested += (sender, args) => Application.Exit();
                ThreadExit += (sender, args) => _app.Shutdown();

                _app.Initialize();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        private DateTimeOffset? GetLastEntry()
        {
            return _app.LastEntry();
        }

        private DateTimeOffset? GetLastChecked()
        {
            return _app.LastChecked();
        }
    }
}
