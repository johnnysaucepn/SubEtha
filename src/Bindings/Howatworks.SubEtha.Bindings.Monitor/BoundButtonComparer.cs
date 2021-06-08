using Howatworks.SubEtha.Common;
using Howatworks.SubEtha.Common.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    internal class BoundButtonComparer : IEqualityComparer<BoundButton>
    {
        public bool Equals(BoundButton x, BoundButton y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.BindingName == y.BindingName;
        }

        public int GetHashCode(BoundButton obj)
        {
            return obj.BindingName.GetHashCode();
        }
    }
}
