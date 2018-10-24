using System.Collections.Generic;
using System;
using System.Linq;
using AutoMapper;
using SeaStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using SeaStore.Entities.Interfaces.DbContexts;
using SeaStore.FilterQuery;

namespace SeaStore.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; private set; }

        private readonly IList<IRepositoryItem> _repositoryList = new List<IRepositoryItem>();

        public UnitOfWork(ISeaStoreDbContext context, IMemoryCache memoryCache)
        {
            Context = (DbContext)context;
            if (QueryableExtensions.Cache == null)
                QueryableExtensions.Cache = memoryCache;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class, IRepositoryItem
        {
            var repoItem = _repositoryList.FirstOrDefault(cv => cv.GetType() == typeof(TRepository));
            if (repoItem == null)
            {
                repoItem = (TRepository)Activator.CreateInstance(typeof(TRepository), new[] { Context });
                _repositoryList.Add(repoItem);
            }
            return (TRepository)repoItem;
        }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var res = await Context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose();
        }
    }
}
