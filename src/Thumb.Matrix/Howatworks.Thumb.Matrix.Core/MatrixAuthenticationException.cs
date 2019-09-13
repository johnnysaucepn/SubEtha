using System;

namespace Howatworks.Thumb.Matrix.Core
{
    internal class MatrixAuthenticationException : Exception
    {
        public MatrixAuthenticationException(string message) : base(message)
        {
        }
    }
}