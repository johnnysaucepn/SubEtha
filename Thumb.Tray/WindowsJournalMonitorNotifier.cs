using System.Media;
using Thumb.Core;

namespace Thumb.Tray
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