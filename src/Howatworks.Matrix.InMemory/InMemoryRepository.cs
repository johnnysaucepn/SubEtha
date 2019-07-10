using System;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.InMemory
{
    public abstract class InMemoryRepository<T> : IRepository<T, long> where T : MatrixEntity
    {
        protected readonly InMemoryDbContext<T> Db;

        protected InMemoryRepository(InMemoryDbContext<T> db)
        {
            Db = db;
        }

        public virtual void Add(T entity)
        {
            Db.Create(entity);
        }

        public virtual T Get(long id)
        {
            return Db.Read(id);
        }

        public virtual void Remove(long id)
        {
            Db.Delete(id);
        }

        public virtual void Remove(T entity)
        {
            Db.Delete(entity);
        }

        public virtual void Update(T entity)
        {
            Db.CreateOrUpdate(entity);
        }

        public virtual IQueryable<T> Query()
        {
            return Db.AsQueryable();
        }
    }
}
