using System.Collections.Concurrent;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public abstract class UploadQueue<T> where T : IState
    {
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

        public abstract void Enqueue(string gameVersion, string cmdrName, T state);
        public abstract void Flush();
    }
}