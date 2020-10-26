using System;

namespace Howatworks.Matrix.Domain
{
    public class ShipState : IShipState, ICloneable<ShipState>, IStateComparable<ShipState>
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

        public bool HasChangedSince(ShipState state)
        {
            if (state == null) return false;

            if (!Equals(Type, state.Type)) return true;
            if (!Equals(ShipId, state.ShipId)) return true;
            if (!Equals(Name, state.Name)) return true;
            if (!Equals(Ident, state.Ident)) return true;
            if (!Equals(ShieldsUp, state.ShieldsUp)) return true;
            if (!Equals(HullIntegrity, state.HullIntegrity)) return true;

            return false;
        }
    }
}
