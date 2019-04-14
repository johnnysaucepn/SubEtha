using System;
using System.Linq;
using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.Core.Repositories
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
