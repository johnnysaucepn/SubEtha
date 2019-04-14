using System;
using System.Collections.Generic;

namespace Howatworks.Matrix.Core.Entities
{
    public class Group : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<string> Users { get; set; }

        public Group(string name)
        {
            Name = name;
            Users = new List<string>();
        }
    }
}
