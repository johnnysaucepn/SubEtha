using System;
using System.Linq;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        protected readonly IDbContext<T> Db;

        protected Repository(IDbContext<T> db)
        {
            Db = db;
        }

        public virtual void Add(T entity)
        {
            Db.Create(entity);
        }

        public virtual T Get(Guid id)
        {
            return Db.Read(id);
        }

        public virtual void Remove(Guid id)
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
