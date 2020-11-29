using System;

namespace Howatworks.Matrix.Wpf
{
    public class AuthenticationDialogEventArgs : EventArgs
    {
        public bool AuthenticationSuccess { get; }

        public AuthenticationDialogEventArgs(bool authenticationSuccess)
        {
            AuthenticationSuccess = authenticationSuccess;
        }
    }
}