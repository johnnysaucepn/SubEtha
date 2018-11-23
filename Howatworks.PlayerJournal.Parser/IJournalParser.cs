using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalParser
    {
        IJournalEntry Parse(string eventType, string line);
    }
}