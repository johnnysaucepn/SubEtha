using System;
using System.Linq;
using Howatworks.Matrix.Data.Entities;

namespace Howatworks.Matrix.Data.Repositories
{
    public interface IRepository<T, in TId> where T : IMatrixEntity
    {
        void Add(T entity);

        T Get(TId id);

        void Update(T entity);

        void Remove(T entity);
        void Remove(TId id);

        IQueryable<T> Query();
    }
}
