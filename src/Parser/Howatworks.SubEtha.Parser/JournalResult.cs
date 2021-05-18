using System;

namespace Howatworks.SubEtha.Parser
{
    public class JournalResult<T> where T : class
    {
        public T Value { get; }
        public string Message { get; }
        public string Line { get; }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        protected JournalResult(T @value, bool success = true, string message = "", string line = "")
        {
            Value = value;
            IsSuccess = success;
            Message = message;
            Line = line;
        }

        public static JournalResult<T> Success(T @value)
        {
            return new JournalResult<T>(value, true);
        }

        public static JournalResult<T> Failure(string message)
        {
            return new JournalResult<T>(null, false, message);
        }

        public static JournalResult<T> Failure(string message, string line)
        {
            return new JournalResult<T>(null, false, message, line);
        }
    }

    public static class JournalResult
    {
        public static JournalResult<T> Success<T>(T @value) where T : class
        {
            return JournalResult<T>.Success(value);
        }

        public static JournalResult<T> Failure<T>(string message) where T : class
        {
            return JournalResult<T>.Failure(message);
        }

        public static JournalResult<T> Failure<T>(string message, string line) where T : class
        {
            return JournalResult<T>.Failure(message, line);
        }
    }
}