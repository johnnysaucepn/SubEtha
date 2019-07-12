using System;
using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkSessionEntityRepository : EntityFrameworkStateEntityRepository<SessionStateEntity>, ISessionEntityRepository
    {
        public EntityFrameworkSessionEntityRepository(MatrixDbContext db) : base(db)
        {
        }

        public IEnumerable<string> EnumerateGameVersions(string cmdrName)
        {
            return Db.Set<SessionStateEntity>()
                .Where(x => x.GameContext.CommanderName == cmdrName)
                .Select(x => x.GameContext.GameVersion)
                .Distinct()
                //.AsEnumerable() // MongoDB doesn't support re-ordering after the query
                //.OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
                .OrderBy(x => x);
        }

        public IEnumerable<string> EnumerateUsers()
        {
            return Db.Set<SessionStateEntity>()
                .Select(x => x.GameContext.CommanderName)
                .Distinct()
                //.AsEnumerable() // MongoDB doesn't support re-ordering after the query
                //.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase);
                .OrderBy(x => x);
        }
    }
}