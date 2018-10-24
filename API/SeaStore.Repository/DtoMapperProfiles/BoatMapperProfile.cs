using AutoMapper;
using SeaStore.Dto.Models;
using SeaStore.Entities.Entities;

namespace SeaStore.Repository.DtoMapperProfiles
{
  public class BoatMapperProfile : Profile
  {
    public BoatMapperProfile()
    {
      CreateMap<Boat, BoatDto>();
      CreateMap<PayType, PayTypeDto>();
      CreateMap<BoatType, BoatTypeDto>();
    }
  }
}
