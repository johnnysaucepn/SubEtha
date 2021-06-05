using System;

namespace Howatworks.SubEtha.Common.Logging
{
    /// <summary>
    /// Instead of being tied to logging frameworks that require dependency injection to work,
    /// expose a simple stream of logging events that a client app might be interested in.
    /// </summary>
    public class SubEthaLog
    {
        public static event EventHandler<SubEthaLogEvent> LogEvent;

        public string Source { get; }

        protected SubEthaLog(string source)
        {
            Source = source;
        }

        protected virtual void OnLogEvent(SubEthaLogEvent e)
        {
            LogEvent?.Invoke(this, e);
        }

        public void Debug(string message, params object[] items)
        {
            OnLogEvent(new SubEthaLogEvent(Source, SubEthaLogLevel.Debug, message, items));
        }

        public void Info(string message, params object[] items)
        {
            OnLogEvent(new SubEthaLogEvent(Source, SubEthaLogLevel.Info, message, items));
        }

        public void Warn(string message, params object[] items)
        {
            OnLogEvent(new SubEthaLogEvent(Source, SubEthaLogLevel.Warn, message, items));
        }

        public void Warn(string message, Exception exception, params object[] items)
        {
            OnLogEvent(new SubEthaLogEvent(Source, SubEthaLogLevel.Warn, message, exception, items));
        }

        public void Error(string message, Exception exception, params object[] items)
        {
            OnLogEvent(new SubEthaLogEvent(Source, SubEthaLogLevel.Error, message, exception, items));
        }

        public static SubEthaLog GetLogger(Type type)
        {
            return new SubEthaLog(type.FullName);
        }

        public static SubEthaLog GetLogger<T>()
        {
            return new SubEthaLog(typeof(T).FullName);
        }
    }
}