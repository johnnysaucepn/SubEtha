using System;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixQueueException : Exception
    {
        public MatrixQueueException() : base()
        {
        }

        public MatrixQueueException(string message) : base(message)
        {
        }

        public MatrixQueueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
