using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Howatworks.Matrix.Data.Entities
{
    public class SessionStateEntity : IMatrixEntity, IGameContextEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
        public string GameVersion { get; set; }
        public string CommanderName { get; set; }

        public string Build { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
    }
}
