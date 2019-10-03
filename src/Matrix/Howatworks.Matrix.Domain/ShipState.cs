using System;

namespace Howatworks.Matrix.Domain
{
    public class ShipState : IShipState, ICloneable<ShipState>
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

        public ShipState Clone()
        {
            return new ShipState
            {
                TimeStamp = this.TimeStamp,
                Type = this.Type,
                ShipId = this.ShipId,
                Name = this.Name,
                Ident = this.Ident,
                ShieldsUp = this.ShieldsUp,
                HullIntegrity = this.HullIntegrity
            };
        }
    }
}
