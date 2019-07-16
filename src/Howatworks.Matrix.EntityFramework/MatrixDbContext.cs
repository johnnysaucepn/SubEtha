using System;
using Howatworks.Matrix.Core.Entities;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"host=localhost;database=Matrix;username=matrix;password=matrix;");
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("public");

            builder.Entity<MatrixEntity>().Property(x => x.Id).ValueGeneratedOnAdd().UseNpgsqlIdentityColumn();
            builder.Ignore<MatrixEntity>();

            builder.Entity<Group>().HasData(new Group(Group.DefaultGroupName) {Id = 1});

            builder.Entity<CommanderGroup>(e =>
            {
                e.HasKey(cg => new {cg.CommanderName, cg.GroupId});
                e.HasOne(cg => cg.Group).WithMany(g => g.CommanderGroups).HasForeignKey(ug => ug.GroupId);
            });

            builder.Entity<LocationStateEntity>(e =>
            {
                e.OwnsOne(x => x.Body);
                e.OwnsOne(x => x.GameContext);
                e.OwnsOne(x => x.SignalSource).OwnsOne(s => s.Type);
                e.OwnsOne(x => x.StarSystem);
                e.OwnsOne(x => x.Station);
                e.OwnsOne(x => x.SurfaceLocation);
            });

            builder.Entity<SessionStateEntity>().OwnsOne(x => x.GameContext);

            builder.Entity<ShipStateEntity>().OwnsOne(x => x.GameContext);
        }
    }
}
