using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public class LocationEntityRepository : StateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public LocationEntityRepository(IDbContext<LocationStateEntity> db) : base(db)
        {
        }
        
    }
}