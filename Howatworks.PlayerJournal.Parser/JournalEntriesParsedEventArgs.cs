using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalEntriesParsedEventArgs : EventArgs
    {
        public JournalEntriesParsedEventArgs(IEnumerable<JournalEntryBase> entries, BatchMode mode)
        {
            Entries = entries;
            BatchMode = mode;
        }

        public IEnumerable<JournalEntryBase> Entries { get; set; }
        public BatchMode BatchMode { get; set; }
    }
}
