using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public interface IUploader<in T> where T : IState
    {
        void Upload(GameContext context, T state);
    }
}