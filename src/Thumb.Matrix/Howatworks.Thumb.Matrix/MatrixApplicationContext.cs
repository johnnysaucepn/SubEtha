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
        private readonly ThumbTrayControl _ui;
        private readonly LoginForm _loginForm;

        public MatrixApplicationContext(MatrixApp app)
        {
            _app = app;
            _loginForm = new LoginForm();

            _ui = new ThumbTrayControl(GetLastChecked, GetLastEntry,
                Resources.ThumbIcon,
                Resources.ExitLabel,
                Resources.NotifyIconDefaultLabel, Resources.NotifyIconNeverUpdatedLabel, Resources.NotifyIconLastUpdatedLabel);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            try
            {
                _app.Initialize();

                _app.OnAuthenticationRequired += (sender, args) =>
                {
                    _app.Stop();
                    _loginForm.Show();
                };

                _loginForm.SiteName = _app.SiteUri;
                _loginForm.OnLogin += (sender, args) =>
                {
                    var authenticated = _app.Authenticate(args.Username, args.Password);
                    if (authenticated)
                    {
                        _loginForm.Hide();
                        _app.Start();
                    }
                    else
                    {
                        MessageBox.Show(Resources.LoginFailedMessage, Resources.LoginFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                if (_app.IsAuthenticated)
                {
                    _app.Start();
                }
                else
                {
                    _loginForm.Show();
                }

                _ui.Initialize();
                _ui.OnExitRequested += (sender, args) => Application.Exit();
                ThreadExit += (sender, args) => _app.Stop();
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
