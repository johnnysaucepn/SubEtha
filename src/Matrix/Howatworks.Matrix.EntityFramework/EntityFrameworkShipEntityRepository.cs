using Howatworks.Matrix.Data.Entities;
using Howatworks.Matrix.Data.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkShipEntityRepository : EntityFrameworkStateEntityRepository<ShipStateEntity>, IShipEntityRepository
    {
        public EntityFrameworkShipEntityRepository(MatrixDbContext db) : base(db)
        {
        }

    }
}