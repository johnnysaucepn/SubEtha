using System.Diagnostics.CodeAnalysis;
using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class DummyUploader<T> : IUploader<T> where T : IState
    {
        public string User { get; }
        public string GameVersion { get; }

        public void Upload(T state)
        {
            // do nothing
        }
    }
}
