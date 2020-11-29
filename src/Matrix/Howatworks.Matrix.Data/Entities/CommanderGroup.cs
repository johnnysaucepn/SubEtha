using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Howatworks.Matrix.Data.Entities
{
    public class CommanderGroup : IMatrixEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CommanderName { get; set; }

        public Group Group { get; set; }
    }
}
