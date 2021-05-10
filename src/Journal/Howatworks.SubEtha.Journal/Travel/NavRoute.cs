using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    /// <summary>
    /// Note that the docs refer to a Route event, written to Route.json.
    /// At the time of writing, this is actually NavRoute.json, written to NavRoute.json.
    /// In addition, the event written to the journal log is a stub with no properties, while NavRoute.json contains all data.
    /// This means that all the below properties must be nullable.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NavRoute : JournalEntryBase
    {
        public class RouteItem
        {
            public string StarSystem { get; set; }
            public long? SystemAddress { get; set; } // NOTE: as noted in the docs, this was corrected for release
            public List<decimal> StarPos { get; set; }
            public string StarClass { get; set; } // TODO: enum
        }

        public List<RouteItem> Route { get; set; }
    }
}
