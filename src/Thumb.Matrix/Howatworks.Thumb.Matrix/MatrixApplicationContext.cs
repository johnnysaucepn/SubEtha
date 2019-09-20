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
        private readonly HttpUploadClient _client;
        private readonly ThumbTrayUserInterface _ui;
        private readonly LoginForm _loginForm;

        public MatrixApplicationContext(MatrixApp app, HttpUploadClient client)
        {
            _app = app;
            _client = client;
            _loginForm = new LoginForm();

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
                _app.Initialize();

                _app.OnAuthenticationError += (sender, args) => { _loginForm.Show(); };

                _loginForm.SiteName = _client.BaseUri.AbsoluteUri;
                _loginForm.OnLogin += (sender, args) =>
                {
                    _client.AuthenticateByBearerToken(args.Username, args.Password);
                    if (_client.Authenticated)
                    {
                        _loginForm.Hide();
                        _app.Start();
                    }
                    else
                    {
                        MessageBox.Show(Resources.LoginFailedMessage, Resources.LoginFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                if (!_client.Authenticated)
                {
                    _loginForm.Show();
                }
                else
                {
                    _app.Start();
                }


                _ui.Initialize();
                _ui.OnExitRequested += (sender, args) =>
                {
                    Application.Exit();
                };
                ThreadExit += (sender, args) => { _app.Stop(); };

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
