using SeaStore.Dto.Models;
using SeaStore.Repository.Interfaces;
using SeaStore.Repository.Repositories;
using SeaStore.Services.Interfaces;
using System.Collections.Generic;

namespace SeaStore.Services.Services
{
  public class BoatService : IBoatService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly BoatRepository _repository;
    public BoatService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.GetRepository<BoatRepository>();
    }
    public IEnumerable<BoatDto> GetAllBoats()
    {
      return _repository.GetAllBoats();
    }
  }
}
