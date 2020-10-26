using System;

namespace Howatworks.Matrix.Domain
{
    /// <summary>
    /// This class is not suitable for heavy duty comparison such as collection keys or distinct lists.
    /// It makes no use of GetHashCode() other than that which may be used by the underlying types.
    /// </summary>
    public static class SimpleComparer
    {
        public static bool Equals<T>(T a, T b) where T : class, IEquatable<T>
        {
            if (ReferenceEquals(a, b)) return true;
            if ((a is null) || (b is null)) return false;

            return a.Equals(b);
        }
    }
}
