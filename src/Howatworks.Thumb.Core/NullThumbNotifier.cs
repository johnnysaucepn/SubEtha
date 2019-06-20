namespace Howatworks.Thumb.Core
{
    public class NullThumbNotifier : IThumbNotifier
    {
        public void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage)
        {
            // Do nothing
        }
    }
}
