using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaStore.Entities.Common;
using SeaStore.Repository.DtoMapperProfiles;
using SeaStore.Repository.Interfaces;
using SeaStore.Repository.UnitOfWork;
using SeaStore.Services.Interfaces;
using SeaStore.Services.Services;

namespace SeaStore.Services.Common
{
  public static class ServiceRegister
  {
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
      services.AddScoped<IBoatService, BoatService>();
      services.AddTransient<IEmailSender, AuthMessageSender>();
      services.AddTransient<ISmsSender, AuthMessageSender>();
      DbContextRegister.Register(services, configuration);

      InitializeAutoMapper();
    }

    public static void InitializeAutoMapper()
    {
      Mapper.Initialize(cfg =>
      {
        cfg.AddProfile<BoatMapperProfile>();
      });
    }
  }
}
