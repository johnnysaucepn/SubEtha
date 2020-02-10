using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Howatworks.Thumb.Matrix.Win
{
    public class AuthenticationDialogViewModel
    {
        private readonly MatrixApplication _app;

        public string Username { get; set; }
        public string Password { get; set; }

        public string SiteName { get; set; }


        public AuthenticationDialogViewModel(MatrixApplication app)
        {
            _app = app;
        }

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
                    var authResult = _app.Authenticate(Username, Password);
                    if (authResult.Success)
                    {
                        ViewManager.ConfirmAuthenticationDialog();
                    }
                    else
                    {
                        MessageBox.Show("LoginFailedMessage", "LoginFailedTitle", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            };

    }
}
