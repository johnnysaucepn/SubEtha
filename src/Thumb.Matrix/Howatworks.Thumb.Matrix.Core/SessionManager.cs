using System;
using System.Reactive.Linq;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Monitor;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class SessionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionManager));

        private readonly Tracker<SessionState> _tracker = new Tracker<SessionState>();
        public IObservable<SessionState> Observable => _tracker.Observable;

        public void SubscribeTo(IObservable<JournalEntry> observable)
        {
            observable.OfJournalType<LoadGame>().Subscribe(ApplyLoadGame);
            observable.OfJournalType<NewCommander>().Subscribe(ApplyNewCommander);
            observable.OfJournalType<ClearSavedGame>().Subscribe(ApplyClearSavedGame);
            observable.OfJournalType<FileHeader>().Subscribe(ApplyFileHeader);
        }

        private void ApplyLoadGame(LoadGame loadGame)
        {
            _tracker.Replace(loadGame.Timestamp, x =>
            {
                x.CommanderName = loadGame.Commander;
                x.GameMode = loadGame.GameMode;
                x.Group = loadGame.Group;
            });
        }

        private void ApplyNewCommander(NewCommander newCommander)
        {
            _tracker.Replace(newCommander.Timestamp, x =>
            {
                x.CommanderName = newCommander.Name;
            });
        }

        private void ApplyClearSavedGame(ClearSavedGame clearSavedGame)
        {
            _tracker.Replace(clearSavedGame.Timestamp, x =>
            {
                x.CommanderName = clearSavedGame.Name;
            });
        }

        private void ApplyFileHeader(FileHeader fileHeader)
        {
            _tracker.Modify(fileHeader.Timestamp, x =>
            {
                x.Build = fileHeader.Build;
            });
        }
    }
}
