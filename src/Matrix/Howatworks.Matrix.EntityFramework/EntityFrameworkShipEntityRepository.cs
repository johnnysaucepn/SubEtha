using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkShipEntityRepository : EntityFrameworkStateEntityRepository<ShipStateEntity>, IShipEntityRepository
    {
        public EntityFrameworkShipEntityRepository(MatrixDbContext db) : base(db)
        {
        }

    }
}