using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaStore.Entities.Common;
using SeaStore.Repository.DtoMapperProfiles;
using SeaStore.Services.Interfaces;
using SeaStore.Services.Services;

namespace SeaStore.Services.Common
{
  public static class ServiceRegister
  {
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
      services.AddScoped<IYachtService, YachtService>();
      DbContextRegister.Register(services, configuration);

      InitializeAutoMapper();
    }

    public static void InitializeAutoMapper()
    {
      Mapper.Initialize(cfg =>
      {
        cfg.AddProfile<YachtMapperProfile>();
      });
    }
  }
}
