using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeaStore.Entities.Configuration;
using SeaStore.Entities.Entities;
using SeaStore.Entities.Interfaces;
using System;

namespace SeaStore.Entities.DbContexts
{
  public class SeaStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, ISeaStoreDbContext
  {
    public Guid Id { get; } = Guid.NewGuid();

    public string CurrentUserId { get; set; }

    public DbSet<Boat> Boats { get; set; }
    public DbSet<PayType> PayTypes { get; set; }
    public DbSet<BoatType> BoatTypes { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<IdentityUserRole<Guid>> IdentityUserRoles { get; set; }
    public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }


    public SeaStoreDbContext() : base()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
      // SUPER FEATURE. Shows the Ids of conflicted entities on Debug
      optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    public SeaStoreDbContext(DbContextOptions<SeaStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new BoatConfiguration());
      builder.ApplyConfiguration(new BoatTypeConfiguration());
      builder.ApplyConfiguration(new PayTypeConfiguration());

      builder.Entity<ApplicationUser>().HasKey(p => p.Id);
      builder.Entity<ApplicationRole>().HasKey(p => p.Id);
      builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });


      base.OnModelCreating(builder);
    }

  }
}
