using System.Collections.Generic;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public interface ISessionEntityRepository : IStateEntityRepository<SessionStateEntity>
    {
        IEnumerable<string> EnumerateGameVersions(string user);
        IEnumerable<string> EnumerateUsers();
    }
}