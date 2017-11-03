using System;
using System.Collections.Generic;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public interface IStateEntityRepository<T> : IRepository<T> where T : IEntity
    { 
        IEnumerable<T> GetRange(string user, string gameVersion, int skip, int take);
        T GetMostRecent(string user, string gameVersion);
        T GetAtDateTime(string user, string gameVersion, DateTime at);
        IEnumerable<DateTime> GetTimestamps(string user, string gameVersion);
    }
}