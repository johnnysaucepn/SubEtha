using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class EngineerProgress : JournalEntryBase
    {
        #region Summary at startup

        public class EngineerProgressItem
        {
            public string Engineer { get; set; } // Note: name

            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long EngineerID { get; set; } // TODO: check datatype

            public int Rank { get; set; } // TODO: possible enum
            public string Progress { get; set; } // TODO: check data type - enum Invited/Acquainted/Unlocked/Barred
            public decimal RankProgress { get; set; } // TODO: check data type - sample suggests int
        }

        public List<EngineerProgressItem> Engineers { get; set; }

        #endregion

        #region Update one Engineer

        public string Engineer { get; set; } // Note: name

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long EngineerID { get; set; } // TODO: check datatype

        public int Rank { get; set; } // TODO: possible enum
        public string Progress { get; set; } // TODO: check data type - enum Invited/Acquainted/Unlocked/Barred
        public decimal RankProgress { get; set; } // TODO: check data type - sample suggests int

        #endregion

    }
}
