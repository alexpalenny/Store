using Microsoft.EntityFrameworkCore;
using SeaStore.Dto.Models;
using SeaStore.Entities.DbContexts;
using SeaStore.Repository.Common;
using SeaStore.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SeaStore.Repository.Repositories
{
  public class BoatRepository : DisposableRepository, IRepositoryItem
  {
    public BoatRepository(SeaStoreDbContext context) : base(context) { }
    public IEnumerable<BoatDto> GetAllBoats()
    {
      var result = _context.Boats
        .Include(cv => cv.BoatType)
        .Include(cv => cv.PayType)
        .MapToEnumerable<BoatDto>();
      return result;
    }
    public BoatDto GetBoatById(int id)
    {
      var result = _context.Boats
        .Include(cv => cv.BoatType)
        .Include(cv => cv.PayType)
        .FirstOrDefault(cv => cv.Id == id)
        .MapTo<BoatDto>();
      return result;
    }
  }
}
