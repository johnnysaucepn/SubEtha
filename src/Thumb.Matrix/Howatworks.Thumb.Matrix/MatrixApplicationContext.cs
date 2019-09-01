using System;
using System.Windows.Forms;
using Howatworks.Thumb.Forms;
using Howatworks.Thumb.Matrix.Core;
using log4net;

namespace Howatworks.Thumb.Matrix
{
    public class MatrixApplicationContext : ApplicationContext
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixApplicationContext));

        private readonly MatrixApp _app;
        private readonly ThumbTrayUserInterface _ui;

        public MatrixApplicationContext(MatrixApp app)
        {
            _app = app;

            _ui = new ThumbTrayUserInterface(GetLastChecked, GetLastEntry,
                Resources.ThumbIcon,
                Resources.ExitLabel,
                Resources.NotifyIconDefaultLabel, Resources.NotifyIconNeverUpdatedLabel, Resources.NotifyIconLastUpdatedLabel);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            try
            {
                _ui.Initialize();
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
