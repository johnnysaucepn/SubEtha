using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkLocationEntityRepository : EntityFrameworkStateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public EntityFrameworkLocationEntityRepository(DbContext db) : base(db)
        {
        }

    }
}