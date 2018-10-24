using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeaStore.Entities.Entities;
using SeaStore.Entities.Interfaces;
using System;

namespace SeaStore.Entities.DbContexts
{
  public class SeaStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, ISeaStoreDbContext
  {
    public Guid Id { get; } = Guid.NewGuid();

    public string CurrentUserId { get; set; }

    public DbSet<Yacht> Yachts { get; set; }

    public SeaStoreDbContext() : base()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(@"Server=.\;Database=SeaStore;Trusted_Connection=True;MultipleActiveResultSets=true");

#if DEBUG
      // SUPER FEATURE. Shows the Ids of conflicted entities on Debug
      optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    public SeaStoreDbContext(DbContextOptions<SeaStoreDbContext> options) : base(options)
    {
    }
  }
}
