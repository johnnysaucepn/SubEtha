using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Howatworks.SubEtha.Parser
{
    /// <summary>
    /// Instead of being tied to logging frameworks that require dependency injection to work,
    /// expose a simple Observable stream of logging events that a client app might be interested in.
    /// </summary>
    public class SubEthaLog : IDisposable
    {
        private static readonly ISubject<SubEthaLogEvent> _logEvents = new Subject<SubEthaLogEvent>();
        public static IObservable<SubEthaLogEvent> LogEvents = _logEvents.AsObservable();

        public string Source { get; }

        protected SubEthaLog(string source)
        {
            Source = source;
        }

        private bool disposedValue;

        public void Debug(string message, params object[] items)
        {
            _logEvents.OnNext(new SubEthaLogEvent(Source, SubEthaLogLevel.Debug, message, items));
        }

        public void Info(string message, params object[] items)
        {
            _logEvents.OnNext(new SubEthaLogEvent(Source, SubEthaLogLevel.Info, message, items));
        }

        public void Warn(string message, Exception exception, params object[] items)
        {
            _logEvents.OnNext(new SubEthaLogEvent(Source, SubEthaLogLevel.Warn, message, exception, items));
        }

        public void Error(string message, Exception exception, params object[] items)
        {
            _logEvents.OnNext(new SubEthaLogEvent(Source, SubEthaLogLevel.Error, message, exception, items));
        }

        public static SubEthaLog GetLogger(Type type)
        {
            return new SubEthaLog(type.FullName);
        }

        public static SubEthaLog GetLogger<T>()
        {
            return new SubEthaLog(typeof(T).FullName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logEvents.OnCompleted();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SubEthaLog()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public enum SubEthaLogLevel
    {
        Debug,
        Info,
        Warn,
        Error
    }

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