using Howatworks.Thumb.Plugin;

namespace Howatworks.Thumb.Console
{
    public class ConsoleThumbNotifier : IThumbNotifier
    {
        public void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage)
        {
            const int scaleInterval = 131;
            int frequency;
            int duration;
            switch (priority)
            {
                case NotificationPriority.High:
                    duration = 500;
                    break;
                case NotificationPriority.Medium:
                    duration = 200;
                    break;
                case NotificationPriority.Low:
                    duration = 100;
                    break;
                default:
                    duration = 0;
                    break;
            }

            switch (eventType)
            {
                case NotificationEventType.FileSystem:
                    frequency = scaleInterval * 2;
                    break;
                case NotificationEventType.JournalEntry:
                    frequency = scaleInterval * 3;
                    break;
                case NotificationEventType.JournalEntryBatch:
                    frequency = scaleInterval * 4;
                    break;
                case NotificationEventType.Update:
                    frequency = scaleInterval * 5;
                    break;
                default:
                    frequency = 0;
                    break;
            }

            System.Console.Beep(frequency, duration);
        }
    }
}