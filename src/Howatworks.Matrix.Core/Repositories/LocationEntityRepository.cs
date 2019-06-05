using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.Core.Repositories
{
    public class LocationEntityRepository : StateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public LocationEntityRepository(IDbContext<LocationStateEntity> db) : base(db)
        {
        }
        
    }
}