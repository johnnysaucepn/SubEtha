using System;
using System.Collections.Generic;

namespace Howatworks.Matrix.Core.Entities
{
    public class Group : MatrixEntity
    {
        public const string DefaultGroupName = "Default";

        public string Name { get; set; }

        public List<MatrixIdentityUser> Users { get; set; }

        public Group(string name)
        {
            Name = name;
            Users = new List<MatrixIdentityUser>();
        }
    }
}
