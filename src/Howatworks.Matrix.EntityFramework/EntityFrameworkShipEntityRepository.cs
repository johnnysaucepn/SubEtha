using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkShipEntityRepository : EntityFrameworkStateEntityRepository<ShipStateEntity>, IShipEntityRepository
    {
        public EntityFrameworkShipEntityRepository(DbContext db) : base(db)
        {
        }

    }
}