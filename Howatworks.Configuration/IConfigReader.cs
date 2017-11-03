using System.Collections.Generic;

namespace Howatworks.Configuration
{
    public interface IConfigReader
    {
        T Get<T>(params string[] parts);
        IEnumerable<T> GetList<T>(params string[] parts);
    }
}