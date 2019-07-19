using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public interface IUploader<in T> where T : IState
    {
        void Upload(GameContext context, T state);
    }
}