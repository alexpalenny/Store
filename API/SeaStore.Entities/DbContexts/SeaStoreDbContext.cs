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

    public SeaStoreDbContext() : base()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      //optionsBuilder.UseSqlServer(@"Server=.\;Database=SeaStore;Trusted_Connection=True;MultipleActiveResultSets=true");

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
    }

  }
}
