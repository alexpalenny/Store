using AutoMapper;
using SeaStore.Dto.Models;
using SeaStore.Entities.Entities;

namespace SeaStore.Repository.DtoMapperProfiles
{
  public class BoatMapperProfile : Profile
  {
    public BoatMapperProfile()
    {
      CreateMap<Boat, BoatDto>()
        .ForMember(cv => cv.BoatType, opt => opt.MapFrom(src => src.BoatType != null ? src.BoatType.Name : null))
        .ForMember(cv => cv.PayType, opt => opt.MapFrom(src => src.PayType != null ? src.PayType.Name : null));
      CreateMap<PayType, PayTypeDto>();
      CreateMap<BoatType, BoatTypeDto>();
    }
  }
}
