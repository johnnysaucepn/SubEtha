using System;
using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkSessionEntityRepository : EntityFrameworkStateEntityRepository<SessionStateEntity>, ISessionEntityRepository
    {
        public EntityFrameworkSessionEntityRepository(DbContext db) : base(db)
        {
        }

        public IEnumerable<string> EnumerateGameVersions(string user)
        {
            return Db.Set<SessionStateEntity>()
                .Where(x => x.GameContext.User == user)
                .Select(x => x.GameContext.GameVersion)
                .Distinct()
                //.AsEnumerable() // MongoDB doesn't support re-ordering after the query
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> EnumerateUsers()
        {
            return Db.Set<SessionStateEntity>()
                .Select(x => x.GameContext.User)
                .Distinct()
                //.AsEnumerable() // MongoDB doesn't support re-ordering after the query
                .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}