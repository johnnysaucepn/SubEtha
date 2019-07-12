using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public abstract class EntityFrameworkRepository<T> : IRepository<T, long> where T : MatrixEntity
    {
        protected readonly MatrixDbContext Db;

        protected EntityFrameworkRepository(MatrixDbContext db)
        {
            Db = db;
        }

        public virtual void Add(T entity)
        {
            Db.Add(entity);
        }

        public virtual T Get(long id)
        {
            return Db.Find<T>(id);
        }

        public virtual void Remove(long id)
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
