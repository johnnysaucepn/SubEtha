namespace Howatworks.Thumb.Core
{
    public interface IThumbNotifier
    {
        void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage);
    }
}