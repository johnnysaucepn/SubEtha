using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Core
{
    public interface IBatchPolicy
    {
        bool Accepts(BatchMode mode);
    }
}