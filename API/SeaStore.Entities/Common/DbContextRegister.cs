using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaStore.Entities.DbContexts;

namespace SeaStore.Entities.Common
{
  public static class DbContextRegister
  {
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
      services.AddMemoryCache();

      services.AddDbContext<SeaStoreDbContext>(options =>
      {
        options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("SeaStore.Entities").UseRowNumberForPaging());
        options.UseOpenIddict();
      });
    }
  }
}
