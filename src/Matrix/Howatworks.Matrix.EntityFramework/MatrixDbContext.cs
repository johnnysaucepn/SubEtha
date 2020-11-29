using Howatworks.Matrix.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class MatrixDbContext : IdentityDbContext<MatrixIdentityUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<CommanderGroup> CommanderGroups { get; set; }
        public DbSet<LocationStateEntity> Locations { get; set; }
        public DbSet<SessionStateEntity> Sessions { get; set; }
        public DbSet<ShipStateEntity> Ships { get; set; }

        public MatrixDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseIdentityColumns();

            builder.HasDefaultSchema("public");

            builder.Entity<Group>(e =>
            {
                e.HasData(new Group(Group.DefaultGroupName) {Id = 1});
            });
        }
    }
}
