using SeaStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace SeaStore.Repository.Interfaces
{
    [Obsolete("This interface should not be used. Instead, create and implement interfaces specific to each repository (e.g. IMyRepository for MyRepository)")]
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        TRepository GetRepository<TRepository>() where TRepository : class, IRepositoryItem;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
