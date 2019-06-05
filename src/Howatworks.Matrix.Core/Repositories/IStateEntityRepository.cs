using System;
using System.Collections.Generic;
using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.Core.Repositories
{
    public interface IStateEntityRepository<T> : IRepository<T> where T : IEntity
    { 
        IEnumerable<T> GetRange(string user, string gameVersion, int skip, int take);
        T GetMostRecent(string user, string gameVersion);
        T GetAtDateTime(string user, string gameVersion, DateTimeOffset at);
        IEnumerable<DateTimeOffset> GetTimestamps(string user, string gameVersion);
    }
}