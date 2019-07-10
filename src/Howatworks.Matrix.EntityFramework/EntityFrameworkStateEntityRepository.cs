using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkStateEntityRepository<T> : EntityFrameworkRepository<T>, IStateEntityRepository<T>
        where T : MatrixEntity, IState, IGameContextEntity
    {
        public EntityFrameworkStateEntityRepository(DbContext db) : base(db)
        {
        }

        public IEnumerable<T> GetRange(string user, string gameVersion, int skip, int take)
        {
            var expression = QueryByVersion(user, gameVersion);
            return Db.Set<T>()
                .Where(expression)
                .Skip(skip)
                .Take(take);
        }

        public T GetMostRecent(string user, string gameVersion)
        {
            var expression = QueryByVersion(user, gameVersion);

            return Db.Set<T>()
                .Where(expression)
                .OrderByDescending(x => x.TimeStamp)
                .FirstOrDefault();
        }

        public T GetAtDateTime(string user, string gameVersion, DateTimeOffset at)
        {
            var expression = QueryByVersion(user, gameVersion);

            return Db.Set<T>()
                .Where(expression)
                .Where(x => x.TimeStamp <= at)
                .OrderByDescending(x => x.TimeStamp)
                .FirstOrDefault();
        }

        public IEnumerable<DateTimeOffset> GetTimestamps(string user, string gameVersion)
        {
            var expression = QueryByVersion(user, gameVersion);

            return Db.Set<T>()
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

        private static Expression<Func<T, bool>> QuerySpecificVersion(string user, string gameVersion) => e => e.GameContext.User == user && e.GameContext.GameVersion == gameVersion;

        private static Expression<Func<T, bool>> QueryLive(string user) => e => e.GameContext.User == user && e.GameContext.IsLive;
    }

}