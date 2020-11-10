﻿using System;

namespace Howatworks.Thumb.Matrix.Core
{

    public class MatrixAuthenticationException : MatrixException
    {
        public MatrixAuthenticationException()
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