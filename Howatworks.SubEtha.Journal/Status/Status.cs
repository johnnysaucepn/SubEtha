using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Status
{
    public class Status : JournalEntryBase
    {
        public class FuelItem
        {
            public decimal FuelMain { get; set; }
            public decimal FuelReservoir { get; set; }
        }

        public StatusFlags Flags { get; set; }
        public List<int> Pips { get; set; } // TODO: measured in half-pips, consider converting
        public int Firegroup { get; set; } // TODO: check spelling

        public GuiFocus GuiFocus { get; set; }
        public FuelItem Fuel { get; set; }
        public decimal? Cargo { get; set; }
        public string LegalState { get; set; } // TODO: enum? ("Clean","IllegalCargo","Speeding","Wanted","Hostile","PassengerWanted","Warrant")
        public decimal? Latitude { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Heading { get; set; } // TODO: check data type
        public string BodyName { get; set; }
        public decimal? PlanetRadius { get; set; } // TODO: check data type

        public bool HasFlag(StatusFlags flag)
        {
            return (Flags & flag) != 0;
        }
    }
}
