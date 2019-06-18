using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class SessionManager
    {
        private readonly IUploader<SessionState> _client;
        // ReSharper disable once UnusedMember.Local


        private SessionState _session;
        private bool _isDirty;

        public SessionManager(JournalEntryRouter router, IUploader<SessionState> client)
        {
            _client = client;

            _session = new SessionState();

            router.RegisterFor<LoadGame>(ApplyLoadGame);
            router.RegisterFor<NewCommander>(ApplyNewCommander);
            router.RegisterFor<ClearSavedGame>(ApplyClearSavedGame);
            router.RegisterFor<FileHeader>(ApplyFileHeader);

            router.RegisterEndBatch(BatchComplete);
        }

        private bool ApplyLoadGame(LoadGame loadGame, BatchMode mode)
        {
            _session = new SessionState
            {
                CommanderName = loadGame.Commander,
                GameMode = loadGame.GameMode,
                Group = loadGame.Group
            };
            Updated(loadGame);
            return true;
        }

        private bool ApplyNewCommander(NewCommander newCommander, BatchMode mode)
        {
            _session = new SessionState
            {
                CommanderName = newCommander.Name
            };
            Updated(newCommander);
            return true;
        }

        private bool ApplyClearSavedGame(ClearSavedGame clearSavedGame, BatchMode mode)
        {
            _session = new SessionState
            {
                CommanderName = clearSavedGame.Name
            };
            Updated(clearSavedGame);
            return true;
        }

        private bool ApplyFileHeader(FileHeader fileHeader, BatchMode mode)
        {
            _session.Build = fileHeader.Build;
            // Not worth an update on its own
            return false;
        }

        private void Updated(IJournalEntry entry)
        {
            _session.TimeStamp = entry.Timestamp;
            _isDirty = true;
        }

        private bool BatchComplete(BatchMode mode)
        {
            if (!_isDirty) return false;

            _client.Upload(_session);
            _isDirty = false;
            return true;
        }

    }
}
