using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;

namespace Howatworks.Matrix.InMemory
{
    public class InMemoryStateEntityRepository<T> : InMemoryRepository<T>, IStateEntityRepository<T>
        where T : MatrixEntity, IState, IGameContextEntity
    {
        public InMemoryStateEntityRepository(InMemoryDbContext<T> db) : base(db)
        {
        }

        public IEnumerable<T> GetRange(string cmdrName, string gameVersion, int skip, int take)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);
            return Db.AsQueryable()
                .Where(expression)
                .Skip(skip)
                .Take(take);
        }

        public T GetMostRecent(string cmdrName, string gameVersion)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);

            return Db.AsQueryable()
                .Where(expression)
                .OrderByDescending(x => x.TimeStamp)
                .FirstOrDefault();
        }

        public T GetAtDateTime(string cmdrName, string gameVersion, DateTimeOffset at)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);

            return Db.AsQueryable()
                .Where(expression)
                .Where(x => x.TimeStamp <= at)
                .OrderByDescending(x => x.TimeStamp)
                .FirstOrDefault();
        }

        public IEnumerable<DateTimeOffset> GetTimestamps(string cmdrName, string gameVersion)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);

            return Db.AsQueryable()
                .Where(expression)
                .Select(x => x.TimeStamp);
        }

        private static Expression<Func<T, bool>> QueryByVersion(string user, string gameVersion)
        {
            Expression<Func<T, bool>> expression;
            switch (gameVersion)
            {
                case "Live":
                    expression = QueryLive(user);
                    break;
                default:
                    expression = QuerySpecificVersion(user, gameVersion);
                    break;
            }
            return expression;
        }

        private static Expression<Func<T, bool>> QuerySpecificVersion(string user, string gameVersion) => e => e.GameContext.CommanderName == user && e.GameContext.GameVersion == gameVersion;

        private static Expression<Func<T, bool>> QueryLive(string user) => e => e.GameContext.CommanderName == user && e.GameContext.IsLive;
    }

}