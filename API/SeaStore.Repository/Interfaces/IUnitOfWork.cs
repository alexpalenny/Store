using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace SeaStore.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        TRepository GetRepository<TRepository>() where TRepository : class, IRepositoryItem;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
