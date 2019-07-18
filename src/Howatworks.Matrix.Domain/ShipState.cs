using System;

namespace Howatworks.Matrix.Domain
{
    public class ShipState : IShipState
    {
        public DateTimeOffset TimeStamp { get; set; }

        public string Type { get; set; }
        public int ShipId { get; set; }
        public string Name { get; set; }
        public string Ident { get; set; }
        public bool? ShieldsUp { get; set; }
        public decimal? HullIntegrity { get; set; }

        public ShipState()
        {
            HullIntegrity = 1;
        }
    }
}
