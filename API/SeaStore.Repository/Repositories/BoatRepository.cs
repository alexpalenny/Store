using SeaStore.Dto.Models;
using SeaStore.Entities.DbContexts;
using SeaStore.Repository.Common;
using SeaStore.Repository.Interfaces;
using System.Collections.Generic;

namespace SeaStore.Repository.Repositories
{
  public class BoatRepository : DisposableRepository, IRepositoryItem
  {
    public BoatRepository(SeaStoreDbContext context) : base(context) { }
    public IEnumerable<BoatDto> GetAllBoats()
    {
      var result = _context.Boats.MapToEnumerable<BoatDto>();
      return result;
    }
  }
}
