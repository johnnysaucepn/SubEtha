﻿using System;
using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.InMemory
{
    public class InMemorySessionEntityRepository : InMemoryStateEntityRepository<SessionStateEntity>, ISessionEntityRepository
    {
        public InMemorySessionEntityRepository(InMemoryDbContext<SessionStateEntity> db) : base(db)
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