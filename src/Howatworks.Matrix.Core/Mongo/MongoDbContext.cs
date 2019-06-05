using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.Core.Mongo
{
    public class MongoDbContext<T> : IDbContext<T> where T : IEntity
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IMongoDatabase db)
        {
            _db = db;
        }

        protected IMongoCollection<T> GetCollection()
        {
            return _db.GetCollection<T>(typeof(T).Name);
        }

        public void Create(T entity)
        {
            GetCollection().InsertOne(entity);
        }

        public T Read(Guid id)
        {
            return GetCollection().Find(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> Read(IEnumerable<Guid> ids)
        {
            //TODO: check implementation of this
            return GetCollection().AsQueryable().Where(x => ids.Contains(x.Id));
        }

        public IEnumerable<T> Read(params Guid[] ids)
        {
            return Read(ids.AsEnumerable());
        }

        public void Update(IEnumerable<T> entities)
        {
            var collection = GetCollection();
            foreach (var entity in entities)
            {
                collection.ReplaceOne(
                    x => x.Id == entity.Id,
                    entity,
                    new UpdateOptions {IsUpsert = true}
                );
            }
        }

        public void Update(params T[] entities)
        {
            Update(entities.AsEnumerable());
        }

        public void Delete(IEnumerable<T> entities)
        {
            var ids = entities.Select(x => x.Id);
            Delete(ids);
        }

        public void Delete(params T[] entities)
        {
            Delete(entities.AsEnumerable());
        }

        public void Delete(IEnumerable<Guid> ids)
        {
            GetCollection().DeleteMany(x => ids.Contains(x.Id));
        }

        public void Delete(params Guid[] ids)
        {
            Delete(ids.AsEnumerable());
        }

        public IEnumerable<T> CreateOrUpdate(IEnumerable<T> entities)
        {
            var updatedEntities = entities as IList<T> ?? entities.ToList();
            Update(updatedEntities);
            return updatedEntities;
        }

        public IEnumerable<T> CreateOrUpdate(params T[] entities)
        {
            return CreateOrUpdate(entities.AsEnumerable());
        }

        public IQueryable<T> AsQueryable()
        {
            return GetCollection().AsQueryable();
        }
    }
}
