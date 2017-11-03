using System.Collections.Generic;

namespace SubEtha.Domain
{
    public class StarSystemComparer : IEqualityComparer<ILocationState>
    {
        public bool Equals(ILocationState x, ILocationState y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.StarSystem?.Name == y.StarSystem?.Name;
        }

        public int GetHashCode(ILocationState obj)
        {
            return obj.StarSystem.GetHashCode();
        }
    }
}