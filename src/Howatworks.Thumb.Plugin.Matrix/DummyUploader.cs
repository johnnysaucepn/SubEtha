using System.Diagnostics.CodeAnalysis;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;

namespace Howatworks.Thumb.Plugin.Matrix
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class DummyUploader<T> : IUploader<T> where T : IState
    {
        public void Upload(GameContext context, T state)
        {
            // do nothing
        }
    }
}
