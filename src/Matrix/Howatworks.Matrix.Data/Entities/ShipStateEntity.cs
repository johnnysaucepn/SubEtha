using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Howatworks.Matrix.Data.Entities
{
    public class ShipStateEntity : IMatrixEntity, IGameContextEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
        public string GameVersion { get; set; }
        public string CommanderName { get; set; }

        public int ShipId { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }
        public string Ident { get; set; }
        public bool? ShieldsUp { get; set; }
        public decimal? HullIntegrity { get; set; }
    }
}
