using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class SessionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionManager));

        private readonly UploadQueue<SessionState> _queue;
        private readonly Tracker<SessionState> _tracker;

        public SessionManager(JournalEntryRouter router, CommanderTracker commander, UploadQueue<SessionState> queue)
        {
            _tracker = new Tracker<SessionState>(commander);
            _queue = queue;

            router.RegisterFor<LoadGame>(ApplyLoadGame);
            router.RegisterFor<NewCommander>(ApplyNewCommander);
            router.RegisterFor<ClearSavedGame>(ApplyClearSavedGame);
            router.RegisterFor<FileHeader>(ApplyFileHeader);

            router.RegisterForBatchComplete(BatchComplete);
        }

        public void FlushQueue()
        {
            _queue.Flush();
        }

        private bool ApplyLoadGame(LoadGame loadGame)
        {
            return _tracker.Replace(loadGame.Timestamp, x =>
            {
                x.CommanderName = loadGame.Commander;
                x.GameMode = loadGame.GameMode;
                x.Group = loadGame.Group;
                return true;
            });
        }

        private bool ApplyNewCommander(NewCommander newCommander)
        {
            return _tracker.Replace(newCommander.Timestamp, x =>
            {
                x.CommanderName = newCommander.Name;
                return true;
            });
        }

        private bool ApplyClearSavedGame(ClearSavedGame clearSavedGame)
        {
            return _tracker.Replace(clearSavedGame.Timestamp, x =>
            {
                x.CommanderName = clearSavedGame.Name;
                return true;
            });
        }

        private bool ApplyFileHeader(FileHeader fileHeader)
        {
            return _tracker.Modify(fileHeader.Timestamp, x =>
            {
                x.Build = fileHeader.Build;
                return true;
            });
        }

        private bool BatchComplete()
        {
            _tracker.Commit(() => { _queue.Enqueue(_tracker.GameVersion, _tracker.CommanderName, _tracker.CurrentState); });

            return true;
        }
    }
}
