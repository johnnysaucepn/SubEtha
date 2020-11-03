using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Howatworks.Thumb.Matrix.Core;
using Howatworks.Thumb.Wpf;
using log4net;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class AuthenticationDialogViewModel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthenticationDialogViewModel));

        private readonly HttpUploadClient _client;

        public string Username { get; set; }
        public string Password { get; set; }

        public string SiteName => _client.SiteUri;

        public event EventHandler RequestClose = delegate { };

        public event EventHandler<AuthenticationDialogEventArgs> OnAuthenticationCompleted = delegate { };

        public AuthenticationDialogViewModel(HttpUploadClient client)
        {
            _client = client;
        }

        public ICommand CancelCommand =>
            new DelegateCommand
            {
                CommandAction = () => CloseDialog()
            };

        private void CloseDialog()
        {
            RequestClose(this, EventArgs.Empty);
        }

        public ICommand OkCommand =>
            new DelegateCommand
            {
                CommandAction = async () =>
                {
                    bool authSucceeded = false;
                    try
                    {
                        authSucceeded = await _client.Authenticate(Username, Password);
                        OnAuthenticationCompleted(this, new AuthenticationDialogEventArgs(authSucceeded));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                    
                    if (authSucceeded)
                    {
                        CloseDialog();
                    }
                    else
                    {
                        MessageBox.Show(Resources.LoginFailedMessage, Resources.LoginFailedTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            };

    }
}
