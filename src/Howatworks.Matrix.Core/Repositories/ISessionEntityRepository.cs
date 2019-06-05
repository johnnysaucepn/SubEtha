using System.Collections.Generic;
using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.Core.Repositories
{
    public interface ISessionEntityRepository : IStateEntityRepository<SessionStateEntity>
    {
        IEnumerable<string> EnumerateGameVersions(string user);
        IEnumerable<string> EnumerateUsers();
    }
}