using System;

namespace Howatworks.Thumb.Matrix
{
    public class LoginEventArgs : EventArgs
    {
        public LoginEventArgs(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }
    }
}