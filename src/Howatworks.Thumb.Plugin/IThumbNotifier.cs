namespace Howatworks.Thumb.Plugin
{
    public interface IThumbNotifier
    {
        void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage);
    }
}