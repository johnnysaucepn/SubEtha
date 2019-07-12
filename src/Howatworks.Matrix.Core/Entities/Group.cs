using System;
using System.Collections.Generic;

namespace Howatworks.Matrix.Core.Entities
{
    public class Group : MatrixEntity
    {
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
