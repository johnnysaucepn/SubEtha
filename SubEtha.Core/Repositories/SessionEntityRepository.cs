using System;
using System.Collections.Generic;
using System.Linq;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public class SessionEntityRepository : StateEntityRepository<SessionStateEntity>, ISessionEntityRepository
    {
        public SessionEntityRepository(IDbContext<SessionStateEntity> db) : base(db)
        {
        }

        public IEnumerable<string> EnumerateGameVersions(string user)
        {
            return Db.AsQueryable()
                .Where(x => x.GameContext.User == user)
                .Select(x => x.GameContext.GameVersion)
                .Distinct()
                .AsEnumerable() // MongoDB doesn't support re-ordering after the query
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> EnumerateUsers()
        {
            return Db.AsQueryable()
                .Select(x => x.GameContext.User)
                .Distinct()
                .AsEnumerable() // MongoDB doesn't support re-ordering after the query
                .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}