using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SeaStore.Entities.DbContexts;

namespace SeaStore.Repository.UnitOfWork
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(SeaStoreDbContext context, IHttpContextAccessor httpAccessor, IMemoryCache memoryCache) : base(context, memoryCache)
        {
            context.CurrentUserId = httpAccessor.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }
    }
}
