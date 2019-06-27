using System;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.MongoDb
{
    public abstract class MongoDbRepository<T> : IRepository<T> where T : IEntity
    {
        protected readonly MongoDbContext<T> Db;

        protected MongoDbRepository(MongoDbContext<T> db)
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
