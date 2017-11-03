using System;
using System.Collections.Generic;
using System.Linq;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public interface IDbContext<T> where T : IEntity
    {
        void Create(T entity);

        T Read(Guid id);
        IEnumerable<T> Read(IEnumerable<Guid> ids);
        IEnumerable<T> Read(params Guid[] ids);

        void Update(IEnumerable<T> entities);
        void Update(params T[] entities);

        void Delete(IEnumerable<T> entities);
        void Delete(params T[] entities);
        void Delete(IEnumerable<Guid> ids);
        void Delete(params Guid[] ids);

        IEnumerable<T> CreateOrUpdate(IEnumerable<T> entities);
        IEnumerable<T> CreateOrUpdate(params T[] entities);

        IQueryable<T> AsQueryable();

    }
}