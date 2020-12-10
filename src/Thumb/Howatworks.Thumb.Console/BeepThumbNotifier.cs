using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Console
{
    public class BeepThumbNotifier : IThumbNotifier
    {
        public void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage)
        {
            const int scaleInterval = 131;
            var duration = priority switch
            {
                NotificationPriority.High => 500,
                NotificationPriority.Medium => 200,
                NotificationPriority.Low => 0,
                _ => 0,
            };
            var frequency = eventType switch
            {
                NotificationEventType.Error => scaleInterval * 1,
                NotificationEventType.FileSystem => scaleInterval * 2,
                NotificationEventType.JournalEntry => scaleInterval * 3,
                NotificationEventType.Update => scaleInterval * 5,
                _ => 0,
            };
            if (frequency > 0 && duration > 0)
            {
                System.Console.Beep(frequency, duration);
            }
        }
    }
}