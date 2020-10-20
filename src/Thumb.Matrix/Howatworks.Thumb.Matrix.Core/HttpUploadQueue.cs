using System;
using System.Collections.Concurrent;
using System.Threading;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class HttpUploadQueue
    {
        private readonly HttpUploadClient _client;

        private ConcurrentQueue<(Uri, IState)> _queue = new ConcurrentQueue<(Uri, IState)>();

        public HttpUploadQueue(HttpUploadClient client)
        {
            _client = client;
        }

        public void Upload(Uri uri, IState state)
        {
            _queue.Enqueue((uri, state));
        }

        public DateTimeOffset? Flush(CancellationToken token)
        {
            DateTimeOffset? lastTimeStamp = null;

            while (!_queue.IsEmpty && !token.IsCancellationRequested)
            {
                // TODO: on failure, this may lose first item on the queue, try peeking
                if (_queue.TryDequeue(out (Uri Uri, IState State) item))
                {
                    _client.Upload(item.Uri, item.State);
                    lastTimeStamp = item.State.TimeStamp;
                }
            }
            return lastTimeStamp;
        }

    }
}
