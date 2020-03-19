using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Howatworks.Thumb.Wpf;

namespace Howatworks.Thumb.Matrix.Win
{
    public class AuthenticationDialogViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string SiteName => ViewManager.App.SiteUri;


        public ICommand CancelCommand =>
            new DelegateCommand
            {
                CommandAction = () => ViewManager.CloseAuthenticationDialog()
            };

        public ICommand OkCommand =>
            new DelegateCommand
            {
                CommandAction = () =>
                {
                    var authResult = ViewManager.Authenticate(Username, Password);
                    if (authResult.Succeeded)
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
