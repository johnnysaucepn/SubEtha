using System;

namespace Howatworks.Thumb.Matrix.Core
{
    internal class MatrixUploadException : Exception
    {
        public MatrixUploadException() : base()
        {
        }

        public MatrixUploadException(string message) : base(message)
        {
        }

        public MatrixUploadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}