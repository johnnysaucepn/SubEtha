using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Howatworks.Matrix.Core;
using Howatworks.Thumb.Wpf;
using log4net;

namespace Howatworks.Matrix.Wpf
{
    public class AuthenticationDialogViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthenticationDialogViewModel));

        private readonly HttpUploadClient _client;
        private bool _dialogEnabled = true;

        public string Username { get; set; }
        public string Password { get; set; }

        public string SiteName => _client.SiteUri;

        public event EventHandler OnCloseRequested = delegate { };

        public event EventHandler<AuthenticationDialogEventArgs> OnAuthenticationCompleted = delegate { };

        public AuthenticationDialogViewModel(HttpUploadClient client)
        {
            _client = client;
        }

        public bool DialogEnabled
        {
            get { return _dialogEnabled; }
            set { _dialogEnabled = value; NotifyPropertyChanged(); }
        }

        public ICommand CancelCommand =>
            new DelegateCommand
            {
                CommandAction = () => CloseDialog()
            };

        private void CloseDialog()
        {
            OnCloseRequested(this, EventArgs.Empty);
        }

        public ICommand OkCommand =>
            new DelegateCommand
            {
                CommandAction = async () =>
                {
                    bool authSucceeded = false;
                    DialogEnabled = false;
                    try
                    {
                        authSucceeded = await _client.Authenticate(Username, Password);
                        OnAuthenticationCompleted(this, new AuthenticationDialogEventArgs(authSucceeded));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                    finally
                    {
                        DialogEnabled = true;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
