using System;

namespace Howatworks.Thumb.Matrix.Wpf
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