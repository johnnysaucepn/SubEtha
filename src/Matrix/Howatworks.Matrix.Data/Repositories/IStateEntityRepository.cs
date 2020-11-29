using System;
using System.Collections.Generic;
using Howatworks.Matrix.Data.Entities;

namespace Howatworks.Matrix.Data.Repositories
{
    public interface IStateEntityRepository<T> : IRepository<T, long> where T : IMatrixEntity
    {
        IEnumerable<T> GetRange(string cmdrName, string gameVersion, int skip, int take);
        T GetMostRecent(string cmdrName, string gameVersion);
        T GetAtDateTime(string cmdrName, string gameVersion, DateTimeOffset at);
        IEnumerable<DateTimeOffset> GetTimestamps(string cmdrName, string gameVersion);
    }
}