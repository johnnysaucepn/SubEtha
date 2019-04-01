using Howatworks.PlayerJournal.Serialization;

namespace Thumb.Plugin
{
    public interface IJournalProcessor
    {
        void Flush();
    }
}