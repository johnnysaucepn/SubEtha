using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.Matrix.Core.Entities
{
    public class CommanderGroup : MatrixEntity
    {
        public string CommanderName { get; set; }

        public long GroupId { get; set; }
        public Group Group { get; set; }
    }
}
