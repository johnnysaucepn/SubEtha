using System;

namespace Howatworks.SubEtha.Common.Logging
{
    public class SubEthaLogEvent
    {
        public SubEthaLogLevel Level { get; }
        public string Source { get; }
        public string Message { get; }
        public Exception Exception { get; }
        public object[] Items { get; }

        public SubEthaLogEvent(string source, SubEthaLogLevel level, string message, params object[] items)
        {
            Source = source;
            Level = level;
            Message = message;
            Items = items;
        }

        public SubEthaLogEvent(string source, SubEthaLogLevel level, string message, Exception exception, params object[] items)
            : this(source, level, message, items)
        {
            Exception = exception;
        }
    }
}