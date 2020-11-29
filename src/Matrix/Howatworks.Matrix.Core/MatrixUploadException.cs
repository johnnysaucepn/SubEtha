using System;

namespace Howatworks.Matrix.Core
{
    public class MatrixUploadException : MatrixException
    {
        public MatrixUploadException()
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