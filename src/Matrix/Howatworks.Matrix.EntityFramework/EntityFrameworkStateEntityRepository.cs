using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkStateEntityRepository<T> : EntityFrameworkRepository<T>, IStateEntityRepository<T>
        where T : class, IMatrixEntity, IGameContextEntity
    {
        public EntityFrameworkStateEntityRepository(MatrixDbContext db) : base(db)
        {
        }

        public IEnumerable<T> GetRange(string cmdrName, string gameVersion, int skip, int take)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);
            return Db.Set<T>()
                .Where(expression)
                .Skip(skip)
                .Take(take);
        }

        public T GetMostRecent(string cmdrName, string gameVersion)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);

            return Db.Set<T>()
                .Where(expression)
                .OrderByDescending(x => x.TimeStamp)
                .FirstOrDefault();
        }

        public T GetAtDateTime(string cmdrName, string gameVersion, DateTimeOffset at)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);

            return Db.Set<T>()
                .Where(expression)
                .Where(x => x.TimeStamp <= at)
                .OrderByDescending(x => x.TimeStamp)
                .FirstOrDefault();
        }

        public IEnumerable<DateTimeOffset> GetTimestamps(string cmdrName, string gameVersion)
        {
            var expression = QueryByVersion(cmdrName, gameVersion);

            return Db.Set<T>()
                .Where(expression)
                .Select(x => x.TimeStamp);
        }

        private static Expression<Func<T, bool>> QueryByVersion(string cmdrName, string gameVersion)
        {
            Expression<Func<T, bool>> expression;
            switch (gameVersion)
            {
                case "Live":
                    expression = QueryLive(cmdrName);
                    break;
                default:
                    expression = QuerySpecificVersion(cmdrName, gameVersion);
                    break;
            }
            return expression;
        }

        private static Expression<Func<T, bool>> QuerySpecificVersion(string cmdrName, string gameVersion) => e => e.CommanderName == cmdrName && e.GameVersion == gameVersion;

        private static Expression<Func<T, bool>> QueryLive(string cmdrName) => e => e.CommanderName == cmdrName && IsLive(e.GameVersion);

        private static bool IsLive(string gameVersion)
        {
            return !gameVersion.Contains("Beta");
        }
    }

}