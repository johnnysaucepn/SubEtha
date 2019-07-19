using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.InMemory
{
    public class InMemoryLocationEntityRepository : InMemoryStateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public InMemoryLocationEntityRepository(InMemoryDbContext<LocationStateEntity> db) : base(db)
        {
        }

    }
}