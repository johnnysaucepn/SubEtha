using System.Media;
using Howatworks.Thumb.Plugin;

namespace Howatworks.Thumb.Tray
{
    public class WindowsJournalMonitorNotifier : IJournalMonitorNotifier
    {
        public void StartedWatchingFile(string path)
        {
            SystemSounds.Beep.Play();
        }

        public void StoppedWatchingFile(string path)
        {
            SystemSounds.Asterisk.Play();
        }

        public void UpdatedService(object state)
        {
            SystemSounds.Hand.Play();
        }
    }
}