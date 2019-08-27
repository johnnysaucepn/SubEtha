using System.Collections.Generic;

namespace Howatworks.Matrix.Core.Entities
{
    public class StarSystemEntityComparer : IEqualityComparer<LocationStateEntity>
    {
        public bool Equals(LocationStateEntity x, LocationStateEntity y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.StarSystem_Name == y.StarSystem_Name;
        }

        public int GetHashCode(LocationStateEntity obj)
        {
            return obj.StarSystem_Name.GetHashCode();
        }
    }
}