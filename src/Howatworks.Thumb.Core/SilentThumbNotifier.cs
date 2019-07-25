using Howatworks.Thumb.Plugin;
using log4net;

namespace Howatworks.Thumb.Core
{
    public class SilentThumbNotifier : IThumbNotifier
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SilentThumbNotifier));

        public void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage)
        {
            Log.Info($"{priority} {eventType}: {logMessage}");
        }
    }
}
