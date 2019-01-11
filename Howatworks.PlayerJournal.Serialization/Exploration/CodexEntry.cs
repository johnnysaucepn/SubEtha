using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class CodexEntry : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long EntryID { get; set; }
        public string Name { get; set; } // TODO: localisation
        public string SubCategory { get; set; }  // TODO: localisation
        public string Category { get; set; } // TODO: localisation
        public string Region { get; set; }
        public string System { get; set; } // Note: name
        public long SystemAddress { get; set; }
        public bool IsNewEntry { get; set; }
        public bool NewTraitsDiscovered { get; set; }
        public List<string> Traits { get; set; }
    }
}
