using System;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalParser
    {
        IJournalEntry Parse(string eventType, string line);
        T Parse<T>(string line) where T : class;

        /// <summary>
        /// Parse the absolute minimum required for an entry - saves time in deserialising
        /// to a specific type too early.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        (string, DateTimeOffset) ParseCommonProperties(string line);
    }
}