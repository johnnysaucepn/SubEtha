﻿using System;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixException : Exception
    {
        public MatrixException() : base()
        {
        }

        public MatrixException(string message) : base(message)
        {
        }


        public MatrixException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}