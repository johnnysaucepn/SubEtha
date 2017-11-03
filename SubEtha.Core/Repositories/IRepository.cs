using System;
using System.Collections.Generic;
using System.Linq;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public interface IRepository<T> where T: IEntity
    {
        void Add(T entity);

        T Get(Guid id);

        void Update(T entity);

        void Remove(T entity);
        void Remove(Guid id);

        IQueryable<T> Query();
    }
}
