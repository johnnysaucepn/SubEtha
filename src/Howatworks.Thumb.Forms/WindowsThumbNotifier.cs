using System.Media;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Forms
{
    public class WindowsThumbNotifier : IThumbNotifier
    {
        public void Notify(NotificationPriority priority, NotificationEventType eventType, string logMessage)
        {
            switch (eventType)
            {
                case NotificationEventType.FileSystem:
                    SystemSounds.Asterisk.Play();
                    break;
                case NotificationEventType.Update:
                    SystemSounds.Hand.Play();
                    break;
                default:
                    SystemSounds.Beep.Play();
                    break;
            }

        }
    }
}