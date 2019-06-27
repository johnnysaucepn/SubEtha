using System;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public abstract class EntityFrameworkRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext Db;

        protected EntityFrameworkRepository(DbContext db)
        {
            Db = db;
        }

        public virtual void Add(T entity)
        {
            Db.Add(entity);
        }

        public virtual T Get(Guid id)
        {
            return Db.Find<T>(id);
        }

        public virtual void Remove(Guid id)
        {
            Db.Remove(Get(id));
        }

        public virtual void Remove(T entity)
        {
            Db.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            Db.Update(entity);
        }

        public virtual IQueryable<T> Query()
        {
            return Db.Set<T>();
        }
    }
}
