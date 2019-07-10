﻿// <auto-generated />
using System;
using Howatworks.Matrix.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Howatworks.Matrix.EntityFramework.Migrations
{
    [DbContext(typeof(MatrixDbContext))]
    partial class MatrixDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Groups");

                    b.HasDiscriminator().HasValue("Group");

                    b.HasData(
                        new { Id = 1L, Name = "Default" }
                    );
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.LocationStateEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasDiscriminator().HasValue("LocationStateEntity");
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.SessionStateEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Build");

                    b.Property<string>("CommanderName");

                    b.Property<string>("GameMode");

                    b.Property<string>("Group");

                    b.Property<DateTimeOffset>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("Sessions");

                    b.HasDiscriminator().HasValue("SessionStateEntity");
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.ShipStateEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("HullIntegrity");

                    b.Property<string>("Ident");

                    b.Property<string>("Name");

                    b.Property<bool?>("ShieldsUp");

                    b.Property<int>("ShipId");

                    b.Property<DateTimeOffset>("TimeStamp");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Ships");

                    b.HasDiscriminator().HasValue("ShipStateEntity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.MatrixIdentityUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<long?>("GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("MatrixIdentityUser");

                    b.HasDiscriminator().HasValue("MatrixIdentityUser");
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.LocationStateEntity", b =>
                {
                    b.OwnsOne("Howatworks.Matrix.Domain.Body", "Body", b1 =>
                        {
                            b1.Property<long?>("LocationStateEntityId");

                            b1.Property<bool>("Docked");

                            b1.Property<string>("Name");

                            b1.Property<string>("Type");

                            b1.ToTable("Locations","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.LocationStateEntity")
                                .WithOne("Body")
                                .HasForeignKey("Howatworks.Matrix.Domain.Body", "LocationStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Howatworks.Matrix.Domain.SignalSource", "SignalSource", b1 =>
                        {
                            b1.Property<long?>("LocationStateEntityId");

                            b1.Property<int>("Threat");

                            b1.ToTable("Locations","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.LocationStateEntity")
                                .WithOne("SignalSource")
                                .HasForeignKey("Howatworks.Matrix.Domain.SignalSource", "LocationStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsOne("Howatworks.Matrix.Domain.LocalisedString", "Type", b2 =>
                                {
                                    b2.Property<long?>("SignalSourceLocationStateEntityId");

                                    b2.Property<string>("Symbol");

                                    b2.Property<string>("Text");

                                    b2.ToTable("Locations","public");

                                    b2.HasOne("Howatworks.Matrix.Domain.SignalSource")
                                        .WithOne("Type")
                                        .HasForeignKey("Howatworks.Matrix.Domain.LocalisedString", "SignalSourceLocationStateEntityId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });

                    b.OwnsOne("Howatworks.Matrix.Domain.StarSystem", "StarSystem", b1 =>
                        {
                            b1.Property<long?>("LocationStateEntityId");

                            b1.Property<decimal[]>("Coords");

                            b1.Property<string>("Name");

                            b1.ToTable("Locations","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.LocationStateEntity")
                                .WithOne("StarSystem")
                                .HasForeignKey("Howatworks.Matrix.Domain.StarSystem", "LocationStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Howatworks.Matrix.Domain.Station", "Station", b1 =>
                        {
                            b1.Property<long?>("LocationStateEntityId");

                            b1.Property<string>("Name");

                            b1.Property<string>("Type");

                            b1.ToTable("Locations","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.LocationStateEntity")
                                .WithOne("Station")
                                .HasForeignKey("Howatworks.Matrix.Domain.Station", "LocationStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Howatworks.Matrix.Domain.SurfaceLocation", "SurfaceLocation", b1 =>
                        {
                            b1.Property<long?>("LocationStateEntityId");

                            b1.Property<bool>("Landed");

                            b1.Property<decimal?>("Latitude");

                            b1.Property<decimal?>("Longitude");

                            b1.ToTable("Locations","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.LocationStateEntity")
                                .WithOne("SurfaceLocation")
                                .HasForeignKey("Howatworks.Matrix.Domain.SurfaceLocation", "LocationStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Howatworks.Matrix.Domain.GameContext", "GameContext", b1 =>
                        {
                            b1.Property<long>("LocationStateEntityId");

                            b1.Property<string>("GameVersion");

                            b1.Property<bool>("IsLive");

                            b1.Property<string>("User");

                            b1.ToTable("Locations","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.LocationStateEntity")
                                .WithOne("GameContext")
                                .HasForeignKey("Howatworks.Matrix.Domain.GameContext", "LocationStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.SessionStateEntity", b =>
                {
                    b.OwnsOne("Howatworks.Matrix.Domain.GameContext", "GameContext", b1 =>
                        {
                            b1.Property<long>("SessionStateEntityId");

                            b1.Property<string>("GameVersion");

                            b1.Property<bool>("IsLive");

                            b1.Property<string>("User");

                            b1.ToTable("Sessions","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.SessionStateEntity")
                                .WithOne("GameContext")
                                .HasForeignKey("Howatworks.Matrix.Domain.GameContext", "SessionStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.ShipStateEntity", b =>
                {
                    b.OwnsOne("Howatworks.Matrix.Domain.GameContext", "GameContext", b1 =>
                        {
                            b1.Property<long>("ShipStateEntityId");

                            b1.Property<string>("GameVersion");

                            b1.Property<bool>("IsLive");

                            b1.Property<string>("User");

                            b1.ToTable("Ships","public");

                            b1.HasOne("Howatworks.Matrix.Core.Entities.ShipStateEntity")
                                .WithOne("GameContext")
                                .HasForeignKey("Howatworks.Matrix.Domain.GameContext", "ShipStateEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Howatworks.Matrix.Core.Entities.MatrixIdentityUser", b =>
                {
                    b.HasOne("Howatworks.Matrix.Core.Entities.Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
