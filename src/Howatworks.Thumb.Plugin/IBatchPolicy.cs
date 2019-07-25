using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Plugin
{
    public interface IBatchPolicy
    {
        bool Accepts(BatchMode mode);
    }
}