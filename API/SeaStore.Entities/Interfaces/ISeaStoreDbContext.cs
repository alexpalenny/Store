using Microsoft.EntityFrameworkCore;
using SeaStore.Entities.Entities;
using System;

namespace SeaStore.Entities.Interfaces
{
  public interface ISeaStoreDbContext : IDisposable
  {
    string CurrentUserId { get; set; }

    DbSet<Yacht> Yachts { get; set; }
  }
}
