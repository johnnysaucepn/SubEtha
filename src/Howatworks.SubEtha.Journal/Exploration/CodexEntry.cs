using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    public class CodexEntry : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long EntryID { get; set; }
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory_Localised { get; set; }
        public string Category { get; set; }
        public string Category_Localised { get; set; }
        public string Region { get; set; }
        public string Region_Localised { get; set; }
        public string System { get; set; } // Note: name
        public long SystemAddress { get; set; }
        public bool? IsNewEntry { get; set; }
        public bool? NewTraitsDiscovered { get; set; }
        public List<string> Traits { get; set; }
        public long? VoucherAmount { get; set; } // WARNING: undocumented
    }
}
