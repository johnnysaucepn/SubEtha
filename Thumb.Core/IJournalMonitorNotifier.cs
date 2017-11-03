namespace Thumb.Core
{
    public interface IJournalMonitorNotifier
    {
        void StartedWatchingFile(string path);
        void StoppedWatchingFile(string path);
        void UpdatedService(object state);
    }
}