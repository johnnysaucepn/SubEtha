using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Processing
{
    public interface IHandler
    {
        bool Invoke(IJournalEntry journal);
    }
}