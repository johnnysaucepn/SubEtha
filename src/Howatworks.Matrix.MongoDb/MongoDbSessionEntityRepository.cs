using System;
using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.MongoDb
{
    public class MongoDbSessionEntityRepository : MongoDbStateEntityRepository<SessionStateEntity>, ISessionEntityRepository
    {
        public MongoDbSessionEntityRepository(MongoDbContext<SessionStateEntity> db) : base(db)
        {
        }

        public IEnumerable<string> EnumerateGameVersions(string cmdrName)
        {
            return Db.AsQueryable()
                .Where(x => x.GameContext.CommanderName == cmdrName)
                .Select(x => x.GameContext.GameVersion)
                .Distinct()
                .AsEnumerable() // MongoDB doesn't support re-ordering after the query
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> EnumerateUsers()
        {
            return Db.AsQueryable()
                .Select(x => x.GameContext.CommanderName)
                .Distinct()
                .AsEnumerable() // MongoDB doesn't support re-ordering after the query
                .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}