using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Monitor
{
    public class JournalEntriesParsedEventArgs : EventArgs
    {
        public JournalEntriesParsedEventArgs(IList<IJournalEntry> journalEntries, BatchMode mode)
        {
            Entries = journalEntries;
            BatchMode = mode;
        }

        public IList<IJournalEntry> Entries { get; set; }
        public BatchMode BatchMode { get; set; }
    }
}
