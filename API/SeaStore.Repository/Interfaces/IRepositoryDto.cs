using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SeaStore.Repository.Interfaces
{
    [Obsolete("This generic interface shouldn't be implemented by new repositories. Instead, create and implement an interface specific to the repository (e.g. IMyRepository for MyRepository)")]
    public interface IRepositoryDto<TDto> : IRepositoryItem where TDto : class
    {
        TDto AddDto(TDto dto);
        IEnumerable<TDto> AddRangeDto(IEnumerable<TDto> dtoes);

        bool UpdateDto(TDto dto);
        void UpdateRangeDto(IEnumerable<TDto> dtoes);

        bool RemoveDto(TDto dto);
        void RemoveRangeDto(IEnumerable<TDto> dtoes);

        int Count();
        
        IEnumerable<TDto> FindDto(Expression<Func<TDto, bool>> predicate, Expression<Func<TDto, dynamic>> orderBy = null, int take = 0, int skip = 0);
        TDto FirstOrDefaultDto(Expression<Func<TDto, bool>> predicate);
        TDto GetDto(int id);
        IEnumerable<TDto> GetAllDto();
        void Clear();

        //ASYNC
        //Task<IEnumerable<TDto>> FindDtoAsync(Expression<Func<TDto, bool>> predicate);
        //Task<IEnumerable<TDto>> GetAllDtoAsync();
        //Task<TDto> GetSingleOrDefaultDtoAsync(Expression<Func<TDto, bool>> predicate);
        //Task<TDto> GetDtoAsync(int id);
        //Task<int> CountDtoAsync(Expression<Func<TDto, bool>> predicate);
    }

}
