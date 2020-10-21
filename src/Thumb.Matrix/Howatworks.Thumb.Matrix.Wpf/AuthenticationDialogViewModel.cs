using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Howatworks.Thumb.Matrix.Core;
using Howatworks.Thumb.Wpf;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class AuthenticationDialogViewModel
    {
        private readonly HttpUploadClient _client;

        public string Username { get; set; }
        public string Password { get; set; }

        public string SiteName => _client.SiteUri;

        public AuthenticationDialogViewModel(HttpUploadClient client)
        {
            // TODO: not sure if this gets runs
            _client = client;
        }

        public ICommand CancelCommand =>
            new DelegateCommand
            {
                CommandAction = () => ViewManager.CloseAuthenticationDialog()
            };

        public ICommand OkCommand =>
            new DelegateCommand
            {
                CommandAction = async () =>
                {
                    var authSucceeded = await _client.Authenticate(Username, Password);
                    if (authSucceeded)
                    {
                        ViewManager.ConfirmAuthenticationDialog();
                    }
                    else
                    {
                        MessageBox.Show(Resources.LoginFailedMessage, Resources.LoginFailedTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            };

    }
}
