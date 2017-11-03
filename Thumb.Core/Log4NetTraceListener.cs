using System;
using System.Diagnostics;
using log4net;
using log4net.Repository;

namespace Thumb.Core
{
    public class Log4NetTraceListener : TraceListener
    {
        private readonly ILog _log;

        public Log4NetTraceListener(ILoggerRepository logRepository, string loggerName)
        {
            _log = LogManager.GetLogger(logRepository.Name, loggerName);
        }

        public Log4NetTraceListener(ILoggerRepository logRepository, Type loggerName)
        {
            _log = LogManager.GetLogger(logRepository.Name, loggerName);
        }

        public override void Write(string message)
        {
            _log?.Info(message);
        }

        public override void WriteLine(string message)
        {
            _log?.Info(message);
        }
    }
}