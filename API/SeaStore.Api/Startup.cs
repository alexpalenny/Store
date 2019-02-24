using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaStore.Entities;
using SeaStore.Entities.DbContexts;
using SeaStore.Services.Common;
using System;
using System.Net;
using System.Text;

namespace SeaStore
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    //public object IdentityServerConstants { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      ServiceRegister.Register(services, Configuration);
      services.AddLogging();
      services.AddResponseCaching();
      services.AddCors();
      // .AddCookie(o => o.LoginPath = new PathString("/login"))
      services.AddDbContext<SeaStoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
      services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SeaStoreDbContext>()
        .AddDefaultTokenProviders();
      ////services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");
      //services.AddAuthentication(options =>
      //{
      //  options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      //  options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
      //  options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
      //})
      //.AddOpenIdConnect(o =>
      //{
      //  o.ClientId = Configuration["Authorization:Google:ClientId"];
      //  o.ClientSecret = Configuration["Authorization:Google:ClientSecret"];
      //  o.Authority = "https://accounts.google.com";
      //  o.ResponseType = "code";
      //  o.GetClaimsFromUserInfoEndpoint = true;
      //})
      services.AddAuthentication()
      .AddFacebook(o =>
      {
        o.AppId = Configuration["Authorization:Facebook:ClientSecret"];
        o.AppSecret = Configuration["Authorization:Facebook:ClientSecret"];
      });
      //.AddGoogle(o =>
      //{
      //  o.ClientId = Configuration["Authorization:Google:ClientId"];
      //  o.ClientSecret = Configuration["Authorization:Google:ClientSecret"];
      //  o.AuthorizationEndpoint = "https://accounts.google.com";
      //});
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }
      //app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
      //{
      //  Authority = Configuration["Settings:Authentication:Authority"],
      //  RequireHttpsMetadata = false,

      //  ApiName = Configuration["Settings:Authentication:ApiName"],
      //  ApiSecret = Configuration["Settings:Authentication:ApiSecret"],
      //  EnableCaching = true,
      //  CacheDuration = TimeSpan.FromMinutes(10),

      //  AutomaticAuthenticate = true,
      //  AutomaticChallenge = true
      //});

      app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                //.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
        );
      app.UseResponseCaching();
      app.UseStaticFiles();
      app.UseExceptionHandler(builder =>
      {
        builder.Run(async context =>
        {
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = "application/json";
          var error = context.Features.Get<IExceptionHandlerFeature>();

          if (error != null)
          {
            var errorMessage = $"Unhandled {error.Error.GetType()} while executing {context.Request.Method} {context.Request.Path}\n";

            var tempEx = error.Error;
            errorMessage += tempEx;
            await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
          }
        });
      });
      app.UseAuthentication();
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        RequireHeaderSymmetry = false,
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });
      
      //app.UseSwagger();
      //app.UseSwaggerUI(c =>
      //{
      //  c.SwaggerEndpoint("/swagger/SeaStore/swagger.json", "API swagger");
      //});
      app.UseMvc(routes =>
      {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
