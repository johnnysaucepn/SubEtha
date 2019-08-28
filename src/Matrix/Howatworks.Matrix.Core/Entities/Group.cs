using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Howatworks.Matrix.Core.Entities
{
    public class Group : IMatrixEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public const string DefaultGroupName = "Default";

        public string Name { get; set; }

        public ICollection<CommanderGroup> CommanderGroups { get; set; }

        public Group(string name)
        {
            Name = name;
            CommanderGroups = new List<CommanderGroup>();
        }
    }
}
