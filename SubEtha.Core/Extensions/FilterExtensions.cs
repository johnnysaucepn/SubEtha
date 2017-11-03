using System.Collections.Generic;

namespace SubEtha.Core.Extensions
{
    public static class FilterExtensions
    {
        public static IEnumerable<T> RemoveSequentialRepeats<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            using (var iterator = source.GetEnumerator())
            {
                comparer = comparer ?? EqualityComparer<T>.Default;

                if (!iterator.MoveNext())
                    yield break;

                var current = iterator.Current;
                yield return current;

                while (iterator.MoveNext())
                {
                    if (comparer.Equals(iterator.Current, current))
                        continue;

                    current = iterator.Current;
                    yield return current;
                }
            }
        }
    }
}