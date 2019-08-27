using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkLocationEntityRepository : EntityFrameworkStateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public EntityFrameworkLocationEntityRepository(MatrixDbContext db) : base(db)
        {
        }

    }
}