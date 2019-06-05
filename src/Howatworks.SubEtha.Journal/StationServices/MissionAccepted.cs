using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class MissionAccepted : JournalEntryBase
    {
        public string Name { get; set; }
        public string LocalisedName { get; set; } // NOTE: not Name_Localised
        public string Faction { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MissionID { get; set; }
        public string Influence { get; set; } // TODO: enum (None/Low/Med/High) (+/++/++/etc.)
        public string Reputation { get; set; } // TODO: enum (None/Low/Med/High) (+/++/++/etc.)
        public long Reward { get; set; }
        public bool Wing { get; set; }

        #region Optional
        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public DateTimeOffset? Expiry { get; set; }
        public string DestinationSystem { get; set; }
        public string DestinationStation { get; set; }
        public int? PassengerCount { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool? PassengerVIPs { get; set; }
        public bool? PassengerWanted { get; set; }
        public string PassengerType { get; set; } // TODO: localised?
        #endregion

    }
}
