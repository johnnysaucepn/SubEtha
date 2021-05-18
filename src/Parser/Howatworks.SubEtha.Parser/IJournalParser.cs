using System;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public interface IJournalParser
    {
        JournalResult<IJournalEntry> Parse(string eventType, string line);
        JournalResult<T> Parse<T>(string line) where T : class, IJournalEntry;
        JournalResult<IJournalEntry> Parse(string line);

        /// <summary>
        /// Parse the absolute minimum required for an entry - saves time in deserialising
        /// to a specific type too early.
        /// </summary>
        /// <param name="line"></param>
        (string eventType, DateTimeOffset timestamp) ParseCommonProperties(string line);
    }
}