using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Assistant.Core
{
    public class AssistantApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantApp));

        private readonly IJournalMonitorState _state;

        private readonly LiveJournalMonitor _monitor;
        private readonly IThumbNotifier _notifier;
        private readonly IJournalParser _parser;

        private readonly AssistantMessageHub _processor;

        private readonly StatusManager _statusManager;

        private readonly Subject<DateTimeOffset> _updateSubject = new Subject<DateTimeOffset>();

        public IObservable<Timestamped<DateTimeOffset>> Updates => _updateSubject
                                                                    .Throttle(TimeSpan.FromSeconds(5))
                                                                    .Timestamp()
                                                                    .AsObservable();

        public AssistantApp(
            IJournalMonitorState state,
            LiveJournalMonitor monitor,
            IThumbNotifier notifier,
            IJournalParser parser,
            AssistantMessageHub processor,
            StatusManager statusManager
            )
        {
            _state = state;

            _monitor = monitor;
            _notifier = notifier;
            _parser = parser;

            _processor = processor;
            _statusManager = statusManager;
        }

        public async Task RunAsync(CancellationToken token)
        {
            Log.Info("Starting up");

            _monitor.JournalFileWatch.Where(x => x.Action == JournalWatchAction.Started).Subscribe(
                e => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{e.File.FullName}'")
            );

            _statusManager.ControlStateObservable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(_ => _notifier.Notify(NotificationPriority.High, NotificationEventType.Update, "Updated control status"));

            var startTime = _state.LastEntrySeen ?? DateTimeOffset.MinValue;
            var source = new JournalEntrySource(_parser, startTime, _monitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.Observable.Publish();

            _statusManager.SubscribeTo(publication);
            _processor.StartListening(token);

            publication.Subscribe(t => _updateSubject.OnNext(t.Entry.Timestamp));

            Updates
                .Subscribe(e =>
                {
                    var lastEntry = e.Value;
                    var lastChecked = e.Timestamp;
                    Log.Info($"Updated at {lastChecked}, last entry stamped {lastEntry}");
                    _state.Update(lastChecked, lastEntry);
                });

            using (publication.Connect())
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

                    publisher.Poll();
                }
            }
        }

        public DateTimeOffset? LastEntry => _state.LastEntrySeen;

        public DateTimeOffset? LastChecked => _state.LastChecked;
    }
}