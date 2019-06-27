using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.MongoDb
{
    public class MongoDbLocationEntityRepository : MongoDbStateEntityRepository<LocationStateEntity>, ILocationEntityRepository
    {
        public MongoDbLocationEntityRepository(MongoDbContext<LocationStateEntity> db) : base(db)
        {
        }

    }
}