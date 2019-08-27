using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public interface IUploader<in T> where T : IState
    {
        void Upload(GameContext context, T state);
    }
}