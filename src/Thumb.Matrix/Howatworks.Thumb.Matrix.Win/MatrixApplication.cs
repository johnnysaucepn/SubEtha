using System;

namespace Howatworks.Thumb.Matrix.Win
{
    public class MatrixApplication
    {
        public AuthenticationResult Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public class AuthenticationResult
        {
            public bool Success { get; private set; }
        }
    }
}