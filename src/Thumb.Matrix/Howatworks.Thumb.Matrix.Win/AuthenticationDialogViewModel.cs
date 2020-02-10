﻿using System;
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
        public string Username { get; set; }
        public string Password { get; set; }

        public string SiteName { get; set; }


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
