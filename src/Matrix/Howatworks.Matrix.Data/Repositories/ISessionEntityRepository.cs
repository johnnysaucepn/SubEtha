using System.Collections.Generic;
using Howatworks.Matrix.Data.Entities;

namespace Howatworks.Matrix.Data.Repositories
{
    public interface ISessionEntityRepository : IStateEntityRepository<SessionStateEntity>
    {
        IEnumerable<string> EnumerateGameVersions(string cmdrName);
        IEnumerable<string> EnumerateUsers();
    }
}