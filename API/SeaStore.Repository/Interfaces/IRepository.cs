using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SeaStore.Repository.Interfaces
{
    [Obsolete("This generic interface shouldn't be implemented by new repositories. Instead, create and implement an interface specific to the repository (e.g. IMyRepository for MyRepository)")]
    public interface IRepository<TEntity> : IRepositoryItem where TEntity : class
    {
        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        bool Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int Count();

        IQueryable<TEntity> Query();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Clear();

        //ASYNC
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(int id);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }

}
