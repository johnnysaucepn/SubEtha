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
        private readonly ThumbTrayControl _tray;
        private readonly LoginForm _loginForm;

        public MatrixApplicationContext(MatrixApp app)
        {
            _app = app;
            _loginForm = new LoginForm();

            _tray = new ThumbTrayControl(GetLastChecked, GetLastEntry,
                Resources.ThumbIcon,
                Resources.ExitLabel,
                Resources.NotifyIconDefaultLabel, Resources.NotifyIconNeverUpdatedLabel, Resources.NotifyIconLastUpdatedLabel);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _app.Initialize();
            _app.OnAuthenticationRequired += (_, args) => { OpenLoginDialog(); };

            _loginForm.SiteName = _app.SiteUri;
            _loginForm.OnLogin += (sender, args) =>
            {
                var authenticated = _app.Authenticate(args.Username, args.Password);
                if (authenticated)
                {
                    _loginForm.Hide();
                }
                else
                {
                    MessageBox.Show(Resources.LoginFailedMessage, Resources.LoginFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            _tray.Initialize();
            _tray.OnExitRequested += (sender, args) =>
            {
                Application.Exit();
            };
            ThreadExit += (sender, args) =>
            {
                _app.Shutdown();
            };
        }

        private void OpenLoginDialog()
        {
            Dispatch(() => { _loginForm.ShowDialog(); });
        }

        private void Dispatch(MethodInvoker invoker)
        {
            if (_loginForm.InvokeRequired)
            {
                _loginForm.Invoke(invoker);
            }
            else
            {
                invoker();
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
