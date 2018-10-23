using AutoMapper;
using SeaStore.Dto.Models;
using SeaStore.Entities.Entities;

namespace SeaStore.Repository.DtoMapperProfiles
{
  public class YachtMapperProfile : Profile
  {
    public YachtMapperProfile()
    {
      CreateMap<Yacht, YachtDto>();
    }
  }
}
