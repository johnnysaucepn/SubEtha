using Howatworks.Matrix.Data.Entities;
using Howatworks.Matrix.Data.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkLocationEntityRepository : EntityFrameworkStateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public EntityFrameworkLocationEntityRepository(MatrixDbContext db) : base(db)
        {
        }

    }
}