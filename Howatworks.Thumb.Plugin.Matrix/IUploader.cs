using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public interface IUploader<in T> where T : IState
    {
        string User { get; }
        string GameVersion { get; }

        void Upload(T state);
    }
}