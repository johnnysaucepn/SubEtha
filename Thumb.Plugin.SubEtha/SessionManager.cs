using Howatworks.PlayerJournal;
using Howatworks.PlayerJournal.Processing;
using Howatworks.PlayerJournal.Startup;

namespace Thumb.Plugin.SubEtha
{
    public class SessionManager : IJournalProcessor
    {
        private readonly JournalEntryRouter _entryRouter;
        private readonly IUploader<SessionState> _client;
        // ReSharper disable once UnusedMember.Local


        private SessionState _session;
        private bool _isDirty;

        public SessionManager(IUploader<SessionState> client)
        {
            _entryRouter = new JournalEntryRouter();
            _client = client;

            _session = new SessionState();

            _entryRouter.RegisterFor<LoadGame>(ApplyLoadGame);
            _entryRouter.RegisterFor<NewCommander>(ApplyNewCommander);
            _entryRouter.RegisterFor<ClearSavedGame>(ApplyClearSavedGame);
            _entryRouter.RegisterFor<FileHeader>(ApplyFileHeader);

        }

        private bool ApplyLoadGame(LoadGame loadGame)
        {
            _session = new SessionState
            {
                CommanderName = loadGame.Commander,
                GameMode = loadGame.GameMode,
                Group = loadGame.Group
            };
            return true;
        }

        private bool ApplyNewCommander(NewCommander newCommander)
        {
            _session = new SessionState
            {
                CommanderName = newCommander.Name
            };
            return true;
        }

        private bool ApplyClearSavedGame(ClearSavedGame clearSavedGame)
        {
            _session = new SessionState
            {
                CommanderName = clearSavedGame.Name
            };
            return true;
        }

        private bool ApplyFileHeader(FileHeader fileHeader)
        {
            _session.Build = fileHeader.Build;
            // Not worth an update on its own
            return false;
        }

        public bool Apply(JournalEntryBase entry)
        {
            if (!_entryRouter.Apply(entry)) return false;
            _session.TimeStamp = entry.TimeStamp;
            _isDirty = true;
            return true;
        }

        public void Flush()
        {
            if (!_isDirty) return;

            _client.Upload(_session);
            _isDirty = false;
        }

    }
}
