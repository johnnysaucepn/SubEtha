using System;

namespace Howatworks.Thumb.Matrix.Core
{
    internal class MatrixAuthenticationException : Exception
    {
        public MatrixAuthenticationException() : base()
        {
        }

        public MatrixAuthenticationException(string message) : base(message)
        {
        }


        public MatrixAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}