using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.InMemory
{
    public class InMemoryDbContext<T> where T : IMatrixEntity
    {
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static long _nextId = 1;
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static readonly object _nextIdLock = new object();

        private readonly Dictionary<long, T> _storage = new Dictionary<long, T>();

        public void Create(T entity)
        {
            EnsureUniqueId(entity);
            if (_storage.ContainsKey(entity.Id))
            {
                throw new ArgumentException("An entity with the same id has already been added.", nameof(entity.Id));
            }
            _storage.Add(entity.Id, entity);
        }

        private static void EnsureUniqueId(T entity)
        {
            if (entity.Id == default(long))
            {
                entity.Id = GetNextId();
            }
        }

        public T Read(long id)
        {
            return _storage.ContainsKey(id) ? _storage[id] : default(T);
        }

        public IEnumerable<T> Read(IEnumerable<long> ids)
        {
            return ids.Select(x => _storage.ContainsKey(x) ? _storage[x] : default(T));
        }

        public IEnumerable<T> Read(params long[] ids)
        {
            return Read(ids.AsEnumerable());
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (_storage.ContainsKey(entity.Id))
                {
                    _storage[entity.Id] = entity;
                }
                else
                {
                    throw new ArgumentException("The given id was not present.", nameof(entity.Id));
                }
            }
        }

        public void Update(params T[] entities)
        {
            Update(entities.AsEnumerable());
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (_storage.ContainsKey(entity.Id))
                {
                    _storage.Remove(entity.Id);
                }
                else
                {
                    throw new ArgumentException("The given id was not present.", nameof(entity.Id));
                }
            }
        }

        public void Delete(params T[] entities)
        {
            Delete(entities.AsEnumerable());
        }

        public void Delete(IEnumerable<long> ids)
        {
            foreach (var id in ids)
            {
                if (_storage.ContainsKey(id))
                {
                    _storage.Remove(id);
                }
                else
                {
                    throw new ArgumentException("The given id was not present.", nameof(id));
                }
            }
        }

        public void Delete(params long[] ids)
        {
            Delete(ids.AsEnumerable());
        }

        public IEnumerable<T> CreateOrUpdate(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (_storage.ContainsKey(entity.Id))
                {
                    _storage[entity.Id] = entity;
                }
                else
                {
                    EnsureUniqueId(entity);
                    _storage.Add(entity.Id, entity);
                }
                yield return entity;
            }
        }

        public IEnumerable<T> CreateOrUpdate(params T[] entities)
        {
            return CreateOrUpdate(entities.AsEnumerable());
        }

        public IQueryable<T> AsQueryable()
        {
            return _storage.Values.AsQueryable();
        }

        private static long GetNextId()
        {
            lock (_nextIdLock)
            {
                var nextId = _nextId;
                _nextId++;
                return nextId;
            }
        }
    }
}
