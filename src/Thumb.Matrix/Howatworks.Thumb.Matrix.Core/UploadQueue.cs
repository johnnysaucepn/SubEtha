using System;
using System.Collections.Concurrent;
using Howatworks.Matrix.Domain;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Matrix.Core
{
    public abstract class UploadQueue<T> where T : IState
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UploadQueue<T>));

        protected class Item
        {
            public string GameVersion { get; }
            public string CommanderName { get; }
            public T State { get; }

            public Item(string gameVersion, string cmdrName, T state)
            {
                GameVersion = gameVersion;
                CommanderName = cmdrName;
                State = state;
            }
        }

        protected readonly HttpUploadClient Client;

        protected readonly ConcurrentQueue<Item> Queue = new ConcurrentQueue<Item>();

        protected UploadQueue(HttpUploadClient client)
        {
            Client = client;
        }

        public void Enqueue(string gameVersion, string cmdrName, T state)
        {
            Log.Info($"Adding {typeof(T).Name} '{gameVersion}' to queue");
            Log.Debug(JsonConvert.SerializeObject(state));

            Queue.Enqueue(new Item(gameVersion, cmdrName, state));
        }

        public void Flush()
        {
            while (Queue.TryPeek(out var item))
            {
                Client.Upload(BuildUri(item), item.State);
                if (!Queue.TryDequeue(out var removedItem) || item.State.TimeStamp != removedItem.State.TimeStamp)
                {
                    throw new MatrixQueueException("Head of queue changed during processing");
                }
            }
        }

        protected abstract Uri BuildUri(Item item);
    }
}