using System;
using Howatworks.Matrix.Domain;

namespace Howatworks.Matrix.Core.Entities
{
    public class ShipStateEntity : IEntity, IGameContextEntity, IShipState
    {
        public Guid Id { get; set; }

        public GameContext GameContext { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public int ShipID { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }
        public string Ident { get; set; }
        public bool? ShieldsUp { get; set; }
        public decimal? HullIntegrity { get; set; }
    }
}
