using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    public interface IUploader<in T> where T : IState
    {
        string User { get; }
        string GameVersion { get; }

        void Upload(T state);
    }
}