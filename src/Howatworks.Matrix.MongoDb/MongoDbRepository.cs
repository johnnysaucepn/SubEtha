using System;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.MongoDb
{
    public abstract class MongoDbRepository<T> : IRepository<T, long> where T : MatrixEntity
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
