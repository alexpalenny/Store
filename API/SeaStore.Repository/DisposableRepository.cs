using SeaStore.Entities.DbContexts;
using System;

namespace SeaStore.Repository
{
  public class DisposableRepository
  {
    protected readonly SeaStoreDbContext _context;

    public DisposableRepository(SeaStoreDbContext context)
    {
      _context = context;
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
