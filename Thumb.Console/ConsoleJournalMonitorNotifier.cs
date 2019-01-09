using Thumb.Core;
using Thumb.Plugin;

namespace Thumb.Console
{
    public class ConsoleJournalMonitorNotifier : IJournalMonitorNotifier
    {
        public void StartedWatchingFile(string path)
        {
            System.Console.Beep(524, 500);
        }

        public void StoppedWatchingFile(string path)
        {
            System.Console.Beep(262, 500);
        }

        public void UpdatedService(object state)
        {
            System.Console.Beep(1000, 100);
        }
    }
}