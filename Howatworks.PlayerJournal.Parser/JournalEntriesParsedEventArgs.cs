using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalEntriesParsedEventArgs : EventArgs
    {
        public JournalEntriesParsedEventArgs(IEnumerable<IJournalEntry> journalEntries, BatchMode mode)
        {
            Entries = journalEntries;
            BatchMode = mode;
        }

        public IEnumerable<IJournalEntry> Entries { get; set; }
        public BatchMode BatchMode { get; set; }
    }
}
